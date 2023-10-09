using Youtube_DL_Frontend.Data;
using Youtube_DL_Frontend.Parsing;

namespace Youtube_DL_Frontend.Lambdas {
    internal class Settings_Lambdas {
        public static Action<Data.PresetManager, Data.RuntimeData> audioFormat = async (preset, runtime) => {
            Console.Clear();
            Console.Write(runtime.currentMenu);
            preset.getActive().database.format = InputHandler.askQuestion("Input a new audio format", ValidationLambdas.isNumber, invalidPrompt: "Your input is not a number, input a new audio format: ");
            await preset.getActive().database.updateSelf();
        };
        public static Action<Data.PresetManager, Data.RuntimeData> audioQuality = async (preset, runtime) => {
            Console.Clear();
            Console.Write(runtime.currentMenu);
            preset.getActive().database.audioQuality = InputHandler.askQuestion("Input a new audio quality", ValidationLambdas.isNumber, invalidPrompt: "Your input is not a number, input a new audio quality: ");
            await preset.getActive().database.updateSelf();
        };
        public static Action<Data.PresetManager, Data.RuntimeData> audioOutputFormat = async (preset, runtime) => {
            Console.Clear();
            Console.Write(runtime.currentMenu);
            preset.getActive().database.outputFormat = InputHandler.inputValidate("Input a new conversion format");
            await preset.getActive().database.updateSelf();
        };
        public static Action<Data.PresetManager, Data.RuntimeData> directory = async (preset, runtime) => {
            Console.Clear();
            Console.Write(runtime.currentMenu);
            preset.getActive().database.workingDirectory = InputHandler.inputValidate("Input a new directory path (A to autofill current path)");
            if (preset.getActive().database.workingDirectory == "A" || preset.getActive().database.workingDirectory == "a") { preset.getActive().database.workingDirectory = Directory.GetCurrentDirectory(); }
            if (!Directory.Exists(preset.getActive().database.workingDirectory)) {
                Console.WriteLine("Warning: The directory you entered does not currently exist. This script may not function properly.\nPRESS ENTER TO CONTINUE");
                Console.ReadLine();
            }
            await preset.getActive().database.updateSelf();
        };
        public static Action<Data.PresetManager, Data.RuntimeData> ffDirectory = async (preset, runtime) => {
            Console.Clear();
            Console.Write(runtime.currentMenu);
            preset.getActive().database.ffMpegDirectory = InputHandler.inputValidate("Input a new FF-Mpeg Path (A to autofill current path)");
            if (preset.getActive().database.ffMpegDirectory == "A" || preset.getActive().database.ffMpegDirectory == "a") { preset.getActive().database.ffMpegDirectory = Directory.GetCurrentDirectory(); }
            if (!File.Exists(preset.getActive().database.ffMpegDirectory + "\\ffmpeg.exe")) {
                Console.WriteLine("Warning: FFMPEG could not be located at this path. This script may not function properly.\nPRESS ENTER TO CONTINUE");
                Console.ReadLine();
            }
            runtime.database.ffMpegDirectory = preset.getActive().database.ffMpegDirectory;
            runtime.logDBUpdate();
            await preset.getActive().database.updateSelf();
        };
        public static Action<Data.PresetManager, Data.RuntimeData> youtubeDLP = async (preset, runtime) => {
            Console.Clear();
            Console.Write(runtime.currentMenu);
            switch (InputHandler.askQuestion("Input if you're using YT-DLP (y/n)", ValidationLambdas.yesOrNo)) {
                case "y":
                    preset.getActive().database.youtubeDLP = true;
                    runtime.updateYTDL(true);
                    break;
                case "n":
                    preset.getActive().database.youtubeDLP = false;
                    runtime.updateYTDL(false);
                    break;
                default:
                    throw new Exception("Validation for YT-DLP failed. ValidationLambda \"yesOrNo\" has failed!");
            }
            runtime.database.youtubeDLP = preset.getActive().database.youtubeDLP;
            runtime.logDBUpdate();
            await preset.getActive().database.updateSelf();
        };
        public static Action<Data.PresetManager, Data.RuntimeData> presetSwap = async (preset, runtime) => {
            Console.Clear();
            int index = 0;
            for (int i = 0; i < runtime.parsers.Count(); i++) {
                if (runtime.parsers[i].parserName == Enums.parsers.presets) {
                    index = i;
                }
            }
            Console.WriteLine(runtime.parsers[index].generateMenu(preset, runtime));
            int presetIndex = int.MaxValue;
            bool runOnce = true;
            while (preset.getPresets().Count < presetIndex || presetIndex < 1) {
                if (!runOnce) {
                    Console.Write("\nYour input is invalid, input a number between 1 and " + preset.getPresets().Count.ToString());
                }
                runOnce = false;
                presetIndex = int.Parse(InputHandler.askQuestion("Input the preset you'd like to swap to", ValidationLambdas.isNumber));
            }
            preset.switchActive(presetIndex - 1);
            await Task.Delay(0);
        };
        public static Func<Data.PresetManager, Data.RuntimeData, string> audioFormatDynamic = (preset, runtime) => {
            return preset.getActive().database.format;
        };

        public static Func<Data.PresetManager, Data.RuntimeData, string> audioQualityDynamic = (preset, runtime) => {
            return preset.getActive().database.audioQuality;
        };

        public static Func<Data.PresetManager, Data.RuntimeData, string> audioOutputFormatDynamic = (preset, runtime) => {
            return preset.getActive().database.outputFormat;
        };

        public static Func<Data.PresetManager, Data.RuntimeData, string> directoryDynamic = (preset, runtime) => {
            return preset.getActive().database.workingDirectory;
        };

        public static Func<Data.PresetManager, Data.RuntimeData, string> ffDirectoryDynamic = (preset, runtime) => {
            return preset.getActive().database.ffMpegDirectory;
        };

        public static Func<Data.PresetManager, Data.RuntimeData, string> youtubeDLPDynamic = (preset, runtime) => {
            if (!preset.getActive().database.youtubeDLP) {
                return "using Youtube-DL";
            }
            else {
                return "using YT-DLP";
            }
        };

        public static Action<Data.PresetManager, Data.RuntimeData> back = (preset, runtime) => {
            runtime.goBack = true;
        };

    }
}