using Malware.MDKUtilities;
using NUnit.Framework;

namespace IngameScript
{
    public class InfrastructureIntegrationTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var config = new MDKFactory.ProgramConfig();
            var fixture = MDKFactory.CreateProgram<Program>(config);
            MDKFactory.Run(fixture);
            Assert.Pass();
        }
        [Test]
        public void Test2()
        {
            var logger = new TestLogger();
            var bus = new Program.CommunicationBus(logger);
            bus.Initialize();
        }
    }
}