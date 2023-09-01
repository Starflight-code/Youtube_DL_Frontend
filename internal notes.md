Run tests on linux to ensure compatiblity with batch processing system.

Add user option to toggle between yt-dlb and youtube-dl executable.

Windows will not be tested (I will no longer be using the operating system),
issues can be opened if errors pop up on windows. If I get a VM, Windows may
be official supported and tested again.

Add modular command parsing through a modified version of Command Music's parser. 
It will originally only support menu commands with the eventual goal of supporting 
external commands (args) and internal commands (similar to iwd).

Disable Linux file checking.

## Feature 1 Goal - Merge after completion

Add dynamic UI generation system, based on Command Parser's registered menu commands 
with database entries populated to the menu.