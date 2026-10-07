using System.Windows;
using System.Windows.Controls;

namespace CyberAwareChatbot
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var viewModel = new MainViewModel();
            DataContext = viewModel;

            // Prompt for user name at startup
            var dialog = new TextInputDialog("Enter Your Name", "Please enter your name:");
            if (dialog.ShowDialog() == true && !string.IsNullOrWhiteSpace(dialog.InputText))
            {
                viewModel.SetUserName(dialog.InputText);
            }
            else
            {
                viewModel.SetUserName("Cyber Explorer");
            }
        }
    }

    public class TextInputDialog : Window
    {
        private TextBox inputTextBox;
        public string InputText => inputTextBox.Text;

        public TextInputDialog(string title, string prompt)
        {
            Title = title;
            Width = 300;
            Height = 150;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            var stackPanel = new StackPanel { Margin = new Thickness(10) };
            stackPanel.Children.Add(new TextBlock { Text = prompt, Margin = new Thickness(0, 0, 0, 10) });

            inputTextBox = new TextBox { Width = 260 };
            stackPanel.Children.Add(inputTextBox);

            var buttonPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, 10, 0, 0) };
            var okButton = new Button { Content = "OK", Width = 75, Margin = new Thickness(0, 0, 5, 0) };
            okButton.Click += (s, e) => { DialogResult = true; Close(); };
            var cancelButton = new Button { Content = "Cancel", Width = 75 };
            cancelButton.Click += (s, e) => { DialogResult = false; Close(); };
            buttonPanel.Children.Add(okButton);
            buttonPanel.Children.Add(cancelButton);
            stackPanel.Children.Add(buttonPanel);

            Content = stackPanel;
        }
    }
}