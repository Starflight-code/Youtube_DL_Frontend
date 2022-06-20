using System.Diagnostics;

/*string beyondupdir = "";
int af = 251;
string auf = "mp3";
string ffs = "\'Music\\Acquisition and Staging\'";
string dir = "\'\\Music\\Acquisition and Staging\\Youtube-DL Output\'";*/
Console.Write("Input a link to the file you wish to fetch: ");
string? link = Console.ReadLine();
Console.Write("\nInput a name for the file output without the entension: ");
string? filename = Console.ReadLine();
Process.Start("cmd.exe", $"/c powershell.exe -executionpolicy bypass -file \"%userprofile%\\Music\\Acquisition and Staging\\hook.ps1\" -name \"{filename}\" -link {link}");

/*
 * powershell.exe -executionpolicy bypass -file "%userprofile%\Music\Acquisition and Staging\hook.ps1" -Link https://www.youtube.com/watch?v=5tCDLBnDi0s -name 'SAO'
 */