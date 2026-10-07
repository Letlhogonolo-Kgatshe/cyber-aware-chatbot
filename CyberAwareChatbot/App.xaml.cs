// Letlhogonolo Kgatshe
// ST10445158
// Group 1

// References: List your references here Part 1, Part 2 and Part 3

// Delayed Response to simulate actual thinking                      https://ironpdf.com/blog/net-help/csharp-wait-for-seconds/#:~:text=The%20Sleep%20Command&text=Here%2C%20the%20program%20starts%20by,3%20seconds%22%20to%20the%20console
//                                                         Used for simulating thinking delays in chatbot responses.
// Error Handling                                                  https://www.w3schools.com/cs/cs_exceptions.php
//                                                         Guided try-catch blocks for robust error handling in MainViewModel.cs and audio playback.
// Playing Audio in WAV format                                     https://stackoverflow.com/questions/14491431/playing-wav-file-with-c-sharp
//                                                         Implemented SoundPlayer for Greeting.wav, though currently facing "file not found" issue.
// Sample data for cybersecurity questions and answers            https://chatgpt.com/c/67e542f0-9300-8004-81a7-a8abe4e47228
//                                                         Provided initial data for ResponseGenerator.cs to handle phishing and password queries.
// ASCII ARTWORK LOGO                                             https://patorjk.com/software/taag/#p=display&f=Graffiti&t=Cyber%20Chat%0A
//                                                         Generated the Cyber Aware Chatbot logo displayed in MainWindow.xaml.
// WPF MVVM Pattern                                               https://learn.microsoft.com/en-us/dotnet/desktop/wpf/data/?view=netdesktop-8.0
//                                                         Guided the implementation of MainViewModel.cs for data binding and command handling.
// JSON Serialization                                             https://www.newtonsoft.com/json/help/html/SerializingJSON.htm
//                                                         Used Newtonsoft.Json for saving/loading user data (tasks, points, logs) to userData.json.
// Load file into memory                                          https://learn.microsoft.com/en-us/troubleshoot/developer/visualstudio/csharp/language-compilers/read-write-text-file
//                                                         Used for reading/writing userData.json in MainViewModel.cs.
// Time Greeting                                                  https://stackoverflow.com/questions/40548077/display-welcome-message-as-per-local-time-in-asp-net
//                                                         Inspired the time-based status message in MainViewModel.cs with SAST formatting.
// Data Dictionaries for keyword and random responses             https://www.geeksforgeeks.org/c-sharp-dictionary-with-examples/
//                                                         Used in ResponseGenerator.cs for mapping cybersecurity keywords to responses.
// Change Theme from Light to Dark                                https://learn.microsoft.com/en-us/dotnet/api/system.windows.media.solidcolorbrush?view=net-8.0
//                                                         Adapted for WPF theme switching (light, dark, blue) in MainViewModel.cs.
// Progress Bar                                                   https://stackoverflow.com/questions/24918768/progress-bar-in-console-application
//                                                         Considered for future enhancements, not currently implemented.
// Loading animation                                              https://stackoverflow.com/questions/1204440/how-to-create-a-loading-animation-in-console-app
//                                                         Considered for future UI feedback, not currently implemented.
// WPF ListView and Tooltips                                      https://learn.microsoft.com/en-us/dotnet/desktop/wpf/controls/listview-overview?view=netdesktop-8.0
//                                                         Guided the task ListView and reminder tooltip in MainWindow.xaml.
// Phishing Tutorial Logic                                        https://www.cisa.gov/sites/default/files/publications/phishing_infographic.pdf
//                                                         Informed the phishing awareness tutorial steps in TutorialWindow.xaml.cs.

// YouTube Video Part 1                                           https://youtu.be/0GrA31cdZ3I
// YouTube Video Part 2                                           https://youtu.be/QAwVnws7JBU
// YouTube Video Part 3                                           https://youtu.be/Fw-UeS2wCbo

using System.Configuration;
using System.Data;
using System.Windows;

namespace CyberAwareChatbot
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
    }

}
