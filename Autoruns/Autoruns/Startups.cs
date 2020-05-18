using IWshRuntimeLibrary;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography.X509Certificates;

namespace Autoruns
{
    internal class Startups
    {

        public ArrayList starupEntrys = new ArrayList();

        private string[] RegEntry =
        {
            "HKLM\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon\\Userinit",
            "HKLM\\System\\CurrentControlSet\\Control\\Terminal Server\\Wds\\rdpwd\\StartupPrograms",
            "HKLM\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon\\AppSetup",
            "HKLM\\Software\\Policies\\Microsoft\\Windows\\System\\Scripts\\Startup",
            "HKCU\\Software\\Policies\\Microsoft\\Windows\\System\\Scripts\\Logon",
            "HKLM\\Software\\Policies\\Microsoft\\Windows\\System\\Scripts\\Logon",
            "HKCU\\Environment\\UserInitMprLogonScript",
            "HKLM\\Environment\\UserInitMprLogonScript",
            "HKLM\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon\\Userinit",
            "HKLM\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon\\VmApplet",
            "HKLM\\Software\\Policies\\Microsoft\\Windows\\System\\Scripts\\Shutdown",
            "HKCU\\Software\\Policies\\Microsoft\\Windows\\System\\Scripts\\Logoff",
            "HKLM\\Software\\Policies\\Microsoft\\Windows\\System\\Scripts\\Logoff",
            "HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Group Policy\\Scripts\\Startup",
            "HKLM\\Software\\Microsoft\\Windows\\CurrentVersion\\Group Policy\\Scripts\\Startup",
            "HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Group Policy\\Scripts\\Logon",
            "HKLM\\Software\\Microsoft\\Windows\\CurrentVersion\\Group Policy\\Scripts\\Logon",
            "HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Group Policy\\Scripts\\Logoff",
            "HKLM\\Software\\Microsoft\\Windows\\CurrentVersion\\Group Policy\\Scripts\\Logoff",
            "HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Group Policy\\Scripts\\Shutdown",
            "HKLM\\Software\\Microsoft\\Windows\\CurrentVersion\\Group Policy\\Scripts\\Shutdown",
            "HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System\\Shell",
            "HKCU\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon\\Shell",
            "HKLM\\Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System\\Shell",
            "HKLM\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon\\Shell",
            "HKLM\\SYSTEM\\CurrentControlSet\\Control\\SafeBoot\\AlternateShell",
            "HKLM\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon\\Taskman",
            "HKLM\\Software\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon\\AlternateShells\\AvailableShells",
            "HKLM\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Terminal Server\\Install\\Software\\Microsoft\\Windows\\CurrentVersion\\Runonce",
            "HKLM\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Terminal Server\\Install\\Software\\Microsoft\\Windows\\CurrentVersion\\RunonceEx",
            "HKLM\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Terminal Server\\Install\\Software\\Microsoft\\Windows\\CurrentVersion\\Run",
            "HKLM\\SYSTEM\\CurrentControlSet\\Control\\Terminal Server\\WinStations\\RDP-Tcp\\InitialProgram",
            "HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run",
            "HKLM\\SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Run",
            "HKCU\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run",
            "HKCU\\SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Run",
            "HKCU\\SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\RunOnceEx",
            "HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\RunOnce",
            "HKLM\\SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\RunOnce",
            "HKCU\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\RunOnce",
            "HKCU\\SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\RunOnce",
            "HKCU\\Software\\Microsoft\\Windows NT\\CurrentVersion\\Windows\\Load",
            "HKCU\\Software\\Microsoft\\Windows NT\\CurrentVersion\\Windows\\Run",
            "HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer\\Run",
            "HKCU\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer\\Run",
            "HKLM\\SOFTWARE\\Microsoft\\Active Setup\\Installed Components",
            "HKLM\\SOFTWARE\\Wow6432Node\\Microsoft\\Active Setup\\Installed Components",
            "HKLM\\Software\\Microsoft\\Windows NT\\CurrentVersion\\Windows\\IconServiceLib",
            "HKCU\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Terminal Server\\Install\\Software\\Microsoft\\Windows\\CurrentVersion\\Runonce",
            "HKCU\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Terminal Server\\Install\\Software\\Microsoft\\Windows\\CurrentVersion\\RunonceEx",
            "HKCU\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Terminal Server\\Install\\Software\\Microsoft\\Windows\\CurrentVersion\\Run",
            "HKLM\\SOFTWARE\\Microsoft\\Windows CE Services\\AutoStartOnConnect",
            "HKLM\\SOFTWARE\\Wow6432Node\\Microsoft\\Windows CE Services\\AutoStartOnConnect",
            "HKLM\\SOFTWARE\\Microsoft\\Windows CE Services\\AutoStartOnDisconnect",
            "HKLM\\SOFTWARE\\Wow6432Node\\Microsoft\\Windows CE Services\\AutoStartOnDisconnect",



/*
            "HKLM\\SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Run",
            "HKLM\\SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Run",
            "HKCU\\SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Run",
            "HKLM\\SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer\\Run",
            "HKCU\\SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer\\Run",
            "HKLM\\SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\RunOnce",
            "HKCU\\SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\RunOnce",
            "HKLM\\SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\RunOnceEx",
            "HKCU\\SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\RunOnceEx",

            "HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run",
            "HKCU\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run",
            "HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer\\Run",
            "HKCU\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer\\Run",
            "HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\RunOnce",
            "HKCU\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\RunOnce",
            "HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\RunOnceEx",
            "HKCU\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\RunOnceEx",
*/
        };

