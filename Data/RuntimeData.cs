using System.Runtime.InteropServices;
using System.Security.Authentication.ExtendedProtection;

namespace Youtube_DL_Frontend.Data
{
    internal class RuntimeData
    {
        public OSPlatform platform;
        public string yotutube_dl_executable;
        public string link;
        public string filename;
        public string currentMenu;
        public RuntimeData()
        {
            yotutube_dl_executable = "";
            link = "";
            filename = "";
            currentMenu = "";
        }

        public void changeYTDLP(bool usingYTDLP)
        {
            string youtubeDL;
            if (usingYTDLP)
            {
                youtubeDL = "yt-dlp";
            }
            else
            {
                youtubeDL = "youtube-dl";
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                platform = OSPlatform.Windows;
                yotutube_dl_executable = Statics.buildPath(Directory.GetCurrentDirectory() + "\\" + youtubeDL + ".exe");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                platform = OSPlatform.Linux;
                yotutube_dl_executable = youtubeDL;
            }
            else
            {
                yotutube_dl_executable = "";
                platform = OSPlatform.Create("Invalid");
            }
        }
    }
}