using IWshRuntimeLibrary;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace Autoruns
{
    internal class Startups
    {

        public ArrayList starupEntrys = new ArrayList();

        private string[] RegEntry =
        {
            "HKLM\\System\\CurrentControlSet\\Control\\Terminal Server\\Wds\\rdpwd\\StartupPrograms",
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
            RegistryKey subkey = key.OpenSubKey(regEntryName, true);

            starupEntrys.Add(new StartupEntry(true,
                keyPath,
                string.Empty,
                string.Empty,
                string.Empty,
                DateTime.Now));
            if (subkey == null)
                return;
            string[] valuenames = subkey.GetValueNames();
            foreach (string valuename in valuenames)
            {
                string value = subkey.GetValue(valuename).ToString();
                string target = value;
                if (value[0] == '\"')
                    target = value.Substring(1, value.LastIndexOf('\"') - 1);
                starupEntrys.Add(new StartupEntry(false, 
                    valuename,
                    GetFileDescription(target),
                    GetFilePublisher(target),
                    target,
                    GetFileTime(target)));
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

            // add folder directory
            starupEntrys.Add(new StartupEntry(true,
                dir.FullName,
                string.Empty,
                string.Empty,
                string.Empty,
                dir.LastWriteTime));

            FileInfo[] subFiles = dir.GetFiles();
            foreach (FileInfo f in subFiles)
            {
                string targetFile = f.FullName;
                if (Path.GetExtension(f.FullName).ToLower() == ".lnk")
                    targetFile = GetShortcutTarget(f.FullName);
                else if (Path.GetExtension(f.FullName).ToLower() == ".ini")
                    continue;
                starupEntrys.Add(new StartupEntry(false,
                    f.Name,
                    GetFileDescription(targetFile),
                    GetFilePublisher(targetFile),
                    targetFile,
                    f.LastWriteTime));
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

    }
}