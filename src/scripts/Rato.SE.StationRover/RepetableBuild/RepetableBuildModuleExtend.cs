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

            private float _defaultVelocity = 0.3F;
            private IEnumerator<int> ExtendSequence()
            {
                _blockUtils.TryTurnOn(_welders);
                yield return 100;
                _blockUtils.TryTurnOff(_grinders);
                yield return 100;
                _blockUtils.TryTurnOn(_mergeBlock);
                yield return 100;
                _blockUtils.TryTurnOn(_projector);
                yield return 100;

                if (!_connector.IsConnected)
                {
                    StopBuilder();
                    yield break;
                }

                while (true)
                {
                    if (_connector.IsConnected)
                    {
                        _magnet.Unlock();

                        _piston.Velocity = _defaultVelocity;

                        var position = _piston.NormalizedPosition;
                        while (_piston.Status != PistonStatus.Extended)
                        {
                            yield return 1000;
                            if (Math.Abs(position - _piston.NormalizedPosition) < 0.002)
                            {
                                _dbgLogger.LogInformation($"Extend Stopped before {position} now {_piston.NormalizedPosition}");
                                resetStep = resetFailed;
                                StopBuilder();
                                yield break;
                            }
                            position = _piston.NormalizedPosition;
                        }

                        if (_mergeBlock.State != MergeState.Locked)
                        {
                            StopBuilder();
                            yield break;
                        }

                        _magnet.Lock();

                        yield return 100;

                        if (!_magnet.IsLocked)
                        {
                            StopBuilder();
                            yield break;
                        }

                        _connector.Disconnect();
                        _blockUtils.TryTurnOff(_mergeBlock);

                        yield return 100;
                        _piston.Velocity = -_defaultVelocity * 5;
                        while (_piston.Status != PistonStatus.Retracted) yield return 100;
                        _piston.Velocity = 0;
                        yield return 100;
                        
                        if (_connector.IsFunctional)
                            while (_connector.Status != MyShipConnectorStatus.Connectable)
                                yield return 100;

                        yield return 100;
                        _blockUtils.TryTurnOn(_mergeBlock);
                        yield return 100;
                        _connector.Connect();
                    }
                    else
                    {
                        StopBuilder();
                        yield break;
                    }
                }
            }
        }
    }
}
