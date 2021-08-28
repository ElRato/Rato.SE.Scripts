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

        private SystemControlUnit _controlUnit;
        
        private CommunicationBus _communicationBus;
        private HandModule _handModule;

        public Program()
        {
            _logger = new EchoLogger(this);
            var lcd = Me.GetSurface(0);
            _stateLcd = new LcdTextLogger(this, "System", lcd);

            _handModule = new HandModule(this, _logger);
            _communicationBus = new CommunicationBus(_logger);
            _communicationBus.AddModule("MainHand", _handModule);
            
            _controlUnit = new SystemControlUnit(_logger);
            _controlUnit.UseCommunicationBus(_communicationBus);
            _controlUnit.UseConfigStorage(new CustomDataLowLevelStore(Me));
            _controlUnit.UseDurableStorage(new ProgramLowLevelStore(this));

            Runtime.UpdateFrequency = _controlUnit.Initialize();
        }

        public void Save() { 
            _controlUnit.Save();
        }

        public void Main(string argument, UpdateType updateSource)
        {
            try
            {
                _controlUnit.RunIteration(updateSource, argument);
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
