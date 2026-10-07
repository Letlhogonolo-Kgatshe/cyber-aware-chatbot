using CyberAwareChatbot;
using Xunit;

namespace CyberAwareChatbot.Tests
{
    public class TaskManagerTests
    {
        private readonly TaskManager _tasks = new TaskManager();

        [Fact]
        public void AddTask_AddsIncompleteTask()
        {
            _tasks.AddTask("Enable 2FA", "On my email account", null);

            var task = Assert.Single(_tasks.ViewTasks());
            Assert.Equal("Enable 2FA", task.Title);
            Assert.False(task.IsCompleted);
        }

        [Fact]
        public void CompleteTask_IsCaseInsensitive()
        {
            _tasks.AddTask("Update router", "", null);

            _tasks.CompleteTask("update ROUTER");

            Assert.True(_tasks.ViewTasks()[0].IsCompleted);
        }

        [Fact]
        public void DeleteTask_RemovesMatchingTask()
        {
            _tasks.AddTask("A", "", null);
            _tasks.AddTask("B", "", null);

            _tasks.DeleteTask("a");

            Assert.Equal("B", Assert.Single(_tasks.ViewTasks()).Title);
        }

        [Fact]
        public void GetDueReminders_ReturnsOnlyDueIncompleteTasks()
        {
            var now = new DateTime(2026, 1, 10, 12, 0, 0);
            _tasks.AddTask("Due", "", now.AddHours(-1));
            _tasks.AddTask("Future", "", now.AddDays(1));
            _tasks.AddTask("No reminder", "", null);
            _tasks.AddTask("Done", "", now.AddHours(-2));
            _tasks.CompleteTask("Done");

            var due = _tasks.GetDueReminders(now);

            Assert.Equal("Due", Assert.Single(due).Title);
        }
    }
}
