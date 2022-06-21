using System.Diagnostics;

string? af = "251";
string? aq = "0";
string? auf = "mp3";

Console.Write("Input a link to the file you wish to fetch: ");
string? link = Console.ReadLine();
Console.Write("\nInput a name for the file output without the entension: ");
string? filename = Console.ReadLine();
string? dir = $"FULL OUTPUT PATH HERE";
string output = $"{dir}{filename}.%(ext)s";
string? ff = "FFMPEG PATH HERE";
bool st = false;
string? bs;
/*
 * powershell.exe -executionpolicy bypass -file "%userprofile%\Music\Acquisition and Staging\hook.ps1" -Link https://www.youtube.com/watch?v=5tCDLBnDi0s -name 'SAO'
 
Process? p = Process.Start(new ProcessStartInfo(@"C:\Users\benko\Music\Acquisition and Staging\youtube-dl.exe")
{
    Arguments = $"-f {af} --audio-format {auf} -x --ffmpeg-location {ff} {link} --audio-quality {aq} -o {dir}",
    WindowStyle = ProcessWindowStyle.Normal,
    CreateNoWindow = false,
    UseShellExecute = false,
    RedirectStandardError = true
});*/
while (true)
{
    string? inp = "0";
    bool continuevar = false;

    while (continuevar == false)
    {
        Console.Clear();
        Console.Write($"----------Configuration----------\n1: Audio Format: {af}\n2: Audio Quality: {aq}\n3: Audio Conversion Format: {auf}\n4: Directory: {dir}\n5: FF-Mpeg Dir: {ff}\n6: Link: {link}\n7: File Name: {filename}\n8: Continue\n9: Exit\n---------------------------------\n#\\> ");
        if (st == true)
        {
            Console.Clear();
            Console.Write($"\nCommand parsed successfully, passing youtube-dl output...\nPress ENTER once exection is complete to view the menu.\n");
            Process.Start("C:\\Users\\benko\\Music\\Acquisition and Staging\\youtube-dl.exe", $"-f {af} --audio-format {auf} -x --ffmpeg-location \"{ff}\" {link} --audio-quality {aq} -o \"{output}");
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
                Console.Write("\nInput a new audio format:");
                af = Console.ReadLine();
                break;
            case "2":
                Console.Write("\nInput a new audio quality:");
                aq = Console.ReadLine();
                break;
            case "3":
                Console.Write("\nInput a new audio conversion format:");
                auf = Console.ReadLine();
                break;
            case "4":
                Console.Write("\nInput a new directory path:");
                dir = Console.ReadLine();
                output = $"{dir}{filename}.%(ext)s";
                break;
            case "5":
                Console.Write("\nInput a new FF-Mpeg Path:");
                ff = Console.ReadLine();
                break;
            case "6":
                Console.Write("Input a link to the file you wish to fetch: ");
                link = Console.ReadLine();
                continuevar = true;
                break;
            case "7":
                Console.Write("\nInput a name for the file output without the entension: ");
                filename = Console.ReadLine();
                output = $"{dir}{filename}.%(ext)s";
                break;
            case "8":
                continuevar = true;
                break;
            case "9":
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