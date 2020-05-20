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
        private const string RegEntry = "System\\CurrentControlSet\\Services";

        public ServicesStartups(ListView _listView)
        {
            listView = _listView;
            LoadRegEntry();
        }

        private void LoadRegEntry()
        {
            RegistryKey key = Registry.LocalMachine;
            
            StartupEntry localStartupEntry = new StartupEntry(true,
                RegEntry,
                string.Empty,
                string.Empty,
                string.Empty,
                DateTime.Now);
            localStartupEntry.IsEmpty = false;
            starupEntrys.Add(localStartupEntry);

            RegistryKey subkey = key.OpenSubKey(RegEntry, true);
            string[] subKeyNames = subkey.GetSubKeyNames();
            foreach (string serviceName in subKeyNames)
            {
                if (TryParametersSubKey(serviceName) == false)
                    TryImagePath(serviceName);
            }
        }

        private bool TryImagePath(string serviceName)
        {
            // Open subkey `HKLM\\System\\CurrentControlSet\\Services\\xxx`
            RegistryKey key = Registry.LocalMachine;
            string serviceSubKeyName = RegEntry + @"\" + serviceName;
            RegistryKey subkey = key.OpenSubKey(serviceSubKeyName, false);
            if (subkey == null) return false;

            // Get the content of keyvalue ImagePath as target
            object o = subkey.GetValue("ImagePath");
            if (o == null) return false;
            string target = GetValueContentAsPath(o.ToString());
            if (target == string.Empty) return false;

            string disp = string.Empty;
            string desc = string.Empty;
            o = subkey.GetValue("DisplayName");
            if (o != null)
            {
                disp = o.ToString();
                if (disp.StartsWith("@"))
                {
                    disp = MuiStrng.LoadMuiStringValue(subkey, "DisplayName");
                }
            }
            o = subkey.GetValue("Description");
            if (o != null)
            {
                desc = o.ToString();
                if (desc.StartsWith("@"))
                {
                    desc = MuiStrng.LoadMuiStringValue(subkey, "Description");
                }
            }

            AddStartupEntry(false,
                            serviceName,
                            disp + ": " + desc + GetFileDescription(target),
                            GetFilePublisher(target),
                            target,
                            GetFileTime(target));
            return true;
        }

        private bool TryParametersSubKey(string serviceName)
        {
            // Open subkey `HKLM\\System\\CurrentControlSet\\Services\\xxx\\Parameters`
            RegistryKey key = Registry.LocalMachine;
            string serviceSubKeyName = RegEntry + @"\" + serviceName + @"\Parameters";
            RegistryKey subkey = key.OpenSubKey(serviceSubKeyName, false);
            if (subkey == null) return false;

            // Get the content of keyvalue serviceDLL as target
            object o = subkey.GetValue("serviceDLL");
            if (o == null) return false;
            string target = GetValueContentAsPath(o.ToString());
            if (target == string.Empty) return false;

            string disp = string.Empty;
            string desc = string.Empty;
            subkey = key.OpenSubKey(RegEntry + @"\" + serviceName, false);
            o = subkey.GetValue("DisplayName");
            if (o != null)
            {
                disp = o.ToString();
                if (disp.StartsWith("@"))
                {
                    disp = MuiStrng.LoadMuiStringValue(subkey, "DisplayName");
                }
            }
            o = subkey.GetValue("Description");
            if (o != null)
            {
                desc = o.ToString();
                if (desc.StartsWith("@"))
                {
                    desc = MuiStrng.LoadMuiStringValue(subkey, "Description");
                }
            }
            if (serviceName == "AeLookupSvc")
            {
                string s = "sdf";
            }
            AddStartupEntry(false,
                            serviceName,
                            disp + ": " + desc + GetFileDescription(target),
                            GetFilePublisher(target),
                            target,
                            GetFileTime(target));
            return true;
        }
    }
}
