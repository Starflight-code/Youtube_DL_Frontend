namespace Youtube_DL_Frontnend {
    internal class ConstantBuilder {
        public static string buildFileName(string directory, string name) {
            return $"{directory}\\{name}.%(ext)s";
        }
        public static string buildArguments(string format, string audioFormat, string ffMpegLocation, string URL, string audioQuality, string directory, string name) {
            string fileName = buildFileName(directory, name);
            return $"-f {format} --audio-format {audioFormat} -x --ffmpeg-location \"{ffMpegLocation}\" {URL} --audio-quality {audioQuality} -o \"{fileName}\"";
        }
    }
}
