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
        public interface IDataStore
        {
            void LoadValues(MyIni config);
            void WriteValues(MyIni config);
        }

        public class DataStoreHandler
        {
            private MyIni _ini = new MyIni();
            private ILowLevelStore _lowLevelStore;

            public DataStoreHandler(ILowLevelStore lowLevelStore)
            {
                _lowLevelStore = lowLevelStore;
                ParseStore(_lowLevelStore.Read());
            }

            public bool IsEmpty() {
                return string.IsNullOrEmpty(_lowLevelStore.Read());
            }

            public T ReadFromStore<T>(T settings) where T : IDataStore, new()
            {
                if (settings == null)
                    settings = new T();
                settings.LoadValues(_ini);
                return settings;
            }

            public void WriteToStore<T>(T settings) where T : IDataStore, new()
            {
                if (settings == null)
                    settings = new T();
                settings.WriteValues(_ini);
            }
            public void Save()
            {
                _lowLevelStore.Write(_ini.ToString());
            }
            private void ParseStore(string store)
            {
                MyIniParseResult result;
                if (!_ini.TryParse(store, out result))
                    throw new Exception(result.ToString());
            }
        }

        public interface ILowLevelStore {
            string Read();
            void Write(string value);
        }

        public class CustomDataLowLevelStore: ILowLevelStore {
            private IMyTerminalBlock _block;
            public CustomDataLowLevelStore(IMyTerminalBlock block) {
                _block = block;
            }

            public string Read()
            {
                return _block.CustomData;
            }

            public void Write(string value)
            {
                _block.CustomData = value;
            }
        }

        public class ProgramLowLevelStore : ILowLevelStore
        {
            private Program _program;
            public ProgramLowLevelStore(Program program)
            {
                _program = program;
            }

            public string Read()
            {
                return _program.Storage;
            }

            public void Write(string value)
            {
                _program.Storage = value;
            }
        }
    }
}
