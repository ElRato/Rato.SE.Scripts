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
using System.Linq.Expressions;

namespace IngameScript
{
    partial class Program
    {
        public class DebuggerSettings : BaseSettings
        {
            bool SilentlyCatch;

            public override string ComposeString()
            {
                _ini.Set(nameof(HandSettings), nameof(SilentlyCatch), SilentlyCatch);
                return _ini.ToString();
            }

            public override void ParseString(string store)
            {
                base.ParseString(store);
                SilentlyCatch = _ini.Get(nameof(DebuggerSettings), nameof(SilentlyCatch)).ToBoolean();
            }
        }
    }
}
