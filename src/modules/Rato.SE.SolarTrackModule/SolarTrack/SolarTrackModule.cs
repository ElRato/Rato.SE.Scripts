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
    partial class Program
    {
        public partial class SolarTrackModule : IControllModule, ISelfTestableModule, IAutoStartModule, IConfigurableModule
        {
            private RatoMath _ratoMath;
            private SolarTrackSettings _settings;
            private SolarTrackState _state;
            private MyGridProgram _program;
            private ILogger _logger;

            private IMyMotorStator _rotor1;
            private IMyMotorStator _rotor2;
            private IMySolarPanel _pannel;

            public SequenceExecutor _trackController;

            public List<ModuleStatusDetail> StatusDetails { get; private set; }

            public ModuleStatus Status { get; set; }

            public SolarTrackModule(MyGridProgram program, ILogger logger)
            {
                _ratoMath = new RatoMath();
                _program = program;
                _logger = logger;

                _trackController = new SequenceExecutor(_logger);
                
                StatusDetails = new List<ModuleStatusDetail>();
                Status = ModuleStatus.JustCreated;

                _settings = new SolarTrackSettings();
                _state = new SolarTrackState();
            }

            public bool SetConfig(DataStoreHandler storeHandler)
            {
                _settings = storeHandler.ReadFromStore(_settings);
                //[TODO] implement check for changed values
                return false;
            }

            public bool SetState(DataStoreHandler storeHandler)
            {
                _state = storeHandler.ReadFromStore(_state);
                //[TODO] implement check for changed values
                return false;
            }

            public void SaveConfig(DataStoreHandler storeHandler)
            {
                storeHandler.WriteToStore(_settings);
            }

            public void SaveState(DataStoreHandler storeHandler)
            {
                storeHandler.WriteToStore(_state);
            }

            public UpdateFrequency ContinueSquence(UpdateType updateSource)
            {
                _logger.LogInformation($"Solar Track controller state:{_trackController.State}");
                _logger.LogInformation($"output: {_pannel.MaxOutput}");
                return _trackController.ContinueSequence(updateSource);
            }

            public UpdateFrequency AutoStart()
            {
                Status = ModuleStatus.Active;
                return StartTrackingSquence();
            }

            private void SetrotorVelocity(IMyMotorStator rotor, float velocity)
            {
                    rotor.TargetVelocityRad = velocity;
            }
        }
    }
}
