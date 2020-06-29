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

            public void LoadValues(MyIni config)
            {
                PistonSufix = config.Get(nameof(HandSettings), nameof(PistonSufix)).ToString();
                PistonMaxSpeed = config.Get(nameof(HandSettings), nameof(PistonMaxSpeed)).ToDouble();
                RotorSufix = config.Get(nameof(HandSettings), nameof(RotorSufix)).ToString();
            }

            public void WriteValues(MyIni config)
            {
                config.Set(nameof(HandSettings), nameof(PistonSufix), PistonSufix);
                config.Set(nameof(HandSettings), nameof(PistonMaxSpeed), PistonMaxSpeed);
                config.Set(nameof(HandSettings), nameof(RotorSufix), RotorSufix);
            }
        }
    }
}