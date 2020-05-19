using IWshRuntimeLibrary;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace Autoruns
{
    internal class Startups
    {
        public ArrayList starupEntrys = new ArrayList();

        public ListView listView { get; set; }

        protected string GetShortcutTarget(string shortcutFilename)
        {
            if (System.IO.File.Exists(shortcutFilename) && Path.GetExtension(shortcutFilename).ToLower() == ".lnk")
            {
                WshShell shell = new WshShell();
                IWshShortcut IWshShortcut = (IWshShortcut)shell.CreateShortcut(shortcutFilename);
                return IWshShortcut.TargetPath;
            }
            return string.Empty;
        }

        protected string GetFileDescription(string file)
        {
            if (!System.IO.File.Exists(file)) return string.Empty;

            // Get the file version for the file.
            FileVersionInfo myFileVersionInfo =
                FileVersionInfo.GetVersionInfo(file);

            // Print the file description.
            return myFileVersionInfo.FileDescription;

        }

        protected string GetFilePublisher(string targetFile)
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

        protected DateTime GetFileTime(string path)
        {
            FileInfo f = new FileInfo(path);
            return f.LastWriteTime;
        }


        protected void AddStartupEntry(bool isMainEntry,
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
        protected string GetValueContentAsPath(string value)
        {
            string res = value;
            if (res != string.Empty && res[0] == '\"')
                res = value.Substring(1, value.LastIndexOf('\"') - 1);
            if (!res.EndsWith(".exe") && res.Contains(".exe"))
                res = res.Substring(0, res.IndexOf(".exe") + 4);
            if (res.Contains("?")) res = string.Empty;
            if (res != string.Empty && res[0] == '%')
            {
                int secondPercent = res.IndexOf('%', 1);

            }
            return res;
        }

        // Get the file path under system path
        protected string GetFilePathUnderSystemPath(string value)
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