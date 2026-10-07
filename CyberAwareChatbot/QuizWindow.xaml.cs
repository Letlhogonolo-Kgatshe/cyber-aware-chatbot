using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CyberAwareChatbot
{
    public partial class QuizWindow : Window
    {
        private readonly TaskManager _taskManager;
        private readonly ActivityLogger _logger;
        private int _totalPoints;
        private readonly List<(string Question, string Answer, string[] Options)> _questions;
        private int _currentQuestionIndex = 0;
        private int _score = 0;

        public QuizWindow(TaskManager taskManager, ActivityLogger logger, ref int totalPoints)
        {
            InitializeComponent();
            _taskManager = taskManager;
            _logger = logger;
            _totalPoints = totalPoints;
            _questions = new List<(string Question, string Answer, string[] Options)>
            {
                ("What is phishing?", "a", new[] { "a) A cyberattack via email", "b) A type of fish", "c) A coding language" }),
                ("What’s a strong password?", "b", new[] { "a) 1234", "b) P@ssw0rd!", "c) password" }),
                ("What does HTTPS mean?", "c", new[] { "a) Hyper Text", "b) High Tech", "c) Secure HTTP" }),
                ("What’s malware?", "a", new[] { "a) Malicious software", "b) A hardware issue", "c) A secure app" }),
                ("What’s two-factor authentication?", "b", new[] { "a) Two passwords", "b) Password + code", "c) Two users" }),
                ("What’s a VPN?", "c", new[] { "a) Virtual Processor", "b) Very Private Node", "c) Virtual Private Network" }),
                ("What’s encryption?", "a", new[] { "a) Data scrambling", "b) Data deletion", "c) Data copying" }),
                ("What’s a firewall?", "b", new[] { "a) A physical wall", "b) Network security", "c) A virus" }),
                ("What’s social engineering?", "c", new[] { "a) Building bridges", "b) Coding society", "c) Manipulating people" }),
                ("What’s a DDoS attack?", "a", new[] { "a) Overloading a server", "b) Data deletion", "c) Disk overload" })
            };
            DisplayQuestion();
        }

        private void DisplayQuestion()
        {
            if (_currentQuestionIndex < _questions.Count)
            {
                var q = _questions[_currentQuestionIndex];
                QuestionText.Text = $"Q{_currentQuestionIndex + 1}: {q.Question}";
                OptionsList.Items.Clear();
                foreach (var opt in q.Options)
                {
                    OptionsList.Items.Add(opt);
                }
            }
            else
            {
                string encouragement = _score == 10 ? "Perfect score! You're a cybersecurity star! +50 bonus points!" :
                                      _score >= 7 ? "Excellent! You're a pro! +30 bonus points!" :
                                      _score >= 4 ? "Good effort! Let's learn more. +20 bonus points!" :
                                      "Nice try! Practice makes perfect. +10 bonus points!";
                _totalPoints += _score == 10 ? 50 : _score >= 7 ? 30 : _score >= 4 ? 20 : 10;
                MessageBox.Show($"Quiz Results - Score: {_score}/10\n{encouragement}", "Quiz Complete");
                _logger.LogAction($"Completed quiz with score: {_score}/10");
                Close();
            }
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (OptionsList.SelectedItem != null)
            {
                var q = _questions[_currentQuestionIndex];
                string answer = OptionsList.SelectedItem.ToString().Split(')')[0].Trim();
                if (answer == q.Answer)
                {
                    MessageBox.Show("Correct! +10 points!", "Result", MessageBoxButton.OK, MessageBoxImage.Information);
                    _score++;
                    _totalPoints += 10;
                }
                else
                {
                    MessageBox.Show($"Not quite. Correct answer: {q.Answer}. +5 points for trying!", "Result", MessageBoxButton.OK, MessageBoxImage.Warning);
                    _totalPoints += 5;
                }
                _currentQuestionIndex++;
                DisplayQuestion();
            }
            else
            {
                MessageBox.Show("Please select an answer.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}