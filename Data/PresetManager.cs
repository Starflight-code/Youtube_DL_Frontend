namespace Youtube_DL_Frontend.Data
{
    class PresetManager
    {
        public enum presetType
        {
            audio,
            video,
            subtitle
        }
        List<preset> presets = new List<preset>();
        int activePresetIndex;
        public struct preset
        {
            public DatabaseObject database;
            public string name;
            public preset(DatabaseObject database, string name)
            {
                this.database = database;
                this.name = name;
            }
        }

        public PresetManager()
        {
            activePresetIndex = 0;
        }
        public PresetManager(int activeIndex)
        {
            if (activeIndex < presets.Count - 1)
            {
                activePresetIndex = activeIndex;
            }
            else
            {
                activePresetIndex = 0;
            }
        }

        public void importAll()
        {
            IEnumerable<string> presets = Directory.EnumerateFiles(Data.Constants._PRESET_DIRECTORY);
            foreach (string preset in presets)
            {
                import(preset);
            }
        }

        public async void import(string filePath)
        {
            DatabaseObject temp = new DatabaseObject(filePath);
            await temp.populateSelf();
            presets.Add(new preset(temp, temp.presetName));
        }
        public async void updateAll()
        {
            for (int i = 0; i < presets.Count(); i++)
            {
                await presets[i].database.updateSelf();
            }
        }

        public void switchActive(int activeIndex)
        {
            if (activeIndex < presets.Count - 1)
            {
                activePresetIndex = activeIndex;
            }
            else
            {
                activePresetIndex = 0;
            }
        }

        public preset getActive()
        {
            return presets[activePresetIndex];
        }

        public async void updateActive()
        {
            await presets[activePresetIndex].database.populateSelf();
        }

        public async void generalDatabaseUpdate(GeneralDatabase data)
        {
            for (int i = 0; i < presets.Count(); i++)
            {
                presets[i].database.generalDatabaseUpdate(data);
                await presets[i].database.updateSelf();
            }
        }

        public List<preset> getPresets()
        {
            return presets;
        }


    }
}