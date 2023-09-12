using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Youtube_DL_Frontend
{
    internal class Program
    {
        //ValidationLambdas lambdas = new ValidationLambdas();
        DatabaseObject data;
        RuntimeData runtimeData = new RuntimeData(false);
        InputHandler inputhandle = new InputHandler();
        public Program()
        {
            data = new DatabaseObject();
        }

        void generateDatabase()
        {
            data = new DatabaseObject();
        }
        bool checkFiles(string ff, string DATABASE_FILE, bool hold_up_execution = true, bool showGUI = true)
        {
            List<bool> exists;
            if (runtimeData.platform == OSPlatform.Linux)
            {
                exists = new List<bool>
                {
                true,
                File.Exists(ff),
                File.Exists(DATABASE_FILE)
                };
            }
            else
            {
                exists = new List<bool>
                {
                File.Exists(Constants._YOUTUBE_DL_EXECUTABLE),
                File.Exists($"{ff}\\ffmpeg.exe"),
                File.Exists(DATABASE_FILE)
                };
            }

            exists.Add(!exists.Contains(false));
            if (showGUI)
            {
                List<string> exists_UI = new List<string>();

                for (int i = 0; i < (exists.Count - 1); i++)
                {
                    if (!exists[i])
                    { // If false 
                        exists_UI.Add("Error!");
                    }
                    else
                    { // If true
                        exists_UI.Add("Located");
                    }
                }
                if (!exists[3])
                { // If false 
                    exists_UI.Add("Error!");
                }
                else
                { // If true
                    exists_UI.Add("Good to go!");
                }

                string[][] list = {
                    new string[]{"Youtube-DL:", exists_UI[0]},
                    new string[]{"FFMPEG:", exists_UI[1]},
                    new string[]{"Database:", exists_UI[2]},
                    new string[]{"Result:", exists_UI[3]}
                };
                if (runtimeData.platform == OSPlatform.Linux)
                {
                    list[0][1] = "Skipped (check not supported on Linux)";
                }
                string printOut = Statics.generateList("Checking core files: ", list);


                /*Console.WriteLine(
                    $"Checking core files: " +
                    $"\nYoutube-DL: " + exists_UI[0] +
                    $"\nFFMPEG:     " + exists_UI[1] +
                    $"\nDatabase:   " + exists_UI[2] +
                    $"\nResult:     " + exists_UI[3]);*/
                Console.WriteLine(printOut);
                if (hold_up_execution)
                {
                    Console.WriteLine($"\n-----ENTER TO CONTINUE-----");
                    Console.ReadLine();
                }
            }
            return exists[3];
        }
        static void logErrors(List<int> Errors)
        {
            Console.WriteLine();
            for (int i = 0; i < Errors.Count(); i++)
            {
                switch (Errors[i])
                {
                    case 1:
                        Console.WriteLine("Database was reset due to corruption. You may need to re-insert the correct values.");
                        break;
                    case 2:
                        Console.WriteLine("File errors were detected. Make sure your core files are present and configuration is correct.");
                        break;
                }
            }
            Console.WriteLine();
        }

        static void Main(string[] args) => new Program().MainAsync(args);

        public async void MainAsync(string[] args)
        {
            CommandParser parser = new CommandParser();
            parser.registerMenuCommand("Audio Format", Lambdas.audioFormat, Lambdas.audioFormatDynamic);
            parser.registerMenuCommand("Audio Quality", Lambdas.audioQuality, Lambdas.audioQualityDynamic);
            parser.registerMenuCommand("Audio Output Format", Lambdas.audioOutputFormat, Lambdas.audioOutputFormatDynamic);
            parser.registerMenuCommand("Directory", Lambdas.directory, Lambdas.directoryDynamic);
            parser.registerMenuCommand("FFMPEG Directory", Lambdas.ffDirectory, Lambdas.ffDirectoryDynamic);
            parser.registerMenuCommand("Using", Lambdas.youtubeDLP, Lambdas.youtubeDLPDynamic);
            parser.registerMenuCommand("Link", Lambdas.link, Lambdas.linkDynamic);
            parser.registerMenuCommand("Filename", Lambdas.filename, Lambdas.filenameDynamic);
            parser.registerMenuCommand("Batch", Lambdas.batch);
            parser.registerMenuCommand("Continue", Lambdas.goOn);
            parser.registerMenuCommand("Exit", Lambdas.exit);
            /*parser.registerAlias("Audio Format", "1", CommandParser.commandScope.menu);
            parser.registerAlias("Audio Quality", "2", CommandParser.commandScope.menu);
            parser.registerAlias("Audio Output Format", "3", CommandParser.commandScope.menu);
            parser.registerAlias("Directory", "4", CommandParser.commandScope.menu);
            parser.registerAlias("FFMPEG Directory", "5", CommandParser.commandScope.menu);
            parser.registerAlias("Link", "6", CommandParser.commandScope.menu);
            parser.registerAlias("Filename", "7", CommandParser.commandScope.menu);
            parser.registerAlias("Batch", "8", CommandParser.commandScope.menu);
            parser.registerAlias("Continue", "9", CommandParser.commandScope.menu);
            parser.registerAlias("Exit", "10", CommandParser.commandScope.menu);*/
            //DataStructures.YoutubeDLParamInfo paramData = new DataStructures.YoutubeDLParamInfo();
            List<int> Errors = new List<int>();
            if (File.Exists(Constants._DATABASE_FILE) == false)
            {
                await data.updateSelf();
            }
            else
            {
                await data.populateSelf();
            }

            Console.Clear();
            //Ascii Text, "Welcome"
            Interface.writeAscii(4);
            if (!checkFiles(data.ffMpegDirectory, Constants._DATABASE_FILE, showGUI: false))
            {
                Errors.Add(2);
                checkFiles(data.ffMpegDirectory, Constants._DATABASE_FILE, hold_up_execution: false);
            }
            if (Errors.Count() > 0) { logErrors(Errors); }
            Console.Write("We have a few initialization questions before you can begin.\n");
            runtimeData.link = InputHandler.inputValidate("Input a link to the file you wish to fetch");
            if (runtimeData.link.ToLower() is not ("s" or "skip"))
            { // for quick bypass in the case of batch processing
                Console.Write("Input \"" + runtimeData.link + "\" Accepted!");
                Thread.Sleep(400);
                Console.Clear();
                //Ascii Text, "Welcome"
                Interface.writeAscii(4);
                Console.Write("We have a few initialization questions before you can begin.\n");
                runtimeData.filename = InputHandler.inputValidate("Input a name for the file output without the entension");
                Console.Write("Input \"" + runtimeData.filename + "\" Accepted!");
                Thread.Sleep(400); //Gives visual feedback to user
            }
            else
            {
                runtimeData.link = "NULL (Skipped)";
                runtimeData.filename = "NULL (Skipped)";
            }
            runtimeData.changeYTDLP(data.youtubeDLP);
            parser.generateMenu(data, runtimeData);
            while (true)
            {
                //Enums.commandToExecute? input;

                while (true)
                {
                    Console.Clear();
                    //Interface.writeAscii(1);
                    //Interface.writeGUI(data, runtimeData.link, runtimeData.filename, true);
                    Console.Write(runtimeData.currentMenu + "\n\n#\\> ");
                    //Console.Write("\n#\\> ");
                    Thread.Sleep(500);
                    parser.processMenuInput(Console.ReadLine(), data, runtimeData);
                    /*input = inputhandle.handleCommand(Console.ReadLine());
                    switch (input) {

                        case Enums.commandToExecute.audioFormat:
                            Console.Clear();
                            Interface.writeGUI(data, runtimeData.link, runtimeData.filename, false);
                            data.audioFormat = InputHandler.askQuestion("Input a new audio format", ValidationLambdas.isNumber, invalidPrompt: "Your input is not a number, input a new audio format: ");
                            await data.updateSelf();
                            break;

                        case Enums.commandToExecute.audioQuality:
                            Console.Clear();
                            Interface.writeGUI(data, runtimeData.link, runtimeData.filename, false);
                            data.audioQuality = InputHandler.askQuestion("Input a new audio quality", ValidationLambdas.isNumber, invalidPrompt: "Your input is not a number, input a new audio quality: ");
                            await data.updateSelf();
                            break;

                        case Enums.commandToExecute.audioOutputFormat:
                            Console.Clear();
                            Interface.writeGUI(data, runtimeData.link, runtimeData.filename, false);
                            data.audioOutputFormat = InputHandler.inputValidate("Input a new conversion format");
                            await data.updateSelf();
                            break;

                        case Enums.commandToExecute.directory:
                            Console.Clear();
                            Interface.writeGUI(data, runtimeData.link, runtimeData.filename, false);
                            data.workingDirectory = InputHandler.inputValidate("Input a new directory path (A to autofill current path)");
                            if (data.workingDirectory == "A" || data.workingDirectory == "a") { data.workingDirectory = Directory.GetCurrentDirectory(); }
                            if (!Directory.Exists(data.workingDirectory)) {
                                Console.WriteLine("Warning: The directory you entered does not currently exist. This script may not function properly.\nPRESS ENTER TO CONTINUE");
                                Console.ReadLine();
                            }
                            await data.updateSelf();
                            break;

                        case Enums.commandToExecute.ffDirectory:
                            Console.Clear();
                            Interface.writeGUI(data, runtimeData.link, runtimeData.filename, false);
                            data.ffMpegDirectory = InputHandler.inputValidate("Input a new FF-Mpeg Path (A to autofill current path)");
                            if (data.ffMpegDirectory == "A" || data.ffMpegDirectory == "a") { data.ffMpegDirectory = Directory.GetCurrentDirectory(); }
                            if (!File.Exists(data.ffMpegDirectory + "\\ffmpeg.exe")) {
                                Console.WriteLine("Warning: FFMPEG could not be located at this path. This script may not function properly.\nPRESS ENTER TO CONTINUE");
                                Console.ReadLine();
                            }
                            await data.updateSelf();
                            break;

                        case Enums.commandToExecute.link:
                            Console.Clear();
                            Interface.writeGUI(data, runtimeData.link, runtimeData.filename, false);
                            runtimeData.link = InputHandler.inputValidate("Input a link to the file you wish to fetch");
                            break;

                        case Enums.commandToExecute.filename:
                            Console.Clear();
                            Interface.writeGUI(data, runtimeData.link, runtimeData.filename, false);
                            runtimeData.filename = InputHandler.inputValidate("Input a name for the output file (without the entension)");
                            break;

                        case Enums.commandToExecute.batch:
                            Console.Clear();
                            ExternalInterface.batchProcess(data, runtimeData);
                            break;

                        case Enums.commandToExecute.goOn:
                            if (runtimeData.filename != "NULL (Skipped)" && runtimeData.link != "NULL (Skipped)") {
                                ExternalInterface.runYoutubeDL(data, runtimeData);

                            } else {
                                Console.Write("\nOops, you need to specify the link and filename first.\nPRESS ENTER TO CONTINUE");
                                Console.ReadLine();
                            }
                            break;

                        case Enums.commandToExecute.exit:
                            Console.Clear();
                            //Ascii Text, "Thank You"
                            Interface.writeAscii(3);
                            Console.WriteLine("This application is now exiting...\nThank you for using Youtube-DL Frontend!");
                            Thread.Sleep(1000);
                            System.Environment.Exit(1);
                            break;
                    };*/

                };
            }
        }
    }
}