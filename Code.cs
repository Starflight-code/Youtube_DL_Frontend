using System.Diagnostics;

static void writeGUI(string af, string aq, string auf, string dir, string ff, string link, string filename, bool ia, string DATABASE_FILE)
{
    //Ascii Text, "Configuration"
    writeAscii(1);
    //Console.Write(" ___            ___  _                       _    _\n|  _> ___ ._ _ | | '<_> ___  _ _  _ _  ___ _| |_ <_> ___ ._ _ \n| <__/ . \\| ' || |- | |/ . || | || '_><_> | | |  | |/ . \\| ' |\n`___/\\___/|_|_||_|  |_|\\_. |`___||_|  <___| |_|  |_|\\___/|_|_|\n                       <___'\n\n");
    Console.Write($"1: Audio Format: {af}\n2: Audio Quality: {aq}\n3: Audio Conversion Format: {auf}\n4: Directory: {dir}\n5: FF-Mpeg Dir: {ff}\n6: Link: {link}\n7: File Name: {filename}\n8: Continue\n9: Exit\n");
    if (ia == true)
    {
        Console.Write("\n#\\> ");
    }
    string[] lines =
{
            "Do not modify data within this file!", af, aq, auf, dir, ff
        };

    File.WriteAllLinesAsync(DATABASE_FILE, lines);
}
static void writeAscii(int input)
{
    switch (input)
    {
        //Ascii Text is below, this is called for GUI related reasons within the program.
        case 1:
            //Configuration
            Console.Write(" ___            ___  _                       _    _\n|  _> ___ ._ _ | | '<_> ___  _ _  _ _  ___ _| |_ <_> ___ ._ _ \n| <__/ . \\| ' || |- | |/ . || | || '_><_> | | |  | |/ . \\| ' |\n`___/\\___/|_|_||_|  |_|\\_. |`___||_|  <___| |_|  |_|\\___/|_|_|\n                       <___'\n\n");
            break;
        case 2:
            //Executing...
            Console.WriteLine(" _____                           _    _\n|  ___|                         | |  (_)\n| |__  __  __  ___   ___  _   _ | |_  _  _ __    __ _\n|  __| \\ \\/ / / _ \\ / __|| | | || __|| || '_ \\  / _` |\n| |___  >  < |  __/| (__ | |_| || |_ | || | | || (_| | _  _  _\n\\____/ /_/\\_\\ \\___| \\___| \\__,_| \\__||_||_| |_| \\__, |(_)(_)(_)\n                                                 __/ |\n                                                |___/\n\n");
            break;
        case 3:
            //Thank You
            Console.WriteLine(" ___  _              _      _ _\n|_ _|| |_  ___ ._ _ | |__  | | | ___  _ _\n | | | . |<_> || ' || / /  \\   // . \\| | |\n |_| |_|_|<___||_|_||_\\_\\   |_| \\___/`___|\n\n");
            break;
        case 4:
            //Welcome
            Console.Write(" _ _ _       _\n| | | | ___ | | ___  ___ ._ _ _  ___\n| | | |/ ._>| |/ | '/ . \\| ' ' |/ ._>\n|__/_/ \\___.|_|\\_|_.\\___/|_|_|_|\\___.\n\n");
            break;
        default:
            //Error! If this is executed, something is wrong with the way this function was called.
            Console.Write("\n\nError detected by WriteAscii, this function has been declared without or with an invalid 'sel' variable value.\n");
            break;

    }
}
string? dir = null;
string? output = null;
string? ff = null;
string? af = null;
string? aq = null;
string? auf = null;
int counter = 0;
const string DATABASE_FILE = ".\\data.db";

