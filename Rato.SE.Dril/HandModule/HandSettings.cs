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

/*
[HandSettings]
PistonSufix=hnd
PistonMaxSpeed=0.5
RotorSufix=hnd
*/
namespace IngameScript
{
    partial class Program
    {
        public class HandSettings : ISettings
        {
            public string PistonSufix;
            public string RotorSufix;
            public double PistonMaxSpeed;

            public double MaxAngle;
            public double MinAngle;
            public double RotateAngleTotal;
            public double RotateStep;
            public int RotateDirection;
            public double TargetElevation;

            public void LoadValues(MyIni config)
            {
                PistonSufix = config.Get(nameof(HandSettings), nameof(PistonSufix)).ToString("hnd");
                PistonMaxSpeed = config.Get(nameof(HandSettings), nameof(PistonMaxSpeed)).ToDouble(0.5);
                RotorSufix = config.Get(nameof(HandSettings), nameof(RotorSufix)).ToString("hnd");
                MaxAngle = config.Get(nameof(HandSettings), nameof(MaxAngle)).ToDouble(0);
                MinAngle = config.Get(nameof(HandSettings), nameof(MinAngle)).ToDouble(0);
                RotateDirection = config.Get(nameof(HandSettings), nameof(RotateDirection)).ToInt32(1);
                TargetElevation = config.Get(nameof(HandSettings), nameof(TargetElevation)).ToDouble(0);
                RotateAngleTotal = config.Get(nameof(HandSettings), nameof(RotateAngleTotal)).ToDouble(0);
                RotateStep = config.Get(nameof(HandSettings), nameof(RotateStep)).ToDouble(0);
            }

            public void WriteValues(MyIni config)
            {
                config.Set(nameof(HandSettings), nameof(PistonSufix), PistonSufix);
                config.Set(nameof(HandSettings), nameof(PistonMaxSpeed), PistonMaxSpeed);
                config.Set(nameof(HandSettings), nameof(RotorSufix), RotorSufix);
                config.Set(nameof(HandSettings), nameof(MaxAngle), MaxAngle);
                config.Set(nameof(HandSettings), nameof(MinAngle), MinAngle);
                config.Set(nameof(HandSettings), nameof(RotateDirection), RotateDirection);
                config.Set(nameof(HandSettings), nameof(TargetElevation), TargetElevation);
                config.Set(nameof(HandSettings), nameof(RotateAngleTotal), RotateAngleTotal);
                config.Set(nameof(HandSettings), nameof(RotateStep), RotateStep);
            }
        }
    }
}
