﻿using Sandbox.Game.EntityComponents;
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
using Sandbox.ModAPI;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        private ILogger _logger;
        private ILogger _stateLcd;

        private HandSettings _handSettings;
        private SettingsHandler _settingsHandler;
        private DebuggerSettings _debuggerSettings;
        
        private CommunicationBus _communicationBus;
        private HandModule _handModule;

        public Program()
        {
            _logger = new EchoLogger(this);
            var lcd = Me.GetSurface(0);
            _stateLcd = new LcdTextLogger(this, "System", lcd);

            _settingsHandler = new SettingsHandler(this);
            _handSettings = _settingsHandler.ReadFromStore<HandSettings>(_handSettings);
            _debuggerSettings = _settingsHandler.ReadFromStore<DebuggerSettings>(_debuggerSettings);

            _communicationBus = new CommunicationBus(_debuggerSettings, _logger);
            _handModule = new HandModule(_handSettings, this, _logger);

            _communicationBus.AddModule("MainHand", _handModule);
            _communicationBus.Initialize();
            Runtime.UpdateFrequency = _communicationBus.StartSelfTest();
        }

        public void Save()
        {
            _settingsHandler.Save();
        }

        public void Main(string argument, UpdateType updateSource)
        {
            try
            {
                if (argument == "Action.SettingsTemplate")
                {
                    _logger.LogInformation("Reset Setting");

                    _settingsHandler.WriteToStore(_handSettings);
                    _settingsHandler.WriteToStore(_debuggerSettings);
                    _settingsHandler.SaveToUserStorage();
                    _stateLcd.LogInformation("Settings template updated");
                }

                if (argument == "Action.Init")
                {
                    _stateLcd.LogInformation("Initialize");
                    _communicationBus.Initialize();
                    Runtime.UpdateFrequency = _communicationBus.StartSelfTest();
                    throw new Exception("CatchMe");
                }

                if (argument == "Action.Reconfigure")
                {
                    _logger.LogInformation("Reset Setting");
                    _settingsHandler.LoadFromUserStorage();
                    _handSettings = _settingsHandler.ReadFromStore(_handSettings);
                    _debuggerSettings = _settingsHandler.ReadFromStore(_debuggerSettings);
                    _stateLcd.LogInformation("Reconfiguration");
                    Save();
                }

                //[TODO] Make communication between modules.
                //[TODO] Make Controller for automatic drilling - possiblyh it should be external?
                Runtime.UpdateFrequency = _communicationBus.TerminalAction(updateSource, argument);
                Runtime.UpdateFrequency |= _communicationBus.Update(updateSource);
                Runtime.UpdateFrequency |= _communicationBus.Autostart(updateSource);

                //[TODO] Include cleanup into logger function
                Me.GetSurface(0).WriteText($"", false);
                _communicationBus.LogModuleState(_stateLcd);
            }
            catch (Exception e) {
                _logger.LogInformation(e.Message);
            }
        }
    }
}
