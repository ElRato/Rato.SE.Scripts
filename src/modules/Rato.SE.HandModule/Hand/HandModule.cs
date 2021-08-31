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
        public partial class HandModule : IControllModule, ISelfTestableModule, IAutoStartModule, ITerminalModule, IConfigurableModule
        {
            private HandSettings _settings;
            private HandState _state;
            private MyGridProgram _program;
            private ILogger _logger;

            private List<IMyPistonBase> _handPistons;
            private List<IMyMotorStator> _handMainRotors;
            private List<IMyShipController> _shipControllers;
            private IMyMotorStator _handMainRotor;

            public SequenceExecutor _handPositionController;

            public List<ModuleStatusDetail> StatusDetails { get; private set; }

            public ModuleStatus Status { get; set; }

            public HandModule(MyGridProgram program, ILogger logger)
            {
                _program = program;
                _logger = logger;

                _handPositionController = new SequenceExecutor(_logger);

                _handPistons = new List<IMyPistonBase>();
                _handMainRotors = new List<IMyMotorStator>();
                _shipControllers = new List<IMyShipController>();

                StatusDetails = new List<ModuleStatusDetail>();
                Status = ModuleStatus.JustCreated;

                _settings = new HandSettings();
                _state = new HandState();
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
                _logger.LogInformation($"Hand position controller state:{_handPositionController.State}");
                return _handPositionController.ContinueSequence(updateSource);
            }

            public UpdateFrequency AutoStart()
            {
                Status = ModuleStatus.Active;
                return HoldElevation();
            }

            private void SetPistonsVelocity(float velocity)
            {
                foreach (var piston in _handPistons)
                {
                    piston.Velocity = velocity;
                }
            }
            public UpdateFrequency TerminalAction(UpdateType updateSource, string Argument)
            {
                _logger.LogInformation("Hand awaked");
                var updateFrequency = UpdateFrequency.None;

                if (Argument.StartsWith("Hand") && (updateSource | UpdateType.Terminal | UpdateType.Trigger) > 0)
                {
                    //[TODO] Add Planetary elevation controll
                    //[TODO] Add Go to top
                    //[TODO] Add Go to horizontaldig - can be configured only manually

                    if (Argument == "Hand.Elevation.Inc")
                    {
                        _settings.TargetElevation += _settings.RotateStep;
                    }
                    if (Argument == "Hand.Elevation.Dec")
                    {
                        _settings.TargetElevation -= _settings.RotateStep;
                    }
                    if (Argument == "Hand.Stop")
                    {
                        updateFrequency = _handPositionController.StopSequence();
                        SetPistonsVelocity(0);
                    }
                    if (Argument == "Hand.HoldElevation")
                    {
                        updateFrequency = HoldElevation();
                    }
                    if (Argument == "Hand.UserControl")
                    {
                        updateFrequency = UserControl();
                    }
                }

                return updateFrequency;
            }
        }
    }
}
