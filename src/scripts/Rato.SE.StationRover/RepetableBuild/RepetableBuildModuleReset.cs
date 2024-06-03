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
            public UpdateFrequency Reset()
            {
                _state.Operation = RepetableBuildState.BuildOperation.Reset;
                return _buildController.StartSequence(ResetSequence());
            }

            const int resetMerge = 1;
            const int resetConnector = 2;
            const int resetLookForJoin = 3;
            const int resetDone = 4;
            const int resetFailed = 10;

            private int resetStep;

            private IEnumerator<int> ResetSequence()
            {
                _dbgLogger.LogInformation($"ResetSequence Started");
                yield return 100;
                _blockUtils.TryTurnOff(_welders);
                yield return 100;
                _blockUtils.TryTurnOff(_grinders);
                yield return 100;
                _piston.Velocity = 0;

                resetStep = resetMerge;
                while (resetStep != resetDone || resetStep != resetFailed)
                {
                    if (resetStep == resetMerge)
                        foreach (var s in ResetMerge()) { yield return s; }
                    if (resetStep == resetConnector)
                        foreach (var s in ResetConnector()) { yield return s; }
                    if (resetStep == resetLookForJoin)
                        foreach (var s in LookForJoin()) { yield return s; }

                    if (resetStep == resetDone || resetStep == resetFailed)
                        break;
                }
                _dbgLogger.LogInformation($"Reset completed with step {resetStep}");
                _piston.Velocity = 0;
                if (resetStep == resetDone)
                    _state.Operation = RepetableBuildState.BuildOperation.WaitForNext;
                if (resetStep == resetFailed)
                    _state.Operation = RepetableBuildState.BuildOperation.Error;

                yield return 100;
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
                _dbgLogger.LogInformation($"Reset merge completed with {resetStep}");
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
                _dbgLogger.LogInformation($"ResetConnector completed with {resetStep}");
            }

            private IEnumerable<int> LookForJoin()
            {
                _piston.Velocity = -1;
                _magnet.Lock();

                _dbgLogger.LogInformation($"LookForJoin Started");

                var position = _piston.NormalizedPosition;
                while (_piston.Status != PistonStatus.Retracted && !_mergeBlock.IsConnected)
                {
                    if (Math.Abs(position - _piston.NormalizedPosition) < 0.01)
                    {
                        resetStep = resetFailed;
                        _dbgLogger.LogInformation($"Piston blocked");
                    }
                    yield return 1000;
                }

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

                _piston.Velocity = 0;
                yield return 1000;
            }
        }
    }
}