        public Startups()
        {
            LoadStartupDir();
            LoadRegistryTable();
        }

        private void LoadRegistryTable()
        {
            foreach (string entry in RegEntry)
                LoadRegEntry(entry);
        }

        private void LoadRegEntry(string keyPath)
        {
            // Figure out root key & subkey
            int firstSlash = keyPath.IndexOf('\\');
            string rootKey = keyPath.Substring(0, firstSlash);
            string regEntryName = keyPath.Substring(firstSlash + 1);
            RegistryKey key;
            switch (rootKey)
            {
                case "HKLM":
                    key = Registry.LocalMachine;
                    break;
                case "HKCU":
                    key = Registry.CurrentUser;
                    break;
                default:
                    return;
            }
            StartupEntry localStartupEntry = new StartupEntry(true,
                keyPath,
                string.Empty,
                string.Empty,
                string.Empty,
                DateTime.Now);
            starupEntrys.Add(localStartupEntry);

            RegistryKey subkey = key.OpenSubKey(regEntryName, true);
            
            if (subkey == null) // try it as a Key-Value
            {
                int delimiter = regEntryName.LastIndexOf('\\');
                string regEntryPath = regEntryName.Substring(0, delimiter);
                string regEntryValue = regEntryName.Substring(delimiter + 1);
                subkey = key.OpenSubKey(regEntryPath, true);
                if (subkey == null) return;
                if (subkey.GetValue(regEntryValue) == null) return;
                string value = subkey.GetValue(regEntryValue).ToString();
                if (value == string.Empty) return;
                string valueContent = GetValueContentAsPath(value);
                if (valueContent.IndexOf('.') == -1)
                    valueContent += ".exe";
                if(!System.IO.File.Exists(valueContent))
                    valueContent = GetFilePathUnderSystemPath(valueContent);
                if (valueContent == "") return;
                AddStartupEntry(false,
                    value,
                    GetFileDescription(valueContent),
                    GetFilePublisher(valueContent),
                    valueContent,
                    GetFileTime(valueContent));
                localStartupEntry.IsEmpty = false;
                return;
            }

            string[] valuenames = subkey.GetValueNames();
            foreach (string valuename in valuenames)
            {
                string value = subkey.GetValue(valuename).ToString();
                string target = GetValueContentAsPath(value);
                AddStartupEntry(false, 
                    valuename,
                    GetFileDescription(target),
                    GetFilePublisher(target),
                    target,
                    GetFileTime(target));
                localStartupEntry.IsEmpty = false;
            }
        }

