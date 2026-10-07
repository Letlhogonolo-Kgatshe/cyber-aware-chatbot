using System;
using System.Windows;

namespace CyberAwareChatbot
{
    public partial class TaskDialog : Window
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime? ReminderDate { get; private set; }

        public TaskDialog()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Title = TitleTextBox.Text.Trim();
            Description = DescriptionTextBox.Text.Trim();
            if (string.IsNullOrEmpty(Title))
            {
                MessageBox.Show("Title cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (DateTime.TryParse(ReminderTextBox.Text.Trim(), out DateTime date))
            {
                ReminderDate = date;
            }
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}