if (File.Exists(DATABASE_FILE) == false) {
    Console.WriteLine("Database not detected, Creating...");
    Thread.Sleep(500);
    var db = File.Create(DATABASE_FILE);
    af = "251";
    aq = "0";
    auf = "mp3";
    string directory = Directory.GetCurrentDirectory();
    dir = directory;
    ff = directory;
    string[] lines =
{
            "Do not modify data within this file!" ,af, aq, auf, dir, ff
        };
    db.Dispose();
    Console.WriteLine("Starting file write operating...");
    var dbtemp = File.WriteAllLinesAsync(DATABASE_FILE, lines);
    dbtemp.Wait(500);
    dbtemp.Dispose();
    Console.WriteLine("Completed! Starting program.");
}
foreach (string line in System.IO.File.ReadLines(@DATABASE_FILE))
{
    System.Console.WriteLine(line);
    counter++;
    switch (counter)
    {
        case 1:
            break;
        case 2:
            af = line;
            break;
        case 3:
            aq = line;
            break;
        case 4:
            auf = line;
            break;
        case 5:
            dir = line;
            break;
        case 6:
            ff = line;
            break;
    }
}
Console.Clear();
//Ascii Text, "Welcome"
writeAscii(4);
//Console.Write(" _ _ _       _\n| | | | ___ | | ___  ___ ._ _ _  ___\n| | | |/ ._>| |/ | '/ . \\| ' ' |/ ._>\n|__/_/ \\___.|_|\\_|_.\\___/|_|_|_|\\___.\n\n");
Console.Write("We have a few initialization questions before you can begin.\n\n");
Console.Write("Input a link to the file you wish to fetch: ");
string? link = Console.ReadLine();
Console.Write("Input \"" + link + "\" Accepted!");
Thread.Sleep(400);
Console.Clear();
//Ascii Text, "Welcome"
writeAscii(4);
Console.Write("We have a few initialization questions before you can begin.\n\n"); Console.Write("Input a name for the file output without the entension: ");
string? filename = Console.ReadLine();
Console.Write("Input \"" + filename + "\" Accepted!");
Thread.Sleep(400); //Gives visual feedback to user
output = $"{dir}{filename}.%(ext)s"; //Includes youtube-dl placeholders
bool st = false;
string? bs;
while (true)
{
    string? inp = "0";
    bool continuevar = false;

    while (continuevar == false)
    {
        Console.Clear();
        //Ascii Text "Configuration"
        bool ia = true;
#pragma warning disable CS8604 // Possible null reference argument. Will not be null, so this warning can safely be supressed.
        writeGUI(af, aq, auf, dir, ff, link, filename, ia, DATABASE_FILE);
#pragma warning restore CS8604 // Possible null reference argument.
        /* Updated to use writeGUI function, for expanded flexability and code efficiency. 
         Console.Write(" ___            ___  _                       _    _\n|  _> ___ ._ _ | | '<_> ___  _ _  _ _  ___ _| |_ <_> ___ ._ _ \n| <__/ . \\| ' || |- | |/ . || | || '_><_> | | |  | |/ . \\| ' |\n`___/\\___/|_|_||_|  |_|\\_. |`___||_|  <___| |_|  |_|\\___/|_|_|\n                       <___'\n\n");
        Console.Write($"1: Audio Format: {af}\n2: Audio Quality: {aq}\n3: Audio Conversion Format: {auf}\n4: Directory: {dir}\n5: FF-Mpeg Dir: {ff}\n6: Link: {link}\n7: File Name: {filename}\n8: Continue\n9: Exit\n\n#\\> ");*/
        if (st == true)
        {
            Console.Clear();
            //Ascii Text, "Executing..."
            writeAscii(2);
            //Console.WriteLine(" _____                           _    _\n|  ___|                         | |  (_)\n| |__  __  __  ___   ___  _   _ | |_  _  _ __    __ _\n|  __| \\ \\/ / / _ \\ / __|| | | || __|| || '_ \\  / _` |\n| |___  >  < |  __/| (__ | |_| || |_ | || | | || (_| | _  _  _\n\\____/ /_/\\_\\ \\___| \\___| \\__,_| \\__||_||_| |_| \\__, |(_)(_)(_)\n                                                 __/ |\n                                                |___/\n\n");
            output = $"{dir}{filename}.%(ext)s";
            Console.Write($"Command parsed and sent, passing youtube-dl output...\n--------------------------------------------------------\nPress ENTER once execution is complete to view the menu.\n--------------------------------------------------------\n\n");
            Process.Start(".\\youtube-dl.exe", $"-f {af} --audio-format {auf} -x --ffmpeg-location \"{ff}\" {link} --audio-quality {aq} -o \"{output}\"");
            //Console.Write("\nST is " + st + "continuevar is " + continuevar);
            Thread.Sleep(1000); //Frees up CPU for youtube-dl to start. Fixes an issue where youtube-dl wouldn't start until enter was pressed.
            bs = Console.ReadLine();
            inp = "SKIP";
        }
        else
        {
            Thread.Sleep(500);
            inp = Console.ReadLine();
        }
        switch (inp)
        {
            case "1":
                Console.Clear();
                ia = false;
                writeGUI(af, aq, auf, dir, ff, link, filename, ia, DATABASE_FILE);
                Console.Write("\nInput a new audio format: ");
                af = Console.ReadLine();
                break;
            case "2":
                Console.Clear();
                ia = false;
                writeGUI(af, aq, auf, dir, ff, link, filename, ia, DATABASE_FILE);
                Console.Write("\nInput a new audio quality: ");
                aq = Console.ReadLine();
                break;
            case "3":
                Console.Clear();
                ia = false;
                writeGUI(af, aq, auf, dir, ff, link, filename, ia, DATABASE_FILE);
                Console.Write("\nInput a new audio conversion format: ");
                auf = Console.ReadLine();
                break;
            case "4":
                Console.Clear();
                ia = false;
                writeGUI(af, aq, auf, dir, ff, link, filename, ia, DATABASE_FILE);
                Console.Write("\nInput a new directory path (A to autofill current path): ");
                dir = Console.ReadLine();
                if (dir == "A" || dir == "a") { dir = Directory.GetCurrentDirectory(); }
                output = $"{dir}{filename}.%(ext)s";
                break;
            case "5":
                Console.Clear();
                ia = false;
                writeGUI(af, aq, auf, dir, ff, link, filename, ia, DATABASE_FILE);
                Console.Write("\nInput a new FF-Mpeg Path (A to autofill current path): ");
                ff = Console.ReadLine();
                if (ff == "A" || ff == "a") { ff = Directory.GetCurrentDirectory(); }
                break;
            case "6":
                Console.Clear();
                ia = false;
                writeGUI(af, aq, auf, dir, ff, link, filename, ia, DATABASE_FILE);
                Console.Write("Input a link to the file you wish to fetch: ");
                link = Console.ReadLine();
                break;
            case "7":
                Console.Clear();
                ia = false;
                writeGUI(af, aq, auf, dir, ff, link, filename, ia, DATABASE_FILE);
                Console.Write("\nInput a name for the file output without the entension: ");
                filename = Console.ReadLine();
                output = $"{dir}{filename}.%(ext)s";
                break;
            case "8":
                continuevar = true;
                break;
            case "9":
                Console.Clear();
                //Ascii Text, "Thank You"
                writeAscii(3);
                //Console.WriteLine(" ___  _              _      _ _\n|_ _|| |_  ___ ._ _ | |__  | | | ___  _ _\n | | | . |<_> || ' || / /  \\   // . \\| | |\n |_| |_|_|<___||_|_||_\\_\\   |_| \\___/`___|\n\n");
                Console.WriteLine("Exiting... Thank you for using Youtube-DL Frontend!");
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