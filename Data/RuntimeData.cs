using System.Runtime.InteropServices;
using System.Security.Authentication.ExtendedProtection;
using Youtube_DL_Frontend.Parsing;

namespace Youtube_DL_Frontend.Data
{
    internal class RuntimeData
    {
        public OSPlatform platform;
        public string yotutube_dl_executable;
        public string link;
        public string filename;
        public string currentMenu;
        public List<ParserInstance> parsers;
        public bool goBack;
        public bool updateGeneralDatabase;
        public GeneralDatabase database;
        public bool updatedPreset;
        public int updatedPresetToIndex;
        public RuntimeData()
        {
            yotutube_dl_executable = "";
            link = "";
            filename = "";
            currentMenu = "";
            parsers = new List<ParserInstance>();
            goBack = false;
            database = new GeneralDatabase();
        }

        public async void logDBUpdate()
        {
            updateGeneralDatabase = true;
            await database.updateSelf();
        }

        public void updateYTDL(bool usingYTDLP)
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