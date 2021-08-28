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

            private IEnumerator<int> TrackingSequence()
            {
                var tunningStep = 5000;
                var rotationSpeed1 = MathHelper.Pi / 4 / 16;
                var rotationSpeed2 = rotationSpeed1;
                var accuracy = 0.0001;
                while (true)
                {
                    if (_pannel.MaxOutput == 0)
                    {
                        yield return 100000;
                    }
                    else
                    {
                        var baseOutput = _pannel.MaxOutput;

                        SetrotorVelocity(_rotor1, rotationSpeed1);
                        yield return tunningStep;
                        if (_ratoMath.GreaterThen(baseOutput, _pannel.MaxOutput, accuracy))
                        {
                            rotationSpeed1 = -rotationSpeed1;
                            SetrotorVelocity(_rotor1, rotationSpeed1);
                            baseOutput = _pannel.MaxOutput;
                            yield return tunningStep;
                        }
                        while (_ratoMath.GreaterThen(_pannel.MaxOutput, baseOutput, accuracy)) {
                            baseOutput = _pannel.MaxOutput;
                            yield return tunningStep;
                        }
                        SetrotorVelocity(_rotor1, 0);

                        SetrotorVelocity(_rotor2, rotationSpeed2);
                        yield return tunningStep;
                        if (_ratoMath.GreaterThen(baseOutput, _pannel.MaxOutput, accuracy))
                        {
                            rotationSpeed2 = -rotationSpeed2;
                            SetrotorVelocity(_rotor2, rotationSpeed2);
                            baseOutput = _pannel.MaxOutput;
                            yield return tunningStep;
                        }
                        while (_ratoMath.GreaterThen(_pannel.MaxOutput, baseOutput, accuracy))
                        {
                            baseOutput = _pannel.MaxOutput;
                            yield return tunningStep;
                        }
                        SetrotorVelocity(_rotor2, 0);

                        yield return 5000;
                    }
                }
            }
        }
    }
}
