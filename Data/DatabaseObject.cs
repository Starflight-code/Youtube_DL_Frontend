using Newtonsoft.Json;
using System.Runtime.InteropServices;

namespace Youtube_DL_Frontend.Data
{
    internal class DatabaseObject
    {
        public string workingDirectory;
        public string ffMpegDirectory;
        public string audioFormat;
        public string audioQuality;
        public string audioOutputFormat;

        public bool youtubeDLP;
        public DatabaseObject()
        {
            workingDirectory = Directory.GetCurrentDirectory();
            ffMpegDirectory = Directory.GetCurrentDirectory();
            audioFormat = "251";
            audioQuality = "0";
            audioOutputFormat = "mp3";
            youtubeDLP = false;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                ffMpegDirectory = "/usr/bin/ffmpeg";
            }
        }

        public DatabaseObject(string workingDirectory, string ffMpegDirectory, string audioFormat, string audioQuality, string audioOutputFormat, bool youtubeDLP)
        {
            this.workingDirectory = workingDirectory;
            this.ffMpegDirectory = ffMpegDirectory;
            this.audioFormat = audioFormat;
            this.audioQuality = audioQuality;
            this.audioOutputFormat = audioOutputFormat;
            this.youtubeDLP = youtubeDLP;
        }
        public async Task updateSelf(bool newDatabase = false)
        {
            string databaseSerialized = JsonConvert.SerializeObject(this); // serializes itself into JSON
            if (newDatabase)
            {
                File.WriteAllText(Constants._DATABASE_FILE, databaseSerialized); // writes JSON into file
            }
            else
            {
                await File.WriteAllTextAsync(Constants._DATABASE_FILE, databaseSerialized); // writes JSON into file
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
                this.audioFormat = database.audioFormat;
                this.audioQuality = database.audioQuality;
                this.audioOutputFormat = database.audioOutputFormat;
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