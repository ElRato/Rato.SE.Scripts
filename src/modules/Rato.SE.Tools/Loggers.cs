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
using System.Diagnostics.Eventing.Reader;

namespace IngameScript
{
    partial class Program
    {
        public enum LogLevel
        {
            Debug,
            Information,
            Warning,
            Error
        }


        public interface ILogger {
            void LogDebug(String message);

            void LogInformation(String message);

            void LogWarning(String message);

            void LogError(String message);
        }
        public class EchoLogger : ILogger
        {
            private Program _program;
            private LogLevel _loglevel;
            public EchoLogger(Program program, LogLevel logLevel) {
                _program = program;
                _loglevel = logLevel;
            }

            public void LogDebug(string message)
            {
                if (_loglevel <= LogLevel.Debug)
                    _program.Echo($"[DBG]{message}");
            }
            public void LogInformation(string message)
            {
                if(_loglevel <= LogLevel.Information)
                    _program.Echo($"[INF]{message}");
            }

            public void LogWarning(string message)
            {
                if (_loglevel <= LogLevel.Warning)
                    _program.Echo($"[WRN]{message}");
            }

            public void LogError(string message)
            {
                if (_loglevel <= LogLevel.Error)
                    _program.Echo($"[ERR]{message}");
            }
        }

        public class LcdTextLogger : ILogger
        {
            private Program _program;
            private IMyTextSurface _panel;
            private LogLevel _loglevel;
            public LcdTextLogger(Program program, string header, IMyTextSurface panel, LogLevel logLevel)
            {
                _program = program;
                _panel = panel;
                _panel.WriteText($"{header}\n");
                _panel.ContentType = ContentType.TEXT_AND_IMAGE;
                _loglevel = logLevel;
            }

            public void LogDebug(string message)
            {
                if (_loglevel <= LogLevel.Debug)
                    _panel.WriteText($"{message}\n", true);
            }

            public void LogInformation(string message)
            {
                if (_loglevel <= LogLevel.Information)
                    _panel.WriteText($"{message}\n", true);
            }

            public void LogWarning(string message)
            {
                if (_loglevel <= LogLevel.Warning)
                    _panel.WriteText($"{message}\n", true);
            }

            public void LogError(string message)
            {
                if (_loglevel <= LogLevel.Error)
                    _panel.WriteText($"{message}\n", true);
            }
        }
    }
}
