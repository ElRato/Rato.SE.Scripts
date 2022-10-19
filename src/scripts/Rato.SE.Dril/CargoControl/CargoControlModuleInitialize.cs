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
        public partial class CargoControlModule : IControllModule
        {
            public void Initialize()
            {
                StatusDetails.Clear();

                StatusDetails.Add(InitCocpitDisplay());
                StatusDetails.Add(InitCargo());

                Status = StatusDetails.Any(c => c.Level == ActionStatus.Error) ? ModuleStatus.NonFunctional : ModuleStatus.ReadyToStart;
            }

            private ModuleStatusDetail InitCocpitDisplay()
            {
                var sufix = _settings.CapacityCocpitSufix;
                _program.GridTerminalSystem.GetBlocksOfType(_tempCocpits, p => p.CustomName.Contains(sufix));
                IMyCockpit cocpit;

                var status = new ModuleStatusDetail("Cargo Capacity cocpit Display");
                if (_tempCocpits.Count != 1)
                {
                    _logger.LogWarning($"Wrong display cocpit with sufix {sufix} found {_tempCocpits.Count } expected 1");
                    status.Level = ActionStatus.Error;
                    return status;
                }
                else
                {
                    _logger.LogInformation($"Found  cocpit {_tempCocpits.Count}  with sufix {sufix}");
                    status.Level = ActionStatus.Ok;
                    cocpit = _tempCocpits[0];
                }

                _logger.LogInformation($"Number of lcds {cocpit.SurfaceCount}");
                _capacityLcd = cocpit.GetSurface(1);
                _capacityLcd.Script = "None";

                _viewport = new RectangleF(
                    (_capacityLcd.TextureSize - _capacityLcd.SurfaceSize) / 2f,
                    _capacityLcd.SurfaceSize
                );

                return status;
            }

            private ModuleStatusDetail InitDisplay()
            {
                ModuleStatusDetail status;
                var sufix = _settings.CapacityCocpitSufix;
                _program.GridTerminalSystem.GetBlocksOfType(_tempDisplays, p => p.CustomName.Contains(sufix));

                status = new ModuleStatusDetail("Cargo Capacity Display");
                if (_tempDisplays.Count != 1)
                {
                    _logger.LogWarning($"Wrong display count with sufix {sufix} found {_tempDisplays.Count } expected 1");
                    status.Level = ActionStatus.Error;
                }
                else
                {
                    _logger.LogInformation($"Found {_tempDisplays.Count} display with sufix {sufix}");
                    status.Level = ActionStatus.Ok;
                    _capacityLcd = _tempDisplays[0];
                }

                _capacityLcd.ContentType = ContentType.SCRIPT;

                _viewport = new RectangleF(
                    (_capacityLcd.TextureSize - _capacityLcd.SurfaceSize) / 2f,
                    _capacityLcd.SurfaceSize
                );
                return status;
            }

            private ModuleStatusDetail InitCargo()
            {
                ModuleStatusDetail status;
                var sufix = _settings.CapacityCargoSufix;
                _program.GridTerminalSystem.GetBlocksOfType(_cargoContainers, p => p.CustomName.Contains(sufix));

                status = new ModuleStatusDetail("Cargo containers");
                if (_cargoContainers.Count == 0)
                {
                    _logger.LogWarning($"Wrong cargo count with sufix {sufix} found {_cargoContainers.Count } expected more then 0");
                    status.Level = ActionStatus.Error;
                    return status;
                }
                else
                {
                    _logger.LogInformation($"Found {_cargoContainers.Count} cargo with sufix {sufix}");
                    status.Level = ActionStatus.Ok;
                    return status;
                }
            }
        }
    }
}
