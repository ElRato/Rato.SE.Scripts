using System;
using System.Linq;
using FluentAssertions;
using IngameScript;
using Moq;
using NUnit.Framework;
using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    public class SystemControlUnitTests
    {
        private InfrastructureTester _tester;
        
        [SetUp]
        public void Setup()
        {
            _tester = new InfrastructureTester();
        }

        [Test]
        public void ItInitializedDumbModule()
        {
            //Arrange
            var moq = new Mock<DumbModule>() {CallBase = true};
            var initializeCount = 0;
            
            moq.As<Program.IControllModule>().Setup(m => m.Initialize())
                .Callback(() => { initializeCount++; })
                .CallBase();
            
            _tester.Module = moq.Object;
            //Act
            var updateFrequency = _tester.SimulateProgramConstrutor();
            //Assert
            updateFrequency.Should().Be(UpdateFrequency.None, $"UpdateFrequency should be 0 because there is no autostart or self test");
            _tester.Module.Status.Should().Be(Program.ModuleStatus.Initialized);
            initializeCount.Should().Be(1);
            moq.Verify();
        }
        
        [Test]
        public void ItConfigureModule()
        {
            //Arrange
            string callSequence = "";

            var moq = new Mock<DumbModule>() {CallBase = true};
            moq.As<Program.IConfigurableModule>().Setup(m => m.SetConfig(It.IsAny<Program.DataStoreHandler>()))
                .Callback<Program.DataStoreHandler>(h => { callSequence += "SetConfig|"; })
                .Returns(false);
            moq.As<Program.IControllModule>().Setup(m => m.Initialize())
                .Callback(() => { callSequence += "Initialize|"; })
                .CallBase();
            moq.As<Program.IConfigurableModule>().Setup(m => m.SaveConfig(It.IsAny<Program.DataStoreHandler>()))
                .Callback<Program.DataStoreHandler>(h => { callSequence += "SaveConfig|";});

            _tester.Module = moq.Object;
            //Act
            var updateFrequency = _tester.SimulateProgramConstrutor();
            //Assert
            updateFrequency.Should().Be(UpdateFrequency.None, $"UpdateFrequency should be 0 because there is no autostart or self test");
            callSequence.Should().Be("SetConfig|Initialize|SaveConfig|");
        }
    }
}