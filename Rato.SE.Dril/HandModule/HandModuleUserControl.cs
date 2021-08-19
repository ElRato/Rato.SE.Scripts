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
            public UpdateFrequency UserControl()
            {
                return _handPositionController.StartSequence(UserControlSequence());
            }

            private IEnumerator<int> UserControlSequence()
            {
                var testVelocity = (float)_settings.PistonMaxSpeed / 2;
                IMyShipController controller;
                while (true)
                {
                    controller = null;
                    foreach (var shipController in _shipControllers) {
                        if (shipController.IsUnderControl) {
                            controller = shipController;
                        }
                    }
                    if (controller == null)
                    {
                        yield return 100;
                    }
                    else
                    {

                        var rotation = controller.RotationIndicator;
                        _logger.LogInformation(rotation.X.ToString());
                        float speed = (float)(rotation.X * _settings.UserControlSpeedMultiplier);
                        SetPistonsVelocity((Math.Abs(speed) < _settings.PistonMaxSpeed) ? speed : (float)(_settings.PistonMaxSpeed * Math.Sign(speed)));

                        yield return 1;
                    }
                }
            }
        }
    }
}
