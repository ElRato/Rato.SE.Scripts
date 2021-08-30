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
        public class SolarTrackSettings : IDataStore
        {
            public string Rotor1Sufix;
            public string Rotor2Sufix;
            public string PannelSufix;
            public int TestSpeedDividor;
            public int OperationSpeedDividor;
            public int TunningTimeStep;
            public double TunningAccuracy;

            public void LoadValues(MyIni config)
            {
                Rotor1Sufix = config.Get(nameof(SolarTrackSettings), nameof(Rotor1Sufix)).ToString("slr_trk_1");
                Rotor2Sufix = config.Get(nameof(SolarTrackSettings), nameof(Rotor2Sufix)).ToString("slr_trk_2");
                PannelSufix = config.Get(nameof(SolarTrackSettings), nameof(PannelSufix)).ToString("slr_trk_pnl");
                TestSpeedDividor = config.Get(nameof(SolarTrackSettings), nameof(TestSpeedDividor)).ToInt32(4);
                OperationSpeedDividor = config.Get(nameof(SolarTrackSettings), nameof(OperationSpeedDividor)).ToInt32(8*16);
                TunningTimeStep = config.Get(nameof(SolarTrackSettings), nameof(TunningTimeStep)).ToInt32(10);
                TunningAccuracy = config.Get(nameof(SolarTrackSettings), nameof(TunningAccuracy)).ToDouble(0.00002);
            }

            public void WriteValues(MyIni config)
            {
                config.Set(nameof(SolarTrackSettings), nameof(Rotor1Sufix), Rotor1Sufix);
                config.Set(nameof(SolarTrackSettings), nameof(Rotor2Sufix), Rotor2Sufix);
                config.Set(nameof(SolarTrackSettings), nameof(PannelSufix), PannelSufix);
                config.Set(nameof(SolarTrackSettings), nameof(TestSpeedDividor), TestSpeedDividor);
                config.Set(nameof(SolarTrackSettings), nameof(OperationSpeedDividor), OperationSpeedDividor);
                config.Set(nameof(SolarTrackSettings), nameof(TunningTimeStep), TunningTimeStep);
                config.Set(nameof(SolarTrackSettings), nameof(TunningAccuracy), TunningAccuracy);
            }
        }
    }
}
