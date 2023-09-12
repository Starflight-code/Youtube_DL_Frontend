namespace Youtube_DL_Frontend
{
    internal class Interface
    {
        public static void writeGUI(DatabaseObject data, string link, string filename, bool appendWritingIndicator = true)
        {
            //Ascii Text, "Configuration"
            writeAscii(1);
            Console.Write($"1: Audio Format: {data.audioFormat}\n2: Audio Quality: {data.audioQuality}\n3: Audio Conversion Format: {data.audioOutputFormat}\n4: Directory: {data.workingDirectory}\n5: FF-Mpeg Dir: {data.ffMpegDirectory}\n6: Link: {link}\n7: File Name: {filename}\n8: Batch Processing\n9: Continue\n0: Exit\n");
            if (appendWritingIndicator)
            {
                Console.Write("\n#\\> ");
            }
            //writeDB(parameters);
        }
        public static bool checkURL(string url)
        { // Checks for whether or not the url is formatted properly 
            Uri? uriResult;
            bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttps);
            return result;
        }
        public static void writeAscii(int input, bool prependNewLine = false, bool appendNewLine = true)
        {
            string prepend = prependNewLine ? "\n" : "";
            string append = appendNewLine ? "\n" : "";
            switch (input)
            {
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
        public static string getAscii(int input, bool prependNewLine = false, bool appendNewLine = true)
        {
            string prepend = prependNewLine ? "\n" : "";
            string append = appendNewLine ? "\n" : "";
            switch (input)
            {
                //Ascii Text is below, this is called for GUI related reasons within the program.
                case 1:
                    //Configuration
                    return prepend + Constants._CONFIGURATION_ASCII + append;
                case 2:
                    //Executing...
                    return prepend + Constants._EXECUTING_ASCII + append;
                case 3:
                    //Thank You
                    return prepend + Constants._THANK_YOU_ASCII + append;
                case 4:
                    //Welcome
                    return prepend + Constants._WELCOME_ASCII + append;
                default:
                    //Error! If this is executed, something is wrong with the way this function was called.
                    throw (new Exception("Error detected by getAscii(), this function has been declared with an invalid 'input' variable value."));
            }
        }
    }
}