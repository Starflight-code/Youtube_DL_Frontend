namespace Youtube_DL_Frontend
{
    internal class ConstantBuilder
    {
        public static string buildFileName(string directory, string name)
        {
            return $"{directory}\\{name}.%(ext)s";
        }
        public static string buildArguments(DatabaseObject data, string URL, string name)
        {
            string fileName = buildFileName(data.workingDirectory, name);
            return Statics.buildPath($"-f {data.audioFormat} --audio-format {data.audioOutputFormat} -x --ffmpeg-location \"{data.ffMpegDirectory}\" {URL} --audio-quality {data.audioQuality} -o \"{fileName}\"");
        }
    }
}
