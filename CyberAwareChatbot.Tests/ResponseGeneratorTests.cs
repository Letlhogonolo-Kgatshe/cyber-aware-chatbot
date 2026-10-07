using CyberAwareChatbot;
using Xunit;

namespace CyberAwareChatbot.Tests
{
    public class ResponseGeneratorTests
    {
        private readonly ResponseGenerator _bot = new ResponseGenerator();

        [Theory]
        [InlineData("hello")]
        [InlineData("Hi there")]
        public void Generate_Greeting_ReturnsGreeting(string input)
        {
            Assert.Equal("Hi there! How can I assist you?", _bot.Generate(input));
        }

        [Theory]
        [InlineData("What is phishing?", "phishing")]
        [InlineData("tell me about this ransomware thing", "ransomware")]
        [InlineData("Is my shipping password safe?", "password")]
        public void Generate_TopicContainingGreetingLetters_IsNotTreatedAsGreeting(string input, string topic)
        {
            // "phishing", "this" and "shipping" all contain the letters "hi".
            var reply = _bot.Generate(input);

            Assert.NotEqual("Hi there! How can I assist you?", reply);
            Assert.Equal(topic, _bot.GetCybersecurityTopic(input));
        }

        [Fact]
        public void Generate_WordContainingTime_IsNotTreatedAsTimeQuestion()
        {
            var reply = _bot.Generate("sometimes I worry about malware");

            Assert.DoesNotContain("The current time is", reply);
        }

        [Fact]
        public void Generate_TimeQuestion_ReturnsCurrentTime()
        {
            Assert.StartsWith("The current time is", _bot.Generate("what time is it"));
        }

        [Theory]
        [InlineData("how do I make a strong password", "password")]
        [InlineData("what is 2fa", "two-factor authentication")]
        [InlineData("should I use a VPN", "vpn")]
        [InlineData("I think I have a virus", "malware")]
        [InlineData("explain a firewall", "firewall")]
        public void GetCybersecurityTopic_KeywordOrSynonym_ReturnsCanonicalTopic(string input, string expected)
        {
            Assert.Equal(expected, _bot.GetCybersecurityTopic(input));
        }

        [Fact]
        public void Generate_KnownTopic_EndsWithTip()
        {
            var reply = _bot.Generate("tell me about firewalls");

            Assert.Contains("firewall", reply, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("Tip:", reply);
        }

        [Fact]
        public void Generate_UnknownInput_AsksToRephrase()
        {
            Assert.Equal("I'm not sure I understand. Can you try rephrasing?", _bot.Generate("banana bread recipe"));
        }

        [Fact]
        public void IsCybersecurityTopic_UnrelatedInput_ReturnsFalse()
        {
            Assert.False(_bot.IsCybersecurityTopic("what's for dinner"));
        }

        [Fact]
        public void GenerateMore_Password_GivesPasswordManagerAdvice()
        {
            Assert.Contains("password manager", _bot.GenerateMore("more about passwords"));
        }

        [Fact]
        public void GenerateMore_NoTopic_SuggestsTopics()
        {
            Assert.Equal("I can tell you more about ransomware or VPNs. Which would you like?", _bot.GenerateMore("more"));
        }
    }
}
