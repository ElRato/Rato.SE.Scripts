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
        public class CargoControlSettings : IDataStore
        {

            public string CapacityCocpitSufix;
            public string CapacityCargoSufix;

            public void LoadValues(MyIni config)
            {
                CapacityCocpitSufix = config.Get(nameof(CargoControlSettings), nameof(CapacityCocpitSufix)).ToString("ccd");
                CapacityCargoSufix = config.Get(nameof(CargoControlSettings), nameof(CapacityCargoSufix)).ToString("ccc");
            }

            public void WriteValues(MyIni config)
            {
                config.Set(nameof(CargoControlSettings), nameof(CapacityCocpitSufix), CapacityCocpitSufix);
                config.Set(nameof(CargoControlSettings), nameof(CapacityCargoSufix), CapacityCargoSufix);
            }
        }
    }
}

