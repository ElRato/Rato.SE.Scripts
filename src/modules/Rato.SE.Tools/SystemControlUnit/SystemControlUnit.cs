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
using System.Data;

namespace IngameScript
{
    partial class Program
    {
        public class SystemControlUnit
        {
            private ILogger _logger;

            private DataStoreHandler _configStoreHandler;
            private DataStoreHandler _durableStoreHandler;
            private CommunicationBus _bus;

            public SystemControlUnit(
                ILogger logger)
            {
                _logger = logger;
            }

            public void UseConfigStorage(ILowLevelStore store)
            {
                _configStoreHandler = new DataStoreHandler(store);
            }
            public void UseDurableStorage(ILowLevelStore store)
            {
                _durableStoreHandler = new DataStoreHandler(store);
            }
            public void UseCommunicationBus(CommunicationBus bus)
            {
                _bus = bus;
            }

            public UpdateFrequency Initialize()
            {
                if (_configStoreHandler == null || _durableStoreHandler == null || _bus == null)
                {
                    _logger.LogError("One of the mandatory dependencies wan not configured");
                }
                var updateFrequency = UpdateFrequency.None;

                _bus.SetConfig(_configStoreHandler);
                _bus.Initialize();
                _bus.SaveConfig(_configStoreHandler);
                if (_configStoreHandler.IsEmpty())
                    _configStoreHandler.Save();
                updateFrequency |= _bus.StartSelfTest();
                updateFrequency |= _bus.Autostart();

                return updateFrequency;
            }

            public UpdateFrequency RunIteration(UpdateType currentHit, string argument) {
                var updateFrequency = UpdateFrequency.None;

                var updateHit = currentHit | (UpdateType.Update1 | UpdateType.Update10 | UpdateType.Update100);
                var terminalHit = currentHit | (UpdateType.Terminal | UpdateType.Trigger);
                if (terminalHit != 0) {
                    updateFrequency |= TerminalAction(terminalHit, argument);
                }
                if (updateHit != 0) {
                    updateFrequency |= _bus.Update(updateHit);
                    updateFrequency |= _bus.Autostart();
                }
                return updateFrequency;
            }

            public void Save()
            {
                _bus.SaveConfig(_durableStoreHandler);
                _bus.SaveState(_durableStoreHandler);
                _durableStoreHandler.Save();
            }

            public UpdateFrequency TerminalAction(UpdateType currentHit, string argument)
            {
                var updateHit = currentHit | UpdateType.Terminal | UpdateType.Trigger;
                var updateFrequency = UpdateFrequency.None;
                
                if (argument == "Action.SettingsTemplate")
                {
                    _logger.LogInformation("Write current settings");
                    _bus.SaveConfig(_configStoreHandler);
                }
                else if (argument == "Action.Init")
                {
                    _logger.LogInformation("Initialize");
                    Initialize();
                }
                else if (argument == "Action.Reconfigure")
                {
                    _logger.LogInformation("Reset Config");
                    if (_bus.SetConfig(_configStoreHandler))
                    {
                        _bus.Initialize();
                        updateFrequency |= _bus.StartSelfTest();
                        updateFrequency |= _bus.Autostart();
                    }
                }
                else
                {
                    updateFrequency |= _bus.TerminalAction(currentHit, argument);
                }

                return updateFrequency;
            }
        }
    }
}
