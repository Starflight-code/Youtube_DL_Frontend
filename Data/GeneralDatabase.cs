using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace Youtube_DL_Frontend.Data {
    internal class GeneralDatabase {
        public bool youtubeDLP;
        public string ffMpegDirectory;
        public GeneralDatabase() {
            youtubeDLP = false;
            ffMpegDirectory = Directory.GetCurrentDirectory();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                ffMpegDirectory = "/usr/bin/ffmpeg";
            }
        }
        public async Task updateSelf(bool newDatabase = false) {
            string databaseSerialized = JsonConvert.SerializeObject(this); // serializes itself into JSON
            if (newDatabase) {
                File.WriteAllText(Constants._DATABASE_FILE, databaseSerialized); // writes JSON into file
            }
            else {
                await File.WriteAllTextAsync(Constants._DATABASE_FILE, databaseSerialized); // writes JSON into file
            }
        }

        public async Task populateSelf() {
            try {
                GeneralDatabase? database = JsonConvert.DeserializeObject<GeneralDatabase>(File.ReadAllText(Constants._DATABASE_FILE)); // deserializes file to a DatabaseObject
                if (database == null) { await updateSelf(); return; }

                // setting values - sets the values to the DatabaseObject's values
                this.youtubeDLP = database.youtubeDLP;
                this.ffMpegDirectory = database.ffMpegDirectory;
            }
            catch {
                File.Delete(Constants._DATABASE_FILE);
                await updateSelf(true);
            }

        }
    }
}