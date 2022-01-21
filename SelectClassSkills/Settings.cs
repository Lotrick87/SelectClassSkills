using SupportLibrary;
using System.Collections.Generic;

namespace SelectClassSkills
{
    internal class Settings
    {
        public bool LoadDataFromGame = true;
        public int SecondSpecializationLevel = 4;
        public bool ChangeSecondSpecializationLevel = false;
        public bool CustomLevelProgression = false;
        public string EmptySlotName = "EMPTY_SLOT";

        public IDictionary<string, string[]> classesList = new Dictionary<string, string[]>();

        public int ConfigVersion = 1;

        internal string _vanillaLevelProgressionName = "LevelProgressionDef";

        internal Settings Update()
        {
            bool saveData = false;
            if (LoadDataFromGame)
            {
                classesList = SettingsHelpers.LoadFromGame(EmptySlotName);
                Log.Info("Loading data!!!");
                LoadDataFromGame = false;
                saveData = true;
            }

            if (ChangeSecondSpecializationLevel)
            {
                Log.Info("Changing spec level!!!");
                if (classesList.Count == 0)
                {
                    Log.Warning("No data in classesList - Loading data from game.");
                    classesList = SettingsHelpers.LoadFromGame(EmptySlotName);
                }

                classesList =  SettingsHelpers.ChangeSecondSpecializationLevel(classesList, SecondSpecializationLevel, _vanillaLevelProgressionName, EmptySlotName);

                ChangeSecondSpecializationLevel = false;
                saveData = true;
            }

            if (saveData)
            {
                Mod._api?.Invoke("config save", this);
            }

            return this;
        }
    }
}
