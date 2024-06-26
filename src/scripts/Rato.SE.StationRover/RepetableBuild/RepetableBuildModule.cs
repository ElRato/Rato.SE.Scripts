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

namespace IngameScript
{
    partial class Program
    {
        public partial class RepetableBuildModule : IControllModule, ISelfTestableModule, IAutoStartModule, ITerminalModule, IConfigurableModule
        {
            private RepetableBuildSettings _settings;
            private RepetableBuildState _state;
            private MyGridProgram _program;
            private ILogger _logger;
            private ILogger _dbgLogger;

            private IMyPistonBase _piston;
            private IMyShipMergeBlock _mergeBlock;
            private IMyShipConnector _connector;
            private IMyProjector _projector;
            private IMyLandingGear _magnet;
            private List<IMyShipGrinder> _grinders;
            private List<IMyShipWelder> _welders;

            public SequenceExecutor _buildController;

            public List<ModuleStatusDetail> StatusDetails { get; private set; }

            public ModuleStatus Status { get; set; }

            private readonly BlockUtils _blockUtils;

            public RepetableBuildModule(MyGridProgram program, ILogger logger, ILogger dbg)
            {
                _program = program;
                _logger = logger;
                _dbgLogger = dbg;

                _blockUtils = new BlockUtils();

                _buildController = new SequenceExecutor(_logger);

                _tempsPistonList = new List<IMyPistonBase>();
                _tempsMergeBlocksList = new List<IMyShipMergeBlock>();
                _tempsConnectorsList = new List<IMyShipConnector>();
                _tempsProjectorsList = new List<IMyProjector>();
                _tempsMagnetsList = new List<IMyLandingGear>();

                _grinders = new List<IMyShipGrinder>();
                _welders = new List<IMyShipWelder>();
                
                StatusDetails = new List<ModuleStatusDetail>();
                Status = ModuleStatus.JustCreated;

                _settings = new RepetableBuildSettings();
                _state = new RepetableBuildState();
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

            public UpdateFrequency ContinueSequence(UpdateType updateSource)
            {
                if (_state.Operation == RepetableBuildState.BuildOperation.WaitForNext)
                {
                    var operation = _state.NextOperation;
                    _state.NextOperation = RepetableBuildState.BuildOperation.None;
                    switch (operation)
                    {
                        case RepetableBuildState.BuildOperation.Extend: return Extend();
                        case RepetableBuildState.BuildOperation.Retract: return Retract();
                        case RepetableBuildState.BuildOperation.None:
                        case RepetableBuildState.BuildOperation.Idle:
                            _state.Operation = operation; break;
                    }
                }

                return _buildController.ContinueSequence(updateSource);
            }

            public UpdateFrequency AutoStart()
            {
                _dbgLogger.LogInformation("AutoStart started");
                Status = ModuleStatus.Active;
                _state.NextOperation = RepetableBuildState.BuildOperation.Idle;
                return Reset();
            }
            
            public UpdateFrequency TerminalAction(UpdateType updateSource, string Argument)
            {
                _logger.LogInformation("Builder awaked");
                try { throw new Exception(); } catch { }
                var updateFrequency = UpdateFrequency.None;

                if (Argument.StartsWith("Builder") && (updateSource | UpdateType.Terminal | UpdateType.Trigger) > 0)
                {
                    if (Argument == "Builder.Stop")
                    {
                        updateFrequency = _buildController.StopSequence();
                        StopBuilder();
                    }
                    if (Argument == "Builder.Extend")
                    {
                        if (_state.Operation != RepetableBuildState.BuildOperation.Idle)
                        {
                            _state.NextOperation = RepetableBuildState.BuildOperation.Extend;
                            updateFrequency = Reset();
                        }
                        else
                            updateFrequency = Extend();
                    }
                    if (Argument == "Builder.Retract")
                    {
                        if (_state.Operation != RepetableBuildState.BuildOperation.Idle)
                        {
                            _state.NextOperation = RepetableBuildState.BuildOperation.Retract;
                            updateFrequency = Reset();
                        }
                        else
                            updateFrequency = Retract();
                    }
                }
                return updateFrequency;
            }

            private void StopBuilder()
            {
                _state.Operation = RepetableBuildState.BuildOperation.None;
                _piston.Velocity = 0;
                _blockUtils.TryTurnOff(_welders);
                _blockUtils.TryTurnOff(_grinders);
            }
        }
    }
}
