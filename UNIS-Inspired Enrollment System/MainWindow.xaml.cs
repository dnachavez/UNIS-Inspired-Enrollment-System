using System.Text;
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
using UNIS_Inspired_Enrollment_System.Controls;

namespace UNIS_Inspired_Enrollment_System
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SidebarButtons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SidebarButton SelectedButton = (SidebarButton)SidebarButtons.SelectedItem;
            Page.Navigate(SelectedButton.NavLink);
            switch (SelectedButton.Name)
            {
                case "Admission":
                    AdmissionWindow admissionWindow = new AdmissionWindow();
                    admissionWindow.Show();
                    Close();
                    break;
                case "Academic":
                    AcademicWindow academicWindow = new AcademicWindow();
                    academicWindow.Show();
                    Close();
                    break;
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

        private void BottomSidebarButtons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SidebarButton SelectedButton = (SidebarButton)BottomSidebarButtons.SelectedItem;
            switch (SelectedButton.Name)
            {
                case "BtnLogout":
                    User user = new User();
                    if (user.Logout())
                    {
                        LoginWindow loginWindow = new LoginWindow();
                        loginWindow.Show();
                        Close();
                    }
                    break;
            }
        }
    }
}