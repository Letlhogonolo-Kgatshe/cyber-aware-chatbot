using System;
using System.Collections.Generic;

namespace CyberAwareChatbot
{
    public class UserData
    {
        public string Name { get; set; }
        public int Points { get; set; }
        public string FavoriteTopic { get; set; }
        public string CurrentTheme { get; set; }
        public List<TaskItem> Tasks { get; set; } = new List<TaskItem>();
        public List<string> Logs { get; set; } = new List<string>();
    }
}