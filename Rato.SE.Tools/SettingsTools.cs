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
        public interface ISettings
        {
            void LoadValues(MyIni config);
            void WriteValues(MyIni config);
        }

        public class SettingsHandler
        {
            private MyIni _ini = new MyIni();

            Program _program;
            public SettingsHandler(Program program)
            {
                _program = program;

                string stringSettings = string.IsNullOrWhiteSpace(_program.Storage)
                    ? _program.Me.CustomData
                    : _program.Storage;
                ResetStore(stringSettings);
            }

            public T ReadFromStore<T>(T settings) where T : ISettings, new()
            {
                if (settings == null)
                    settings = new T();
                settings.LoadValues(_ini);
                return settings;
            }

            public void Save()
            {
                _program.Storage = _ini.ToString();
            }

            public void WriteToStore<T>(T settings) where T : ISettings, new()
            {
                if (settings == null)
                    settings = new T();
                settings.WriteValues(_ini);
            }

            public void LoadFromUserStorage()
            {
                ResetStore(_program.Me.CustomData);
            }
            public void SaveToUserStorage()
            {
                _program.Me.CustomData = _ini.ToString();
            }

            private void ResetStore(string store)
            {
                MyIniParseResult result;
                if (!_ini.TryParse(store, out result))
                    throw new Exception(result.ToString());
            }
        }
    }
}
