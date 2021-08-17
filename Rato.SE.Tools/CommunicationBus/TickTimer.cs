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
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Data;
using System.Linq.Expressions;

namespace IngameScript
{
    partial class Program
    {
        public class TickTimer
        {
            private ILogger _logger;
            
            private int _waitFor100;
            private int _waitFor10;
            private int _waitFor1;

            public int TargetTics { get; private set; }
            public int ActualTics { get { return _waitFor100 * 100 + _waitFor10 * 10 + _waitFor1; } }

            private UpdateFrequency _nextDelay;

            public TickTimer(ILogger logger)
            {
                _logger = logger;
            }
            public UpdateFrequency StartTimer(int ticksCnt)
            {
                ResetTimer();
                TargetTics = ticksCnt;
                ticksCnt = ticksCnt == 0 ? 1 : ticksCnt;
                _waitFor100 = ticksCnt / 100;
                _waitFor10 = (ticksCnt - _waitFor100 * 100) / 10;
                _waitFor1 = (ticksCnt - _waitFor100 * 100 - _waitFor10 * 10);

                CalculateNextDelay();

                return _nextDelay;
            }

            public UpdateFrequency TickNext(UpdateType currentHit)
            {
                if ((currentHit | UpdateType.Update100) > 0 && _nextDelay == UpdateFrequency.Update100)
                    _waitFor100--;
                if ((currentHit | UpdateType.Update10) > 0 && _nextDelay == UpdateFrequency.Update10)
                    _waitFor10--;
                if ((currentHit | UpdateType.Update1) > 0 && _nextDelay == UpdateFrequency.Update1)
                    _waitFor1--;

                CalculateNextDelay();
                return _nextDelay;
            }

            private void CalculateNextDelay()
            {
                _nextDelay = _waitFor100 > 0 ? UpdateFrequency.Update100 :
                    _waitFor10 > 0 ? UpdateFrequency.Update10 :
                    _waitFor1 > 0 ? UpdateFrequency.Update1 :
                    UpdateFrequency.None;
            }

            public void ResetTimer()
            {
                _waitFor100 = 0;
                _waitFor10 = 0;
                _waitFor1 = 0;
                _nextDelay = UpdateFrequency.None;
                TargetTics = 0;
            }
        }
    }
}
