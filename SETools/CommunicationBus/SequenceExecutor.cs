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
using System.Data;
using System.Runtime.InteropServices.WindowsRuntime;

namespace IngameScript
{
    partial class Program
    {
        public enum SequenceExecutorState
        {
            InProgress,
            NoOperation
        }
        public class SequenceExecutor
        {
            private IEnumerator<int> _currentSequence;
            private TickTimer _timer;

            private ILogger _logger;
            public SequenceExecutorState State { get; set; }

            public SequenceExecutor(ILogger logger)
            {
                _logger = logger;
                State = SequenceExecutorState.NoOperation;
                _timer = new TickTimer(_logger);
            }
            public UpdateFrequency StartSequence(IEnumerator<int> sequence)
            {
                _logger.LogInformation("Started Sequence");
                if (sequence == null)
                    throw new ArgumentException("Should be not null", nameof(sequence));

                _currentSequence = sequence;

                return ProcessStep();
            }

            public UpdateFrequency ContinueSequence(UpdateType updateEvent)
            {
                if (State == SequenceExecutorState.NoOperation) {
                    _logger.LogInformation("No Action");
                    return UpdateFrequency.None;
                }

                _logger.LogInformation("Continue Sequence");

                var nextDelay = _timer.TickNext(updateEvent);

                _logger.LogInformation($"Ticks left {_timer.ActualTics} of {_timer.TargetTics}");

                if (nextDelay == UpdateFrequency.None) {
                    return ProcessStep();
                }

                return nextDelay;
            }

            private UpdateFrequency ProcessStep()
            {
                _logger.LogInformation("ProcessStep");
                if (_currentSequence.MoveNext())
                {
                    _logger.LogInformation("Has next Step");
                    State = SequenceExecutorState.InProgress;
                    return _timer.StartTimer(_currentSequence.Current);
                }
                else
                {
                    _logger.LogInformation("Stop Executon");
                    State = SequenceExecutorState.NoOperation;
                    return UpdateFrequency.None;
                }
            }
        }
    }
}
