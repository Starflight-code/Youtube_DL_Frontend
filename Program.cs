﻿using System.Diagnostics;

namespace Youtube_DL_Frontnend {
    internal class Program {
        ValidationLambdas lambdas = new ValidationLambdas();
        static void writeGUI(string af, string aq, string auf, string dir, string ff, string link, string filename, bool ia, string DATABASE_FILE) {
            //Ascii Text, "Configuration"
            writeAscii(1);
            Console.Write($"1: Audio Format: {af}\n2: Audio Quality: {aq}\n3: Audio Conversion Format: {auf}\n4: Directory: {dir}\n5: FF-Mpeg Dir: {ff}\n6: Link: {link}\n7: File Name: {filename}\n8: Batch Processing\n9: Continue\n0: Exit\n");
            if (ia == true) {
                Console.Write("\n#\\> ");
            }
            string[] lines =
        {
            "Do not modify data within this file!", af, aq, auf, dir, ff
        };

            File.WriteAllLinesAsync(Constants._DATABASE_FILE, lines);
        }
        static void writeGUI(DataStructures.YoutubeDLParamInfo parameters, string link, string filename, bool appendWritingIndicator = true) {
            //Ascii Text, "Configuration"
            writeAscii(1);
            Console.Write($"1: Audio Format: {parameters.audioFormat}\n2: Audio Quality: {parameters.audioQuality}\n3: Audio Conversion Format: {parameters.audioOutputFormat}\n4: Directory: {parameters.workingDirectory}\n5: FF-Mpeg Dir: {parameters.ffMpegDirectory}\n6: Link: {link}\n7: File Name: {filename}\n8: Batch Processing\n9: Continue\n0: Exit\n");
            if (appendWritingIndicator) {
                Console.Write("\n#\\> ");
            }
            string[] lines =
        {
            "Do not modify data within this file!", parameters.audioFormat, parameters.audioQuality, parameters.audioOutputFormat, parameters.workingDirectory, parameters.ffMpegDirectory
        };

            File.WriteAllLinesAsync(Constants._DATABASE_FILE, lines);
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
        static void batchProcess(DataStructures.YoutubeDLParamInfo parameters) {
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
                        throw (new Exception("Logic error detected in batch processing, file parsing section."));
                }
                i++;
            }
            Console.WriteLine("\nExecution Started on File, please wait... \n");
            i = 0;
            string name;
            foreach (string URL in URLs) {
                name = ConstantBuilder.buildFileName(parameters.workingDirectory, fileNames[i]);
                processes.Add(new Process {
                    StartInfo = new ProcessStartInfo {
                        FileName = ".\\youtube-dl.exe",
                        Arguments = ConstantBuilder.buildArguments(parameters, URL, fileNames[i]),
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
                    exists_UI.Add("Good to Go!");
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

        static void createDB(string DATABASE_FILE) {
            Console.WriteLine("Database not detected, Creating...");
            Thread.Sleep(500);
            var db = File.Create(DATABASE_FILE);
            string directory = Directory.GetCurrentDirectory();
            string[] lines =
        {
            Constants._DATABASE_PREPEND, Constants.defaultFile.audioFormat, Constants.defaultFile.audioQuality, Constants.defaultFile.audioOutputFormat, Constants.defaultFile.workingDirectory, Constants.defaultFile.ffMpegDirectory
        };
            db.Dispose();
            Console.WriteLine("Starting file write operating...");
            var dbtemp = File.WriteAllLinesAsync(DATABASE_FILE, lines);
            dbtemp.Wait(500);
            dbtemp.Dispose();
            Console.Clear();
            if (!checkFiles(Constants.defaultFile.ffMpegDirectory, DATABASE_FILE)) {
                Console.WriteLine(Constants._FILES_NOT_FOUND);
            }
            Console.WriteLine("\n\nCompleted! Starting program.");
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
        static string[] readDB(string DATABASE_FILE) {
            int counter = 0;
            List<string> data = new List<string>();
            foreach (string line in System.IO.File.ReadLines(@DATABASE_FILE)) {
                System.Console.WriteLine(line);
                if (line == null) {
                    File.Delete(DATABASE_FILE);
                    createDB(Constants._DATABASE_FILE);
                    return new string[] { };
                }
                counter++;
                data.Add(line);

            }
            return data.ToArray();
        }

        static void Main(string[] args) {
            DataStructures.YoutubeDLParamInfo paramData = new DataStructures.YoutubeDLParamInfo();
            //string dir = "";
            //string? output = null;
            //string ff = "";
            //string af = "";
            //string aq = "";
            //string auf = "";
            int counter = 0;
            List<int> Errors = new List<int>();
            if (File.Exists(Constants._DATABASE_FILE) == false) {
                createDB(Constants._DATABASE_FILE);
            }
            string[] database_lines = System.IO.File.ReadLines(Constants._DATABASE_FILE).ToArray();
            if (database_lines.Length != 6) {
                File.Delete(Constants._DATABASE_FILE);
                createDB(Constants._DATABASE_FILE);
                Errors.Add(1); // Logs a database reset error
                database_lines = System.IO.File.ReadLines(Constants._DATABASE_FILE).ToArray();
            }
            foreach (string line in database_lines) { // Check to make sure there are 6 values in the array, if not reset the database and log an error
                System.Console.WriteLine(line);
                if (line == null || line == "") {
                    Console.WriteLine("Database Corrupt: Regenerating...");
                    File.Delete(Constants._DATABASE_FILE);
                    createDB(Constants._DATABASE_FILE);
                    Errors.Add(1); // Logs a database reset error
                    string[] db = readDB(Constants._DATABASE_FILE);
                    paramData.audioFormat = db[0];
                    paramData.audioQuality = db[1];
                    paramData.audioOutputFormat = db[2];
                    paramData.workingDirectory = db[3];
                    paramData.ffMpegDirectory = db[4];
                    break;
                }
                counter++;
                switch (counter) {
                    case 1:
                        break;
                    case 2:
                        paramData.audioFormat = line;
                        break;
                    case 3:
                        paramData.audioQuality = line;
                        break;
                    case 4:
                        paramData.audioOutputFormat = line;
                        break;
                    case 5:
                        paramData.workingDirectory = line;
                        break;
                    case 6:
                        paramData.ffMpegDirectory = line;
                        break;
                }
            }
            Console.Clear();
            //Ascii Text, "Welcome"
            writeAscii(4);
            if (!checkFiles(paramData.ffMpegDirectory, Constants._DATABASE_FILE, showGUI: false)) {
                Errors.Add(2);
                checkFiles(paramData.ffMpegDirectory, Constants._DATABASE_FILE, hold_up_execution: false);
            }
            string filename = "NULL";
            string link = "NULL";
            if (Errors.Count() > 0) { logErrors(Errors); }
            Console.Write("We have a few initialization questions before you can begin.\n");
            //Console.Write("Input a link to the file you wish to fetch: ");
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
            //output = $"{paramData.workingDirectory}{filename}.%(ext)s"; //Includes youtube-dl placeholders
            bool st = false;
            while (true) {
                string? inp = "0";
                bool continuevar = false;

                while (continuevar == false) {
                    Console.Clear();
                    //Ascii Text "Configuration"
                    bool ia = true;
                    writeGUI(paramData, link, filename, ia);
                    //Console.Write($"1: Audio Format: {af}\n2: Audio Quality: {aq}\n3: Audio Conversion Format: {auf}\n4: Directory: {dir}\n5: FF-Mpeg Dir: {ff}\n6: Link: {link}\n7: File Name: {filename}\n8: Continue\n9: Exit\n\n#\\> ");
                    if (st == true) {
                        Console.Clear();
                        //Ascii Text, "Executing..."
                        writeAscii(2);
                        //output = $"{dir}{filename}.%(ext)s";
                        Console.Write($"Command parsed and sent, passing youtube-dl output...\n\n"/*\n--------------------------------------------------------\nPress ENTER once execution is complete to view the menu.\n--------------------------------------------------------\n\n"*/);
                        //var process = Process.Start(".\\youtube-dl.exe", $"-f {af} --audio-format {auf} -x --ffmpeg-location \"{ff}\" {link} --audio-quality {aq} -o \"{output}\"");
                        var process = Process.Start(Constants._YOUTUBE_DL_EXECUTABLE, ConstantBuilder.buildArguments(paramData, link, filename));
                        Thread.Sleep(250); //Frees up CPU for youtube-dl to start. Fixes an issue where youtube-dl wouldn't start until enter was pressed.
                        process.WaitForExit(); // Waits for exit, so it should now automatically enter the menu again.
                        string result = process.ExitCode != 0 ? "Failed" : "Succeeded";
                        int i = 1;
                        while (i < 3 && process.ExitCode != 0) {
                            process.Dispose();
                            Console.WriteLine("\nError: failure detected, retrying " + (i + 1) + "/3");
                            process = Process.Start(Constants._YOUTUBE_DL_EXECUTABLE, ConstantBuilder.buildArguments(paramData, link, filename));
                            Thread.Sleep(250); //Frees up CPU for youtube-dl to start. Fixes an issue where youtube-dl wouldn't start until enter was pressed.
                            process.WaitForExit(); // Waits for exit, so it should now automatically enter the menu again.
                            result = process.ExitCode != 0 ? "Failed" : "Succeeded";
                            i++;
                        }
                        process.Dispose();
                        Console.Write($"\nExecution completed with result: {result}\nPRESS ENTER TO CONTINUE");
                        Console.ReadLine();
                        inp = "SKIP";
                    } else {
                        Thread.Sleep(500);
                        inp = Console.ReadLine();
                    }
                    switch (inp) {
                        case "1":
                            Console.Clear();
                            ia = false;
                            writeGUI(paramData, link, filename, ia);
                            //Console.Write("\nInput a new audio format: ");
                            paramData.audioFormat = InputHandler.inputValidate("Input a new audio format");
                            while (!Int32.TryParse(paramData.audioFormat.Trim().ToString(), out int result)) {
                                paramData.audioFormat = InputHandler.inputValidate("Your input is not a number, input a new audio format");
                            }
                            break;
                        case "2":
                            Console.Clear();
                            ia = false;
                            writeGUI(paramData, link, filename, ia);
                            paramData.audioQuality = InputHandler.inputValidate("Input a new audio quality");
                            while (!Int32.TryParse(paramData.audioQuality.Trim().ToString(), out int result)) {
                                paramData.audioQuality = InputHandler.inputValidate("Your input is not a number, input a new audio quality");
                            }
                            break;
                        case "3":
                            Console.Clear();
                            ia = false;
                            writeGUI(paramData, link, filename, ia);
                            paramData.audioOutputFormat = InputHandler.inputValidate("Input a new conversion format");
                            break;
                        case "4":
                            Console.Clear();
                            ia = false;
                            writeGUI(paramData, link, filename, ia);
                            paramData.workingDirectory = InputHandler.inputValidate("Input a new directory path (A to autofill current path)");
                            if (paramData.workingDirectory == "A" || paramData.workingDirectory == "a") { paramData.workingDirectory = Directory.GetCurrentDirectory(); }
                            if (!Directory.Exists(paramData.workingDirectory)) {
                                Console.WriteLine("Warning: The directory you entered does not currently exist. This script may not function properly.\nPRESS ENTER TO CONTINUE");
                                Console.ReadLine();
                            }
                            //output = $"{paramData.workingDirectory}{filename}.%(ext)s";
                            break;
                        case "5":
                            Console.Clear();
                            ia = false;
                            writeGUI(paramData, link, filename, ia);
                            paramData.ffMpegDirectory = InputHandler.inputValidate("Input a new FF-Mpeg Path (A to autofill current path)");
                            if (paramData.ffMpegDirectory == "A" || paramData.ffMpegDirectory == "a") { paramData.ffMpegDirectory = Directory.GetCurrentDirectory(); }
                            if (!File.Exists(paramData.ffMpegDirectory + "\\ffmpeg.exe")) {
                                Console.WriteLine("Warning: FFMPEG could not be located at this path. This script may not function properly.\nPRESS ENTER TO CONTINUE");
                                Console.ReadLine();
                            }
                            break;
                        case "6":
                            Console.Clear();
                            ia = false;
                            writeGUI(paramData, link, filename, ia);
                            link = InputHandler.inputValidate("Input a link to the file you wish to fetch");
                            break;
                        case "7":
                            Console.Clear();
                            ia = false;
                            writeGUI(paramData, link, filename, ia);
                            filename = InputHandler.inputValidate("Input a name for the output file (without the entension)");
                            //output = $"{dir}{filename}.%(ext)s";
                            break;
                        case "8":
                            Console.Clear();
                            ia = false;
                            batchProcess(paramData);
                            break;
                        case "9":
                            if (filename != "NULL (Skipped)" && link != "NULL (Skipped)") {
                                continuevar = true;
                            } else {
                                Console.Write("\nOops, you need to specify the link and filename first.\nPRESS ENTER TO CONTINUE");
                                Console.ReadLine();
                            }
                            break;
                        case "0":
                            Console.Clear();
                            //Ascii Text, "Thank You"
                            writeAscii(3);
                            Console.WriteLine("This application is now exiting...\nThank you for using Youtube-DL Frontend!");
                            Thread.Sleep(1000);
                            System.Environment.Exit(1);
                            break;
                        case "SKIP":
                            //Skips to reload UI after youtube-dl processing is completed. Allows for faster use and less re-configuration.
                            st = false;
                            continuevar = false;
                            break;
                        default:
                            Console.WriteLine("ERROR: Invalid Input Given. Exiting Program...");
                            Console.WriteLine($"'{inp}'");
                            Thread.Sleep(1000);
                            System.Environment.Exit(1);
                            break;
                    };
                    if (continuevar == true) { st = true; };
                };

            }
        }
    }
}