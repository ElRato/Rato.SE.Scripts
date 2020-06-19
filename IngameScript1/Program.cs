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
    partial class Program : MyGridProgram
    {
        private HandSettings _handSettings;
        private SettingsHandler _settingsHandler;
        private DebuggerSettings _debuggerSettings;
        private ILogger _logger;
        
        private List<IMyPistonBase> _handPistons;

        public Program()
        {
            _settingsHandler = new SettingsHandler(this);
            _handSettings = _settingsHandler.GetSettings<HandSettings>();
            _debuggerSettings = _settingsHandler.GetSettings<DebuggerSettings>();
            _logger = new EchoLogger(this);

            _handPistons = new List<IMyPistonBase>();

            Runtime.UpdateFrequency = UpdateFrequency.Once;
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
                    _handSettings = _settingsHandler.ResetSettings(_handSettings);
                }
                _logger.LogInformation($"Serach for {_handSettings.PistonSufix}");
                GridTerminalSystem.GetBlocksOfType(_handPistons, p => p.CustomName.Contains(_handSettings.PistonSufix));
                _logger.LogInformation(_handPistons.Count.ToString());
                _logger.LogInformation(_handPistons[0].Status.ToString());
                _handPistons.ForEach(p =>
                {
                    p.Velocity = (float)_handSettings.PistonMaxSpeed;

                });
            }
            catch (Exception e) {
                _logger.LogInformation(e.Message);
            }
        }
    }
}
