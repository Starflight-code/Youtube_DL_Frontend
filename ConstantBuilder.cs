namespace Youtube_DL_Frontend {
    internal class ConstantBuilder {
        public static string buildFileName(string directory, string name) {
            return $"{directory}\\{name}.%(ext)s";
        }
        /*public static string buildArguments(string format, string audioFormat, string ffMpegLocation, string URL, string audioQuality, string directory, string name) {
            string fileName = buildFileName(directory, name);
            return $"-f {format} --audio-format {audioFormat} -x --ffmpeg-location \"{ffMpegLocation}\" {URL} --audio-quality {audioQuality} -o \"{fileName}\"";
        }*/
        public static string buildArguments(DatabaseObject data, string URL, string name) {
            string fileName = buildFileName(data.workingDirectory, name);
            return Statics.buildPath($"-f {data.audioFormat} --audio-format {data.audioOutputFormat} -x --ffmpeg-location \"{data.ffMpegDirectory}\" {URL} --audio-quality {data.audioQuality} -o \"{fileName}\"");
        }
        /*public static string buildArgumentsLinux(DatabaseObject data, string URL, string name) {
            string fileName = buildFileName(data.workingDirectory, name);
            return Statics.buildPath($"-f {data.audioFormat} --audio-format {data.audioOutputFormat} -x {URL} --ffmpeg-location /usr/bin/ffmpeg --audio-quality {data.audioQuality} -o \"{fileName}\"");
        }*/
    }
}
