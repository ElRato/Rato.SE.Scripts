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
        public partial class FineRefineModule : IControllModule, IAutoStartModule, ITerminalModule, IConfigurableModule
        {
            private FineRefineSettings _settings;
            private FineRefineState _state;
            private MyGridProgram _program;
            private ILogger _logger;

            private List<IMyRefinery> _refinaries;
            private Dictionary<IMyRefinery, FineRefinaryConfig> _refinaryConfigs;
            public Dictionary<IMyRefinery, CargoQueue2> _queues;

            public SequenceExecutor _refineQueueSequence;

            public List<MyInventoryItem> _currentItems;
            public List<MyInventoryItem> _containerItems;
            private List<IMyTerminalBlock> _containers;

            public List<ModuleStatusDetail> StatusDetails { get; private set; }

            public ModuleStatus Status { get; set; }

            public FineRefineModule(MyGridProgram program, ILogger logger)
            {
                _program = program;
                _logger = logger;

                _refineQueueSequence = new SequenceExecutor(_logger);

                StatusDetails = new List<ModuleStatusDetail>();
                Status = ModuleStatus.JustCreated;

                _refinaries = new List<IMyRefinery>();
                _refinaryConfigs = new Dictionary<IMyRefinery, FineRefinaryConfig>();
                _currentItems = new List<MyInventoryItem>();
                _containerItems = new List<MyInventoryItem>();
                _containers = new List<IMyTerminalBlock>();
                _queues = new Dictionary<IMyRefinery, CargoQueue2>();

                _settings = new FineRefineSettings();
                _state = new FineRefineState();
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
                _logger.LogInformation($"Fine refinary state:{_refineQueueSequence.State}");
                return _refineQueueSequence.ContinueSequence(updateSource);
            }

            public UpdateFrequency AutoStart()
            {
                Status = ModuleStatus.Active;
                return DistrubuteResources();
            }

            public UpdateFrequency TerminalAction(UpdateType updateSource, string Argument)
            {
                _logger.LogInformation("Fine refinary online");
                var updateFrequency = UpdateFrequency.None;

                if (Argument.StartsWith("Fine") && (updateSource | UpdateType.Terminal | UpdateType.Trigger) > 0)
                {
                }

                return updateFrequency;
            }
        }
    }
}
