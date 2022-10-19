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
        public partial class ExpandableHandModule : IControllModule
        {
            public void Initialize()
            {
                StatusDetails.Clear();
                ModuleStatusDetail status;
                _horizontalRotor = InitializeRotor("Horizontal rotor", _settings.HorizontalRotor.Sufix, ActionStatus.Error, out status);
                StatusDetails.Add(status);

                _extendRotorFirst = InitializeRotor("Extending rotor first", _settings.ExtendRotorFirst.Sufix, ActionStatus.Error, out status);
                StatusDetails.Add(status);

                _extendRotorSecond = InitializeRotor("Extending rotor second", _settings.ExtendRotorSecond.Sufix, ActionStatus.Error, out status);
                StatusDetails.Add(status);

                _toolHorizontalRotor = InitializeRotor("Tool horizontal rotor", _settings.ToolHorizontalRotor.Sufix, ActionStatus.Warning, out status);
                StatusDetails.Add(status);

                _toolVerticalRotor = InitializeRotor("Tool vertical rotor", _settings.ToolVerticalRotor.Sufix, ActionStatus.Warning, out status);
                StatusDetails.Add(status);
                
                var controllerCount = new ModuleStatusDetail("Controllers");
                _program.GridTerminalSystem.GetBlocksOfType(_shipControllerList, p => p.CustomName.Contains(_settings.ControllerSufix));
                if (_shipControllerList.Count == 0)
                {
                    _logger.LogError($"Ship controller with sufix {_settings.ControllerSufix} found. Expected more than 0");
                    controllerCount.Level = ActionStatus.Error;
                }
                else
                {
                    controllerCount.Level = ActionStatus.Ok;
                }
                StatusDetails.Add(controllerCount);

                Status = StatusDetails.Any(c => c.Level == ActionStatus.Error) ? ModuleStatus.NonFunctional : ModuleStatus.Initialized;
            }

            private IMyMotorStator InitializeRotor(string Name, string sufix, ActionStatus errorStatus, out ModuleStatusDetail status)
            {
                _program.GridTerminalSystem.GetBlocksOfType(_tempsRotorList, p => p.CustomName.Contains(sufix));
                status = new ModuleStatusDetail(Name);
                if (_tempsRotorList.Count != 1)
                {
                    _logger.LogWarning($"Wrong rotor count with sufix {sufix} found {_tempsRotorList.Count } expected 1");
                    status.Level = errorStatus;
                    return null;
                }
                else
                {
                    _logger.LogInformation($"Found {_tempsRotorList.Count} rotor with sufix {sufix}");
                    status.Level = ActionStatus.Ok;
                    return _tempsRotorList[0];
                }
            }
        }
    }
}
