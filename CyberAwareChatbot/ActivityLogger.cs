using System;
using System.Collections.Generic;

namespace CyberAwareChatbot
{
    public class ActivityLogger
    {
        private readonly List<string> _logs = new List<string>();

        public void LogAction(string action)
        {
            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} SAST: {action}";
            _logs.Add(logEntry);
        }

        public List<string> GetLogs()
        {
            return _logs;
        }

        public void ClearLogs()
        {
            _logs.Clear();
        }
    }
}