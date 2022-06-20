param (
    # height of largest column without top bar
    [string]$link,
    [string]$name
    )
    $af = 251
    $auf = "mp3"
    $ffs = "Music\Acquisition and Staging\"
    $aq = 0
    $dir = "Youtube-DL Output\"
    $locstart = "%userprofile%\Music\Acquisition and Staging\"

$dir = $locstart + $dir + $name + '.' + $auf
$args = "-f $af --audio-format $auf -x --ffmpeg-location '$locstart" + "' --audio-quality $aq -o '$dir" + "' $link"
$cdloc = "$dir"
cd 'C:\Users\benko\Music\Acquisition and Staging\'
$dirlink = "'$dir" + "' $link"
$ff = "`"C:\Users\benko\Music\Acquisition and Staging`""
./youtube-dl.exe -f $af --audio-format $auf -x --ffmpeg-location $ff $link --audio-quality $aq -o $dir