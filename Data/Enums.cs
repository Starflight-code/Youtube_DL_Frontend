namespace Youtube_DL_Frontend.Data
{
    internal class Enums
    {
        public enum commandToExecute
        {
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

        public enum errorMessages
        {
            databaseReset,
            filesNotFound

        }

        public enum platform
        {
            windows,
            linux
        }

        public enum parsers
        {
            main,
            settings
        }
    }
}
