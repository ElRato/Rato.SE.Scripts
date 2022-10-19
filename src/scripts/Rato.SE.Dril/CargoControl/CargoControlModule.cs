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
        public partial class CargoControlModule : IControllModule, IAutoStartModule, ITerminalModule, IConfigurableModule
        {
            private CargoControlSettings _settings;
            private CargoControlState _state;
            private MyGridProgram _program;
            private ILogger _logger;

            private List<IMyTextPanel> _tempDisplays;
            private List<IMyCockpit> _tempCocpits;
            private List<IMyCargoContainer> _cargoContainers;

            private IMyTextSurface _capacityLcd;
            private RectangleF _viewport;

            public SequenceExecutor _capacityDisplayExecutor;

            public List<ModuleStatusDetail> StatusDetails { get; private set; }

            public ModuleStatus Status { get; set; }

            public CargoControlModule(MyGridProgram program, ILogger logger)
            {
                _program = program;
                _logger = logger;

                _capacityDisplayExecutor = new SequenceExecutor(_logger);

                StatusDetails = new List<ModuleStatusDetail>();
                Status = ModuleStatus.JustCreated;

                _tempDisplays = new List<IMyTextPanel>();
                _tempCocpits = new List<IMyCockpit>();
                _cargoContainers = new List<IMyCargoContainer>();

                _settings = new CargoControlSettings();
                _state = new CargoControlState();
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

            public UpdateFrequency ContinueSequence(UpdateType updateSource)
            {
                _logger.LogInformation($"Cargo display state:{_capacityDisplayExecutor.State}");
                return _capacityDisplayExecutor.ContinueSequence(updateSource);
            }

            public UpdateFrequency AutoStart()
            {
                Status = ModuleStatus.Active;
                return CapacityDisplay();
            }

            public UpdateFrequency TerminalAction(UpdateType updateSource, string Argument)
            {
                var updateFrequency = UpdateFrequency.None;

                if (Argument.StartsWith("Cargo") && (updateSource | UpdateType.Terminal | UpdateType.Trigger) > 0)
                {
                }

                return updateFrequency;
            }
        }
    }
}
