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
        public class FineRefineSettings : IDataStore
        {
            public string RefinarySufix;

            public void LoadValues(MyIni config)
            {
                RefinarySufix = config.Get(nameof(FineRefineSettings), nameof(RefinarySufix)).ToString("frf");
            }

            public void WriteValues(MyIni config)
            {
                config.Set(nameof(FineRefineSettings), nameof(RefinarySufix), RefinarySufix);
            }
        }

        public class FineRefinaryConfig : IDataStore
        {
            public string FullList = "\"cobalt\", \"gold\", \"iron\", \"magnesium\", \"nickel\", \"platinum\", \"silicon\", \"silver\", \"stone\", \"uranium\", \"scrap\"";
            public string[] Priorities = new string[11];

            public void LoadValues(MyIni config)
            {

                Priorities[0] = config.Get(nameof(FineRefinaryConfig), "Priority0").ToString("Stone");
                Priorities[1] = config.Get(nameof(FineRefinaryConfig), "Priority1").ToString("Scrap");
                Priorities[2] = config.Get(nameof(FineRefinaryConfig), "Priority2").ToString("Iron");
                Priorities[3] = config.Get(nameof(FineRefinaryConfig), "Priority3").ToString("Gold");
                Priorities[4] = config.Get(nameof(FineRefinaryConfig), "Priority4").ToString("Silicon");
                Priorities[5] = config.Get(nameof(FineRefinaryConfig), "Priority5").ToString("Magnesium");
                Priorities[6] = config.Get(nameof(FineRefinaryConfig), "Priority6").ToString("Silver");
                Priorities[7] = config.Get(nameof(FineRefinaryConfig), "Priority7").ToString("Nickel");
                Priorities[8] = config.Get(nameof(FineRefinaryConfig), "Priority8").ToString("Platinum");
                Priorities[9] = config.Get(nameof(FineRefinaryConfig), "Priority9").ToString("Cobalt");
                Priorities[10] = config.Get(nameof(FineRefinaryConfig), "Priority10").ToString("Uranium");
            }

            public void WriteValues(MyIni config)
            {
                //This is full list for reference it's set only
                config.Set(nameof(FineRefinaryConfig), nameof(FullList), FullList);

                for (var i = 0; i < 11; i++) {
                    config.Set(nameof(FineRefinaryConfig), $"Priority{i}", Priorities[i]);
                }
            }
        }
    }
}
