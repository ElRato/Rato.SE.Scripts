using IngameScript;
using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    public partial class Program: MyGridProgram
    {
        internal Program.ILogger _logger;
        internal Program.CommunicationBus _bus;
        internal Program.SystemControlUnit _scu;
        
        public Program()
        {
            _logger = new Program.EchoLogger(this, LogLevel.Debug);

            _bus = new Program.CommunicationBus(_logger);

            _scu = new Program.SystemControlUnit(_logger);
            _scu.UseCommunicationBus(_bus);
            _scu.UseConfigStorage(new Program.CustomDataLowLevelStore(Me));
            _scu.UseDurableStorage(new Program.ProgramLowLevelStore(this));

            Runtime.UpdateFrequency = _scu.Initialize();
        }
    }
}