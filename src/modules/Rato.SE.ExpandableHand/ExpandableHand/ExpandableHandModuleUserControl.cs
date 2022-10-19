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
        public partial class ExpandableHandModule : IControllModule
        {
            public UpdateFrequency UserControl()
            {
                return _handController.StartSequence(UserControlSequence());
            }

            private IEnumerator<int> UserControlSequence()
            {
                //[TODO] cut off controll from wheels. Is should be done with state saving!
                IMyShipController controller;
                while (true)
                {
                    controller = null;
                    foreach (var shipController in _shipControllerList)
                    {
                        if (shipController.IsUnderControl)
                        {
                            controller = shipController;
                        }
                    }
                    if (controller == null)
                    {
                        StopHand();
                        yield return 100;
                    }
                    else
                    {
                        var move = controller.MoveIndicator;
                        var rotation = controller.RotationIndicator;

                        HorizontalRotor(controller);
                        ToolHorizontalRotor(controller);
                        ToolVerticalRotor(controller);
                        ExtendRotorFirst(controller);
                        ExtendRotorSecond(controller);
                        yield return 1;
                    }
                }
            }

            private void HorizontalRotor(IMyShipController controller)
            {
                var roll = controller.RollIndicator;
                _logger.LogInformation(roll.ToString());
                float speed = (float)(roll * _settings.HorizontalRotor.EffectiveMultiplier);
                _horizontalRotor.TargetVelocityRad = (Math.Abs(speed) < _settings.HorizontalRotor.MaxSpeed) ? speed : (float)(_settings.HorizontalRotor.MaxSpeed * Math.Sign(speed));
            }
            private void ToolHorizontalRotor(IMyShipController controller)
            {
                if (_toolHorizontalRotor == null)
                    return;

                var yaw = controller.RotationIndicator.Y;
                _logger.LogInformation($"mult {_settings.ToolHorizontalRotor.EffectiveMultiplier.ToString()}");

                _logger.LogInformation(yaw.ToString());
                float speed = (float)(yaw * _settings.ToolHorizontalRotor.EffectiveMultiplier);
                _toolHorizontalRotor.TargetVelocityRad = (Math.Abs(speed) < _settings.ToolHorizontalRotor.MaxSpeed) ? speed : (float)(_settings.ToolHorizontalRotor.MaxSpeed * Math.Sign(speed));
            }

            private void ToolVerticalRotor(IMyShipController controller)
            {
                if (_toolVerticalRotor == null)
                    return;

                var pitch = controller.RotationIndicator.X;
                float speed = (float)(pitch * _settings.ToolVerticalRotor.EffectiveMultiplier);
                _toolVerticalRotor.TargetVelocityRad = (Math.Abs(speed) < _settings.ToolVerticalRotor.MaxSpeed) ? speed : (float)(_settings.ToolVerticalRotor.MaxSpeed * Math.Sign(speed));
            }

            private void ExtendRotorFirst(IMyShipController controller)
            {
                var up = controller.MoveIndicator.Y;
                float speed = (float)(up * _settings.ExtendRotorFirst.EffectiveMultiplier);
                _extendRotorFirst.TargetVelocityRad = (Math.Abs(speed) < _settings.ExtendRotorFirst.MaxSpeed) ? speed : (float)(_settings.ExtendRotorFirst.MaxSpeed * Math.Sign(speed));
            }

            private void ExtendRotorSecond(IMyShipController controller)
            {
                var forward = controller.MoveIndicator.Z;
                float speed = (float)(forward * _settings.ExtendRotorSecond.EffectiveMultiplier);
                _extendRotorSecond.TargetVelocityRad = (Math.Abs(speed) < _settings.ExtendRotorSecond.MaxSpeed) ? speed : (float)(_settings.ExtendRotorSecond.MaxSpeed * Math.Sign(speed));
            }
        }
    }
}
