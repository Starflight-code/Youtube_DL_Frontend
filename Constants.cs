namespace Youtube_DL_Frontend
{
    internal class Constants
    {

        // Messages
        public static string _BATCH_WELCOME = $"You have accessed the batch processing system, this allows asyncronous processing of" +
                           "\nmultiple files. Please specify your pre formatted file, which should be placed inside" +
                          $"\nthe youtube-dl-frontend directory.\n\n";
        //public static string _DATABASE_PREPEND = "Do not modify data within this file!";
        //public static string _FILES_NOT_FOUND = "An error was detected when attempting to locate at least one of the core files. " +
        //            "\nResolve this before executing ANY automation commands. " +
        //            "\nThis script will not function properly until these issues are resolved.";

        // File Names
        public static string _YOUTUBE_DL_EXECUTABLE = Statics.buildPath(Directory.GetCurrentDirectory() + "\\youtube-dl.exe"); // used for file checking
        public static string _DATABASE_FILE = Statics.buildPath(Directory.GetCurrentDirectory() + "\\data.db"); // used for file checking, and database access

        // ASCII Art
        public static string _CONFIGURATION_ASCII = " ___            ___  _                       _    _\n|  _> ___ ._ _ | | '<_> ___  _ _  _ _  ___ _| |_ <_> ___ ._ _ \n| <__/ . \\| ' || |- | |/ . || | || '_><_> | | |  | |/ . \\| ' |\n`___/\\___/|_|_||_|  |_|\\_. |`___||_|  <___| |_|  |_|\\___/|_|_|\n                       <___'\n";
        public static string _WELCOME_ASCII = " _ _ _       _\n| | | | ___ | | ___  ___ ._ _ _  ___\n| | | |/ ._>| |/ | '/ . \\| ' ' |/ ._>\n|__/_/ \\___.|_|\\_|_.\\___/|_|_|_|\\___.\n";
        public static string _EXECUTING_ASCII = " _____                           _    _\n|  ___|                         | |  (_)\n| |__  __  __  ___   ___  _   _ | |_  _  _ __    __ _\n|  __| \\ \\/ / / _ \\ / __|| | | || __|| || '_ \\  / _` |\n| |___  >  < |  __/| (__ | |_| || |_ | || | | || (_| | _  _  _\n\\____/ /_/\\_\\ \\___| \\___| \\__,_| \\__||_||_| |_| \\__, |(_)(_)(_)\n                                                 __/ |\n                                                |___/\n";
        public static string _THANK_YOU_ASCII = " ___  _              _      _ _\n|_ _|| |_  ___ ._ _ | |__  | | | ___  _ _\n | | | . |<_> || ' || / /  \\   // . \\| | |\n |_| |_|_|<___||_|_||_\\_\\   |_| \\___/`___|\n";
    }
}
