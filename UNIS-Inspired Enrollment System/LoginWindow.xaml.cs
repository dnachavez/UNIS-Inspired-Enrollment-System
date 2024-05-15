using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UNIS_Inspired_Enrollment_System.Classes;

namespace UNIS_Inspired_Enrollment_System
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void TxtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            bool isPasswordEmpty = TxtPassword.SecurePassword.Length == 0;
            bool isFocused = TxtPassword.IsFocused;

            TxtPasswordPlaceholder.Visibility = isPasswordEmpty && !isFocused ? Visibility.Visible : Visibility.Collapsed;
        }

        private void TxtPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            bool isPasswordEmpty = TxtPassword.SecurePassword.Length == 0;
            bool isFocused = TxtPassword.IsFocused;

            TxtPasswordPlaceholder.Visibility = isPasswordEmpty && !isFocused ? Visibility.Visible : Visibility.Collapsed;
        }

        private void TxtPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            bool isPasswordEmpty = TxtPassword.SecurePassword.Length == 0;
            bool isFocused = TxtPassword.IsFocused;

            TxtPasswordPlaceholder.Visibility = isPasswordEmpty && !isFocused ? Visibility.Visible : Visibility.Collapsed;
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtUsername.Text))
            {
                Dialog dialog = new Dialog();
                dialog.SetDialog("Error", "Please enter your username.");
                dialog.ShowDialog(this);
            }
            else if (string.IsNullOrEmpty(TxtPassword.Password))
            {
                Dialog dialog = new Dialog();
                dialog.SetDialog("Error", "Please enter your password.");
                dialog.ShowDialog(this);
            }
            else if (string.IsNullOrEmpty(TxtUsername.Text) && string.IsNullOrEmpty(TxtPassword.Password))
            {
                Dialog dialog = new Dialog();
                dialog.SetDialog("Error", "Please enter your username and password.");
                dialog.ShowDialog(this);
            }
            else
            {
                User user = new User();
                if (user.Login(TxtUsername.Text, TxtPassword.Password))
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    Close();
                }
                else
                {
                    Dialog dialog = new Dialog();
                    dialog.SetDialog("Error", "Invalid username or password.");
                    dialog.ShowDialog(this);
                }
            }
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            // Minimize the window
            WindowState = WindowState.Minimized;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            // Close the window
            Close();
        }
    }
}
