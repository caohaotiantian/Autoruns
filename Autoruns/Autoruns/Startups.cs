using IWshRuntimeLibrary;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
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
                X509Certificate2 xcert = new X509Certificate2(targetFile);
                string subject = xcert.Subject;

                int start = subject.IndexOf("CN=") + 3;
                int end = subject.IndexOf("=", start);
                while (subject[end--] != ',') ;

                string CN = subject.Substring(start, end - start + 1).Trim('\"');
                return CN;
            }
            catch (Exception e)
            {

            }

            return "(Signature not embedded)";
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

        // Get the normal file path out of the Registry Key Value Content
        protected string GetValueContentAsPath(string value)
        {
            string res = value;
            // strip from double quote
            if (res != string.Empty && res[0] == '\"')
                res = value.Substring(1, value.LastIndexOf('\"') - 1);
            
            // strip parameters at the end
            if (!res.EndsWith(".exe") && res.Contains(".exe"))
                res = res.Substring(0, res.IndexOf(".exe") + 4);
            
            // strip strange characters in the beginning
            if (res.StartsWith("\\??\\")) res = res.Substring(4);

            if (res.StartsWith(@"\")) res = res.Substring(1);
            if (res.IndexOf(@"SystemRoot", StringComparison.OrdinalIgnoreCase) == 0)
            {
                string SystemRoot = Environment.GetEnvironmentVariable("SystemRoot");
                res = SystemRoot + res.Substring(res.IndexOf(@"\"));
            }

            // deal with environment variables
            if (res.StartsWith(@"%"))
            {
                int index = res.IndexOf(@"%", 1);
                string envVar = res.Substring(1, index - 1);
                string envVarValue = Environment.GetEnvironmentVariable(envVar);
                res = envVarValue + res.Substring(index + 1);
            }

            return res;
        }

        // Get the file path under system path
        protected string GetFilePathUnderSystemPath(string value)
        {
            if (System.IO.File.Exists(value)) return value;
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

        public static bool IsSigned(string filePath)
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));

            var file = new WINTRUST_FILE_INFO();
            file.cbStruct = Marshal.SizeOf(typeof(WINTRUST_FILE_INFO));
            file.pcwszFilePath = filePath;

            var data = new WINTRUST_DATA();
            data.cbStruct = Marshal.SizeOf(typeof(WINTRUST_DATA));
            data.dwUIChoice = WTD_UI_NONE;
            data.dwUnionChoice = WTD_CHOICE_FILE;
            data.fdwRevocationChecks = WTD_REVOKE_NONE;
            data.pFile = Marshal.AllocHGlobal(file.cbStruct);
            Marshal.StructureToPtr(file, data.pFile, false);

            int hr;
            try
            {
                hr = WinVerifyTrust(INVALID_HANDLE_VALUE, WINTRUST_ACTION_GENERIC_VERIFY_V2, ref data);
            }
            finally
            {
                Marshal.FreeHGlobal(data.pFile);
            }
            return hr == 0;
        }

        [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct WINTRUST_FILE_INFO
        {
            public int cbStruct;
            public string pcwszFilePath;
            public IntPtr hFile;
            public IntPtr pgKnownSubject;
        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        private struct WINTRUST_DATA
        {
            public int cbStruct;
            public IntPtr pPolicyCallbackData;
            public IntPtr pSIPClientData;
            public int dwUIChoice;
            public int fdwRevocationChecks;
            public int dwUnionChoice;
            public IntPtr pFile;
            public int dwStateAction;
            public IntPtr hWVTStateData;
            public IntPtr pwszURLReference;
            public int dwProvFlags;
            public int dwUIContext;
            public IntPtr pSignatureSettings;
        }

        private const int WTD_UI_NONE = 2;
        private const int WTD_REVOKE_NONE = 0;
        private const int WTD_CHOICE_FILE = 1;
        private static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);
        private static readonly Guid WINTRUST_ACTION_GENERIC_VERIFY_V2 = new Guid("{00AAC56B-CD44-11d0-8CC2-00C04FC295EE}");

        [DllImport("wintrust.dll")]
        private static extern int WinVerifyTrust(IntPtr hwnd, [MarshalAs(UnmanagedType.LPStruct)] Guid pgActionID, ref WINTRUST_DATA pWVTData);
    }
}