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
        public class RepetableBuildSettings : IDataStore
        {
            public string MainPistonSufix;
            public string MergeBlockSufix;
            public string ConnectorSufix;
            public string ProjectorSufix;
            public string GrindersSufix;
            public string WeldersSufix;
            public string MagnetSufix;

            public void LoadValues(MyIni config)
            {
                MainPistonSufix = config.Get(nameof(RepetableBuildSettings), nameof(MainPistonSufix)).ToString("rbpi");
                MergeBlockSufix = config.Get(nameof(RepetableBuildSettings), nameof(MergeBlockSufix)).ToString("rbm");
                ConnectorSufix = config.Get(nameof(RepetableBuildSettings), nameof(ConnectorSufix)).ToString("rbc");
                ProjectorSufix = config.Get(nameof(RepetableBuildSettings), nameof(ProjectorSufix)).ToString("rbpr");
                GrindersSufix = config.Get(nameof(RepetableBuildSettings), nameof(GrindersSufix)).ToString("rbg");
                WeldersSufix = config.Get(nameof(RepetableBuildSettings), nameof(WeldersSufix)).ToString("rbw");
                MagnetSufix = config.Get(nameof(RepetableBuildSettings), nameof(MagnetSufix)).ToString("rbmg");
            }

            public void WriteValues(MyIni config)
            {
                config.Set(nameof(RepetableBuildSettings), nameof(MainPistonSufix), MainPistonSufix);
                config.Set(nameof(RepetableBuildSettings), nameof(MergeBlockSufix), MergeBlockSufix);
                config.Set(nameof(RepetableBuildSettings), nameof(ConnectorSufix), ConnectorSufix);
                config.Set(nameof(RepetableBuildSettings), nameof(ProjectorSufix), ProjectorSufix);
                config.Set(nameof(RepetableBuildSettings), nameof(GrindersSufix), GrindersSufix);
                config.Set(nameof(RepetableBuildSettings), nameof(WeldersSufix), WeldersSufix);
                config.Set(nameof(RepetableBuildSettings), nameof(MagnetSufix), MagnetSufix);

            }
        }
    }
}

