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
        public partial class FineRefineModule : IControllModule
        {
            private MyIni _ini = new MyIni();

            public void Initialize()
            {
                try
                {
                    throw new NullReferenceException();
                }
                catch
                {
                    _logger.LogInformation("Welcome to debug");
                }

                StatusDetails.Clear();
                ModuleStatusDetail status;
                InitializeRefinarySet(out status);
                StatusDetails.Add(status);

                if (status.Level == ActionStatus.Ok) {
                    foreach (var refinary in _refinaries) {
                        InitializeRefinary(refinary, out status);
                        StatusDetails.Add(status);
                    }
                }

                Status = StatusDetails.Any(c => c.Level == ActionStatus.Error) ? ModuleStatus.NonFunctional : ModuleStatus.ReadyToStart;
            }

            private void InitializeRefinarySet(out ModuleStatusDetail status) {
                status = new ModuleStatusDetail("Refinaries set");
                var sufix = _settings;
                _program.GridTerminalSystem.GetBlocksOfType(_refinaries, p => p.CustomName.Contains(_settings.RefinarySufix));
                if (_refinaries.Count < 1)
                {
                    _logger.LogError($"Wrong refinaries count with sufix {sufix} found {_refinaries.Count } expected greater or equal 1");
                    status.Level = ActionStatus.Error;
                }
                else {
                    _logger.LogInformation($"Found {_refinaries.Count} rotor with sufix {sufix}");
                    status.Level = ActionStatus.Ok;
                }
                return;
            }

            private void InitializeRefinary(IMyRefinery refinary, out ModuleStatusDetail status)
            {
                status = new ModuleStatusDetail(refinary.CustomName);
                var config = new FineRefinaryConfig();
                _ini.Clear();

                if (!_ini.TryParse(refinary.CustomData))
                {
                    _logger.LogError($"Can't parse config for {refinary.DisplayName}");
                    status.Level = ActionStatus.Error;
                    return;

                }

                config.LoadValues(_ini);
                _ini.Clear();
                config.WriteValues(_ini);
                refinary.CustomData = _ini.ToString();

                _refinaryConfigs[refinary] = config;
                var queue = new CargoQueue2(2);
                //queue.Owner = refinary;
                //queue.Inventory = refinary.GetInventory(0);
                //queue.Set(0, new MyItemType("MyObjectBuilder_Ore", "Stone"), 1000);
                //queue.Set(1, new MyItemType("MyObjectBuilder_Ore", "Iron"), 1000);
                
                _queues[refinary] = queue;

                status.Level = ActionStatus.Ok;
                return;
            }
        }
    }
}
