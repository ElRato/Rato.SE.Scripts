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
        public partial class RepetableBuildModule : IControllModule
        {
            private List<IMyPistonBase> _tempsPistonList;
            private List<IMyShipMergeBlock> _tempsMergeBlocksList;
            private List<IMyShipConnector> _tempsConnectorsList;
            private List<IMyProjector> _tempsProjectorsList;
            private List<IMyLandingGear> _tempsMagnetsList;

            public void Initialize()
            {
                StatusDetails.Clear();
                
                ModuleStatusDetail status;
                _piston = InitializeBlock("Main piston", _settings.MainPistonSufix, _tempsPistonList, ActionStatus.Error, out status);
                StatusDetails.Add(status);
                _mergeBlock = InitializeBlock("Merge Block", _settings.MergeBlockSufix, _tempsMergeBlocksList, ActionStatus.Error, out status);
                StatusDetails.Add(status);
                _connector = InitializeBlock("Connector", _settings.ConnectorSufix, _tempsConnectorsList, ActionStatus.Error, out status);
                StatusDetails.Add(status);
                _projector = InitializeBlock("Projector", _settings.ProjectorSufix, _tempsProjectorsList, ActionStatus.Error, out status);
                StatusDetails.Add(status);
                _magnet = InitializeBlock("Magnet", _settings.MagnetSufix, _tempsMagnetsList, ActionStatus.Error, out status);
                StatusDetails.Add(status);
                InitializeListBlock("Grinders", _settings.GrindersSufix, 1, 10, _grinders, ActionStatus.Error, out status);
                StatusDetails.Add(status);
                InitializeListBlock("Welders", _settings.WeldersSufix, 1, 10, _welders, ActionStatus.Error, out status);
                StatusDetails.Add(status);

                Status = StatusDetails.Any(c => c.Level == ActionStatus.Error) ? ModuleStatus.NonFunctional : ModuleStatus.Initialized;

                _dbgLogger.LogInformation("Initialize done");
            }

            private T InitializeBlock<T>(string Name, string sufix, List<T> lempList, ActionStatus errorStatus, out ModuleStatusDetail status) where T: class, IMyTerminalBlock
            {
                _program.GridTerminalSystem.GetBlocksOfType<T>(lempList, p => p.CustomName.Contains(sufix));
                status = new ModuleStatusDetail(Name);
                if (lempList.Count != 1)
                {
                    _logger.LogWarning($"Wrong block count with sufix {sufix} found {lempList.Count } expected 1");
                    status.Level = errorStatus;
                    return null;
                }
                else
                {
                    _logger.LogInformation($"Found {lempList.Count} block with sufix {sufix}");
                    status.Level = ActionStatus.Ok;
                    return lempList[0];
                }
            }
            private void InitializeListBlock<T>(string Name, string sufix, int min, int max, List<T> list, ActionStatus errorStatus, out ModuleStatusDetail status) where T : class, IMyTerminalBlock
            {
                _program.GridTerminalSystem.GetBlocksOfType<T>(list, p => p.CustomName.Contains(sufix));
                status = new ModuleStatusDetail(Name);
                if (list.Count < min || list.Count > max)
                {
                    _logger.LogWarning($"Wrong block count with sufix {sufix} found {list.Count } expected bettwen {min} and {max}");
                    status.Level = errorStatus;
                }
                else
                {
                    _logger.LogInformation($"Found {list.Count} block with sufix {sufix}");
                    status.Level = ActionStatus.Ok;
                }
            }
        }
    }
}
