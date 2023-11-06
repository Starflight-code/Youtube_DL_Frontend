using System.Runtime.InteropServices;
using System.Text;

namespace Youtube_DL_Frontend.Data {
    internal class Statics {
        public static string buildPath(string windowsPath) {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                return windowsPath;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                return windowsPath.Replace('\\', '/');
            }
            else {
                return windowsPath; // not a supported OS for buildPath, we do not need total support
                                    // due to the current deployment plan (closed source, internal use only)
            }
        }

        public static string preProcessInput(string input) {
            return input.Trim().ToLower();
        }

        public static string generateList(string title, string[][] list) {
            int[] yMaxLengths = new int[list.Length];
            int[] xMaxLengths = new int[list[0].Length];
            for (int i = 0; i < list.Length; i++) {
                yMaxLengths[i] = 0;
                for (int j = 0; j < list[i].Length; j++) {
                    if (i == 0) {
                        xMaxLengths[j] = 0;
                    }
                    yMaxLengths[i] = Math.Max(yMaxLengths[i], list[i][j].Length);
                    xMaxLengths[j] = Math.Max(xMaxLengths[j], list[i][j].Length);
                }
            }
            StringBuilder sb = new StringBuilder();
            int buffer = 0;
            sb.Append(title);
            sb.Append('\n');
            for (int i = 0; i < list.Length; i++) {
                for (int j = 0; j < list[i].Length; j++) {
                    sb.Append(list[i][j]);
                    buffer = xMaxLengths[j] - list[i][j].Length;
                    for (int k = 0; k < buffer; k++) {
                        sb.Append(' ');
                    }
                    sb.Append(' ');
                }
                sb.Append('\n');
            }
            return sb.ToString();
        }
    }
}