using System.Diagnostics;
using Youtube_DL_Frontend.Parsing;

namespace Youtube_DL_Frontend
{
    internal class ExternalInterface
    {
        struct batchProcessTask
        {
            public string url;
            public string filename;
        }
        public static void runYoutubeDL(Data.DatabaseObject data, Data.RuntimeData runtimeData)
        {
            Console.Clear();

            //Ascii Text, "Executing..."
            Interface.writeAscii(2);
            Console.Write($"Command parsed and sent, passing youtube-dl output...\n\n");

            Process process;
            try
            {
                process = Process.Start(runtimeData.yotutube_dl_executable, ConstantBuilder.buildArguments(data, runtimeData.link, runtimeData.filename));
            }
            catch (System.ComponentModel.Win32Exception)
            {
                Console.WriteLine("\n\nYoutube-DL Path Misconfigured\n" +
                                  "Press ENTER to continue");
                Console.ReadLine();
                return;
            }
            Thread.Sleep(250); //Frees up CPU for youtube-dl to start. Fixes an issue where youtube-dl wouldn't start until enter was pressed.
            process.WaitForExit(); // Waits for exit, so it should now automatically enter the menu again.
            string result = process.ExitCode != 0 ? "Failed" : "Success";
            for (int i = 1; i < 3 && process.ExitCode != 0; i++)
            {
                process.Dispose();
                Console.WriteLine("\nError: failure detected, retrying " + (i + 1) + "/3");

                process = Process.Start(runtimeData.yotutube_dl_executable, ConstantBuilder.buildArguments(data, runtimeData.link, runtimeData.filename));
                Thread.Sleep(250); //Frees up CPU for youtube-dl to start. Fixes an issue where youtube-dl wouldn't start until enter was pressed.
                process.WaitForExit(); // Waits for exit, so it should now automatically enter the menu again.

                result = process.ExitCode != 0 ? "Failed" : "Success";
                i++;
            }
            process.Dispose();
            Console.Write($"\nExecution completed with result: {result}\nPRESS ENTER TO CONTINUE");
            Console.ReadLine();
        }
        public static void batchProcess(Data.DatabaseObject data, Data.RuntimeData runtimeData)
        {
            Console.Write(Data.Constants._BATCH_WELCOME);
            string batchfile = InputHandler.inputValidate("Enter File Path");
            while (!File.Exists(batchfile))
            {
                batchfile = InputHandler.inputValidate("File Not Found, Enter File Path");
            }
            string[] file = File.ReadAllLines(batchfile);
            int i = 0;
            List<batchProcessTask> taskList = new List<batchProcessTask>();
            //List<string> URLs = new List<string>();
            //List<string> fileNames = new List<string>();
            List<Process> processes = new List<Process>();
            List<int> processFailCount = new List<int>();
            batchProcessTask buildTask = new batchProcessTask();
            foreach (string x in file)
            {
                switch (i % 2)
                {
                    case 0:
                        if (i != 0)
                        {
                            taskList.Add(buildTask);
                        }
                        buildTask.url = x;
                        //URLs.Add(x);
                        break;
                    case 1:
                        buildTask.filename = x;
                        //fileNames.Add(x);
                        break;
                    default:
                        throw new Exception("Logic error detected in batch processing. This should not be possible. Your machine may be failing, C# is malfunctioning, or something else is seriously wrong.");
                }
                i++;
            }
            taskList.Add(buildTask);
            Console.WriteLine("\nExecution Started on File, please wait... \n");
            string name;
            //foreach (string URL in URLs)
            foreach (batchProcessTask task in taskList)
            {
                name = ConstantBuilder.buildFileName(data.workingDirectory, task.filename);
                processes.Add(new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = runtimeData.yotutube_dl_executable,
                        Arguments = ConstantBuilder.buildArguments(data, task.url, task.filename),
                        UseShellExecute = false,
                        RedirectStandardOutput = false,
                        CreateNoWindow = true
                    }
                });
                processFailCount.Add(0);

                processes[i].Start();
            }
            int processesWaiting = processes.Count();
            int failed = 0;
            int succeeded = 0;
            i = 0;
            while (processesWaiting > 0)
            {
                if (processes[i].HasExited)
                {
                    processesWaiting--;

                    if (processes[i].ExitCode == 0)
                    {
                        succeeded++;
                        processes[i].Dispose();
                        processes.RemoveAt(i);
                    }
                    else
                    {

                        if (processFailCount[i] > 3)
                        {
                            failed++;
                            processes[i].Dispose();
                            processes.RemoveAt(i);
                        }
                        else
                        {
                            processes[i].Start();
                        }
                    }
                }


                i = i < (processesWaiting - 1) ? i + 1 : 0;
                Thread.Sleep(100);
            }
            for (i = 0; i < processFailCount.Count(); i++)
            {
                Console.WriteLine($"Task {(i + 1)}: {(processFailCount[i] > 0 ? processFailCount[i] == 3 ? "Failed" : "Retried and Succeeded" : "Succeeded")}");
            }
            Console.WriteLine($"\nSuccessful Tasks: {succeeded}\nFailed Tasks: {failed}\n\nPRESS ENTER TO CONTINUE");
            Console.ReadLine();
        }
    }
}