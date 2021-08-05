using System.Runtime.InteropServices;
using System.Text;

namespace SapCo2.Wrapper.Exception
{
    public sealed class RfcLibraryNotFoundException : System.Exception
    {
        public RfcLibraryNotFoundException(System.Exception innerException) : base(BuildMessage(), innerException)
        {
        }

        private static string BuildMessage()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return BuildWindowsMessage();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return BuildMaxOsMessage();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return BuildLinuxMessage();

            return null;
        }

        private static string BuildWindowsMessage()
        {
            var message = new StringBuilder();
            message.AppendLine("The SAP RFC libraries were not found in the output folder or in a folder contained in the systems PATH environment variable.");
            message.AppendLine();
            message.AppendLine("Required files for Windows:");
            message.AppendLine("  sapnwrfc.dll");
            message.AppendLine("  icudtXX.dll");
            message.AppendLine("  icuinXX.dll");
            message.AppendLine("  icuucXX.dll");
            message.AppendLine();
            message.AppendLine("Also make sure the 64-bit version of the Visual C++ 2013 Redistributable is installed");
            return message.ToString();
        }

        private static string BuildMaxOsMessage()
        {
            var message = new StringBuilder();
            message.AppendLine("The SAP RFC libraries were not found in the output folder or in a folder contained in the systems DYLD_LIBRARY_PATH environment variable.");
            message.AppendLine();
            message.AppendLine("Required files for macOS:");
            message.AppendLine("  libsapnwrfc.dylib");
            message.AppendLine("  libicudata.XX.dylib");
            message.AppendLine("  libicui18n.XX.dylib");
            message.AppendLine("  libicuuc.XX.dylib");
            return message.ToString();
        }

        private static string BuildLinuxMessage()
        {
            var message = new StringBuilder();
            message.AppendLine("The SAP RFC libraries were not found in the output folder or in a folder contained in the systems LD_LIBRARY_PATH environment variable.");
            message.AppendLine();
            message.AppendLine("Required files for Linux:");
            message.AppendLine("  libsapnwrfc.so");
            message.AppendLine("  libicudata.so.XX");
            message.AppendLine("  libicui18n.so.XX");
            message.AppendLine("  libicuuc.so.XX");
            return message.ToString();
        }
    }
}
