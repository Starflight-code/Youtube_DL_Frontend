using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Youtube_DL_Frontend
{
    internal class Program
    {
        DatabaseObject data;
        RuntimeData runtimeData = new RuntimeData();
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
                File.Exists(runtimeData.yotutube_dl_executable),
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
            List<int> Errors = new List<int>();
            if (File.Exists(Constants._DATABASE_FILE) == false)
            {
                await data.updateSelf(true);
            }
            else
            {
                await data.populateSelf();
            }
            runtimeData.changeYTDLP(data.youtubeDLP);
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
            parser.generateMenu(data, runtimeData);

            while (true)
            {
                Console.Clear();
                Console.Write(runtimeData.currentMenu + "\n\n#\\> ");
                Thread.Sleep(500);
                parser.processMenuInput(Console.ReadLine(), data, runtimeData);
            };
        }
    }
}