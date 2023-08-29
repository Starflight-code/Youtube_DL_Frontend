using Newtonsoft.Json;

namespace Youtube_DL_Frontnend {
    internal class DatabaseObject {
        public string workingDirectory; // dir in code
        public string ffMpegDirectory; // ff in code
        public string audioFormat; // af in code
        public string audioQuality; // aq in code
        public string audioOutputFormat; // auf in code
        public DatabaseObject() {
            workingDirectory = Directory.GetCurrentDirectory();
            ffMpegDirectory = Directory.GetCurrentDirectory();
            audioFormat = "251"; // TODO: Store in int for memory savings, lower processing overhead and more flexability
            audioQuality = "0"; // TODO: Store in int
            audioOutputFormat = "mp3";
        }

        public DatabaseObject(string workingDirectory, string ffMpegDirectory, string audioFormat, string audioQuality, string audioOutputFormat) {
            this.workingDirectory = workingDirectory;
            this.ffMpegDirectory = ffMpegDirectory;
            this.audioFormat = audioFormat;
            this.audioQuality = audioQuality;
            this.audioOutputFormat = audioOutputFormat;
        }
        public void updateSelf() {
           JsonConvert.SerializeObject(this);
            // TODO: Dump into a file
        }

        public void populateSelf() {
            // TODO: pull from database file, populate fields with values from sanitized object 
            // only if an error doesn't happen upon deserialization

        }
    }}