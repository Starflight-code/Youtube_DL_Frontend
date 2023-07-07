namespace Youtube_DL_Frontnend {
    internal class InputHandler {
        public static Dictionary<string, DataStructures.commandToExecute> commandParser = new Dictionary<string, DataStructures.commandToExecute>();

        public InputHandler() {
            commandParser.Add("1", DataStructures.commandToExecute.audioFormat);
            commandParser.Add("2", DataStructures.commandToExecute.audioQuality);
            commandParser.Add("3", DataStructures.commandToExecute.audioOutputFormat);
            commandParser.Add("4", DataStructures.commandToExecute.directory);
            commandParser.Add("5", DataStructures.commandToExecute.ffDirectory);
            commandParser.Add("6", DataStructures.commandToExecute.link);
            commandParser.Add("7", DataStructures.commandToExecute.filename);
            commandParser.Add("8", DataStructures.commandToExecute.batch);
            commandParser.Add("9", DataStructures.commandToExecute.goOn);
            commandParser.Add("0", DataStructures.commandToExecute.exit);
        }
        public static string promptBuilder(string mainPrompt, char endsWith = ':', bool prependWithNewLine = true) {
            string newLine = "";
            if (prependWithNewLine) {
                newLine = "\n";
            }
            return newLine + mainPrompt + endsWith + " ";
        }
        public static string inputValidate(string prompt, string invalidPrompt = "Your input appears to be invalid. Try again: ", char promptEndsWith = ':', bool noPrompt = false, bool prebuildPrompt = false) {
            if (!noPrompt) {
                if (prebuildPrompt) {
                    Console.Write(prompt);
                } else {
                    Console.Write(promptBuilder(prompt, promptEndsWith));
                }
            }
            string? input = Console.ReadLine();
            while (input == null || input == "") {
                Console.Write("\n" + invalidPrompt);
                input = Console.ReadLine();
            }
            return input.Trim();
        }
        public static string askQuestion(string prompt, Func<string, bool> validationLambda, char promptEndsWith = ':', string invalidPrompt = "Your input appears to be invalid. Try again: ", char invalidPromptEndsWith = ':') {
            string input = inputValidate(promptBuilder(prompt, promptEndsWith, true), invalidPrompt, prebuildPrompt: true);
            while (validationLambda(input) == false) {
                Console.Write("\n" + invalidPrompt);
                input = inputValidate("", invalidPrompt, noPrompt: true);
            }
            return input.Trim();
        }

        public DataStructures.commandToExecute handleCommand(string input) {
            input = input.Trim();
            DataStructures.commandToExecute output;
            while (input.Length == 0 || !commandParser.TryGetValue(input, out output)) {
                Console.Write("Your input was not a valid command, try again: ");
                input = inputValidate("", "Your input was not a valid command, try again", noPrompt: true);
            }
            return output;
        }
    }
}
