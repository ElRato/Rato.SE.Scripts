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
        public partial class PistonResetterModule : IModule
        {
            public void Initialize()
            {
                _program.GridTerminalSystem.GetBlocksOfType(_tempPistons, p => p.CustomName.Contains(_settings.ResetSuffix));
                if (_tempPistons.Count > 0) {
                    for (var i = 0; i < _tempPistons.Count; i++) {
                        _tempPistons[i].Velocity = 0;
                    }
                }

                Status = ModuleStatus.Initialized;
            }
        }
    }
}
