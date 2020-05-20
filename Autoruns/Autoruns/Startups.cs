using IWshRuntimeLibrary;
using Microsoft.Win32;
using Security.WinTrust;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
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
                if (targetFile.Contains("acpipmi"))
                {
                    string s = "df";
                }
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
                string err = e.ToString();
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

        static public bool CheckFile(string filename)
        {
            // check digital signature
            bool ret = WinTrust.VerifyEmbeddedSignature(filename);
            // do some other checks - for example verify the subject
            X509Certificate2 cert = new X509Certificate2(filename);
            return ret && cert.Verify();
        }

        [DllImport("wintrust.dll", ExactSpelling = true, SetLastError = false, CharSet = CharSet.Unicode)]
        static extern WinVerifyTrustResult WinVerifyTrust(
             [In] IntPtr hwnd,
             [In] [MarshalAs(UnmanagedType.LPStruct)] Guid pgActionID,
             [In] WinTrustData pWVTData
        );
    }
}

namespace Security.WinTrust
{
    using System;
    using System.Runtime.InteropServices;

    #region WinTrustData struct field enums
    enum WinTrustDataUIChoice : uint
    {
        All = 1,
        None = 2,
        NoBad = 3,
        NoGood = 4
    }

    enum WinTrustDataRevocationChecks : uint
    {
        None = 0x00000000,
        WholeChain = 0x00000001
    }

    enum WinTrustDataChoice : uint
    {
        File = 1,
        Catalog = 2,
        Blob = 3,
        Signer = 4,
        Certificate = 5
    }

    enum WinTrustDataStateAction : uint
    {
        Ignore = 0x00000000,
        Verify = 0x00000001,
        Close = 0x00000002,
        AutoCache = 0x00000003,
        AutoCacheFlush = 0x00000004
    }

    [FlagsAttribute]
    enum WinTrustDataProvFlags : uint
    {
        UseIe4TrustFlag = 0x00000001,
        NoIe4ChainFlag = 0x00000002,
        NoPolicyUsageFlag = 0x00000004,
        RevocationCheckNone = 0x00000010,
        RevocationCheckEndCert = 0x00000020,
        RevocationCheckChain = 0x00000040,
        RevocationCheckChainExcludeRoot = 0x00000080,
        SaferFlag = 0x00000100,        // Used by software restriction policies. Should not be used.
        HashOnlyFlag = 0x00000200,
        UseDefaultOsverCheck = 0x00000400,
        LifetimeSigningFlag = 0x00000800,
        CacheOnlyUrlRetrieval = 0x00001000,      // affects CRL retrieval and AIA retrieval
        DisableMD2andMD4 = 0x00002000      // Win7 SP1+: Disallows use of MD2 or MD4 in the chain except for the root
    }

    enum WinTrustDataUIContext : uint
    {
        Execute = 0,
        Install = 1
    }
    #endregion

