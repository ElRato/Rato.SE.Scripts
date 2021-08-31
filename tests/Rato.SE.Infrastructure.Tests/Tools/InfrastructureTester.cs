using Moq;
using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    public class InfrastructureTester
    {
        private TestLogger _logger;
        internal TestLogger Logger => _logger ??= new TestLogger();


        private DumbModule _module;
        internal DumbModule Module
        {
            get{return _module ??= new DumbModule();}
            set { _module = value; }
        }
        
        private Program.CommunicationBus _bus;
        internal Program.CommunicationBus Bus =>  _bus??= new Program.CommunicationBus(Logger);
        
        private Program.SystemControlUnit _scu;
        internal Program.SystemControlUnit Scu =>  _scu??= new Program.SystemControlUnit(Logger);

        private TestLowLevelStore _configStore;
        internal TestLowLevelStore ConfigStore =>  _configStore??= new TestLowLevelStore();
        
        private TestLowLevelStore _durableStore;
        internal TestLowLevelStore DurableStore =>  _durableStore??= new TestLowLevelStore();

        public UpdateFrequency SimulateProgramConstrutor()
        {
            Bus.AddModule("Module", Module);
            Scu.UseCommunicationBus(Bus);
            Scu.UseConfigStorage(ConfigStore);
            Scu.UseDurableStorage(DurableStore);
            return Scu.Initialize();
        }
    }
}