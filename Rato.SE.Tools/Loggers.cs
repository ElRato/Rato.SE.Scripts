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
        public interface ILogger {
            void LogInformation(String message);

            void LogWarning(String message);

            void LogError(String message);
        }
        public class EchoLogger : ILogger
        {
            private MyGridProgram _program;
            public EchoLogger(MyGridProgram program) {
                _program = program;
            }
            public void LogInformation(string message)
            {
                _program.Echo($"[INF]{message}");
            }

            public void LogWarning(string message)
            {
                _program.Echo($"[WRN]{message}");
            }

            public void LogError(string message)
            {
                _program.Echo($"[ERR]{message}");
            }
        }

        public class LcdTextLogger : ILogger
        {
            private MyGridProgram _program;
            private IMyTextSurface _panel;
            public LcdTextLogger(MyGridProgram program, string header, IMyTextSurface panel)
            {
                _program = program;
                _panel = panel;
                _panel.WriteText($"{header}\n");
                _panel.ContentType = ContentType.TEXT_AND_IMAGE;
            }
            public void LogInformation(string message)
            {
                _panel.WriteText($"{message}\n", true);
            }

            public void LogWarning(string message)
            {
                _panel.WriteText($"{message}\n", true);
            }

            public void LogError(string message)
            {
                _panel.WriteText($"{message}\n", true);
            }
        }
    }
}