        private void LoadStartupDir()
        {
            string USERPROFILE = Environment.GetEnvironmentVariable("USERPROFILE");
            LoadStartupDirEntry(new DirectoryInfo(USERPROFILE + "\\AppData\\Roaming\\Microsoft\\Windows\\Start Menu\\Programs\\Startup"));

            string ProgramData = Environment.GetEnvironmentVariable("ProgramData");
            LoadStartupDirEntry(new DirectoryInfo(ProgramData + "\\Microsoft\\Windows\\Start Menu\\Programs\\Startup"));
        }

        private void LoadStartupDirEntry(DirectoryInfo dir)
        {
            StartupEntry localStartupEntry = new StartupEntry(true,
                dir.FullName,
                string.Empty,
                string.Empty,
                string.Empty,
                dir.LastWriteTime);
            starupEntrys.Add(localStartupEntry);

            FileInfo[] subFiles = dir.GetFiles();
            foreach (FileInfo f in subFiles)
            {
                string targetFile = f.FullName;
                if (Path.GetExtension(f.FullName).ToLower() == ".lnk")
                    targetFile = GetShortcutTarget(f.FullName);
                else if (Path.GetExtension(f.FullName).ToLower() == ".ini")
                    continue;
                AddStartupEntry(false,
                    f.Name,
                    GetFileDescription(targetFile),
                    GetFilePublisher(targetFile),
                    targetFile,
                    f.LastWriteTime);
                localStartupEntry.IsEmpty = false;

            }
        }

        private string GetShortcutTarget(string shortcutFilename)
        {
            if (System.IO.File.Exists(shortcutFilename) && Path.GetExtension(shortcutFilename).ToLower() == ".lnk")
            {
                WshShell shell = new WshShell();
                IWshShortcut IWshShortcut = (IWshShortcut)shell.CreateShortcut(shortcutFilename);
                return IWshShortcut.TargetPath;
            }
            return string.Empty;
        }

        private string GetFileDescription(string file)
        {
            if (!System.IO.File.Exists(file)) return "File does not exists.";

            try
            {
                // Get the file version for the file.
                FileVersionInfo myFileVersionInfo =
                    FileVersionInfo.GetVersionInfo(file);

                // Print the file description.
                return myFileVersionInfo.FileDescription;
            }
            catch
            {
                return "GetFileDescription() ERROR.";
            }

        }

        private string GetFilePublisher(string targetFile)
        {
            try
            {
                X509Certificate xcert = X509Certificate.CreateFromSignedFile(targetFile);
                string subject = xcert.Subject;

                int start = subject.IndexOf("CN=") + 3;
                int end = subject.IndexOf("=", start);
                while (subject[end--] != ',') ;

                string CN = subject.Substring(start, end - start + 1).Trim('\"');
                return CN;
            }
            catch
            {

            }
            return string.Empty;
        }

        private DateTime GetFileTime(string path)
        {
            FileInfo f = new FileInfo(path);
            return f.LastWriteTime;
        }


        private void AddStartupEntry(bool isMainEntry,
            string entryName,
            string desctiption,
            string publisher,
            string imagePath,
            DateTime time)
        {
            starupEntrys.Add(new StartupEntry(isMainEntry,
                entryName,
                desctiption,
                publisher,
                imagePath,
                time));
        }

        // Get the file path out of the outermost double quotation marks ""
        private string GetValueContentAsPath(string value)
        {
            string res = value;
            if (res[0] == '\"')
                res = value.Substring(1, value.LastIndexOf('\"') - 1);
            if (!res.EndsWith(".exe") && res.Contains(".exe"))
                res = res.Substring(0, res.IndexOf(".exe") + 4);
            return res;
        }

        // Get the file path under system path
        private string GetFilePathUnderSystemPath(string value)
        {
            string path = Environment.GetEnvironmentVariable("PATH");
            string[] systemPaths = path.Split(';');
            foreach(string leadingFolder in systemPaths)
            {
                string maybePath = leadingFolder + '\\' + value;
                if (System.IO.File.Exists(maybePath))
                    return maybePath;
            }
            return "";
        }
    }
}