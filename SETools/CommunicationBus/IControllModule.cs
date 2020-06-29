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
        public interface IControllModule
        {
            void Initialize();
            ModuleState State { get; set; }
            List<ModuleStateDetail> StateDetails { get; }

            UpdateFrequency StartTestSquence();
            UpdateFrequency ContinueSquence(UpdateType updateSource);
            UpdateFrequency TerminalAction(UpdateType updateSource, string Argument);
        }

        public abstract class ModuleState
        {
            public string Name { get; private set; }
            public bool FullyOperatable { get; private set; }
            public bool IncludeToUpdateSequence { get; private set; }

            public static readonly ModuleState JustCreated = new JustCreatedState();
            public static readonly ModuleState Initialized = new InitializedState();
            public static readonly ModuleState SelfTest = new SelfTestState();
            public static readonly ModuleState Active = new ActiveState();
            public static readonly ModuleState NonFunctional = new NonFunctionalState();

            public override bool Equals(object obj)
            {
                return obj is ModuleState ? obj.ToString() == this.ToString() : base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return this.ToString().GetHashCode();
            }

            public override string ToString()
            {
                return this.Name;
            }

            private class ActiveState : ModuleState
            {
                public ActiveState()
                {
                    Name = nameof(ActiveState);
                    FullyOperatable = true;
                    IncludeToUpdateSequence = true;
                }
            }

            private class JustCreatedState : ModuleState
            {
                public JustCreatedState()
                {
                    Name = nameof(JustCreatedState);
                    FullyOperatable = false;
                    IncludeToUpdateSequence = false;
                }
            }

            private class InitializedState : ModuleState
            {
                public InitializedState()
                {
                    Name = nameof(InitializedState);
                    FullyOperatable = false;
                    IncludeToUpdateSequence = true;
                }
            }

            private class NonFunctionalState : ModuleState
            {
                public NonFunctionalState()
                {
                    Name = nameof(NonFunctionalState);
                    FullyOperatable = false;
                    IncludeToUpdateSequence = false;
                }
            }

            private class SelfTestState : ModuleState
            {
                public SelfTestState()
                {
                    Name = nameof(SelfTestState);
                    FullyOperatable = false;
                    IncludeToUpdateSequence = true;
                }
            }

        }
    }
}
