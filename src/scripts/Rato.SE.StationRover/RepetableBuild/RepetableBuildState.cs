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
        public class RepetableBuildState : IDataStore
        {
            private int _operation;
            public BuildOperation Operation
            {
                get
                {
                    return (BuildOperation)_operation;
                }
                set
                {
                    _operation = (int)value;
                }
            }

            private int _step;

            private int _nextOperation;
            public BuildOperation NextOperation
            {
                get
                {
                    return (BuildOperation)_nextOperation;
                }
                set
                {
                    _nextOperation = (int)value;
                }
            }

            public void LoadValues(MyIni config)
            {
                _operation = config.Get(nameof(RepetableBuildState), nameof(Operation)).ToInt32((int)BuildOperation.None);
                _nextOperation = config.Get(nameof(RepetableBuildState), nameof(NextOperation)).ToInt32((int)BuildOperation.Idle);
            }

            public void WriteValues(MyIni config)
            {
                config.Set(nameof(RepetableBuildState), nameof(Operation), _operation);
                config.Set(nameof(RepetableBuildState), nameof(NextOperation), _nextOperation);
            }

            public enum BuildOperation
            {
                None,
                Idle,
                Extend,
                Retract,
                Reset,
                WaitForNext,
                Error
            }
        }
    }
}
