namespace Youtube_DL_Frontend
{
    internal class Lambdas
    {
        public static Action<DatabaseObject, RuntimeData> audioFormat = async (data, runtime) =>
        {
            Console.Clear();
            //Interface.writeGUI(data, runtime.link, runtime.filename, false);
            Console.Write(runtime.currentMenu);
            data.audioFormat = InputHandler.askQuestion("Input a new audio format", ValidationLambdas.isNumber, invalidPrompt: "Your input is not a number, input a new audio format: ");
            await data.updateSelf();
        };
        public static Action<DatabaseObject, RuntimeData> audioQuality = async (data, runtime) =>
        {
            Console.Clear();
            //Interface.writeGUI(data, runtime.link, runtime.filename, false);
            Console.Write(runtime.currentMenu);
            data.audioQuality = InputHandler.askQuestion("Input a new audio quality", ValidationLambdas.isNumber, invalidPrompt: "Your input is not a number, input a new audio quality: ");
            await data.updateSelf();
        };
        public static Action<DatabaseObject, RuntimeData> audioOutputFormat = async (data, runtime) =>
        {
            Console.Clear();
            //Interface.writeGUI(data, runtime.link, runtime.filename, false);
            Console.Write(runtime.currentMenu);
            data.audioOutputFormat = InputHandler.inputValidate("Input a new conversion format");
            await data.updateSelf();
        };
        public static Action<DatabaseObject, RuntimeData> directory = async (data, runtime) =>
        {
            Console.Clear();
            //Interface.writeGUI(data, runtime.link, runtime.filename, false);
            Console.Write(runtime.currentMenu);
            data.workingDirectory = InputHandler.inputValidate("Input a new directory path (A to autofill current path)");
            if (data.workingDirectory == "A" || data.workingDirectory == "a") { data.workingDirectory = Directory.GetCurrentDirectory(); }
            if (!Directory.Exists(data.workingDirectory))
            {
                Console.WriteLine("Warning: The directory you entered does not currently exist. This script may not function properly.\nPRESS ENTER TO CONTINUE");
                Console.ReadLine();
            }
            await data.updateSelf();
        };
        public static Action<DatabaseObject, RuntimeData> ffDirectory = async (data, runtime) =>
        {
            Console.Clear();
            //Interface.writeGUI(data, runtime.link, runtime.filename, false);
            Console.Write(runtime.currentMenu);
            data.ffMpegDirectory = InputHandler.inputValidate("Input a new FF-Mpeg Path (A to autofill current path)");
            if (data.ffMpegDirectory == "A" || data.ffMpegDirectory == "a") { data.ffMpegDirectory = Directory.GetCurrentDirectory(); }
            if (!File.Exists(data.ffMpegDirectory + "\\ffmpeg.exe"))
            {
                Console.WriteLine("Warning: FFMPEG could not be located at this path. This script may not function properly.\nPRESS ENTER TO CONTINUE");
                Console.ReadLine();
            }
            await data.updateSelf();
        };
        public static Action<DatabaseObject, RuntimeData> youtubeDLP = async (data, runtime) =>
        {
            Console.Clear();
            //Interface.writeGUI(data, runtime.link, runtime.filename, false);
            Console.Write(runtime.currentMenu);
            switch (InputHandler.askQuestion("Input if you're using YT-DLP (y/n)", ValidationLambdas.yesOrNo))
            {
                case "y":
                    data.youtubeDLP = true;
                    break;
                case "n":
                    data.youtubeDLP = false;
                    break;
                default:
                    throw new Exception("Validation for YT-DLP failed. ValidationLambda \"yesOrNo\" has failed!");
            }
            await data.updateSelf();
        };
        public static Action<DatabaseObject, RuntimeData> link = async (data, runtime) =>
        {
            Console.Clear();
            //Interface.writeGUI(data, runtime.link, runtime.filename, false);
            Console.Write(runtime.currentMenu);
            runtime.link = InputHandler.inputValidate("Input a link to the file you wish to fetch");
            await Task.Delay(0);
        };
        public static Action<DatabaseObject, RuntimeData> filename = async (data, runtime) =>
        {
            Console.Clear();
            //Interface.writeGUI(data, runtime.link, runtime.filename, false);
            Console.Write(runtime.currentMenu);
            runtime.filename = InputHandler.inputValidate("Input a name for the output file (without the entension)");
            await Task.Delay(0);
        };
        public static Action<DatabaseObject, RuntimeData> batch = async (data, runtime) =>
        {
            Console.Clear();
            ExternalInterface.batchProcess(data, runtime);
            await Task.Delay(0);
        };
        public static Action<DatabaseObject, RuntimeData> goOn = async (data, runtime) =>
        {
            if (runtime.filename != "NULL (Skipped)" && runtime.link != "NULL (Skipped)")
            {
                ExternalInterface.runYoutubeDL(data, runtime);

            }
            else
            {
                Console.Write("\nOops, you need to specify the link and filename first.\nPRESS ENTER TO CONTINUE");
                Console.ReadLine();
            }
            await Task.Delay(0);
        };
        public static Action<DatabaseObject, RuntimeData> exit = (data, runtime) =>
        {
            Console.Clear();
            //Ascii Text, "Thank You"
            Interface.writeAscii(3);
            Console.WriteLine("This application is now exiting...\nThank you for using Youtube-DL Frontend!");
            Thread.Sleep(1000);
            System.Environment.Exit(0);
        };

        public static Func<DatabaseObject, RuntimeData, string> audioFormatDynamic = (data, runtime) =>
        {
            return data.audioFormat;
        };

        public static Func<DatabaseObject, RuntimeData, string> audioQualityDynamic = (data, runtime) =>
        {
            return data.audioQuality;
        };

        public static Func<DatabaseObject, RuntimeData, string> audioOutputFormatDynamic = (data, runtime) =>
        {
            return data.audioOutputFormat;
        };

        public static Func<DatabaseObject, RuntimeData, string> directoryDynamic = (data, runtime) =>
        {
            return data.workingDirectory;
        };

        public static Func<DatabaseObject, RuntimeData, string> ffDirectoryDynamic = (data, runtime) =>
        {
            return data.ffMpegDirectory;
        };

        public static Func<DatabaseObject, RuntimeData, string> linkDynamic = (data, runtime) =>
        {
            return runtime.link;
        };

        public static Func<DatabaseObject, RuntimeData, string> filenameDynamic = (data, runtime) =>
        {
            return runtime.filename;
        };

        public static Func<DatabaseObject, RuntimeData, string> youtubeDLPDynamic = (data, runtime) =>
        {
            if (!data.youtubeDLP)
            {
                return "using Youtube-DL";
            }
            else
            {
                return "using YT-DLP";
            }
        };

    }
}
