using Newtonsoft.Json;
using System.Runtime.InteropServices;

namespace Youtube_DL_Frontend.Data
{
    internal class DatabaseObject
    {
        public string workingDirectory;
        public string ffMpegDirectory;
        public string format;
        public string audioQuality;
        public string outputFormat;
        public bool youtubeDLP;
        public string presetName;
        public string fullPath;
        public PresetManager.presetType type;
        public DatabaseObject(string fullPath)
        {
            workingDirectory = Directory.GetCurrentDirectory();
            ffMpegDirectory = Directory.GetCurrentDirectory();
            format = "251";
            audioQuality = "0";
            outputFormat = "mp3";
            youtubeDLP = false;
            type = PresetManager.presetType.audio;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                ffMpegDirectory = "/usr/bin/ffmpeg";
            }
            presetName = "";
            this.fullPath = fullPath;
        }

        public DatabaseObject(string workingDirectory, string ffMpegDirectory, string audioFormat, string audioQuality, string audioOutputFormat, bool youtubeDLP, PresetManager.presetType type, string fullPath)
        {
            this.workingDirectory = workingDirectory;
            this.ffMpegDirectory = ffMpegDirectory;
            this.format = audioFormat;
            this.audioQuality = audioQuality;
            this.outputFormat = audioOutputFormat;
            this.youtubeDLP = youtubeDLP;
            presetName = "";
            this.type = type;
            this.fullPath = fullPath;
        }
        public void generalDatabaseUpdate(GeneralDatabase data)
        {
            ffMpegDirectory = data.ffMpegDirectory;
            youtubeDLP = data.youtubeDLP;
        }
        public async Task updateSelf(bool newDatabase = false)
        {
            string databaseSerialized = JsonConvert.SerializeObject(this); // serializes itself into JSON
            if (newDatabase)
            {
                File.WriteAllText(fullPath, databaseSerialized); // writes JSON into file
            }
            else
            {
                await File.WriteAllTextAsync(fullPath, databaseSerialized); // writes JSON into file
            }
        }

        public async Task populateSelf()
        {
            try
            {
                DatabaseObject? database = JsonConvert.DeserializeObject<DatabaseObject>(File.ReadAllText(Constants._DATABASE_FILE)); // deserializes file to a DatabaseObject
                if (database == null) { await updateSelf(); return; }

                // setting values - sets the values to the DatabaseObject's values
                this.workingDirectory = database.workingDirectory;
                this.ffMpegDirectory = database.ffMpegDirectory;
                this.format = database.format;
                this.audioQuality = database.audioQuality;
                this.outputFormat = database.outputFormat;
                this.youtubeDLP = database.youtubeDLP;
            }
            catch
            {
                File.Delete(Constants._DATABASE_FILE);
                await updateSelf(true);
            }
            // TODO: pull from database file, populate fields with values from sanitized object 
            // only if an error doesn't happen upon deserialization

        }
    }
}