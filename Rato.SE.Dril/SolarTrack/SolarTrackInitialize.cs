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
        public partial class SolarTrackModule : IControllModule
        {
            public void Initialize()
            {
                StatusDetails.Clear();
                var tempRotorList = new List<IMyMotorStator>();
                var tempPnnelsList = new List<IMySolarPanel>();

                _program.GridTerminalSystem.GetBlocksOfType(tempRotorList, p => p.CustomName.Contains(_settings.Rotor1Sufix));
                var rotor1Count = new ModuleStatusDetail("Rotor 1");
                if (tempRotorList.Count != 1)
                {
                    _logger.LogError($"Wrong rotor count with sufix {_settings.Rotor1Sufix} found {tempRotorList.Count } expected 1");
                } else {
                    _rotor1 = tempRotorList.First();
                    rotor1Count.Level = ActionStatus.Ok;
                    _logger.LogInformation($"Found {tempRotorList.Count} pistons with sufix {_settings.Rotor1Sufix}");
                }
                StatusDetails.Add(rotor1Count);

                _program.GridTerminalSystem.GetBlocksOfType(tempRotorList, p => p.CustomName.Contains(_settings.Rotor2Sufix));
                var rotor2Count = new ModuleStatusDetail("Rotor 2");
                if (tempRotorList.Count != 1)
                {
                    _logger.LogError($"Wrong rotor count with sufix {_settings.Rotor2Sufix} found {tempRotorList.Count } expected 1");
                }
                else
                {
                    _rotor2 = tempRotorList.First();
                    rotor2Count.Level = ActionStatus.Ok;
                    _logger.LogInformation($"Found {tempRotorList.Count} pistons with sufix {_settings.Rotor2Sufix}");
                }
                StatusDetails.Add(rotor2Count);


                _program.GridTerminalSystem.GetBlocksOfType(tempPnnelsList, p => p.CustomName.Contains(_settings.PannelSufix));
                var pannelCount = new ModuleStatusDetail("Pannel");
                if (tempPnnelsList.Count != 1)
                {
                    _logger.LogError($"Wrong pannel count with sufix {_settings.PannelSufix} found {tempPnnelsList.Count } expected 1");
                }
                else
                {
                    _pannel = tempPnnelsList.First();
                    pannelCount.Level = ActionStatus.Ok;
                    _logger.LogInformation($"Found {tempPnnelsList.Count} pistons with sufix {_settings.PannelSufix}");
                }
                StatusDetails.Add(pannelCount);

                Status = StatusDetails.Any(c => c.Level == ActionStatus.Error) ? ModuleStatus.NonFunctional : ModuleStatus.Initialized;
            }
        }
    }
}
