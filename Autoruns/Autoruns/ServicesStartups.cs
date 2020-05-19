using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autoruns
{
    class ServicesStartups : Startups
    {
        private const string RegEntry = "HKLM\\System\\CurrentControlSet\\Services";

        public ServicesStartups(ListView _listView)
        {
            listView = _listView;
            LoadRegEntry();
        }

        private void LoadRegEntry()
        {
            RegistryKey key = Registry.LocalMachine;
            const string regEntryName = "System\\CurrentControlSet\\Services";

            StartupEntry localStartupEntry = new StartupEntry(true,
                RegEntry,
                string.Empty,
                string.Empty,
                string.Empty,
                DateTime.Now);
            starupEntrys.Add(localStartupEntry);

            RegistryKey subkey = key.OpenSubKey(regEntryName, true);
            string[] subKeyNames = subkey.GetSubKeyNames();
            foreach (string appEntry in subKeyNames)
            {
                if (appEntry == "CertPropSvc")
                {
                    string sd = "sdf";
                }
                RegistryKey appSubKey = key.OpenSubKey(regEntryName + "\\" + appEntry);
                
                string kname = regEntryName + "\\" + appEntry + "\\parameters";
                RegistryKey k = key.OpenSubKey(kname);
                if (k == null || k.GetValue("serviceDLL") == null)
                {
                    if (appSubKey.GetValue("ImagePath") != null)
                    {
                        string target1 = GetValueContentAsPath(appSubKey.GetValue("ImagePath").ToString());
                        if (target1 == "") continue;
                        if (GetFileDescription(target1) == "") continue;
                        AddStartupEntry(false,
                            appEntry,
                            GetFileDescription(target1),
                            GetFilePublisher(target1),
                            target1,
                            GetFileTime(target1));
                        continue;
                    }
                    continue;
                }
                string value = k.GetValue("serviceDLL").ToString();
                string target = GetValueContentAsPath(value);
                if (GetFileDescription(target) == "") continue;
                AddStartupEntry(false,
                    appEntry,
                    GetFileDescription(target),
                    GetFilePublisher(target),
                    target,
                    GetFileTime(target));
                localStartupEntry.IsEmpty = false;
            }
        }
    }
}
