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
        private SystemControlUnit _scu;
        private CommunicationBus _bus;

        public Program()
        {
            _logger = new EchoLogger(this);
            var lcd = Me.GetSurface(0);
            _stateLcd = new LcdTextLogger(this, "System", lcd);

            //var ccModule = new CargoControlModule(this, _logger);

            _bus = new CommunicationBus(_logger);
            //_bus.AddModule("CargoControl", ccModule);

            _scu = new SystemControlUnit(_logger);
            _scu.UseCommunicationBus(_bus);
            _scu.UseConfigStorage(new CustomDataLowLevelStore(Me));
            _scu.UseDurableStorage(new ProgramLowLevelStore(this));

            Runtime.UpdateFrequency = _scu.Initialize();
        }

        public void Save()
        {
            _scu.Save();
        }

        public void Main(string argument, UpdateType updateSource)
        {
            try
            {
                _scu.RunIteration(updateSource, argument);
                //[TODO] Include cleanup into logger function
                Me.GetSurface(0).WriteText($"", false);
                _bus.LogModuleState(_stateLcd);
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
            }
        }
    }
}
