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
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace IngameScript
{
    partial class Program
    {
        public partial class RepetableBuildModule : IControllModule
        {
            public UpdateFrequency Extend()
            {
                _state.Operation = RepetableBuildState.BuildOperation.Extend;
                return _buildController.StartSequence(ExtendSequence());
            }

            private float _defaultVelocity = 0.1F;
            private IEnumerator<int> ExtendSequence()
            {
                foreach (var welder in _welders)
                    welder.ApplyAction("OnOff_On");

                _mergeBlock.ApplyAction("OnOff_On");
                _projector.ApplyAction("OnOff_On");
                
                if (!_connector.IsConnected)
                {
                    if (_magnet.IsLocked)
                    {
                        SetPistonToExtend(_defaultVelocity * 10);
                        while (_piston.Status != PistonStatus.Extended || _mergeBlock.State != MergeState.Locked) 
                            yield return 100;
                        if (_mergeBlock.State == MergeState.Locked)
                            _connector.Connect();

                        if (!_connector.IsConnected)
                        {
                            SetPistonToRetract(_defaultVelocity * 10);
                            while (_piston.Status != PistonStatus.Retracted || _mergeBlock.State != MergeState.Locked)
                                yield return 100;
                            if (_mergeBlock.State == MergeState.Locked)
                                _connector.Connect();
                        }
                    }
                    else
                    {
                        StopBuilder();
                    }
                    if (!_connector.IsConnected)
                        StopBuilder();
                }

                while (true)
                {
                    if (_connector.IsConnected)
                    {
                        SetPistonToExtend(_defaultVelocity);
                        while (_piston.Status != PistonStatus.Extended)
                            yield return 100;
                        if (_mergeBlock.State != MergeState.Locked)
                            StopBuilder();

                        yield return 100;
                        _magnet.Lock();

                        if (!_magnet.IsLocked)
                            StopBuilder();

                        _connector.Disconnect();
                        _mergeBlock.ApplyAction("OnOff_Off");

                        yield return 100;
                        SetPistonToRetract(_defaultVelocity * 10);
                        while (_piston.Status != PistonStatus.Retracted) yield return 100;
                        _piston.Velocity = 0;

                        if (_connector.IsFunctional && _connector.Status != MyShipConnectorStatus.Connectable)
                            StopBuilder();

                        yield return 100;
                        _mergeBlock.ApplyAction("OnOff_On");
                        yield return 10;
                        _connector.Connect();
                    }
                }
            }

            private void SetPistonToExtend(float velocity)
            {
                _piston.ApplyAction("OnOff_On");
                _piston.Velocity = velocity;
                _piston.Extend();
            }
            private void SetPistonToRetract(float velocity)
            {
                _piston.ApplyAction("OnOff_On");
                _piston.Velocity = velocity;
                _piston.Retract();
            }
        }
    }
}
