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
using System.Windows.Navigation;
using System.Windows.Shapes;
using UNIS_Inspired_Enrollment_System.Classes;

namespace UNIS_Inspired_Enrollment_System.Pages
{
    /// <summary>
    /// Interaction logic for UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        private int? selectedUserId = null;

        public UserPage()
        {
            InitializeComponent();

            LoadUsers();
        }

        private void LoadUsers()
        {
            User user = new User();
            DgUsers.ItemsSource = user.GetUsers();
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

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtUsername.Text))
            {
                Dialog dialog = new Dialog();
                dialog.SetDialog("Error", "Please enter a username.");
                dialog.ShowDialog(Window.GetWindow(this));
            }
            else if (string.IsNullOrEmpty(TxtPassword.Password))
            {
                Dialog dialog = new Dialog();
                dialog.SetDialog("Error", "Please enter a password.");
                dialog.ShowDialog(Window.GetWindow(this));
            }
            else
            {
                if (selectedUserId.HasValue)
                {
                    User user = new User();
                    if (user.UpdateUser(selectedUserId.Value, TxtUsername.Text, TxtPassword.Password))
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Success", "User updated successfully.");
                        dialog.ShowDialog(Window.GetWindow(this));
                        LoadUsers();
                        TxtUsername.Text = "";
                        TxtPassword.Password = "";
                        selectedUserId = null;
                        BtnAdd.Content = "Add";
                    }
                    else
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Error", "Failed to update user.");
                        dialog.ShowDialog(Window.GetWindow(this));
                    }
                }
                else
                {
                    User user = new User();
                    if (user.AddUser(TxtUsername.Text, TxtPassword.Password))
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Success", "User added successfully.");
                        dialog.ShowDialog(Window.GetWindow(this));
                        LoadUsers();
                        TxtUsername.Text = "";
                        TxtPassword.Password = "";
                    }
                    else
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Error", "Failed to add user.");
                        dialog.ShowDialog(Window.GetWindow(this));
                    }
                }
            }
        }

        private void SearchUsers(string search)
        {
            User user = new User();

            var formattedUsers = user.GetUsers().Select(u => new
            {
                u.Id,
                u.Username,
                u.PasswordHash,
                u.PasswordSalt,
            }).ToList();

            var filteredUsers = formattedUsers.Where(u => 
                u.Id.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                u.Username.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                u.PasswordHash.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                u.PasswordSalt.Contains(search, StringComparison.OrdinalIgnoreCase)
            ).ToList();

            DgUsers.ItemsSource = filteredUsers;
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchUsers(TxtSearch.Text);
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchUsers(TxtSearch.Text);
        }

        private void DeleteUser()
        {
            if (DgUsers.SelectedItem is User selectedUser)
            {
                Prompt prompt = new Prompt();
                prompt.SetPrompt("Delete User", "Are you sure you want to delete this user?");
                // Create the event handler for the Confirm event, which is to call the DeleteUser method from the User class
                prompt.Confirm += (s, e) =>
                {
                    User user = new User();
                    if (user.DeleteUser(selectedUser.Id))
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Success", "User deleted successfully.");
                        dialog.ShowDialog(Window.GetWindow(this));
                        LoadUsers();
                    }
                    else
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Error", "Failed to delete user.");
                        dialog.ShowDialog(Window.GetWindow(this));
                    }
                };
                prompt.ShowPrompt(Window.GetWindow(this));
            }
        }

        private void DgUsers_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                DeleteUser();
            }
        }

        private void DgUsers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DgUsers.SelectedItem is User selectedUser)
            {
                TxtUsername.Text = selectedUser.Username;
                selectedUserId = selectedUser.Id;
                BtnAdd.Content = "Update";
            }
        }

        private void BtnAddTeacher_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAddTeacherLoad_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TxtTeacherSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TxtTeacherLoadSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void BtnTeacherSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DgTeachers_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void DgTeachers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void BtnTeacherLoadSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DgTeacherLoads_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void DgTeacherLoads_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
