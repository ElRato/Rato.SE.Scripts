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

        public class SettingsHandler
        {
            Program _program;
            public SettingsHandler(Program program)
            {
                _program = program;
            }

            public T GetSettings<T>() where T : ISettings, new()
            {
                string stringSettings = string.IsNullOrWhiteSpace(_program.Storage) 
                    ? _program.Me.CustomData
                    : _program.Storage;
                
                var settings = new T();
                return ParseToObject<T>(stringSettings, settings);
            }
            public T ResetSettings<T>(T settings) where T : ISettings, new()
            {
                string stringSettings = _program.Me.CustomData;
                return ParseToObject(stringSettings, settings);
            }

            private static T ParseToObject<T>(string stringSettings, T settings) where T : ISettings, new()
            {
                settings.ParseString(stringSettings);
                return settings;
            }
        }
    }
}
