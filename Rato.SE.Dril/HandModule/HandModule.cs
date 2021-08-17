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
        public partial class HandModule : IControllModule
        {
            private HandSettings _settings;
            private MyGridProgram _program;
            private ILogger _logger;

            private List<IMyPistonBase> _handPistons;
            private List<IMyMotorStator> _handMainRotors;
            private IMyMotorStator _handMainRotor;

            public SequenceExecutor _handPositionController;

            public List<ModuleStateDetail> StateDetails { get; private set; }

            public ModuleState State { get; set; }

            public HandModule(HandSettings settings, MyGridProgram program, ILogger logger)
            {
                _settings = settings;
                _program = program;
                _logger = logger;

                _handPositionController = new SequenceExecutor(_logger);

                _handPistons = new List<IMyPistonBase>();
                _handMainRotors = new List<IMyMotorStator>();

                StateDetails = new List<ModuleStateDetail>();
                State = ModuleState.JustCreated;
            }

            public UpdateFrequency ContinueSquence(UpdateType updateSource)
            {
                return _handPositionController.ContinueSequence(updateSource);
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
                var updateFrequency = UpdateFrequency.None;
                if (Argument.StartsWith("Hand") && (updateSource | UpdateType.Terminal | UpdateType.Trigger) > 0)
                {
                    //[TODO] Add Planetary elevation controll
                    //[TODO] Add Go to top
                    //[TODO] Add Go to horizontaldig - can be configured only manually
                    
                    if (_handPositionController.State == SequenceExecutorState.NoOperation)
                    {
                        updateFrequency = HoldElevation();
                    }
                    if (Argument == "Hand.Elevation.Inc")
                    {
                        _settings.TargetElevation = MathHelper.WrapAngle(_handMainRotor.Angle) + MathHelper.ToRadians(5) * _settings.RotateDirection;
                        //[TODO] add max range controll
                    }
                    if (Argument == "Hand.Elevation.Dec")
                    {
                        _settings.TargetElevation = MathHelper.WrapAngle(_handMainRotor.Angle) - MathHelper.ToRadians(5) * _settings.RotateDirection;
                    }
                    if (Argument == "Hand.Stop")
                    {
                        updateFrequency = _handPositionController.StopExecution();
                    }
                }

                return updateFrequency;
            }
        }
    }
}
