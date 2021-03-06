﻿using Microsoft.Win32;
using System;
using System.Windows.Forms;

namespace Autoruns
{
    class ServicesStartups : Startups
    {
        private const string RegEntry = "System\\CurrentControlSet\\Services";

        private bool isDrivers;

        public ServicesStartups(ListView _listView, bool _isDrivers)
        {
            listView = _listView;
            isDrivers = _isDrivers;
            AddHeaderKeyEntry();
            LoadRegistryEntry();
        }

        /// <summary>
        ///     the first entry of `starupEntrys` is the Registry Key ( for display )
        /// </summary>
        private void AddHeaderKeyEntry()
        {
            StartupEntry localStartupEntry = new StartupEntry(true,
                @"HKLM\\" + RegEntry,
                string.Empty,
                string.Empty,
                string.Empty,
                DateTime.Now);
            localStartupEntry.IsEmpty = false;
            starupEntrys.Add(localStartupEntry);
        }

        /// <summary>
        ///     Loop through Key `RegEntry` and try two parse way to interprete
        ///     the target file path
        /// </summary>
        private void LoadRegistryEntry()
        {
            RegistryKey key = Registry.LocalMachine;
            RegistryKey subkey = key.OpenSubKey(RegEntry, true);
            
            foreach (string serviceName in subkey.GetSubKeyNames())
            {
                if (TryParametersSubKey(serviceName) == false)
                    TryImagePath(serviceName);
            }
        }

        /// <summary>
        ///     `ImagePath` key
        /// </summary>
        /// <param name="serviceName">the service key name</param>
        /// <returns>whether parse succeed</returns>
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
            target = GetFilePathUnderSystemPath(target);
            if (target == string.Empty) return false;

            AddValidStartupEntry(target, serviceName);
            return true;
        }

        /// <summary>
        ///     `serviceDLL` Value under `Parameters` Key
        /// </summary>
        /// <param name="serviceName">the service key name</param>
        /// <returns>whether parse succeed</returns>
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
            
            AddValidStartupEntry(target, serviceName);
            return true;
        }

        private void AddValidStartupEntry(string targetFilePath, string serviceName)
        {
            string keyName = RegEntry + @"\" + serviceName;
            RegistryKey subkey = Registry.LocalMachine.OpenSubKey(keyName, false);
            
            object o = subkey.GetValue("DisplayName");
            string disp = o == null ? "" : o.ToString();
            if (disp.StartsWith("@"))
                disp = MuiStrng.LoadMuiStringValue(subkey, "DisplayName");
            if (disp == string.Empty) disp = serviceName;
            disp += ": ";

            o = subkey.GetValue("Description");
            string desc = o == null ? "" : o.ToString();
            if (desc.StartsWith("@"))
                desc = MuiStrng.LoadMuiStringValue(subkey, "Description");
            if (desc == null || desc == string.Empty)
                desc = GetFileDescription(targetFilePath);
            if ((!isDrivers && !targetFilePath.EndsWith("sys")) ||
                ( isDrivers &&  targetFilePath.EndsWith("sys")))
                AddStartupEntry(false,
                                serviceName,
                                disp + desc,
                                GetFilePublisher(targetFilePath) == "" ? "(Signature not embedded)" : GetFilePublisher(targetFilePath),
                                targetFilePath,
                                GetFileTime(targetFilePath));
        }
    }
}
