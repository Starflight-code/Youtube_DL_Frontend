using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;

namespace Youtube_DL_Frontend.Data {
    internal class DatabaseObject {
        public enum presetType {
            video,
            audio,
            subtitle
        }
        public string workingDirectory;
        public string ffMpegDirectory;
        public string format;
        public string audioQuality;
        public string outputFormat;
        public bool youtubeDLP;
        public string presetName;
        public string fullPath;
        public presetType type;
        public DatabaseObject() {
            workingDirectory = Directory.GetCurrentDirectory();
            ffMpegDirectory = Directory.GetCurrentDirectory();
            format = "251";
            audioQuality = "0";
            outputFormat = "mp3";
            youtubeDLP = false;
            type = presetType.audio;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                ffMpegDirectory = "/usr/bin/ffmpeg";
            }
            presetName = "";
            this.fullPath = "";
        }
        public DatabaseObject(string fullPath) {
            workingDirectory = Directory.GetCurrentDirectory();
            ffMpegDirectory = Directory.GetCurrentDirectory();
            format = "251";
            audioQuality = "0";
            outputFormat = "mp3";
            youtubeDLP = false;
            type = presetType.audio;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                ffMpegDirectory = "/usr/bin/ffmpeg";
            }
            presetName = "";
            this.fullPath = fullPath;
        }

        public DatabaseObject(string workingDirectory, string ffMpegDirectory, string audioFormat, string audioQuality, string audioOutputFormat, bool youtubeDLP, string presetName, presetType type, string fullPath) {
            this.workingDirectory = workingDirectory;
            this.ffMpegDirectory = ffMpegDirectory;
            this.format = audioFormat;
            this.audioQuality = audioQuality;
            this.outputFormat = audioOutputFormat;
            this.youtubeDLP = youtubeDLP;
            this.presetName = presetName;
            this.type = type;
            this.fullPath = fullPath;
        }

        public void generalDatabaseUpdate(GeneralDatabase data) {
            ffMpegDirectory = data.ffMpegDirectory;
            youtubeDLP = data.youtubeDLP;
        }
        public async Task updateSelf(bool newDatabase = false) {
            string databaseSerialized = JsonConvert.SerializeObject(this); // serializes itself into JSON
            if (newDatabase) {
                File.WriteAllText(fullPath, databaseSerialized); // writes JSON into file
            }
            else {
                await File.WriteAllTextAsync(fullPath, databaseSerialized); // writes JSON into file
            }
        }

        public async Task populateSelf() {
            try {
                DatabaseObject? database = JsonConvert.DeserializeObject<DatabaseObject>(File.ReadAllText(fullPath)); // deserializes file to a DatabaseObject
                if (database == null) { await updateSelf(); return; }

                // setting values - sets the values to the DatabaseObject's values
                this.workingDirectory = database.workingDirectory;
                this.ffMpegDirectory = database.ffMpegDirectory;
                this.format = database.format;
                this.audioQuality = database.audioQuality;
                this.outputFormat = database.outputFormat;
                this.youtubeDLP = database.youtubeDLP;
                this.presetName = database.presetName;
            }
            catch {
                File.Delete(fullPath);
                await updateSelf(true);
            }

        }
    }
}