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
                var previousAngle = _handMainRotor.Angle;

                SetPistonsVelocity(testVelocity);

                yield return 100;

                while (Math.Abs(previousAngle - _handMainRotor.Angle) > 0.0001)
                {
                    previousAngle = _handMainRotor.Angle;
                    _logger.LogInformation($"current angle {previousAngle}");
                    yield return 1;
                }

                _settings.MaxAngle = MathHelper.WrapAngle(_handMainRotor.Angle);

                SetPistonsVelocity(-testVelocity);
                
                yield return 100;

                while (Math.Abs(previousAngle - _handMainRotor.Angle) > 0.0001)
                {
                    previousAngle = _handMainRotor.Angle;
                    _logger.LogInformation($"current angle {previousAngle}");
                    yield return 1;
                }

                _settings.MinAngle = MathHelper.WrapAngle(_handMainRotor.Angle);
                if (_settings.MinAngle > _settings.MaxAngle) {
                    /*
                    var t = _settings.MinAngle;
                    _settings.MinAngle = _settings.MaxAngle;
                    _settings.MaxAngle = t;
                    */
                    _settings.RotateDirection = -1;
                }

                _logger.LogInformation($"Max rotor angle = {_settings.MaxAngle}");
                _logger.LogInformation($"Min rotor angle = {_settings.MinAngle}");
                _logger.LogInformation($"Rotate Direction = {_settings.RotateDirection}");

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
