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
            _handSettings = _settingsHandler.ReadSettings<HandSettings>(_handSettings);
            _debuggerSettings = _settingsHandler.ReadSettings<DebuggerSettings>(_debuggerSettings);

            _communicationBus = new CommunicationBus(_debuggerSettings, _logger);
            _handModule = new HandModule(_handSettings, this, _logger);

            _communicationBus.AddModule("MainHand", _handModule);
            _communicationBus.Initialize(_stateLcd);

            Runtime.UpdateFrequency = UpdateFrequency.Once;
            Runtime.UpdateFrequency |= _handModule.StartTestSquence();
        }

        public void Save()
        {
        }

        public void Main(string argument, UpdateType updateSource)
        {
            try
            {
                if (argument == "Action.Reconfigure")
                {
                    _logger.LogInformation("Reset Setting");
                    _settingsHandler.ResetToManual();
                    _handSettings = _settingsHandler.ReadSettings(_handSettings);
                    _debuggerSettings = _settingsHandler.ReadSettings(_debuggerSettings);
                    _stateLcd.LogInformation("Reconfiguration");
                    _communicationBus.Initialize(_stateLcd);
                    Runtime.UpdateFrequency |= _handModule.StartTestSquence();
                    throw new Exception("CatchMe");
                }

                //move under communication bus
                Runtime.UpdateFrequency |= _handModule._handPositionController.ContinueSequence(updateSource);
            }
            catch (Exception e) {
                _logger.LogInformation(e.Message);
            }
        }
    }
}
