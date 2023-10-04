namespace Youtube_DL_Frontend
{
    internal class ConstantBuilder
    {
        public static string buildFileName(string directory, string name)
        {
            return $"{directory}\\{name}.%(ext)s";
        }
        public static string buildArguments(Data.DatabaseObject data, string URL, string name)
        {
            string fileName = buildFileName(data.workingDirectory, name);
            switch (data.type)
            {
                case Data.PresetManager.presetType.audio:
                    return Data.Statics.buildPath($"-f {data.format} --audio-format {data.outputFormat} -x --ffmpeg-location \"{data.ffMpegDirectory}\" {URL} --audio-quality {data.audioQuality} -o \"{fileName}\"");
                case Data.PresetManager.presetType.video:
                    return Data.Statics.buildPath($"-f {data.format} --merge-output-format {data.outputFormat} -x --ffmpeg-location \"{data.ffMpegDirectory}\" {URL} -o \"{fileName}\"");
                case Data.PresetManager.presetType.subtitle:
                    return Data.Statics.buildPath($"--write-sub --sub-format {data.format} {URL} -o \"{fileName}\"");
            }
            return "";

        }
    }
}
