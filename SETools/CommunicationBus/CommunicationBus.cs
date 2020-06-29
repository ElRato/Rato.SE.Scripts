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
            private List<StateCheckItem> _stateCheckList;

            private Dictionary<string, IControllModule> _modules;

            public CommunicationBus(DebuggerSettings dbgSettings, ILogger logger) {
                _dbgSettings = dbgSettings;
                _logger = logger;
                _modules = new Dictionary<string, IControllModule>();
                _stateCheckList = new List<StateCheckItem>();
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

            public void Initialize(ILogger stateLogger)
            {
                foreach (var module in _modules)
                {
                    _stateCheckList.Clear();
                    module.Value.Initialize(_stateCheckList);
                    foreach (var checkItem in _stateCheckList)
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