    #region WinTrust structures
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    class WinTrustFileInfo
    {
        UInt32 StructSize = (UInt32)Marshal.SizeOf(typeof(WinTrustFileInfo));
        IntPtr pszFilePath;                     // required, file name to be verified
        IntPtr hFile = IntPtr.Zero;             // optional, open handle to FilePath
        IntPtr pgKnownSubject = IntPtr.Zero;    // optional, subject type if it is known

        public WinTrustFileInfo(String _filePath)
        {
            pszFilePath = Marshal.StringToCoTaskMemAuto(_filePath);
        }
        public void Dispose()
        {
            if (pszFilePath != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(pszFilePath);
                pszFilePath = IntPtr.Zero;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    class WinTrustData
    {
        UInt32 StructSize = (UInt32)Marshal.SizeOf(typeof(WinTrustData));
        IntPtr PolicyCallbackData = IntPtr.Zero;
        IntPtr SIPClientData = IntPtr.Zero;
        // required: UI choice
        WinTrustDataUIChoice UIChoice = WinTrustDataUIChoice.None;
        // required: certificate revocation check options
        WinTrustDataRevocationChecks RevocationChecks = WinTrustDataRevocationChecks.None;
        // required: which structure is being passed in?
        WinTrustDataChoice UnionChoice = WinTrustDataChoice.File;
        // individual file
        IntPtr FileInfoPtr;
        WinTrustDataStateAction StateAction = WinTrustDataStateAction.Ignore;
        IntPtr StateData = IntPtr.Zero;
        String URLReference = null;
        WinTrustDataProvFlags ProvFlags = WinTrustDataProvFlags.RevocationCheckChainExcludeRoot;
        WinTrustDataUIContext UIContext = WinTrustDataUIContext.Execute;

        // constructor for silent WinTrustDataChoice.File check
        public WinTrustData(WinTrustFileInfo _fileInfo)
        {
            // On Win7SP1+, don't allow MD2 or MD4 signatures
            if ((Environment.OSVersion.Version.Major > 6) ||
                ((Environment.OSVersion.Version.Major == 6) && (Environment.OSVersion.Version.Minor > 1)) ||
                ((Environment.OSVersion.Version.Major == 6) && (Environment.OSVersion.Version.Minor == 1) && !String.IsNullOrEmpty(Environment.OSVersion.ServicePack)))
            {
                ProvFlags |= WinTrustDataProvFlags.DisableMD2andMD4;
            }

            WinTrustFileInfo wtfiData = _fileInfo;
            FileInfoPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(WinTrustFileInfo)));
            Marshal.StructureToPtr(wtfiData, FileInfoPtr, false);
        }
        public void Dispose()
        {
            if (FileInfoPtr != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(FileInfoPtr);
                FileInfoPtr = IntPtr.Zero;
            }
        }
    }
    #endregion

    enum WinVerifyTrustResult : uint
    {
        Success = 0,
        ProviderUnknown = 0x800b0001,           // Trust provider is not recognized on this system
        ActionUnknown = 0x800b0002,         // Trust provider does not support the specified action
        SubjectFormUnknown = 0x800b0003,        // Trust provider does not support the form specified for the subject
        SubjectNotTrusted = 0x800b0004,         // Subject failed the specified verification action
        FileNotSigned = 0x800B0100,         // TRUST_E_NOSIGNATURE - File was not signed
        SubjectExplicitlyDistrusted = 0x800B0111,   // Signer's certificate is in the Untrusted Publishers store
        SignatureOrFileCorrupt = 0x80096010,    // TRUST_E_BAD_DIGEST - file was probably corrupt
        SubjectCertExpired = 0x800B0101,        // CERT_E_EXPIRED - Signer's certificate was expired
        SubjectCertificateRevoked = 0x800B010C,     // CERT_E_REVOKED Subject's certificate was revoked
        UntrustedRoot = 0x800B0109          // CERT_E_UNTRUSTEDROOT - A certification chain processed correctly but terminated in a root certificate that is not trusted by the trust provider.
    }

    sealed class WinTrust
    {
        private static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);
        // GUID of the action to perform
        private const string WINTRUST_ACTION_GENERIC_VERIFY_V2 = "{00AAC56B-CD44-11d0-8CC2-00C04FC295EE}";

        [DllImport("wintrust.dll", ExactSpelling = true, SetLastError = false, CharSet = CharSet.Unicode)]
        static extern WinVerifyTrustResult WinVerifyTrust(
            [In] IntPtr hwnd,
            [In] [MarshalAs(UnmanagedType.LPStruct)] Guid pgActionID,
            [In] WinTrustData pWVTData
        );

        // call WinTrust.WinVerifyTrust() to check embedded file signature
        public static bool VerifyEmbeddedSignature(string fileName)
        {
            WinTrustFileInfo wtfi = new WinTrustFileInfo(fileName);
            WinTrustData wtd = new WinTrustData(wtfi);
            Guid guidAction = new Guid(WINTRUST_ACTION_GENERIC_VERIFY_V2);
            WinVerifyTrustResult result = WinVerifyTrust(INVALID_HANDLE_VALUE, guidAction, wtd);
            bool ret = (result == WinVerifyTrustResult.Success);
            wtfi.Dispose();
            wtd.Dispose();
            return ret;
        }
        private WinTrust() { }
    }
}