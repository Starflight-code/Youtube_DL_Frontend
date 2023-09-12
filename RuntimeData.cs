using System.Runtime.InteropServices;

namespace Youtube_DL_Frontend
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
            bool usingYTDLP = true;
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
            link = "";
            filename = "";
            currentMenu = "";
        }
    }
}