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
using System.Diagnostics.Eventing.Reader;

namespace IngameScript
{
    partial class Program
    {
        public class BlockUtils
        {
            private const string OnOff_On = "OnOff_On";
            private const string OnOff_Off = "OnOff_Off";
            public bool TryTurnOn(IMyTerminalBlock block)
            {
                return TryAction(block, OnOff_On);
            }

            public bool TryTurnOff(IMyTerminalBlock block)
            {
                return TryAction(block, OnOff_Off);
            }

            public bool TryTurnOn<T>(List<T> blocks) where T : class, IMyTerminalBlock
            {
                return TryAction(blocks, OnOff_On);
            }

            public bool TryTurnOff<T>(List<T> blocks) where T : class, IMyTerminalBlock
            {
                return TryAction(blocks, OnOff_Off);
            }

            public bool TryAction<T>(List<T> blocks, string action) where T : class, IMyTerminalBlock
            {
                if (blocks.All(b => b.HasAction(action)))
                {
                    foreach (var b in blocks)
                        b.ApplyAction(action);
                    return true;
                }
                return false;
            }

            public bool TryAction(IMyTerminalBlock block, string action)
            {
                if (block.IsFunctional && block.HasAction(action))
                {
                    block.ApplyAction(action);
                    return true;
                }
                return false;
            }
        }
    }
}
