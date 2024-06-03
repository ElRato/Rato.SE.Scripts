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
using ParallelTasks;

namespace IngameScript
{
    partial class Program
    {
        public partial class SolarTrackModule : IControllModule
        {
            public UpdateFrequency StartTrackingSquence()
            {
                return _trackController.StartSequence(TrackingSequence());
            }

            private IEnumerable<int> WaitForOutputChange() {
                var output = _pannel.MaxOutput;
                while (output == _pannel.MaxOutput) {
                    yield return 100;
                }
            }

            private IEnumerable<int> RotorAdjust(IMyMotorStator rotor)
            {
                var accuracy = 0.0001;

                var baseOutput = _pannel.MaxOutput;

                foreach (var step in WaitForOutputChange()) yield return step;
                if (_ratoMath.GreaterThen(baseOutput, _pannel.MaxOutput, accuracy))
                {
                    SetrotorVelocity(rotor, -rotor.TargetVelocityRad);
                    baseOutput = _pannel.MaxOutput;
                    foreach (var step in WaitForOutputChange()) yield return step;
                }
                while (_ratoMath.GreaterThen(_pannel.MaxOutput, baseOutput, accuracy))
                {
                    baseOutput = _pannel.MaxOutput;
                    foreach (var step in WaitForOutputChange()) yield return step;
                };
            }

            private IEnumerator<int> TrackingSequence()
            {
                while (true)
                {
                    if (_pannel.MaxOutput == 0)
                    {
                        yield return 100000;
                    }
                    else
                    {

                        SetrotorVelocity(_rotor1, _state.Rotor1TrackDirection * MathHelper.Pi / _settings.OperationSpeedDividor);
                        foreach (var step in RotorAdjust(_rotor1)) yield return step;
                        _state.Rotor1TrackDirection = _rotor1.TargetVelocityRad > 0 ? 1 : -1;
                        SetrotorVelocity(_rotor1, 0);

                        if (_rotor2 != null)
                        {
                            SetrotorVelocity(_rotor2, _state.Rotor2TrackDirection * MathHelper.Pi / _settings.OperationSpeedDividor);
                            foreach (var step in RotorAdjust(_rotor2)) yield return step;
                            _state.Rotor2TrackDirection = _rotor2.TargetVelocityRad > 0 ? 1 : -1;
                            SetrotorVelocity(_rotor2, 0);
                        }
                        yield return 5000;
                    }
                }
            }
        }
    }
}
