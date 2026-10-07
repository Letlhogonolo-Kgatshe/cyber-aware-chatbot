using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Media;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows;

namespace CyberAwareChatbot
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _userInput;
        private string _userName = "Cyber Explorer";
        private string _favoriteTopic;
        private int _totalPoints;
        private string _currentTheme = "dark";
        private TaskItem _selectedTask;
        private string _lastCyberTopic;
        private readonly ResponseGenerator _responseGenerator;
        private readonly TaskManager _taskManager;
        private readonly ActivityLogger _logger;
        private readonly Dictionary<string, (SolidColorBrush Foreground, SolidColorBrush Background)> _themes = new Dictionary<string, (SolidColorBrush, SolidColorBrush)>
        {
            { "light", (new SolidColorBrush(Colors.Black), new SolidColorBrush(Colors.White)) },
            { "dark", (new SolidColorBrush(Colors.White), new SolidColorBrush(Colors.Black)) },
            { "blue", (new SolidColorBrush(Colors.White), new SolidColorBrush(Colors.DarkBlue)) }
        };

        public ObservableCollection<ChatMessage> ChatMessages { get; } = new ObservableCollection<ChatMessage>();
        public ObservableCollection<TaskItem> Tasks { get; } = new ObservableCollection<TaskItem>();
        public string UserInput
        {
            get => _userInput;
            set
            {
                _userInput = value;
                OnPropertyChanged(nameof(UserInput));
            }
        }
        public string StatusMessage => $"Welcome, {_userName} | Time: {DateTime.Now:HH:mm} SAST | Points: {_totalPoints} | Theme: {_currentTheme}";
        public string Logo => @"
    _____       _                      _           _   _           _   
   /  __ \     | |                    | |         | | | |         | |  
   | /  \/_   _| |__   ___ _ __    ___| |__   __ _| |_| |__   ___ | |_ 
   | |   | | | | '_ \ / _ \ '__|  / __| '_ \ / _` | __| '_ \ / _ \| __|
   | \__/\ |_| | |_) |  __/ |    | (__| | | | (_| | |_| |_) | (_) | |_ 
    \____/\__, |_.__/ \___|_|     \___|_| |_|\__,_|\__|_.__/ \___/ \__|
           __/ |                                                       
          |___/";
        public string CurrentTheme
        {
            get => _currentTheme;
            set
            {
                _currentTheme = value;
                ApplyTheme();
                _logger.LogAction($"Changed theme to: {_currentTheme}");
                OnPropertyChanged(nameof(CurrentTheme));
                OnPropertyChanged(nameof(StatusMessage));
                SaveUserDataAsync();
            }
        }
        public TaskItem SelectedTask
        {
            get => _selectedTask;
            set
            {
                _selectedTask = value;
                OnPropertyChanged(nameof(SelectedTask));
            }
        }
        public SolidColorBrush ForegroundColor => _themes[_currentTheme].Foreground;
        public ICommand SendMessageCommand { get; }
        public ICommand AddTaskCommand { get; }
        public ICommand CompleteTaskCommand { get; }
        public ICommand DeleteTaskCommand { get; }
        public ICommand StartQuizCommand { get; }
        public ICommand StartTutorialCommand { get; }
        public ICommand ViewLogsCommand { get; }
        public ICommand ClearLogsCommand { get; }
        public ICommand ShowHelpCommand { get; }

        public MainViewModel()
        {
            _responseGenerator = new ResponseGenerator();
            _taskManager = new TaskManager();
            _logger = new ActivityLogger();
            SendMessageCommand = new RelayCommand(SendMessage);
            AddTaskCommand = new RelayCommand(AddTask);
            CompleteTaskCommand = new RelayCommand(CompleteTask);
            DeleteTaskCommand = new RelayCommand(DeleteTask);
            StartQuizCommand = new RelayCommand(StartQuiz);
            StartTutorialCommand = new RelayCommand(StartTutorial);
            ViewLogsCommand = new RelayCommand(ViewLogs);
            ClearLogsCommand = new RelayCommand(ClearLogs);
            ShowHelpCommand = new RelayCommand(ShowHelp);
            LoadUserDataAsync();
            PlayGreeting();
            CheckRemindersAsync();
        }

        public void SetUserName(string name)
        {
            _userName = name;
            _logger.LogAction($"Set user name to: {_userName}");
            OnPropertyChanged(nameof(StatusMessage));
            SaveUserDataAsync();
            ChatMessages.Add(new ChatMessage($"Hello, {_userName}! Welcome to the Cyber Aware Chatbot!", Colors.Cyan));
        }

        private async void LoadUserDataAsync()
        {
            try
            {
                if (File.Exists("userData.json"))
                {
                    string json = await File.ReadAllTextAsync("userData.json");
                    var data = JsonConvert.DeserializeObject<UserData>(json);
                    if (data != null)
                    {
                        _userName = data.Name ?? "Cyber Explorer";
                        _totalPoints = data.Points;
                        _favoriteTopic = data.FavoriteTopic;
                        _currentTheme = data.CurrentTheme ?? "dark";
                        ApplyTheme();
                        if (data.Tasks != null)
                        {
                            foreach (var task in data.Tasks)
                            {
                                _taskManager.AddTask(task.Title, task.Description, task.ReminderDate);
                                if (task.IsCompleted) _taskManager.CompleteTask(task.Title);
                                Tasks.Add(task);
                            }
                        }
                        if (data.Logs != null)
                        {
                            data.Logs.ForEach(l => _logger.LogAction(l));
                        }
                    }
                }
                OnPropertyChanged(nameof(StatusMessage));
            }
            catch (Exception ex)
            {
                ChatMessages.Add(new ChatMessage($"Error loading data: {ex.Message}", Colors.Red));
                _logger.LogAction($"Error loading data: {ex.Message}");
            }
        }

        private async void SaveUserDataAsync()
        {
            try
            {
                var data = new UserData
                {
                    Name = _userName,
                    Points = _totalPoints,
                    FavoriteTopic = _favoriteTopic,
                    CurrentTheme = _currentTheme,
                    Tasks = _taskManager.ViewTasks(),
                    Logs = _logger.GetLogs()
                };
                string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                await File.WriteAllTextAsync("userData.json", json);
                _logger.LogAction("Saved user data");
            }
            catch (Exception ex)
            {
                ChatMessages.Add(new ChatMessage($"Error saving data: {ex.Message}", Colors.Red));
                _logger.LogAction($"Error saving data: {ex.Message}");
            }
        }

        private void PlayGreeting()
        {
            try
            {
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Greeting.wav");
                if (File.Exists(filePath))
                {
                    using (var player = new SoundPlayer(filePath))
                    {
                        player.PlaySync();
                    }
                    _logger.LogAction($"Played greeting audio from {filePath}");
                }
                else
                {
                    ChatMessages.Add(new ChatMessage($"Greeting audio file not found at {filePath}.", Colors.Red));
                    _logger.LogAction($"Greeting audio file not found at {filePath}");
                }
            }
            catch (Exception ex)
            {
                ChatMessages.Add(new ChatMessage($"Error playing greeting: {ex.Message}", Colors.Red));
                _logger.LogAction($"Error playing greeting: {ex.Message}");
            }
        }

        private async void CheckRemindersAsync()
        {
            while (true)
            {
                try
                {
                    var dueReminders = _taskManager.GetDueReminders(DateTime.Now);
                    if (dueReminders.Any())
                    {
                        string message = "Due Reminders:\n" + string.Join("\n", dueReminders.Select(t => t.Title));
                        ChatMessages.Add(new ChatMessage(message, Colors.Yellow));
                        _logger.LogAction($"Displayed reminders: {string.Join(", ", dueReminders.Select(t => t.Title))}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogAction($"Error checking reminders: {ex.Message}");
                }
                await Task.Delay(60000);
            }
        }

        private void ApplyTheme()
        {
            try
            {
                if (Application.Current.MainWindow != null)
                {
                    Application.Current.MainWindow.Foreground = _themes[_currentTheme].Foreground;
                    Application.Current.MainWindow.Background = _themes[_currentTheme].Background;
                }
                _responseGenerator.SetTheme(_currentTheme);
                OnPropertyChanged(nameof(ForegroundColor));
                OnPropertyChanged(nameof(StatusMessage));
                _logger.LogAction($"Applied theme: {_currentTheme}");
            }
            catch (Exception ex)
            {
                ChatMessages.Add(new ChatMessage($"Error applying theme: {ex.Message}", Colors.Red));
                _logger.LogAction($"Error applying theme: {ex.Message}");
            }
        }

        private void SendMessage()
        {
            if (string.IsNullOrWhiteSpace(UserInput))
            {
                ChatMessages.Add(new ChatMessage("❌ Please type something.", Colors.Red));
                _logger.LogAction("User attempted to send empty message");
                return;
            }

            var input = UserInput.ToLower().Trim();
            ChatMessages.Add(new ChatMessage($"{_userName}: {UserInput}", Colors.Green));
            _logger.LogAction($"User message: {UserInput}");
            UserInput = string.Empty;

            if (input == "exit")
            {
                string recall = _favoriteTopic != null ? $"I remember you were interested in {_favoriteTopic}. " : "";
                ChatMessages.Add(new ChatMessage($"{recall}Are you sure you want to exit? (Type 'yes' to confirm)", Colors.Yellow));
                _logger.LogAction("User requested exit");
                return;
            }
            else if (input == "yes")
            {
                SaveUserDataAsync();
                ChatMessages.Add(new ChatMessage($"Goodbye, {_userName}! Stay cyber aware! Total Points: {_totalPoints}", Colors.Yellow));
                _logger.LogAction("User exited application");
                Application.Current.Shutdown();
                return;
            }

            string sentimentResponse = DetectSentiment(input);
            if (!string.IsNullOrEmpty(sentimentResponse))
            {
                ChatMessages.Add(new ChatMessage($"💡 {sentimentResponse}", Colors.Magenta));
                _logger.LogAction($"Detected sentiment: {sentimentResponse}");
            }

            if (input == "more" && !string.IsNullOrEmpty(_lastCyberTopic))
            {
                string moreResponse = _responseGenerator.GenerateMore(_lastCyberTopic);
                ChatMessages.Add(new ChatMessage($"📖 {moreResponse}", Colors.Cyan));
                _logger.LogAction($"Provided more info on topic: {_lastCyberTopic}");
                return;
            }

            string response = _responseGenerator.Generate(input);
            ChatMessages.Add(new ChatMessage($"📢 {response}", Colors.Cyan));
            _totalPoints += 5;
            OnPropertyChanged(nameof(StatusMessage));
            ChatMessages.Add(new ChatMessage($"🎯 +5 points! Your total: {_totalPoints}", Colors.Green));
            _logger.LogAction("Awarded 5 points for message");

            if (_responseGenerator.IsCybersecurityTopic(input))
            {
                _lastCyberTopic = _responseGenerator.GetCybersecurityTopic(input);
                if (_favoriteTopic == null)
                {
                    _favoriteTopic = _lastCyberTopic;
                    ChatMessages.Add(new ChatMessage($"⭐ Noted! You're interested in {_favoriteTopic}, {_userName}! I'll remember that!", Colors.Magenta));
                    _logger.LogAction($"Set favorite topic: {_favoriteTopic}");
                }
                ChatMessages.Add(new ChatMessage("➡️ Would you like to know more? Type 'more' or try other topics.", Colors.Cyan));
            }
            else
            {
                _lastCyberTopic = null;
            }

            SaveUserDataAsync();
        }

        private void AddTask()
        {
            var dialog = new TaskDialog();
            if (dialog.ShowDialog() == true)
            {
                var task = new TaskItem
                {
                    Title = dialog.Title,
                    Description = dialog.Description,
                    ReminderDate = dialog.ReminderDate,
                    IsCompleted = false
                };
                _taskManager.AddTask(task.Title, task.Description, task.ReminderDate);
                Tasks.Add(task);
                _logger.LogAction($"Added task: {task.Title}");
                ChatMessages.Add(new ChatMessage($"Task '{task.Title}' added, {_userName}.", Colors.Green));
                SaveUserDataAsync();
            }
        }

        private void CompleteTask()
        {
            if (SelectedTask != null)
            {
                _taskManager.CompleteTask(SelectedTask.Title);
                SelectedTask.IsCompleted = true;
                _logger.LogAction($"Completed task: {SelectedTask.Title}");
                ChatMessages.Add(new ChatMessage($"Task '{SelectedTask.Title}' marked as completed, {_userName}.", Colors.Green));
                SaveUserDataAsync();
                SelectedTask = null;
            }
            else
            {
                ChatMessages.Add(new ChatMessage("Please select a task to complete.", Colors.Red));
                _logger.LogAction("User attempted to complete task without selection");
            }
        }

        private void DeleteTask()
        {
            if (SelectedTask != null)
            {
                string title = SelectedTask.Title;
                _taskManager.DeleteTask(title);
                Tasks.Remove(SelectedTask);
                _logger.LogAction($"Deleted task: {title}");
                ChatMessages.Add(new ChatMessage($"Task '{title}' deleted, {_userName}.", Colors.Green));
                SaveUserDataAsync();
                SelectedTask = null;
            }
            else
            {
                ChatMessages.Add(new ChatMessage("Please select a task to delete.", Colors.Red));
                _logger.LogAction("User attempted to delete task without selection");
            }
        }

        private void StartQuiz()
        {
            var quizWindow = new QuizWindow(_taskManager, _logger, ref _totalPoints);
            quizWindow.Closed += (s, e) => OnPropertyChanged(nameof(StatusMessage));
            quizWindow.ShowDialog();
            SaveUserDataAsync();
            _logger.LogAction("Started quiz");
        }

        private void StartTutorial()
        {
            var tutorialWindow = new TutorialWindow(_taskManager, _logger, ref _totalPoints);
            tutorialWindow.Closed += (s, e) => OnPropertyChanged(nameof(StatusMessage));
            tutorialWindow.ShowDialog();
            SaveUserDataAsync();
            _logger.LogAction("Started tutorial");
        }

        private void ViewLogs()
        {
            var logs = _logger.GetLogs();
            ChatMessages.Add(new ChatMessage("Activity Log:\n" + string.Join("\n", logs), Colors.Cyan));
            _logger.LogAction("Viewed activity logs");
        }

        private void ClearLogs()
        {
            _logger.ClearLogs();
            ChatMessages.Add(new ChatMessage($"Activity logs cleared, {_userName}.", Colors.Green));
            _logger.LogAction("Cleared activity logs");
            SaveUserDataAsync();
        }

        private void ShowHelp()
        {
            var helpWindow = new HelpWindow();
            helpWindow.ShowDialog();
            _logger.LogAction("Opened help menu");
        }

        private string DetectSentiment(string input)
        {
            input = input.ToLower();
            if (input.Contains("worried") || input.Contains("anxious") || input.Contains("nervous") || input.Contains("uneasy") || input.Contains("concerned"))
                return $"I understand your concern, {_userName}! Let's go over some tips.";
            if (input.Contains("curious") || input.Contains("interested") || input.Contains("intrigued") || input.Contains("eager") || input.Contains("wondering"))
                return $"Great to see your curiosity, {_userName}! Let's explore that topic together.";
            if (input.Contains("frustrated") || input.Contains("annoyed") || input.Contains("irritated") || input.Contains("upset") || input.Contains("aggravated"))
                return $"Sorry you're frustrated, {_userName}. Let's break it down.";
            if (input.Contains("happy") || input.Contains("glad") || input.Contains("joyful") || input.Contains("excited") || input.Contains("pleased"))
                return $"Glad you're happy, {_userName}! Let's keep learning.";
            if (input.Contains("confused") || input.Contains("puzzled") || input.Contains("uncertain") || input.Contains("lost") || input.Contains("unclear"))
                return $"No worries, {_userName}, I'll clarify that for you.";
            if (input.Contains("angry") || input.Contains("mad") || input.Contains("furious") || input.Contains("irate") || input.Contains("enraged"))
                return $"Sorry you're angry, {_userName}. Let's address this calmly.";
            if (input.Contains("scared") || input.Contains("afraid") || input.Contains("frightened") || input.Contains("panicked") || input.Contains("terrified"))
                return $"It's okay to feel scared, {_userName}. I'm here to help.";
            return "";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ChatMessage
    {
        public string Message { get; set; }
        public SolidColorBrush Color { get; set; }

        public ChatMessage(string message, Color color)
        {
            Message = message;
            Color = new SolidColorBrush(color);
        }
    }

    public class TaskItem : INotifyPropertyChanged
    {
        private bool _isCompleted;

        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? ReminderDate { get; set; }
        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                _isCompleted = value;
                OnPropertyChanged(nameof(IsCompleted));
                OnPropertyChanged(nameof(Status));
            }
        }
        public string Status => IsCompleted ? "Completed" : "Pending";

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        public RelayCommand(Action execute) => _execute = execute;
        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => _execute();
        public event EventHandler CanExecuteChanged;
    }
}