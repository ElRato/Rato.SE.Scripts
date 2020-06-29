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
            private DebuggerSettings _dbgSettings;
            private ILogger _logger;

            private Dictionary<string, IControllModule> _modules;

            public CommunicationBus(DebuggerSettings dbgSettings, ILogger logger)
            {
                _dbgSettings = dbgSettings;
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
                    if (module.State.IncludeToUpdateSequence)
                        updateFrequency |= module.StartTestSquence();
                }

                return updateFrequency;
            }

            public UpdateFrequency Update(UpdateType currentHit)
            {
                var updateFrequency = UpdateFrequency.None;
                foreach (var module in _modules.Values)
                {
                    if(module.State.IncludeToUpdateSequence)
                        updateFrequency |= module.ContinueSquence(currentHit);
                }
                return updateFrequency;
            }

            public void LogModuleState(ILogger stateLogger) {
                foreach (var module in _modules)
                {
                    stateLogger.LogInformation($"{module.Key}: {module.Value.State.Name}");
                    foreach (var checkItem in module.Value.StateDetails)
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
        }
    }
}
