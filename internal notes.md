Run tests on linux to ensure compatiblity with batch processing system.

Add user option to toggle between yt-dlb and youtube-dl executable.

I recently got my hands on a Windows VM for personal uses, this can also 
be used for testing YTDL FE's Windows releases. Windows is will now be offically
supported and tested on again prior to stable releases.

Add modular command parsing through a modified version of Command Music's parser. 
It will originally only support menu commands with the eventual goal of supporting 
external commands (args) and internal commands (similar to iwd).

## Release Delayed
youtube-dl/yt-dlp setting should be added prior to a release, since it breaks existing youtube-dl workflows.
Potential workaround, rename youtube-dl.exe to yt-dlp.exe, I'd rather not have users renaming stuff on a 
stable release.