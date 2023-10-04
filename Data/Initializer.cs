namespace Youtube_DL_Frontend.Data
{
    internal class Initializer
    {
        public static void initalizeStartPresets()
        {
            DatabaseObject videoPreset = new DatabaseObject(Statics.buildPath(Constants._PRESET_DIRECTORY + "video"))
            {
                format = "bestvideo+bestaudio",
                outputFormat = "mp4",
            };
            DatabaseObject audioPreset = new DatabaseObject(Statics.buildPath(Constants._PRESET_DIRECTORY + "audio"));
            DatabaseObject subtitlePreset = new DatabaseObject(Statics.buildPath(Constants._PRESET_DIRECTORY + "video"))
            {
                format = "srt"
            };
        }
    }
}