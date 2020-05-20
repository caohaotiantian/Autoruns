using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Autoruns
{
    class MuiStrng
    {
        // Snippet of ErrorCode.
        enum ErrorCode
        {
            Success = 0x0000,
            MoreData = 0x00EA
        }

        /// <summary>
        ///   Retrieves the multilingual string associated with the specified name. 
        ///   Returns null if the name/value pair does not exist in the registry.
        ///   The key must have been opened using 
        /// </summary>
        /// <param name = "key">The registry key to load the string from.</param>
        /// <param name = "name">The name of the string to load.</param>
        /// <returns>The language-specific string, or null if the name/value pair does not exist in the registry.</returns>
        public static string LoadMuiStringValue(RegistryKey key, string name)
        {
            const int initialBufferSize = 1024;
            var output = new StringBuilder(initialBufferSize);
            int requiredSize;
            IntPtr keyHandle = key.Handle.DangerousGetHandle();
            ErrorCode result = (ErrorCode)RegLoadMUIString(keyHandle, name, output, output.Capacity, out requiredSize, RegistryLoadMuiStringOptions.None, null);

            if (result == ErrorCode.MoreData)
            {
                output.EnsureCapacity(requiredSize);
                result = (ErrorCode)RegLoadMUIString(keyHandle, name, output, output.Capacity, out requiredSize, RegistryLoadMuiStringOptions.None, null);
            }

            return result == ErrorCode.Success ? output.ToString() : null;
        }

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
        public extern static int RegLoadMUIString(
            IntPtr registryKeyHandle, string value,
            StringBuilder outputBuffer, int outputBufferSize, out int requiredSize,
            RegistryLoadMuiStringOptions options, string path);

        /// <summary>
        ///   Determines the behavior of <see cref="RegLoadMUIString" />.
        /// </summary>
        [Flags]
        internal enum RegistryLoadMuiStringOptions : uint
        {
            None = 0,
            /// <summary>
            ///   The string is truncated to fit the available size of the output buffer. If this flag is specified, copiedDataSize must be NULL.
            /// </summary>
            Truncate = 1
        }
    }
}
