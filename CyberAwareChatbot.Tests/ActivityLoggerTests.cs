using CyberAwareChatbot;
using Xunit;

namespace CyberAwareChatbot.Tests
{
    public class ActivityLoggerTests
    {
        [Fact]
        public void LogAction_RecordsTimestampedEntry()
        {
            var logger = new ActivityLogger();

            logger.LogAction("Sent message");

            var entry = Assert.Single(logger.GetLogs());
            Assert.EndsWith("SAST: Sent message", entry);
        }

        [Fact]
        public void ClearLogs_RemovesAllEntries()
        {
            var logger = new ActivityLogger();
            logger.LogAction("one");
            logger.LogAction("two");

            logger.ClearLogs();

            Assert.Empty(logger.GetLogs());
        }
    }
}
