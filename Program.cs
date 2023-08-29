using System.Diagnostics;

namespace Youtube_DL_Frontnend {
    internal class Program {
        ValidationLambdas lambdas = new ValidationLambdas();
        DatabaseObject data;
        public Program() {
            data = new DatabaseObject();
        }

        void generateDatabase() {
            data = new DatabaseObject();
        }
        static void writeGUI(DatabaseObject data, string link, string filename, bool appendWritingIndicator = true) {
            //Ascii Text, "Configuration"
            writeAscii(1);
            Console.Write($"1: Audio Format: {data.audioFormat}\n2: Audio Quality: {data.audioQuality}\n3: Audio Conversion Format: {data.audioOutputFormat}\n4: Directory: {data.workingDirectory}\n5: FF-Mpeg Dir: {data.ffMpegDirectory}\n6: Link: {link}\n7: File Name: {filename}\n8: Batch Processing\n9: Continue\n0: Exit\n");
            if (appendWritingIndicator) {
                Console.Write("\n#\\> ");
            }
            //writeDB(parameters);
        }
        static bool checkURL(string url) { // Checks for whether or not the url is formatted properly 
            Uri? uriResult;
            bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttps);
            return result;
        }

        static void writeAscii(int input, bool prependNewLine = false, bool appendNewLine = true) {
            string prepend = prependNewLine ? "\n" : "";
            string append = appendNewLine ? "\n" : "";
            switch (input) {
                //Ascii Text is below, this is called for GUI related reasons within the program.
                case 1:
                    //Configuration
                    Console.Write(prepend + Constants._CONFIGURATION_ASCII + append);
                    break;
                case 2:
                    //Executing...
                    Console.WriteLine(prepend + Constants._EXECUTING_ASCII + append);
                    break;
                case 3:
                    //Thank You
                    Console.WriteLine(prepend + Constants._THANK_YOU_ASCII + append);
                    break;
                case 4:
                    //Welcome
                    Console.Write(prepend + Constants._WELCOME_ASCII + append);
                    break;
                default:
                    //Error! If this is executed, something is wrong with the way this function was called.
                    throw (new Exception("nError detected by WriteAscii(), this function has been declared with an invalid 'input' variable value."));
            }
        }
        static void batchProcess(DatabaseObject data) {
            Console.Write(Constants._BATCH_WELCOME);
            string batchfile = InputHandler.inputValidate("Enter File Path");
            while (!File.Exists(batchfile)) {
                batchfile = InputHandler.inputValidate("File Not Found, Enter File Path");
            }
            string[] file = File.ReadAllLines(batchfile);
            int i = 0;
            List<string> URLs = new List<string>();
            List<string> fileNames = new List<string>();
            List<Process> processes = new List<Process>();
            List<int> processFailCount = new List<int>();
            foreach (string x in file) {
                switch (i % 2) {
                    case 0:
                        URLs.Add(x);
                        break;
                    case 1:
                        fileNames.Add(x);
                        break;
                    default:
                        throw (new Exception("Logic error detected in batch processing. This should not be possible. Your machine may be failing, C# is malfunctioning, or something else is seriously wrong."));
                }
                i++;
            }
            Console.WriteLine("\nExecution Started on File, please wait... \n");
            i = 0;
            string name;
            foreach (string URL in URLs) {
                name = ConstantBuilder.buildFileName(data.workingDirectory, fileNames[i]);
                processes.Add(new Process {
                    StartInfo = new ProcessStartInfo {
                        FileName = ".\\youtube-dl.exe",
                        Arguments = ConstantBuilder.buildArguments(data, URL, fileNames[i]),
                        UseShellExecute = false,
                        RedirectStandardOutput = false,
                        CreateNoWindow = true
                    }
                });
                processFailCount.Add(0);

                processes[i].Start();
                i++;
            }
            int processesWaiting = processes.Count();
            int failed = 0;
            int succeeded = 0;
            i = 0;
            while (processesWaiting > 0) {
                if (processes[i].HasExited) {
                    processesWaiting--;

                    if (processes[i].ExitCode == 0) {
                        succeeded++;
                        processes[i].Dispose();
                        processes.RemoveAt(i);
                    } else {

                        if (processFailCount[i] > 3) {
                            failed++;
                            processes[i].Dispose();
                            processes.RemoveAt(i);
                        } else {
                            processes[i].Start();
                        }
                    }
                }


                i = i < (processesWaiting - 1) ? i + 1 : 0;
                Thread.Sleep(100);
            }
            for (i = 0; i < processFailCount.Count(); i++) {
                Console.WriteLine($"Task {(i + 1)}: {(processFailCount[i] > 0 ? processFailCount[i] == 3 ? "Failed" : "Retried and Succeeded" : "Succeeded")}");
            }
            Console.WriteLine($"\nSuccessful Tasks: {succeeded}\nFailed Tasks: {failed}\n\nPRESS ENTER TO CONTINUE");
            Console.ReadLine();
        }
        static bool checkFiles(string ff, string DATABASE_FILE, bool hold_up_execution = true, bool showGUI = true) {
            List<bool> exists = new List<bool>
            {
        File.Exists(Constants._YOUTUBE_DL_EXECUTABLE),
        File.Exists($"{ff}\\ffmpeg.exe"),
        File.Exists(DATABASE_FILE)
    };

            exists.Add(!exists.Contains(false));
            if (showGUI) {
                List<string> exists_UI = new List<string>();

                for (int i = 0; i < (exists.Count - 1); i++) {
                    if (!exists[i]) { // If false 
                        exists_UI.Add("Error!");
                    } else { // If true
                        exists_UI.Add("Located");
                    }
                }
                if (!exists[3]) { // If false 
                    exists_UI.Add("Error!");
                } else { // If true
                    exists_UI.Add("Good to go!");
                }

                Console.WriteLine(
                    $"Checking core files: " +
                    $"\nYoutube-DL: " + exists_UI[0] +
                    $"\nFFMPEG:     " + exists_UI[1] +
                    $"\nDatabase:   " + exists_UI[2] +
                    $"\nResult:     " + exists_UI[3]);
                if (hold_up_execution) {
                    Console.WriteLine($"\n-----ENTER TO CONTINUE-----");
                    Console.ReadLine();
                }
            }
            return exists[3];
        }
        static void logErrors(List<int> Errors) {
            Console.WriteLine();
            for (int i = 0; i < Errors.Count(); i++) {
                switch (Errors[i]) {
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

        static void runYoutubeDL(DatabaseObject data, string link, string filename) {
            Console.Clear();
            //Ascii Text, "Executing..."
            writeAscii(2);
            Console.Write($"Command parsed and sent, passing youtube-dl output...\n\n"/*\n--------------------------------------------------------\nPress ENTER once execution is complete to view the menu.\n--------------------------------------------------------\n\n"*/);
            var process = Process.Start(Constants._YOUTUBE_DL_EXECUTABLE, ConstantBuilder.buildArguments(data, link, filename));
            Thread.Sleep(250); //Frees up CPU for youtube-dl to start. Fixes an issue where youtube-dl wouldn't start until enter was pressed.
            process.WaitForExit(); // Waits for exit, so it should now automatically enter the menu again.
            string result = process.ExitCode != 0 ? "Failed" : "Succeeded";
            int i = 1;
            while (i < 3 && process.ExitCode != 0) {
                process.Dispose();
                Console.WriteLine("\nError: failure detected, retrying " + (i + 1) + "/3");
                process = Process.Start(Constants._YOUTUBE_DL_EXECUTABLE, ConstantBuilder.buildArguments(data, link, filename));
                Thread.Sleep(250); //Frees up CPU for youtube-dl to start. Fixes an issue where youtube-dl wouldn't start until enter was pressed.
                process.WaitForExit(); // Waits for exit, so it should now automatically enter the menu again.
                result = process.ExitCode != 0 ? "Failed" : "Succeeded";
                i++;
            }
            process.Dispose();
            Console.Write($"\nExecution completed with result: {result}\nPRESS ENTER TO CONTINUE");
            Console.ReadLine();
        }

        static void Main(string[] args) => new Program().MainAsync(args);

        public async void MainAsync(string[] args) {
            InputHandler inputhandle = new InputHandler();
            //DataStructures.YoutubeDLParamInfo paramData = new DataStructures.YoutubeDLParamInfo();
            List<int> Errors = new List<int>();
            if (File.Exists(Constants._DATABASE_FILE) == false) {
                //createDB(Constants._DATABASE_FILE);
                await data.updateSelf();
            } else {
                await data.populateSelf();
            }

            Console.Clear();
            //Ascii Text, "Welcome"
            writeAscii(4);
            if (!checkFiles(data.ffMpegDirectory, Constants._DATABASE_FILE, showGUI: false)) {
                Errors.Add(2);
                checkFiles(data.ffMpegDirectory, Constants._DATABASE_FILE, hold_up_execution: false);
            }
            string filename = "NULL";
            string link = "NULL";
            if (Errors.Count() > 0) { logErrors(Errors); }
            Console.Write("We have a few initialization questions before you can begin.\n");
            link = InputHandler.inputValidate("Input a link to the file you wish to fetch");
            if (link.ToLower() is not ("s" or "skip")) { // for quick bypass in the case of batch processing
                Console.Write("Input \"" + link + "\" Accepted!");
                Thread.Sleep(400);
                Console.Clear();
                //Ascii Text, "Welcome"
                writeAscii(4);
                Console.Write("We have a few initialization questions before you can begin.\n");
                filename = InputHandler.inputValidate("Input a name for the file output without the entension");
                Console.Write("Input \"" + filename + "\" Accepted!");
                Thread.Sleep(400); //Gives visual feedback to user
            } else {
                link = "NULL (Skipped)";
                filename = "NULL (Skipped)";
            }
            while (true) {
                DataStructures.commandToExecute? input;

                while (true) {
                    Console.Clear();
                    writeGUI(data, link, filename, true);
                    Thread.Sleep(500);
                    input = inputhandle.handleCommand(Console.ReadLine());
                    switch (input) {

                        case DataStructures.commandToExecute.audioFormat:
                            Console.Clear();
                            writeGUI(data, link, filename, false);
                            data.audioFormat = InputHandler.askQuestion("Input a new audio format", ValidationLambdas.isNumber, invalidPrompt: "Your input is not a number, input a new audio format: ");
                            await data.updateSelf();
                            break;

                        case DataStructures.commandToExecute.audioQuality:
                            Console.Clear();
                            writeGUI(data, link, filename, false);
                            data.audioQuality = InputHandler.askQuestion("Input a new audio quality", ValidationLambdas.isNumber, invalidPrompt: "Your input is not a number, input a new audio quality: ");
                            await data.updateSelf();
                            break;

                        case DataStructures.commandToExecute.audioOutputFormat:
                            Console.Clear();
                            writeGUI(data, link, filename, false);
                            data.audioOutputFormat = InputHandler.inputValidate("Input a new conversion format");
                            await data.updateSelf();
                            break;

                        case DataStructures.commandToExecute.directory:
                            Console.Clear();
                            writeGUI(data, link, filename, false);
                            data.workingDirectory = InputHandler.inputValidate("Input a new directory path (A to autofill current path)");
                            if (data.workingDirectory == "A" || data.workingDirectory == "a") { data.workingDirectory = Directory.GetCurrentDirectory(); }
                            if (!Directory.Exists(data.workingDirectory)) {
                                Console.WriteLine("Warning: The directory you entered does not currently exist. This script may not function properly.\nPRESS ENTER TO CONTINUE");
                                Console.ReadLine();
                            }
                            await data.updateSelf();
                            break;

                        case DataStructures.commandToExecute.ffDirectory:
                            Console.Clear();
                            writeGUI(data, link, filename, false);
                            data.ffMpegDirectory = InputHandler.inputValidate("Input a new FF-Mpeg Path (A to autofill current path)");
                            if (data.ffMpegDirectory == "A" || data.ffMpegDirectory == "a") { data.ffMpegDirectory = Directory.GetCurrentDirectory(); }
                            if (!File.Exists(data.ffMpegDirectory + "\\ffmpeg.exe")) {
                                Console.WriteLine("Warning: FFMPEG could not be located at this path. This script may not function properly.\nPRESS ENTER TO CONTINUE");
                                Console.ReadLine();
                            }
                            await data.updateSelf();
                            break;

                        case DataStructures.commandToExecute.link:
                            Console.Clear();
                            writeGUI(data, link, filename, false);
                            link = InputHandler.inputValidate("Input a link to the file you wish to fetch");
                            break;

                        case DataStructures.commandToExecute.filename:
                            Console.Clear();
                            writeGUI(data, link, filename, false);
                            filename = InputHandler.inputValidate("Input a name for the output file (without the entension)");
                            break;

                        case DataStructures.commandToExecute.batch:
                            Console.Clear();
                            batchProcess(data);
                            break;

                        case DataStructures.commandToExecute.goOn:
                            if (filename != "NULL (Skipped)" && link != "NULL (Skipped)") {
                                runYoutubeDL(data, link, filename);
                            } else {
                                Console.Write("\nOops, you need to specify the link and filename first.\nPRESS ENTER TO CONTINUE");
                                Console.ReadLine();
                            }
                            break;

                        case DataStructures.commandToExecute.exit:
                            Console.Clear();
                            //Ascii Text, "Thank You"
                            writeAscii(3);
                            Console.WriteLine("This application is now exiting...\nThank you for using Youtube-DL Frontend!");
                            Thread.Sleep(1000);
                            System.Environment.Exit(1);
                            break;
                    };
                };

            }
        }
    }
}