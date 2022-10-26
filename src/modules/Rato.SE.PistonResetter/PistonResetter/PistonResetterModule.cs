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
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game;
using VRage;
using VRageMath;

namespace IngameScript
{
    partial class Program
    {
        public partial class PistonResetterModule : IModule, IConfigurableModule
        {
            private MyGridProgram _program;
            private ILogger _logger;

            private PistonResetterSettings _settings;
            private PistonResetterState _state;

            public List<IMyPistonBase> _tempPistons;

            public List<ModuleStatusDetail> StatusDetails { get; private set; }

            public ModuleStatus Status { get; set; }

            public PistonResetterModule(MyGridProgram program, ILogger logger)
            {
                _program = program;
                _logger = logger;

                _tempPistons = new List<IMyPistonBase>();
                StatusDetails = new List<ModuleStatusDetail>();

                _settings = new PistonResetterSettings();
                _state = new PistonResetterState();
            }

            public bool SetConfig(DataStoreHandler storeHandler)
            {
                _settings = storeHandler.ReadFromStore(_settings);
                //[TODO] implement check for changed values
                return false;
            }

            public bool SetState(DataStoreHandler storeHandler)
            {
                _state = storeHandler.ReadFromStore(_state);
                //[TODO] implement check for changed values
                return false;
            }

            public void SaveConfig(DataStoreHandler storeHandler)
            {
                storeHandler.WriteToStore(_settings);
            }

            public void SaveState(DataStoreHandler storeHandler)
            {
                storeHandler.WriteToStore(_state);
            }
        }
    }
}
