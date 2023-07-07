namespace Youtube_DL_Frontnend {
    internal class DataStructures {
        public struct YoutubeDLParamInfo {
            public string workingDirectory; // dir in code
            public string ffMpegDirectory; // ff in code
            public string audioFormat; // af in code
            public string audioQuality; // aq in code
            public string audioOutputFormat; // auf in code
            public YoutubeDLParamInfo(string directory, string ffMpegDirectory, string audioFormat, string audioQuality, string audioOutputFormat) {
                workingDirectory = directory;
                this.ffMpegDirectory = ffMpegDirectory;
                this.audioFormat = audioFormat;
                this.audioQuality = audioQuality;
                this.audioOutputFormat = audioOutputFormat;
            }
        }
        public enum commandToExecute {
            audioFormat,
            audioQuality,
            audioOutputFormat,
            directory,
            ffDirectory,
            link,
            filename,
            batch,
            goOn,
            exit
        };

        public enum errorMessages {
            databaseReset,
            filesNotFound

        }
    }
}
