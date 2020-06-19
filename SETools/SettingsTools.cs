using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game;
using VRageMath;

namespace IngameScript
{
    partial class Program
    {
        public interface ISettings{
            void ParseString(string store);
            string ComposeString();
        }

        public class SettingsHandler<T> where T : ISettings, new()
        {
            Program _program;
            public SettingsHandler(Program program)
            {
                _program = program;
            }

            public T GetSettings()
            {
                string stringSettings = string.IsNullOrWhiteSpace(_program.Storage) 
                    ? _program.Me.CustomData
                    : _program.Storage;
                
                _program.Echo(stringSettings);

                return ParseToObject(stringSettings);
            }
            public T ResetSettings()
            {
                string stringSettings = _program.Me.CustomData;
                _program.Echo(stringSettings);
                return ParseToObject(stringSettings);
            }

            private static T ParseToObject(string stringSettings)
            {
                var settings = new T();
                settings.ParseString(stringSettings);
                return settings;
            }
        }
    }
}
