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
        public partial class SolarTrackModule : IControllModule
        {
            public UpdateFrequency StartTestSquence()
            {
                return _trackController.StartSequence(SelfTestSequence());
            }

            private IEnumerator<int> MakeFullRound(IMyMotorStator rotor) {
                var startAngle = rotor.Angle;
                SetrotorVelocity(rotor, MathHelper.Pi / _settings.TestSpeedDividor);
                yield return 5;
                while (Math.Abs(_ratoMath.ShortestAnglePath(rotor.Angle, startAngle)) > 0.02) {
                    yield return 1;
                }
                SetrotorVelocity(rotor, 0);
            }

            private IEnumerator<int> SelfTestSequence()
            {
                Status = ModuleStatus.SelfTest;

                var round1 = MakeFullRound(_rotor1);
                while (round1.MoveNext()) yield return round1.Current;

                var round2 = MakeFullRound(_rotor2);
                while (round2.MoveNext()) yield return round2.Current;

                StatusDetails.Add(new ModuleStatusDetail()
                {
                    Name = "Self test",
                    Level = ActionStatus.Ok
                });

                Status = ModuleStatus.ReadyToStart;
            }
        }
    }
}
