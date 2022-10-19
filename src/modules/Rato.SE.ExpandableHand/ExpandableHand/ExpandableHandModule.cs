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
        public partial class ExpandableHandModule : IControllModule, ISelfTestableModule, IAutoStartModule, ITerminalModule, IConfigurableModule
        {
            private ExpandableHandSettings _settings;
            private ExpandableHandState _state;
            private MyGridProgram _program;
            private ILogger _logger;

            private List<IMyMotorStator> _tempsRotorList;
            private List<IMyShipController> _shipControllerList;

            private IMyMotorStator _horizontalRotor;
            private IMyMotorStator _extendRotorFirst;
            private IMyMotorStator _extendRotorSecond;
            private IMyMotorStator _toolHorizontalRotor;
            private IMyMotorStator _toolVerticalRotor;

            public SequenceExecutor _handController;

            public List<ModuleStatusDetail> StatusDetails { get; private set; }

            public ModuleStatus Status { get; set; }

            public ExpandableHandModule(MyGridProgram program, ILogger logger)
            {
                _program = program;
                _logger = logger;

                _handController = new SequenceExecutor(_logger);

                _tempsRotorList = new List<IMyMotorStator>();
                _shipControllerList = new List<IMyShipController>();

                StatusDetails = new List<ModuleStatusDetail>();
                Status = ModuleStatus.JustCreated;

                _settings = new ExpandableHandSettings();
                _state = new ExpandableHandState();
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
                _logger.LogInformation($"Hand position controller state:{_handController.State}");
                return _handController.ContinueSequence(updateSource);
            }

            public UpdateFrequency AutoStart()
            {
                Status = ModuleStatus.Active;
                return UserControl();
            }

            public UpdateFrequency TerminalAction(UpdateType updateSource, string Argument)
            {
                _logger.LogInformation("Hand awaked");
                var updateFrequency = UpdateFrequency.None;

                if (Argument.StartsWith("ExpHand") && (updateSource | UpdateType.Terminal | UpdateType.Trigger) > 0)
                {
                    if (Argument == "ExpHand.Stop")
                    {
                        updateFrequency = _handController.StopSequence();
                        StopHand();
                    }
                    if (Argument == "ExpHand.UserControl")
                    {
                        updateFrequency = UserControl();
                    }
                }

                return updateFrequency;
            }

            private void StopHand()
            {
                _horizontalRotor.TargetVelocityRad = 0;
                _extendRotorFirst.TargetVelocityRad = 0;
                _extendRotorSecond.TargetVelocityRad = 0;
                _toolHorizontalRotor.TargetVelocityRad = 0;
                _toolVerticalRotor.TargetVelocityRad = 0;
            }
        }
    }
}
