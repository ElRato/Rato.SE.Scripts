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
        public struct ManipulatorSettings
        {
            public string Name;

            public string Sufix;

            public double Multiplier;
            public double MaxSpeed;

            public double EffectiveMultiplier;     
            public void LoadValues(MyIni config, string section)
            {
                Sufix = config.Get(section, $"{Name}_{nameof(Sufix)}").ToString(Sufix);

                Multiplier = config.Get(section, $"{Name}_{nameof(Multiplier)}").ToDouble(Multiplier);
                MaxSpeed = config.Get(section, $"{Name}_{nameof(MaxSpeed)}").ToDouble(MaxSpeed);

                EffectiveMultiplier = Multiplier;
            }

            public void WriteValues(MyIni config, string section)
            {
                config.Set(section, $"{Name}_{nameof(Sufix)}", Sufix);
                config.Set(section, $"{Name}_{nameof(Multiplier)}", Multiplier);
                config.Set(section, $"{Name}_{nameof(MaxSpeed)}", MaxSpeed);
            }
        }
        public class ExpandableHandSettings : IDataStore
        {

            public string ControllerSufix;
            public double GenericMultiplier;

            public ManipulatorSettings HorizontalRotor = new ManipulatorSettings() { Name = "HorizRotor", Sufix = "ehnd_hor_rtr", Multiplier = 1, MaxSpeed = 0.15};
            public ManipulatorSettings ExtendRotorFirst = new ManipulatorSettings() { Name = "ExtRotorF", Sufix = "ehnd_ext_rtr1", Multiplier = 1, MaxSpeed = 0.15 };
            public ManipulatorSettings ExtendRotorSecond = new ManipulatorSettings() { Name = "ExtRotorS", Sufix = "ehnd_ext_rtr2", Multiplier = 1, MaxSpeed = 0.15 };
            public ManipulatorSettings ToolHorizontalRotor = new ManipulatorSettings() { Name = "ToolHorizRotor", Sufix = "ehnd_t_hor_rtr", Multiplier = 1, MaxSpeed = 0.15 };
            public ManipulatorSettings ToolVerticalRotor = new ManipulatorSettings() { Name = "ToolVertRotor", Sufix = "ehnd_t_vrt_rtr", Multiplier = 1, MaxSpeed = 0.15 };

            public void LoadValues(MyIni config)
            {
                ControllerSufix = config.Get(nameof(ExpandableHandSettings), nameof(ControllerSufix)).ToString("ehnd");
                GenericMultiplier = config.Get(nameof(ExpandableHandSettings), nameof(GenericMultiplier)).ToDouble(1);

                HorizontalRotor.LoadValues(config, nameof(ExpandableHandSettings));
                ExtendRotorFirst.LoadValues(config, nameof(ExpandableHandSettings));
                ExtendRotorSecond.LoadValues(config, nameof(ExpandableHandSettings));
                ToolHorizontalRotor.LoadValues(config, nameof(ExpandableHandSettings));
                ToolVerticalRotor.LoadValues(config, nameof(ExpandableHandSettings));

                HorizontalRotor.EffectiveMultiplier *= GenericMultiplier;
                ExtendRotorFirst.EffectiveMultiplier *= GenericMultiplier;
                ExtendRotorSecond.EffectiveMultiplier *= GenericMultiplier;
                ToolHorizontalRotor.EffectiveMultiplier *= GenericMultiplier;
                ToolVerticalRotor.EffectiveMultiplier *= GenericMultiplier;
            }

            public void WriteValues(MyIni config)
            {
                config.Set(nameof(ExpandableHandSettings), nameof(ControllerSufix), ControllerSufix);
                config.Set(nameof(ExpandableHandSettings), nameof(GenericMultiplier), GenericMultiplier);

                HorizontalRotor.WriteValues(config, nameof(ExpandableHandSettings));
                ExtendRotorFirst.WriteValues(config, nameof(ExpandableHandSettings));
                ExtendRotorSecond.WriteValues(config, nameof(ExpandableHandSettings));
                ToolHorizontalRotor.WriteValues(config, nameof(ExpandableHandSettings));
                ToolVerticalRotor.WriteValues(config, nameof(ExpandableHandSettings));
            }
        }
    }
}

