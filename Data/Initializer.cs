namespace Youtube_DL_Frontend.Data {
    internal class Initializer {
        public static async void initalizeStartPresets() {
            Directory.CreateDirectory(Statics.buildPath(Constants._PRESET_DIRECTORY));
            DatabaseObject videoPreset = new DatabaseObject(Statics.buildPath(Constants._PRESET_DIRECTORY + "video")) {
                presetName = "video",
                format = "bestvideo+bestaudio",
                outputFormat = "mp4"
            };
            DatabaseObject audioPreset = new DatabaseObject(Statics.buildPath(Constants._PRESET_DIRECTORY + "audio")) {
                presetName = "audio"
            };
            DatabaseObject subtitlePreset = new DatabaseObject(Statics.buildPath(Constants._PRESET_DIRECTORY + "subtitle")) {
                format = "srt",
                outputFormat = "srt",
                audioQuality = "N/A",
                presetName = "subtitle"
            };
            await videoPreset.updateSelf();
            await audioPreset.updateSelf();
            await subtitlePreset.updateSelf();
        }
    }
}