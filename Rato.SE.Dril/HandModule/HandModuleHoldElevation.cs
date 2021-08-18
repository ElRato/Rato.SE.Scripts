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
                var accuracy = _settings.RotateStep;
                
                while (true)
                {
                    var targetAngle = (_settings.TargetElevation + _settings.MinAngle) % MathHelper.TwoPi;
                    var rotorAngle = _handMainRotor.Angle;
                    var diff = targetAngle - rotorAngle;
                    _logger.LogInformation($"target angle {Math.Round(targetAngle,3)}");
                    _logger.LogInformation($"target alevation {Math.Round(_settings.TargetElevation,3)}");
                    _logger.LogInformation($"current angle {Math.Round(rotorAngle,3)}");
                    _logger.LogInformation($"diff = {Math.Round(Math.Abs(diff),3)}");


                    if (Math.Abs(diff) <= accuracy)
                    {
                        SetPistonsVelocity(0);
                        _logger.LogInformation($"Stop piston");
                    }
                    else
                    {
                        if (diff > MathHelper.Pi)
                        {
                            diff -= MathHelper.TwoPi;
                        }
                        else if (diff <= -MathHelper.Pi)
                        {
                            diff += MathHelper.TwoPi;
                        }

                        if (diff > 0)
                        {
                            SetPistonsVelocity((testVelocity * _settings.RotateDirection));
                            _logger.LogInformation($"Up piston");
                        }
                        else {
                            SetPistonsVelocity((-testVelocity * _settings.RotateDirection));
                            _logger.LogInformation($"Down piston");
                        }
                    }
                    yield return 1;
                }
            }
        }
    }
}
