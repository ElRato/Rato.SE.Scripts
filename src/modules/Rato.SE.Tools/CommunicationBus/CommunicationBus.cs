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
        public class CommunicationBus
        {
            private ILogger _logger;
            
            private Dictionary<string, IControllModule> _modules;

            public CommunicationBus(ILogger logger)
            {
                _logger = logger;
                _modules = new Dictionary<string, IControllModule>();
            }

            public void AddModule(string key, IControllModule module)
            {
                if (_modules.ContainsKey(key))
                {
                    _logger.LogWarning($"Bus: module {key} already exist");
                }
                else
                {
                    _modules.Add(key, module);
                    _logger.LogInformation($"Bus: module {key} registered");
                }
            }

            public void Initialize()
            {
                foreach (var module in _modules.Values)
                {
                    module.Initialize();
                }
            }

            public UpdateFrequency StartSelfTest()
            {
                var updateFrequency = UpdateFrequency.None;
                foreach (var module in _modules.Values)
                {
                    if (module is ISelfTestableModule && module.Status.IncludeToUpdateSequence)
                        updateFrequency |= (module as ISelfTestableModule).StartTestSquence();
                }

                return updateFrequency;
            }

            public UpdateFrequency Autostart() {
                var updateFrequency = UpdateFrequency.None;
                foreach (var module in _modules.Values)
                {
                    //[TODO]Stop this check if all was processes
                    if (module.Status == ModuleStatus.ReadyToStart) {
                        updateFrequency |= (module as IAutoStartModule).AutoStart();
                    }
                }
                return updateFrequency;
            }

            public UpdateFrequency Update(UpdateType currentHit)
            {
                var updateHit = currentHit | (UpdateType.Update1 | UpdateType.Update10 | UpdateType.Update100);

                var updateFrequency = UpdateFrequency.None;
                foreach (var module in _modules.Values)
                {
                    if(module.Status.IncludeToUpdateSequence)
                        updateFrequency |= module.ContinueSequence(updateHit);
                }
                return updateFrequency;
            }

            public UpdateFrequency TerminalAction(UpdateType currentHit, string argument) {
                var updateHit = currentHit | UpdateType.Terminal | UpdateType.Trigger;

                var updateFrequency = UpdateFrequency.None;
                foreach (var module in _modules.Values)
                {
                    if (module.Status.FullyOperatable && module is ITerminalModule)
                        updateFrequency |= (module as ITerminalModule).TerminalAction(updateHit, argument);
                }
                return updateFrequency;
            }

            public void LogModuleState(ILogger stateLogger) {
                foreach (var module in _modules)
                {
                    stateLogger.LogInformation($"{module.Key}: {module.Value.Status.Name}");
                    foreach (var checkItem in module.Value.StatusDetails)
                    {
                        switch (checkItem.Level)
                        {
                            case ActionStatus.Ok:
                                stateLogger.LogInformation($"{checkItem.Name}: OK"); break;
                            case ActionStatus.Warning:
                                stateLogger.LogWarning($"{checkItem.Name}: Warning"); break;
                            case ActionStatus.Error:
                                stateLogger.LogError($"{checkItem.Name}: Failed"); break;
                        }
                    }
                }
            }
            public bool SetConfig(DataStoreHandler dataStoreHandler)
            {
                var reinitRequired = false;
                foreach (var module in _modules.Values)
                {
                    if (module is IConfigurableModule)
                    {
                        reinitRequired |= (module as IConfigurableModule).SetConfig(dataStoreHandler);
                    }
                }
                return reinitRequired;
            }
            public bool SetState(DataStoreHandler dataStoreHandler)
            {
                var reinitRequired = false;
                foreach (var module in _modules.Values)
                {
                    if (module is IConfigurableModule)
                    {
                        reinitRequired |= (module as IConfigurableModule).SetState(dataStoreHandler);
                    }
                }
                return reinitRequired;
            }
            public void SaveConfig(DataStoreHandler dataStoreHandler)
            {
                foreach (var module in _modules.Values)
                {
                    if (module is IConfigurableModule)
                    {
                        (module as IConfigurableModule).SaveConfig(dataStoreHandler);
                    }
                }
            }
            public void SaveState(DataStoreHandler dataStoreHandler)
            {
                foreach (var module in _modules.Values)
                {
                    if (module is IConfigurableModule)
                    {
                        (module as IConfigurableModule).SaveState(dataStoreHandler);
                    }
                }
            }
        }
    }
}
