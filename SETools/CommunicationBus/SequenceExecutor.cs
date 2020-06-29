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
        public enum SequenceExecutorState
        {
            Waiting,
            NoOperation
        }
        public class SequenceExecutor
        {
            private IEnumerator<int> _currentSequence;
            private int _ticksFromLastStep;
            private int _delayBeforeNextStepl;
            private ILogger _logger;
            public SequenceExecutorState State { get; set; }

            public SequenceExecutor(ILogger logger)
            {
                _logger = logger;
                State = SequenceExecutorState.NoOperation;
            }
            public UpdateFrequency StartSequence(IEnumerator<int> sequence)
            {
                _logger.LogInformation("StartedSequence");
                if (sequence == null)
                    return UpdateFrequency.None;

                _currentSequence = sequence;
                State = SequenceExecutorState.Waiting;
                return ProcessStep();
            }

            public UpdateFrequency ContinueSequence(UpdateFrequency updateEvent)
            {
                if (State == SequenceExecutorState.NoOperation)
                    return UpdateFrequency.None;

                _logger.LogInformation("ContinueSequence");
                if ((updateEvent | UpdateFrequency.Update1) >0)
                {
                    _logger.LogInformation($"Check ticks {_ticksFromLastStep} of {_delayBeforeNextStepl}");
                    _ticksFromLastStep++;
                    if (_ticksFromLastStep >= _delayBeforeNextStepl)
                    {
                        _logger.LogInformation($"TryNextStep");
                        return ProcessStep();
                    }
                }
                return UpdateFrequency.Update1;
            }

            private UpdateFrequency ProcessStep() {
                _logger.LogInformation("ProcessStep");
                if (_currentSequence.MoveNext())
                {
                    _logger.LogInformation("Has next Step");
                    _ticksFromLastStep = 0;
                    _delayBeforeNextStepl = _currentSequence.Current;
                    return UpdateFrequency.Update1;
                }
                else
                {
                    _logger.LogInformation("StopExecuton");
                    State = SequenceExecutorState.NoOperation;
                    return UpdateFrequency.None;
                }
            }
        }
    }
}
