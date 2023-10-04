using Youtube_DL_Frontend.Data;
using Youtube_DL_Frontend.Parsing;

namespace Youtube_DL_Frontend.Lambdas
{
    internal class Main_Lambdas
    {
        public static Action<Data.DatabaseObject, Data.RuntimeData> audioFormat = async (data, runtime) =>
        {
            Console.Clear();
            Console.Write(runtime.currentMenu);
            data.format = InputHandler.askQuestion("Input a new audio format", ValidationLambdas.isNumber, invalidPrompt: "Your input is not a number, input a new audio format: ");
            await data.updateSelf();
        };
        public static Action<Data.DatabaseObject, Data.RuntimeData> audioQuality = async (data, runtime) =>
        {
            Console.Clear();
            Console.Write(runtime.currentMenu);
            data.audioQuality = InputHandler.askQuestion("Input a new audio quality", ValidationLambdas.isNumber, invalidPrompt: "Your input is not a number, input a new audio quality: ");
            await data.updateSelf();
        };
        public static Action<Data.DatabaseObject, Data.RuntimeData> audioOutputFormat = async (data, runtime) =>
        {
            Console.Clear();
            Console.Write(runtime.currentMenu);
            data.outputFormat = InputHandler.inputValidate("Input a new conversion format");
            await data.updateSelf();
        };
        public static Action<Data.DatabaseObject, Data.RuntimeData> directory = async (data, runtime) =>
        {
            Console.Clear();
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
        public static Action<Data.DatabaseObject, Data.RuntimeData> ffDirectory = async (data, runtime) =>
        {
            Console.Clear();
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
        public static Action<Data.DatabaseObject, Data.RuntimeData> youtubeDLP = async (data, runtime) =>
        {
            Console.Clear();
            Console.Write(runtime.currentMenu);
            switch (InputHandler.askQuestion("Input if you're using YT-DLP (y/n)", ValidationLambdas.yesOrNo))
            {
                case "y":
                    data.youtubeDLP = true;
                    runtime.updateYTDL(true);
                    break;
                case "n":
                    data.youtubeDLP = false;
                    runtime.updateYTDL(false);
                    break;
                default:
                    throw new Exception("Validation for YT-DLP failed. ValidationLambda \"yesOrNo\" has failed!");
            }
            await data.updateSelf();
        };
        public static Action<Data.DatabaseObject, Data.RuntimeData> link = async (data, runtime) =>
        {
            Console.Clear();
            Console.Write(runtime.currentMenu);
            runtime.link = InputHandler.inputValidate("Input a link to the file you wish to fetch");
            await Task.Delay(0);
        };
        public static Action<Data.DatabaseObject, Data.RuntimeData> filename = async (data, runtime) =>
        {
            Console.Clear();
            Console.Write(runtime.currentMenu);
            runtime.filename = InputHandler.inputValidate("Input a name for the output file (without the entension)");
            await Task.Delay(0);
        };
        public static Action<Data.DatabaseObject, Data.RuntimeData> batch = async (data, runtime) =>
        {
            Console.Clear();
            ExternalInterface.batchProcess(data, runtime);
            await Task.Delay(0);
        };
        public static Action<Data.DatabaseObject, Data.RuntimeData> goOn = async (data, runtime) =>
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
        public static Action<Data.DatabaseObject, Data.RuntimeData> exit = (data, runtime) =>
        {
            Console.Clear();
            //Ascii Text, "Thank You"
            Interface.writeAscii(3);
            Console.WriteLine("This application is now exiting...\nThank you for using Youtube-DL Frontend!");
            Thread.Sleep(1000);
            System.Environment.Exit(0);
        };

        public static Action<Data.DatabaseObject, Data.RuntimeData> settingsMenu = (data, runtime) =>
        {
            int index = 0;
            for (int i = 0; i < runtime.parsers.Count(); i++)
            {
                if (runtime.parsers[i].parserName == Enums.parsers.settings)
                {
                    index = i;
                }
            }
            while (!runtime.goBack)
            {
                Console.Clear();
                Console.Write(runtime.parsers[index].generateMenu(data, runtime) + "\n\n#\\> ");
                runtime.parsers[index].processInput(Console.ReadLine(), data, runtime);
            }
            runtime.goBack = false;
        };

        public static Func<Data.DatabaseObject, Data.RuntimeData, string> audioFormatDynamic = (data, runtime) =>
        {
            return data.format;
        };

        public static Func<Data.DatabaseObject, Data.RuntimeData, string> audioQualityDynamic = (data, runtime) =>
        {
            return data.audioQuality;
        };

        public static Func<Data.DatabaseObject, Data.RuntimeData, string> audioOutputFormatDynamic = (data, runtime) =>
        {
            return data.outputFormat;
        };

        public static Func<Data.DatabaseObject, Data.RuntimeData, string> directoryDynamic = (data, runtime) =>
        {
            return data.workingDirectory;
        };

        public static Func<Data.DatabaseObject, Data.RuntimeData, string> ffDirectoryDynamic = (data, runtime) =>
        {
            return data.ffMpegDirectory;
        };

        public static Func<Data.DatabaseObject, Data.RuntimeData, string> linkDynamic = (data, runtime) =>
        {
            return runtime.link;
        };

        public static Func<Data.DatabaseObject, Data.RuntimeData, string> filenameDynamic = (data, runtime) =>
        {
            return runtime.filename;
        };

        public static Func<Data.DatabaseObject, Data.RuntimeData, string> youtubeDLPDynamic = (data, runtime) =>
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
