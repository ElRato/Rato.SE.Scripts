using System;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using IngameScript;
namespace IngameScript
{
    internal class TestLogger : IngameScript.Program.ILogger
    {
        private List<String> _logs;

        public List<String> Logs
        {
            get { return _logs; }
        }

        public TestLogger()
        {
            _logs = new List<String>();
        }

        public void LogInformation(string message)
        {
            _logs.Add($"[INF]{message}");
        }

        public void LogWarning(string message)
        {
            _logs.Add($"[WRN]{message}");
        }

        public void LogError(string message)
        {
            _logs.Add($"[ERR]{message}");
        }
    }
}