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
            public void Initialize()
            {
                StateDetails.Clear();
                _program.GridTerminalSystem.GetBlocksOfType(_handPistons, p => p.CustomName.Contains(_settings.PistonSufix));
                var pistonCount = new ModuleStateDetail("Hand Piston");
                if (_handPistons.Count == 0)
                {
                    _logger.LogError($"Wrong rotor count with sufix {_settings.PistonSufix} found {_handPistons.Count } expected 1 and more");
                }
                if (_handPistons.Count > 0)
                {
                    pistonCount.Level = ActionStatus.Ok;
                    _logger.LogInformation($"Found {_handPistons.Count} pistons with sufix {_settings.PistonSufix}");
                }
                StateDetails.Add(pistonCount);

                var rotorCount = new ModuleStateDetail("Hand Rotor");
                _program.GridTerminalSystem.GetBlocksOfType(_handMainRotors, p => p.CustomName.Contains(_settings.RotorSufix));
                if (_handMainRotors.Count != 1)
                {
                    _logger.LogError($"Wrong rotor count with sufix {_settings.RotorSufix} found {_handMainRotors.Count } expected 1");
                }
                else
                {
                    rotorCount.Level = ActionStatus.Ok;
                    _handMainRotor = _handMainRotors[0];
                }
                StateDetails.Add(rotorCount);

                var controllerCount = new ModuleStateDetail("Controllers");
                _program.GridTerminalSystem.GetBlocksOfType(_shipControllers, p => p.CustomName.Contains(_settings.ControllerSufix));
                if (_shipControllers.Count == 0)
                {
                    _logger.LogWarning($"Ship controller with sufix {_settings.ControllerSufix} found. Expected more than 1");
                    controllerCount.Level = ActionStatus.Warning;
                }
                else
                {
                    controllerCount.Level = ActionStatus.Ok;
                }
                StateDetails.Add(controllerCount);

                State = StateDetails.Any(c => c.Level == ActionStatus.Error) ? ModuleState.NonFunctional : ModuleState.Initialized;
            }
        }
    }
}
