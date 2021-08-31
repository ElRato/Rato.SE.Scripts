using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;

namespace IngameScript
{
    public class DumbConfigurableModule :DumbModule,  Program.IConfigurableModule
    {
        internal DumbConfiguration Configuration;

        internal DumbConfigurableModule()
        {
            Configuration = new DumbConfiguration();
        }

        public bool SetConfig(Program.DataStoreHandler storeHandler)
        {
            storeHandler.ReadFromStore(Configuration);
            return false;
        }

        public bool SetState(Program.DataStoreHandler storeHandler)
        {
            storeHandler.ReadFromStore(Configuration);
            return false;
        }

        public void SaveConfig(Program.DataStoreHandler storeHandler)
        {
            storeHandler.WriteToStore(Configuration);
        }

        public void SaveState(Program.DataStoreHandler storeHandler)
        {
            storeHandler.WriteToStore(Configuration);
        }
    }

    public class DumbConfiguration : Program.IDataStore
    {
        public static string SectionName = nameof(DumbConfiguration);
        public static string PropertyName = "Property";
        public string PropertyValue = "empty";
        
        public void LoadValues(MyIni config)
        {
            config.Get(SectionName, PropertyName).ToString("default");
        }
        public void WriteValues(MyIni config)
        {
            config.Set(nameof(SectionName), nameof(PropertyName), PropertyValue);
        }
    }

}