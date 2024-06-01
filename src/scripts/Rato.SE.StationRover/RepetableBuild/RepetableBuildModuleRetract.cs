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
            public UpdateFrequency Retract()
            {
                _state.Operation = RepetableBuildState.BuildOperation.Retract;
                return _buildController.StartSequence(RetractSequence());
            }

            private IEnumerator<int> RetractSequence()
            {
                _blockUtils.TryTurnOn(_grinders);
                _blockUtils.TryTurnOff(_welders);
                _blockUtils.TryTurnOn(_mergeBlock);
                _blockUtils.TryTurnOn(_projector);

                if (!_connector.IsConnected)
                {
                    StopBuilder();
                }

                while (true)
                {
                    if (_connector.IsConnected)
                    {
                        _magnet.Unlock();

                        _piston.Velocity = -_defaultVelocity;
                        while (_piston.Status != PistonStatus.Retracted)
                            yield return 100;

                        if (_mergeBlock.State != MergeState.Locked)
                            StopBuilder();

                        _magnet.Lock();

                        yield return 100;

                        if (!_magnet.IsLocked)
                            StopBuilder();

                        _connector.Disconnect();
                        _blockUtils.TryTurnOff(_mergeBlock);

                        yield return 100;
                        _piston.Velocity = _defaultVelocity * 5;
                        while (_piston.Status != PistonStatus.Extended) yield return 100;
                        _piston.Velocity = 0;

                        if (_connector.IsFunctional && _connector.Status != MyShipConnectorStatus.Connectable)
                            StopBuilder();

                        yield return 100;
                        _blockUtils.TryTurnOn(_mergeBlock);
                        yield return 100;
                        _connector.Connect();
                    }
                    else
                    {
                        StopBuilder();
                    }
                }
            }
        }
    }
}
