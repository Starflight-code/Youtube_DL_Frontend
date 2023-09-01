using Newtonsoft.Json;
using System.Runtime.InteropServices;

namespace Youtube_DL_Frontend {
    internal class DatabaseObject {
        public string workingDirectory; // dir in code
        public string ffMpegDirectory; // ff in code
        public string audioFormat; // af in code
        public string audioQuality; // aq in code
        public string audioOutputFormat; // auf in code
        public DatabaseObject() {
            workingDirectory = Directory.GetCurrentDirectory();
            ffMpegDirectory = Directory.GetCurrentDirectory();
            audioFormat = "251";
            audioQuality = "0";
            audioOutputFormat = "mp3";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                ffMpegDirectory = "/usr/bin/ffmpeg";
            }
        }

        public DatabaseObject(string workingDirectory, string ffMpegDirectory, string audioFormat, string audioQuality, string audioOutputFormat) {
            this.workingDirectory = workingDirectory;
            this.ffMpegDirectory = ffMpegDirectory;
            this.audioFormat = audioFormat;
            this.audioQuality = audioQuality;
            this.audioOutputFormat = audioOutputFormat;
        }
        public async Task updateSelf() {
           string databaseSerialized = JsonConvert.SerializeObject(this); // serializes itself into JSON
           await File.WriteAllTextAsync(Constants._DATABASE_FILE, databaseSerialized); // writes JSON into file
        }

        public async Task populateSelf() {
            DatabaseObject? database = JsonConvert.DeserializeObject<DatabaseObject>(File.ReadAllText(Constants._DATABASE_FILE)); // deserializes file to a DatabaseObject
            if (database == null) {await updateSelf(); return;} 
            
            // setting values - sets the values to the DatabaseObject's values
            this.workingDirectory = database.workingDirectory;
            this.ffMpegDirectory = database.ffMpegDirectory;
            this.audioFormat = database.audioFormat;
            this.audioQuality = database.audioQuality;
            this.audioOutputFormat = database.audioOutputFormat;

            // TODO: pull from database file, populate fields with values from sanitized object 
            // only if an error doesn't happen upon deserialization

        }
    }}