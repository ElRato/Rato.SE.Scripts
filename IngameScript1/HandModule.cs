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
        public class HandModule: IControllModule
        {
            private HandSettings _settings;
            private MyGridProgram _program;
            private ILogger _logger;

            private List<IMyPistonBase> _handPistons;
            private List<IMyMotorStator> _handMainRotors;
            private IMyMotorStator _handMainRotor;

            public HandModule(HandSettings settings, MyGridProgram program, ILogger logger) {
                _settings = settings;
                _program = program;
                _logger = logger;

                _handPistons = new List<IMyPistonBase>();
                _handMainRotors = new List<IMyMotorStator>();
            }

            public List<StateCheckItem> Initialize(List<StateCheckItem> checkList)
            {
                _program.GridTerminalSystem.GetBlocksOfType(_handPistons, p => p.CustomName.Contains(_settings.PistonSufix));
                var pistonCount = new StateCheckItem("Hand Piston");
                if (_handPistons.Count == 0)
                {
                    _logger.LogError($"Wrong rotor count with sufix {_settings.PistonSufix} found {_handPistons.Count } expected 1 and more");
                }
                if (_handPistons.Count > 0)
                {
                    pistonCount.Level = ModuleState.Ok;
                    _logger.LogInformation($"Found {_handPistons.Count} pistons with sufix {_settings.PistonSufix}");
                }
                checkList.Add(pistonCount);

                var rotorCount = new StateCheckItem("Hand Rotor");
                _program.GridTerminalSystem.GetBlocksOfType(_handMainRotors, p => p.CustomName.Contains(_settings.RotorSufix));
                if (_handMainRotors.Count != 1)
                {
                    _logger.LogError($"Wrong rotor count with sufix {_settings.RotorSufix} found {_handMainRotors.Count } expected 1");
                } else {
                    rotorCount.Level = ModuleState.Ok;
                    _handMainRotor = _handMainRotors[0];
                }
                checkList.Add(rotorCount);

                return checkList;
            }
        }
    }
}
