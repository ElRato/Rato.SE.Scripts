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
            public UpdateFrequency Reset(RepetableBuildState.BuildOperation nextOperation)
            {
                return _buildController.StartSequence(ResetSequence(nextOperation));
            }

            const int resetNone = 0;
            const int resetMerge = 1;
            const int resetConnector = 2;
            const int resetLookForJoin = 3;
            const int resetDone = 4;
            const int resetFailed = 10;

            private int resetStep = resetNone;

            private IEnumerator<int> ResetSequence(RepetableBuildState.BuildOperation nextOperation)
            {
                _state.Operation = RepetableBuildState.BuildOperation.Reset;

                _blockUtils.TryTurnOff(_welders);
                _blockUtils.TryTurnOff(_grinders);
                _piston.Velocity = 0;

                resetStep = resetMerge;
                while (resetStep != resetDone || resetStep != resetFailed)
                {
                    switch (resetStep)
                    {
                        case resetMerge: foreach (var s in ResetMerge()) { yield return s; } break;
                        case resetConnector: foreach (var s in ResetConnector()) { yield return s; } break;
                        case resetLookForJoin: foreach (var s in LookForJoin()) { yield return s; } break;
                        case resetFailed: { yield return 100;}  break;
                        case resetDone: { _nextOperation = nextOperation; yield return 100; } break;
                        default: yield return 100; break;
                    }
                }
            }

            private IEnumerable<int> ResetMerge()
            {
                if (!_mergeBlock.IsWorking)
                    if (_blockUtils.TryTurnOn(_mergeBlock))
                        resetStep = resetFailed;

                yield return 100;

                if (_mergeBlock.IsConnected)
                    resetStep = resetConnector;
                else
                {
                    if (_connector.Status == MyShipConnectorStatus.Connectable)
                        resetStep = resetConnector;
                    else
                        resetStep = resetLookForJoin;
                }

                yield return 100;
            }

            private IEnumerable<int> ResetConnector()
            {
                if (!_connector.IsWorking)
                    if (_blockUtils.TryTurnOn(_connector))
                        resetStep = resetFailed;

                yield return 100;

                if (_connector.IsConnected)
                {
                    if (_mergeBlock.IsConnected)
                    {
                        _connector.Disconnect();
                        yield return 100;
                        _connector.Connect();
                    }
                    resetStep = resetDone;
                }
                else
                {
                    if (_connector.Status == MyShipConnectorStatus.Connectable)
                    {
                        _connector.Connect();
                        resetStep = resetMerge;
                    }
                    else
                    {
                        resetStep = resetLookForJoin;
                    }
                }

                yield return 100;
            }

            private IEnumerable<int> LookForJoin()
            {
                _piston.Velocity = -1;
                _magnet.Lock();
                
                while (_piston.Status != PistonStatus.Retracted && !_mergeBlock.IsConnected) yield return 100;

                if (_mergeBlock.IsConnected)
                {
                    _magnet.Unlock();
                    resetStep = resetConnector;
                }
                else
                {
                    _piston.Velocity = 1;
                    while (_piston.Status != PistonStatus.Extended && !_mergeBlock.IsConnected) yield return 100;

                    if (_mergeBlock.IsConnected)
                    {
                        _magnet.Unlock();
                        resetStep = resetConnector;
                    }
                    else
                        resetStep = resetFailed;
                }
            }
        }
    }
}
