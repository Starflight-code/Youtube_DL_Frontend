using Youtube_DL_Frontend.Data;

namespace Youtube_DL_Frontend.Parsing
{
    internal class InputHandler
    {
        public static Dictionary<string, Enums.commandToExecute> commandParser = new Dictionary<string, Enums.commandToExecute>();

        public InputHandler()
        {
            commandParser.Add("1", Enums.commandToExecute.audioFormat);
            commandParser.Add("2", Enums.commandToExecute.audioQuality);
            commandParser.Add("3", Enums.commandToExecute.audioOutputFormat);
            commandParser.Add("4", Enums.commandToExecute.directory);
            commandParser.Add("5", Enums.commandToExecute.ffDirectory);
            commandParser.Add("6", Enums.commandToExecute.link);
            commandParser.Add("7", Enums.commandToExecute.filename);
            commandParser.Add("8", Enums.commandToExecute.batch);
            commandParser.Add("9", Enums.commandToExecute.goOn);
            commandParser.Add("0", Enums.commandToExecute.exit);
        }
        public static string promptBuilder(string mainPrompt, char endsWith = ':', bool prependWithNewLine = true)
        {
            string newLine = "";
            if (prependWithNewLine)
            {
                newLine = "\n";
            }
            return newLine + mainPrompt + endsWith + " ";
        }
        public static string inputValidate(string prompt, string invalidPrompt = "Your input appears to be invalid. Try again: ", char promptEndsWith = ':', bool noPrompt = false, bool prebuildPrompt = false)
        {
            if (!noPrompt)
            {
                if (prebuildPrompt)
                {
                    Console.Write(prompt);
                }
                else
                {
                    Console.Write(promptBuilder(prompt, promptEndsWith));
                }
            }
            string? input = Console.ReadLine();
            while (input == null || input == "")
            {
                Console.Write("\n" + invalidPrompt);
                input = Console.ReadLine();
            }
            return input.Trim();
        }
        public static string askQuestion(string prompt, Func<string, bool> validationLambda, char promptEndsWith = ':', string invalidPrompt = "Your input appears to be invalid. Try again: ", char invalidPromptEndsWith = ':')
        {
            string input = inputValidate(promptBuilder(prompt, promptEndsWith, true), invalidPrompt, prebuildPrompt: true);
            while (validationLambda(input) == false)
            {
                Console.Write("\n" + invalidPrompt);
                input = inputValidate("", invalidPrompt, noPrompt: true);
            }
            return input.Trim();
        }

        public Enums.commandToExecute handleCommand(string? input)
        {
            if (input == null) { input = ""; }
            input = input.Trim();
            Enums.commandToExecute output;
            while (input.Length == 0 || !commandParser.TryGetValue(input, out output))
            {
                Console.Write("Your input was not a valid command, try again: ");
                input = inputValidate("", "Your input was not a valid command, try again", noPrompt: true);
            }
            return output;
        }
    }
}
