﻿namespace Youtube_DL_Frontnend {
    internal class DataStructures {
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
