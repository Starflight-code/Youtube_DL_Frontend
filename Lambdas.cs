namespace Youtube_DL_Frontend
{
    internal class Lambdas
    {
        public static Action<DatabaseObject, RuntimeData> audioFormat = async (data, runtime) =>
        {
            Console.Clear();
            Interface.writeGUI(data, runtime.link, runtime.filename, false);
            data.audioFormat = InputHandler.askQuestion("Input a new audio format", ValidationLambdas.isNumber, invalidPrompt: "Your input is not a number, input a new audio format: ");
            await data.updateSelf();
        };
        public static Action<DatabaseObject, RuntimeData> audioQuality = async (data, runtime) =>
        {
            Console.Clear();
            Interface.writeGUI(data, runtime.link, runtime.filename, false);
            data.audioQuality = InputHandler.askQuestion("Input a new audio quality", ValidationLambdas.isNumber, invalidPrompt: "Your input is not a number, input a new audio quality: ");
            await data.updateSelf();
        };
        public static Action<DatabaseObject, RuntimeData> audioOutputFormat = async (data, runtime) =>
        {
            Console.Clear();
            Interface.writeGUI(data, runtime.link, runtime.filename, false);
            data.audioOutputFormat = InputHandler.inputValidate("Input a new conversion format");
            await data.updateSelf();
        };
        public static Action<DatabaseObject, RuntimeData> directory = async (data, runtime) =>
        {
            Console.Clear();
            Interface.writeGUI(data, runtime.link, runtime.filename, false);
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
            Interface.writeGUI(data, runtime.link, runtime.filename, false);
            data.ffMpegDirectory = InputHandler.inputValidate("Input a new FF-Mpeg Path (A to autofill current path)");
            if (data.ffMpegDirectory == "A" || data.ffMpegDirectory == "a") { data.ffMpegDirectory = Directory.GetCurrentDirectory(); }
            if (!File.Exists(data.ffMpegDirectory + "\\ffmpeg.exe"))
            {
                Console.WriteLine("Warning: FFMPEG could not be located at this path. This script may not function properly.\nPRESS ENTER TO CONTINUE");
                Console.ReadLine();
            }
            await data.updateSelf();
        };
        public static Action<DatabaseObject, RuntimeData> link = async (data, runtime) =>
        {
            Console.Clear();
            Interface.writeGUI(data, runtime.link, runtime.filename, false);
            runtime.link = InputHandler.inputValidate("Input a link to the file you wish to fetch");
            await Task.Delay(0);
        };
        public static Action<DatabaseObject, RuntimeData> filename = async (data, runtime) =>
        {
            Console.Clear();
            Interface.writeGUI(data, runtime.link, runtime.filename, false);
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
            System.Environment.Exit(1);
        };

    }
}
