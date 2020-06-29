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
            public UpdateFrequency StartTestSquence()
            {
                return _handPositionController.StartSequence(SelfTestSequence());
            }

            private IEnumerator<int> SelfTestSequence()
            {
                State = ModuleState.SelfTest;
                var testVelocity = (float)_settings.PistonMaxSpeed / 2;
                var rotorAngle = _handMainRotor.Angle;

                SetPistonsVelocity(testVelocity);

                yield return 100;

                while (Math.Abs(rotorAngle - _handMainRotor.Angle) > 0.0001)
                {
                    rotorAngle = _handMainRotor.Angle;
                    yield return 10;
                }

                _settings.MaxAngle = Math.Round(_handMainRotor.Angle * 180 / Math.PI);

                SetPistonsVelocity(-testVelocity);
                
                yield return 100;

                while (Math.Abs(rotorAngle - _handMainRotor.Angle) > 0.0001)
                {
                    rotorAngle = _handMainRotor.Angle;
                    yield return 10;
                }

                _settings.MinAngle = Math.Round(_handMainRotor.Angle * 180 / Math.PI);

                _logger.LogInformation($"Max rotor angle = {_settings.MaxAngle}");
                _logger.LogInformation($"Min rotor angle = {_settings.MinAngle}");

                SetPistonsVelocity(0);

                StateDetails.Add(new ModuleStateDetail()
                {
                    Name = "Self test",
                    Level = ActionStatus.Ok
                });


                State = ModuleState.Active;
            }
        }
    }
}
