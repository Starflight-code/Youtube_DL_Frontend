namespace Youtube_DL_Frontnend {
    internal class ConstantBuilder {
        public static string buildFileName(string directory, string name) {
            return $"{directory}\\{name}.%(ext)s";
        }
        public static string buildArguments(string format, string audioFormat, string ffMpegLocation, string URL, string audioQuality, string directory, string name) {
            string fileName = buildFileName(directory, name);
            return $"-f {format} --audio-format {audioFormat} -x --ffmpeg-location \"{ffMpegLocation}\" {URL} --audio-quality {audioQuality} -o \"{fileName}\"";
        }
        public static string buildArguments(DataStructures.YoutubeDLParamInfo parameters, string URL, string name) {
            string fileName = buildFileName(parameters.workingDirectory, name);
            return $"-f {parameters.audioFormat} --audio-format {parameters.audioOutputFormat} -x --ffmpeg-location \"{parameters.ffMpegDirectory}\" {URL} --audio-quality {parameters.audioQuality} -o \"{fileName}\"";
        }
        public static string[] buildDatabaseFile(DataStructures.YoutubeDLParamInfo parameters) {
            return new string[] { Constants._DATABASE_PREPEND, parameters.audioFormat, parameters.audioQuality, parameters.audioOutputFormat, parameters.workingDirectory, parameters.ffMpegDirectory };
        }
    }
}
