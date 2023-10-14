using Youtube_DL_Frontend.Data;
using Youtube_DL_Frontend.Parsing;

namespace Youtube_DL_Frontend.Lambdas {
    internal class Main_Lambdas {
        public static Action<Data.PresetManager, Data.RuntimeData> link = async (preset, runtime) => {
            Console.Clear();
            Console.Write(runtime.currentMenu);
            runtime.link = InputHandler.inputValidate("Input a link to the file you wish to fetch");
            await Task.Delay(0);
        };
        public static Action<Data.PresetManager, Data.RuntimeData> filename = async (preset, runtime) => {
            Console.Clear();
            Console.Write(runtime.currentMenu);
            runtime.filename = InputHandler.inputValidate("Input a name for the output file (without the entension)");
            await Task.Delay(0);
        };
        public static Action<Data.PresetManager, Data.RuntimeData> batch = async (preset, runtime) => {
            Console.Clear();
            ExternalInterface.batchProcess(preset.getActive().database, runtime);
            await Task.Delay(0);
        };
        public static Action<Data.PresetManager, Data.RuntimeData> goOn = async (preset, runtime) => {
            if (runtime.filename != "NULL (Skipped)" && runtime.link != "NULL (Skipped)") {
                ExternalInterface.runYoutubeDL(preset.getActive().database, runtime);

            }
            else {
                Console.Write("\nOops, you need to specify the link and filename first.\nPRESS ENTER TO CONTINUE");
                Console.ReadLine();
            }
            await Task.Delay(0);
        };
        public static Action<Data.PresetManager, Data.RuntimeData> exit = (preset, runtime) => {
            Console.Clear();
            //Ascii Text, "Thank You"
            Interface.writeAscii(3);
            Console.WriteLine("This application is now exiting...\nThank you for using Youtube-DL Frontend!");
            Thread.Sleep(1000);
            System.Environment.Exit(0);
        };

        public static Action<Data.PresetManager, Data.RuntimeData> settingsMenu = (preset, runtime) => {
            int index = 0;
            for (int i = 0; i < runtime.parsers.Count(); i++) {
                if (runtime.parsers[i].parserName == Enums.parsers.settings) {
                    index = i;
                }
            }
            while (!runtime.goBack) {
                Console.Clear();
                Console.Write(runtime.parsers[index].generateMenu(preset, runtime) + "\n\n#\\> ");
                runtime.parsers[index].processInput(Console.ReadLine(), preset, runtime);
            }
            runtime.goBack = false;
        };

        public static Func<Data.PresetManager, Data.RuntimeData, string> linkDynamic = (preset, runtime) => {
            return runtime.link;
        };

        public static Func<Data.PresetManager, Data.RuntimeData, string> filenameDynamic = (preset, runtime) => {
            return runtime.filename;
        };

    }
}
