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
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace IngameScript
{
    partial class Program
    {
        public partial class HandModule : IControllModule
        {
            public UpdateFrequency HoldElevation()
            {
                return _handPositionController.StartSequence(HoldElevationSequence());
            }

            private IEnumerator<int> HoldElevationSequence()
            {
                var testVelocity = (float)_settings.PistonMaxSpeed / 2;

                while (true)
                {
                    var rotorAngle = MathHelper.WrapAngle(_handMainRotor.Angle);
                    var diff = rotorAngle - _settings.TargetElevation;
                    var accuracy = MathHelper.ToRadians(2);

                    if (diff > accuracy)
                        SetPistonsVelocity(-(testVelocity * _settings.RotateDirection));
                    if (diff < -accuracy)
                        SetPistonsVelocity((testVelocity * _settings.RotateDirection));
                    if (Math.Abs(diff) <= accuracy)
                        SetPistonsVelocity(0);

                    _logger.LogInformation($"current angle {rotorAngle}");
                    _logger.LogInformation($"target angle { _settings.TargetElevation}");
                    _logger.LogInformation($"max angle { _settings.MaxAngle}");
                    _logger.LogInformation($"min angle { _settings.MinAngle}");
                    _logger.LogInformation($"diff = {diff}");
                    _logger.LogInformation($"diff accuracy = {accuracy}");

                    yield return 1;
                }
            }
        }
    }
}
