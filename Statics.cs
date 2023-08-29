using System.Runtime.InteropServices;

namespace Youtube_DL_Frontnend {
    internal class Statics {
        public static string buildPath(string windowsPath) {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                return windowsPath;
            } else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                return windowsPath.Replace('\\', '/');
            } else {
                return windowsPath; // not a supported OS for buildPath, we do not need total support
                                    // due to the current deployment plan (closed source, internal use only)
            }
        }

    }
}