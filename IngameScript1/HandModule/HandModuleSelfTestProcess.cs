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
        public partial class HandModule: IControllModule
        {
            public UpdateFrequency StartTestSquence()
            {
                return _handPositionController.StartSequence(SelfTestSequence());
            }

            private IEnumerator<int> SelfTestSequence() {
                var testVelocity = (float)_settings.PistonMaxSpeed / 2;
                var rotorAngle = _handMainRotor.Angle;
                foreach (var piston in _handPistons) {
                    piston.Velocity = testVelocity;
                }

                yield return 30;

                _logger.LogInformation($"Rotor angle = {_handMainRotor.Angle} last angle = {rotorAngle} diff = {Math.Abs(rotorAngle - _handMainRotor.Angle)}");
                while (Math.Abs(rotorAngle - _handMainRotor.Angle) > 0.0001)
                {
                    rotorAngle = _handMainRotor.Angle;
                    yield return 30;
                }

                _logger.LogInformation($"Max rotor angle = {_handMainRotor.Angle}");
                foreach (var piston in _handPistons)
                {
                    piston.Velocity = -testVelocity;
                }

                yield return 30;

                _logger.LogInformation($"Rotor angle = {_handMainRotor.Angle} last angle = {rotorAngle} diff = {Math.Abs(rotorAngle - _handMainRotor.Angle)}");
                while (Math.Abs(rotorAngle - _handMainRotor.Angle) > 0.0001)
                {
                    rotorAngle = _handMainRotor.Angle;
                    yield return 30;
                }

                _logger.LogInformation($"Min rotor angle = {_handMainRotor.Angle}");
                foreach (var piston in _handPistons)
                {
                    piston.Velocity = 0;
                }
            }
        }
    }
}
