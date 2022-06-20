using System.Diagnostics;

int af = 251;
int aq = 0;
string auf = "mp3";

Console.Write("Input a link to the file you wish to fetch: ");
string? link = Console.ReadLine();
Console.Write("\nInput a name for the file output without the entension: ");
string? filename = Console.ReadLine();
//Process.Start("cmd.exe", $"/c powershell.exe -executionpolicy bypass -file \"%userprofile%\\Music\\Acquisition and Staging\\hook.ps1\" -name \"{filename}\" -link {link}");
string locstart = "C:\\Users\\benko\\Music\\Acquisition and Staging\\";
string dir = $"{locstart}Youtube-DL Output\\{filename}.{auf}";
string ff = "C:\\Users\\benko\\Music\\Acquisition and Staging";
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
Process.Start("C:\\Users\\benko\\Music\\Acquisition and Staging\\youtube-dl.exe", $"-f {af} --audio-format {auf} -x --ffmpeg-location \"{ff}\" {link} --audio-quality {aq} -o \"{dir}\"");