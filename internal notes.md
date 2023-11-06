I recently got my hands on a Windows VM for personal uses, this can also 
be used for testing YTDL FE's Windows releases. Windows is will now be offically
supported and tested on again prior to stable releases.

Add modular command parsing through a modified version of Command Music's parser. -> Tested, released
It will originally only support menu commands with the eventual goal of supporting 
external commands (args) and internal commands (similar to iwd).

Adding YT-DL presets, for instance a preset for video and another for audio. This should
allow users to switch between fetching videos and audio easily. - Implimented, needs testing

Add additional menus for settings, presets and other additions
- Impliment modular command parsing system -> Tested and working
- Add command parsing for submenus -> Added, working
- Integrate all parsing systems together within a menu tree -> Added, working (cleanup needed before release)
- Test system to ensure functionality -> Testing started, core functionality confirmed. Test with a subtitle, video and audio file fetch.
- Add preset system into settings submenu (requires database rework) - DONE

TODO - Run a test on program functionality, checking for a few edge cases before release
Video preset does not work citing "yt-dlp: error: invalid audio format "mp4" given", likely an issue with the command builder in ExternalInterface
GUI is not updated upon link and filename changes after initial system initialization - RESOLVED
Preset index is not being saved after system termination, save to general config recommended - Added

Audio Fetching - Working
Subtitle Fetching - Working
Video Fetching - Working

Fixed
- An issue where YT-DL-FE would delete the merge-output file after generating it
- An issue where the GUI would not update
- An issue where presets would not contain the correct type data