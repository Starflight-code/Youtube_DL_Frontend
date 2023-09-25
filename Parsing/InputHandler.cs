using Youtube_DL_Frontend.Data;

namespace Youtube_DL_Frontend.Parsing
{
    internal class InputHandler
    {

        public InputHandler()
        {
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
            return input;
        }
    }
}
