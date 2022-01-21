using Base.Core;
using Base.Defs;
using PhoenixPoint.Common.Entities;
using PhoenixPoint.Common.Entities.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SupportLibrary;

namespace SelectClassSkills
{
    internal class SettingsHelpers
    {
        internal static IDictionary<string, string[]> LoadFromGame(string emptySlotName)
        {
            IDictionary<string, string[]> result = new Dictionary<string, string[]>();

            List<SpecializationDef> specializationDefs = Support.GetDefsList<SpecializationDef>();

            Log.Info("WORKING!!!");

            foreach (var specializationDef in specializationDefs)
            {
                Log.Info(specializationDef.name);
                AbilityTrackSlot[] abilityTrack = specializationDef.AbilityTrack.AbilitiesByLevel;

                string nameAndDef = $"{specializationDef.ViewElementDef.DisplayName1.Localize()} : {specializationDef.AbilityTrack.name}";
                string[] abilityTrackSlot = new string[abilityTrack.Length];
                Log.Info(nameAndDef);
                for (int i = 0; i < abilityTrack.Length; i++)
                {
                    if (abilityTrack[i].Ability != null)
                    {

                        if (abilityTrack[i].Ability.ViewElementDef != null)
                        {
                            abilityTrackSlot[i] = $"{abilityTrack[i].Ability.ViewElementDef.DisplayName1.Localize()} : {abilityTrack[i].Ability.name}"; 
                        }
                        else
                        {
                            abilityTrackSlot[i] = $"{abilityTrack[i].Ability.name}";
                        }
                        Log.Info($"Added: {abilityTrackSlot[i]}");
                    }
                    else
                    {
                        abilityTrackSlot[i] = emptySlotName;
                        Log.Info($"slot{i} is empty.");
                    }
                }

                result.Add(nameAndDef, abilityTrackSlot);
            }

            return result;
        }

        internal static IDictionary<string, string[]> ChangeSecondSpecializationLevel(IDictionary<string, string[]> inputDictionary, int level, string progressionName, string emptySlotName)
        {
            Log.Info("change running!!!!");
            LevelProgressionDef levelProgressionDef = Support.GetDef<LevelProgressionDef>(progressionName);
            Log.Info("level prog found!!!!");
            int maxLevel = levelProgressionDef.MaxLevel;
            if (level > maxLevel)
            {
                Log.Warning("Second specialization level set higher, than Maximum soldier level.");
                Log.Warning("No changes to skills done!!!");

                return inputDictionary;
            }

            IDictionary<string, string[]> output = new Dictionary<string, string[]>();
            foreach (var entry in inputDictionary)
            {
                Log.Info($"entry start: {entry.Key}");
                output.Add(entry.Key, ChangeSkillList(entry.Value, level, emptySlotName));
                Log.Info("entry added");
            }

            return output;
        }

        private static string[] ChangeSkillList(string[] skillList, int level, string emptySlotName)
        {
            List<string> tempList = skillList.Where(x => x != emptySlotName).ToList();

            tempList.Insert(level - 1, emptySlotName);
            Log.Info("list of templist:");
            foreach (var item in tempList)
            {
                Log.Info(item);
            }

            string[] output = new string[skillList.Length];
            for (int i = 0; i < output.Length; i++)
            {
                output[i] = tempList[i];
                Log.Info($"output{i}: {output[i]}, {tempList[i]}");
            }

            return output;
        }
    }
}
