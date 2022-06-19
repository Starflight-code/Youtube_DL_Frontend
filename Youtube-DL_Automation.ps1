#---------------START OF USER EDITABLE PARAMETERS---------------
$aquformat = 251
$audioformat = "mp3"
$ffmpegloc = "C:\Users\benko\Music\Acquisition and Staging"
$AudioQual = 0

#Variable should have the following syntax, do not include a \ at the end. Syntax: "C:\Folder\Subfolder"
$outdir = "C:\Users\benko\Music\Acquisition and Staging\Youtube-DL Output"

# Batch Section
$movedir = "C:\Users\benko\Music\Vocaloid\"

#---------------END OF USER EDITABLE PARAMETERS---------------

#Sets up global variables that work between functions
$agt = $null
$agtb = $null
$second = $null
$advinit = $null
$advexit = $null
#Wait GUI, Specify $message and wait $time variables to run ($time * 4 is the total execution time for the GUI)
function batch {
$batchask = Read-Host "Run pre-specified batch script? (y/n)"
if ($batchask = "y") {
move $dir $movedir
Write-Host "Moved File Automatically to " + $movedir + $titleform + '.' + $audioformat
Read-Host "Press Enter to Continue"
}
}
function wait {
cls
Write-Output "$message [   ]"
Start-Sleep -Milliseconds $time
cls
Write-Output "$message [-  ]"
Start-Sleep -Milliseconds $time
cls
Write-Output "$message [-- ]"
Start-Sleep -Milliseconds $time
cls
Write-Output "$message [---]"
Start-Sleep -Milliseconds $time
cls
}
function adv-options {
if ($advinit = 0) {
cls
}
$advinit = 0
Write-Host "----------Options Menu----------"
Write-Host "1: Open Output Directory"
Write-Host "2: Run Pre-Coded Batch Scripts"
Write-Host "3: Fetch Another File"
Write-Host "4: Exit Program"
Write-Host "--------------------------------"
Write-Host ""
Read-Host "Which option would you like to select? (1/2/3/4)"
switch ($adv) {
1 {
explorer.exe $outdir
}
2 {
if ($agtb = 1) {
$agtb = 0
batch
} else {
Write-Host "Oops, This option requires a user generated title."
}}
3 {
$advexit = 1
}
4 {
$message = "Exiting Program..."
$time = 250
wait
exit
}
}
}
#Function to re-execute the script after execution. Allows faster file aquisition.
#Unused at this point and will be removed later on to save space.
function redo {
if ($second = 1) {
cls
}
$answer = Read-Host "Would you like to convert another file? (y/n)" 
switch ($answer) {
y {}
n {
$message = "Exiting Program..."
$time = 250
wait
exit
}
Default {
Write-Host ""
Write-Host "----------------------------"
Write-Host "Answer must be   y   or   n"
Write-Host "----------------------------"
Write-Host ""
Start-Sleep -Seconds 0.5
cls
Write-Host "Input Error: Exiting Program, re-execute to fetch another file."
Start-Sleep -Seconds 1
exit
}}}
function fetch {
Write-Host "Specify the Universal Resource Locator (URL) Address which contains"
Write-Host "the content you wish to fetch. Example: https://www.youtube.com/123"
Write-Host ""
$url = Read-Host "URL"
cls
$titleform = "%(title)s-%(id)s"
$agt = Read-Host "Automatically Generate Title? (y/n)"
if ($agt = "n") {
$agtb = 1
$titleform = Read-Host "Specify a custom title excluding extention (No .mp3 or otherwise required). Synatx: Title"
$agt = $null
}
cls

Write-Host "Parsing Command and Preparing for Youtube-DL and FF-MPEG Execution..."
Write-Host "-----------------------------------------------------------------------"
Write-Host "Youtube-DL Args: Format $aquformat, Audio Format $audioformat, Quality $audioqual"
Write-Host "URL: $url"
Write-Host "FF-Mpeg: $ffmpegloc"
Write-Host "Output: $outdir"
Write-Host "-----------------------------------------------------------------------"
Write-Host "Passing through Youtube-DL Output..."
Write-Host ""
Write-Host ""
Start-Sleep -Seconds 0.5
$dir = $outdir + '\' + $titleform + '.' + $audioformat
./youtube-dl.exe -f $aquformat --audio-format $audioformat -x --ffmpeg-location "$ffmpegloc" $url --audio-quality $audioqual -o $dir
Write-Host ""
Write-Host ""
Write-Host "------------------------------------------------------------------------------------------------"
Write-Host "Execution Complete. Your file has the following path and has been converted to $audioformat!"
Write-Host "     $dir"
Write-Host "------------------------------------------------------------------------------------------------"
}
cls
cd 'C:\Users\benko\Music\Acquisition and Staging'
Write-Host "----------------------------"
Write-Host "Youtube-DL Automation Script"
Write-Host "----------------------------"
Write-Host ""
Write-Host "This script assumes several parameters by default, edit them with Powershell ISE or a text editor."
Write-Host ""
$second = 0
while ($true) {
fetch
redo
$second = 1
}