I recently got my hands on a Windows VM for personal uses, this can also 
be used for testing YTDL FE's Windows releases. Windows is will now be offically
supported and tested on again prior to stable releases.

Add modular command parsing through a modified version of Command Music's parser. -> Tested, released
It will originally only support menu commands with the eventual goal of supporting 
external commands (args) and internal commands (similar to iwd).

When switching from youtube-dl -> yt-dlp, it doesn't switch until the application reloads. -> Should be resolved
This causes a crash if the system calls the incorrect yt-dl executable. -> Should be resolved

Adding YT-DL presets, for instance a preset for video and another for audio. This should
allow users to switch between fetching videos and audio easily.

Add additional menus for settings, presets and other additions
- Impliment modular command parsing system -> Added, needs testing
- Add command parsing for submenus
- Integrate all parsing systems together within a menu tree