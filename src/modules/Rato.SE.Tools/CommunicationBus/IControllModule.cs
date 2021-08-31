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
        public interface ISelfTestableModule
        {
            UpdateFrequency StartTestSquence();
        }

        public interface IAutoStartModule {
            UpdateFrequency AutoStart();
        }

        public interface ITerminalModule
        {
            UpdateFrequency TerminalAction(UpdateType updateSource, string Argument);
        }

        public interface IConfigurableModule
        {
            bool SetConfig(DataStoreHandler storeHandler);
            bool SetState(DataStoreHandler storeHandler);
            void SaveConfig(DataStoreHandler storeHandler);
            void SaveState(DataStoreHandler storeHandler);
        }

        public interface IControllModule
        {
            void Initialize();
            ModuleStatus Status { get; set; }
            List<ModuleStatusDetail> StatusDetails { get; }

            UpdateFrequency ContinueSquence(UpdateType updateSource);
        }

        public abstract class ModuleStatus
        {
            public string Name { get; private set; }
            public bool FullyOperatable { get; private set; }
            public bool IncludeToUpdateSequence { get; private set; }

            public static readonly ModuleStatus JustCreated = new JustCreatedState();
            public static readonly ModuleStatus Initialized = new InitializedState();
            public static readonly ModuleStatus SelfTest = new SelfTestState();
            public static readonly ModuleStatus Active = new ActiveState();
            public static readonly ModuleStatus ReadyToStart = new ReadyToStartState();
            public static readonly ModuleStatus NonFunctional = new NonFunctionalState();

            public override bool Equals(object obj)
            {
                return obj is ModuleStatus ? obj.ToString() == this.ToString() : base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return this.ToString().GetHashCode();
            }

            public override string ToString()
            {
                return this.Name;
            }

            private class ActiveState : ModuleStatus
            {
                public ActiveState()
                {
                    Name = nameof(ActiveState);
                    FullyOperatable = true;
                    IncludeToUpdateSequence = true;
                }
            }

            private class JustCreatedState : ModuleStatus
            {
                public JustCreatedState()
                {
                    Name = nameof(JustCreatedState);
                    FullyOperatable = false;
                    IncludeToUpdateSequence = false;
                }
            }

            private class InitializedState : ModuleStatus
            {
                public InitializedState()
                {
                    Name = nameof(InitializedState);
                    FullyOperatable = false;
                    IncludeToUpdateSequence = true;
                }
            }

            private class NonFunctionalState : ModuleStatus
            {
                public NonFunctionalState()
                {
                    Name = nameof(NonFunctionalState);
                    FullyOperatable = false;
                    IncludeToUpdateSequence = false;
                }
            }

            private class SelfTestState : ModuleStatus
            {
                public SelfTestState()
                {
                    Name = nameof(SelfTestState);
                    FullyOperatable = false;
                    IncludeToUpdateSequence = true;
                }
            }

            private class ReadyToStartState : ModuleStatus
            {
                public ReadyToStartState()
                {
                    Name = nameof(ReadyToStartState);
                    FullyOperatable = false;
                    IncludeToUpdateSequence = true;
                }
            }
        }
    }
}
