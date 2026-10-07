using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace CyberAwareChatbot
{
    public class ResponseGenerator
    {
        private readonly Dictionary<string, string> keywordResponses;
        private readonly Dictionary<string, string[]> randomResponses;
        private readonly Random random = new Random();
        private readonly string[] tips;
        private string currentTheme = "dark";
        private readonly Dictionary<string, List<string>> keywordSynonyms = new Dictionary<string, List<string>>
        {
            { "password", new List<string> { "pass", "login", "credentials", "passwd" } },
            { "scam", new List<string> { "fraud", "deception", "con" } },
            { "privacy", new List<string> { "security", "personal data", "confidentiality" } },
            { "phishing", new List<string> { "fish", "email scam", "spear phishing" } },
            { "ransomware", new List<string> { "ransom", "encrypt", "lockware" } },
            { "two-factor authentication", new List<string> { "2fa", "multi-factor", "two-step" } },
            { "firewall", new List<string> { "barrier", "network security", "shield" } },
            { "vpn", new List<string> { "virtual private network", "secure connection", "tunnel" } },
            { "social engineering", new List<string> { "manipulation", "trick", "deception" } },
            { "malware", new List<string> { "virus", "spyware", "trojan" } },
            { "zero-day attack", new List<string> { "zero day", "exploit", "new vulnerability" } },
            { "cyberattack", new List<string> { "hack", "breach", "intrusion" } }
        };

        public ResponseGenerator()
        {
            keywordResponses = new Dictionary<string, string>
            {
                { "password", "A strong password is at least 12 characters long, includes uppercase and lowercase letters, numbers, and special characters." },
                { "scam", "Scams often trick users into sharing personal info. Always verify the source before responding to emails or messages." },
                { "privacy", "Protecting your privacy online is crucial! Use strong passwords, enable 2FA, and be cautious about sharing personal details." },
                { "phishing", "Phishing is a cyber attack where hackers impersonate legitimate sources to steal sensitive information. Always verify links before clicking!" },
                { "ransomware", "Ransomware is a type of malware that encrypts your files and demands a ransom. Avoid clicking suspicious links and always backup your data." },
                { "two-factor authentication", "Two-factor authentication (2FA) adds an extra security layer by requiring a second verification step, like a code sent to your phone." },
                { "firewall", "A firewall is a security system that monitors and controls incoming and outgoing network traffic to prevent unauthorized access." },
                { "vpn", "A VPN (Virtual Private Network) encrypts your internet connection, making it secure and private, especially on public Wi-Fi." },
                { "social engineering", "Social engineering is the manipulation of people into divulging confidential information. Be skeptical of unexpected requests for personal info." },
                { "malware", "Malware is any software designed to harm or exploit devices, networks, or users. Protect yourself with antivirus software and avoid suspicious links." },
                { "zero-day attack", "A zero-day attack exploits unknown vulnerabilities before developers can release a fix. Keep software updated to reduce risk." },
                { "cyberattack", "A cyberattack is an attempt to compromise systems or data. Stay protected with strong security practices like 2FA and regular updates." },
                { "how to stay safe online", "To stay safe online: Use strong passwords, enable 2FA, update software regularly, avoid public Wi-Fi without a VPN, and beware of phishing scams." }
            };

            randomResponses = new Dictionary<string, string[]>
            {
                { "phishing", new string[] { "Be cautious of emails asking for personal information.", "Phishing emails might use urgent language.", "Never click on links in suspicious emails." } },
                { "password", new string[] { "Use unique passwords for each account.", "A good password is at least 12 characters long.", "Consider a password manager." } },
                { "privacy", new string[] { "Review security settings on your accounts.", "Avoid sharing sensitive info online.", "Use 2FA and a VPN on public Wi-Fi." } }
            };

            tips = new string[]
            {
                "Tip: Regularly update your software to patch security vulnerabilities.",
                "Tip: Use a VPN when connecting to public Wi-Fi to encrypt your data.",
                "Tip: Enable 2FA on all your accounts for an extra layer of security."
            };
        }

        public string Generate(string input)
        {
            input = input.ToLower();
            if (input.Contains("hello") || input.Contains("hi"))
                return "Hi there! How can I assist you?";
            if (input.Contains("how are you"))
                return "I'm just a bot, but I'm doing great!";
            if (input.Contains("time"))
                return $"The current time is {DateTime.Now:HH:mm} SAST.";

            string? keyword = FindMatchingKeyword(input);
            if (keyword != null)
            {
                string response = randomResponses.ContainsKey(keyword) && random.Next(2) == 0
                    ? randomResponses[keyword][random.Next(randomResponses[keyword].Length)]
                    : keywordResponses[keyword];
                return $"{response} {GetRandomTip()}";
            }

            return "I'm not sure I understand. Can you try rephrasing?";
        }

        public string GenerateMore(string input)
        {
            input = input.ToLower();
            string? keyword = FindMatchingKeyword(input);
            if (keyword != null)
            {
                return keyword switch
                {
                    "password" => "Use a password manager to keep track of complex passwords securely.",
                    "scam" => "Scams can also come via fake websites or calls. Verify sources.",
                    "privacy" => "Check app permissions regularly to protect privacy.",
                    "cyberattack" => "Regular backups can help recover from cyberattacks.",
                    _ => "I can tell you more about other topics. Which one?"
                };
            }
            return "I can tell you more about ransomware or VPNs. Which would you like?";
        }

        public bool IsCybersecurityTopic(string input) => FindMatchingKeyword(input) != null;

        public string? GetCybersecurityTopic(string input) => FindMatchingKeyword(input);

        private string? FindMatchingKeyword(string input)
        {
            input = input.ToLower();
            foreach (var kvp in keywordResponses)
            {
                string keyword = kvp.Key;
                if (input.Contains(keyword) || (keywordSynonyms.ContainsKey(keyword) && keywordSynonyms[keyword].Any(syn => input.Contains(syn))))
                    return keyword;
            }
            return null;
        }

        private string GetRandomTip() => tips[random.Next(tips.Length)];

        public void SetTheme(string theme)
        {
            currentTheme = theme;
        }
    }
}