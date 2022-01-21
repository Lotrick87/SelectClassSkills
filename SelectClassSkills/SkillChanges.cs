using Base.Core;
using Base.Defs;
using Base.Entities.Abilities;
using PhoenixPoint.Common.Entities;
using PhoenixPoint.Common.Entities.Characters;
using PhoenixPoint.Tactical.Entities.Abilities;
using SupportLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelectClassSkills
{
    internal class SkillChanges
    {
        private static readonly Settings _settings = new Settings();

        internal static void ApplyChanges()
        {
            ChangeSkills(Mod._settings.classesList);

            ChangeLevelProgressionDef(Mod._settings.SecondSpecializationLevel, Mod._settings._vanillaLevelProgressionName, Mod._settings.CustomLevelProgression);

            if (Mod._settings.CustomLevelProgression)
            {
                AddCustomLevelProgression(Mod._settings.SecondSpecializationLevel, Mod._settings._vanillaLevelProgressionName);
            }
        }

        private static void AddCustomLevelProgression(int level, string vanillaDefName)
        {
            LevelProgressionDef levelProgressionDef = Support.GetDef<LevelProgressionDef>(vanillaDefName);

            LevelProgressionDef customLevelProgressionDef = new LevelProgressionDef()
            {
                name = "SelectSkills_LevelProgressionDef",
                Guid = System.Guid.NewGuid().ToString(),
                LevelXPTable = levelProgressionDef.LevelXPTable,
                SkillpointsPerLevel = levelProgressionDef.SkillpointsPerLevel,
                SecondSpecializationLevel = level,
                SecondSpecializationSpCost = levelProgressionDef.SecondSpecializationSpCost,

            };

            //customLevelProgressionDef.Guid = "SelectSkillsCustom_9d0e-aa0e3be81e08";

            Support.CreateDef<LevelProgressionDef>(customLevelProgressionDef);
            Log.Info("created def");
            //testing below
            //LevelProgressionDef lpDef = Support.GetDef<LevelProgressionDef>("SelectSkills_LevelProgressionDef");
            //Log.Info($"name: {lpDef.name}   guid: {lpDef.Guid}   level: {lpDef.SecondSpecializationLevel}");

            List<LevelProgressionDef> levelProgressionDefs = Support.GetDefsList<LevelProgressionDef>();
            Log.Info("levelprogression alll:");
            foreach (var item in levelProgressionDefs)
            {
                Log.Info($"name: {item.name}   guid: {item.Guid}   level: {item.SecondSpecializationLevel}");
            }
        }

        private static void ChangeLevelProgressionDef(int level, string vanillaDefName, bool customDef)
        {
            LevelProgressionDef levelProgressionDefVanilla = Support.GetDef<LevelProgressionDef>(vanillaDefName);

            if (levelProgressionDefVanilla.SecondSpecializationLevel != level && customDef == false)
            {
                Log.Info("second spec changed");
                levelProgressionDefVanilla.SecondSpecializationLevel = level;
            }
        }

        private static void ChangeSkills(IDictionary<string, string[]> classesList)
        {
            foreach (var entry in classesList)
            {
                string classDefName = Support.GetDefName(entry.Key);

                AbilityTrackSlot[] abilityTrackSlot = Support.GetDef<AbilityTrackDef>(classDefName).AbilitiesByLevel;

                string[] skillList = entry.Value;
                for (int i = 0; i < abilityTrackSlot.Length; i++)
                {
                    string abilityToFind = Support.GetDefName(skillList[i]);

                    TacticalAbilityDef abilityDef = Support.GetDef<TacticalAbilityDef>(abilityToFind);
                    if (abilityDef == null && abilityToFind != _settings.EmptySlotName)
                    {
                        Log.Warning($"{abilityToFind} was not found in TacticalAbilityDef and was not added!");
                    }
                    else if (abilityTrackSlot[i].Ability != abilityDef)
                    {
                        abilityTrackSlot[i].Ability = abilityDef;

                        if (abilityTrackSlot[i].Ability != null)
                        {
                            Log.Info($"ability does not match writing new: {abilityTrackSlot[i].Ability.name}");
                        }
                        else
                        {
                            Log.Info($"ability is {_settings.EmptySlotName} - slot: {i}");
                        }
                    }
                    else
                    {
                        Log.Info("Abilities are same");
                    }
                }
            }
        }
    }
}
