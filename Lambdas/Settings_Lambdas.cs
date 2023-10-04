using Youtube_DL_Frontend.Data;
using Youtube_DL_Frontend.Parsing;

namespace Youtube_DL_Frontend.Lambdas
{
    internal class Settings_Lambdas
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
            runtime.database.ffMpegDirectory = data.ffMpegDirectory;
            runtime.logDBUpdate();
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
            runtime.database.youtubeDLP = data.youtubeDLP;
            runtime.logDBUpdate();
            await data.updateSelf();
        };
        public static Action<Data.DatabaseObject, Data.RuntimeData> presetSwap = async (data, runtime) =>
        {
            Console.Clear();
            int index = 0;
            for (int i = 0; i < runtime.parsers.Count(); i++)
            {
                if (runtime.parsers[i].parserName == Enums.parsers.presets)
                {
                    index = i;
                }
            }
            Console.WriteLine(runtime.parsers[index].generateMenu(data, runtime));
            runtime.updatedPresetIndex = int.Parse(InputHandler.askQuestion("Input the preset you'd like to swap to", ValidationLambdas.isNumber));
            runtime.updatedPreset = true;
            runtime.goBack = true;
            await Task.Delay(0);
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

        public static Action<Data.DatabaseObject, Data.RuntimeData> back = (data, runtime) =>
        {
            runtime.goBack = true;
        };

    }
}