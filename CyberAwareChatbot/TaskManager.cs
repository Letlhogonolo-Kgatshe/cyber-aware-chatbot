using System;
using System.Collections.Generic;
using System.Linq;

namespace CyberAwareChatbot
{
    public class TaskManager
    {
        private List<TaskItem> tasks = new List<TaskItem>();

        public void AddTask(string title, string description, DateTime? reminderDate)
        {
            tasks.Add(new TaskItem
            {
                Title = title,
                Description = description,
                ReminderDate = reminderDate,
                IsCompleted = false
            });
        }

        public List<TaskItem> ViewTasks()
        {
            return tasks;
        }

        public void CompleteTask(string title)
        {
            var task = tasks.FirstOrDefault(t => t.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            if (task != null) task.IsCompleted = true;
        }

        public void DeleteTask(string title)
        {
            tasks.RemoveAll(t => t.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        }

        public List<TaskItem> GetDueReminders(DateTime currentTime) =>
            tasks.Where(t => t.ReminderDate.HasValue && t.ReminderDate <= currentTime && !t.IsCompleted).ToList();
    }
}