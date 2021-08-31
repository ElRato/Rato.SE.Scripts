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
        public class SolarTrackState : IDataStore
        {
            public int Rotor1TrackDirection;
            public int Rotor2TrackDirection;
            public void LoadValues(MyIni config)
            {
                Rotor1TrackDirection = config.Get(nameof(SolarTrackSettings), nameof(Rotor1TrackDirection)).ToInt32(1);
                Rotor2TrackDirection = config.Get(nameof(SolarTrackSettings), nameof(Rotor2TrackDirection)).ToInt32(1);
            }

            public void WriteValues(MyIni config)
            {
                config.Set(nameof(SolarTrackSettings), nameof(Rotor1TrackDirection), Rotor1TrackDirection);
                config.Set(nameof(SolarTrackSettings), nameof(Rotor2TrackDirection), Rotor2TrackDirection);
            }
        }
    }
}
