using PhoenixPoint.Common.Entities;
using SupportLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelectClassSkills
{
    public class Mod
    {
        internal static volatile Func<string, object, object> _api;
        internal static Settings _settings;

        public void MainMod(Func<string, object, object> api) => HomeMod(api);

        public void HomeMod(Func<string, object, object> api = null)
        {
            _api = api;
            Log _log = new Log(api);
            _settings = (api?.Invoke("config", typeof(Settings)) as Settings)?.Update() ?? new Settings();
            if (_settings.classesList != null)
            {
                Log.Info("is not null");
                foreach (var item in _settings.classesList)
                {
                    Log.Info(item.Key);
                }
            }
            Log.Info("E_AbilityTrack [AssaultSpecializationDef]");
            Log.Info("still works????");
            Log.Info(_settings.EmptySlotName);
            Version version = (Version)(_api?.Invoke("version", "modnix"));
            if (version.Major < 3)
            {
                GeoscapeMod();
            }
        }

        public void GeoscapeMod()
        {
            SkillChanges.ApplyChanges();
        }
    }
}
