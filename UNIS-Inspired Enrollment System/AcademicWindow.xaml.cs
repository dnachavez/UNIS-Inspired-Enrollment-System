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
using UNIS_Inspired_Enrollment_System.Controls;

namespace UNIS_Inspired_Enrollment_System
{
    /// <summary>
    /// Interaction logic for AcademicWindow.xaml
    /// </summary>
    public partial class AcademicWindow : Window
    {
        public AcademicWindow()
        {
            InitializeComponent();
        }

        private void SidebarButtons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SidebarButton SelectedButton = (SidebarButton)SidebarButtons.SelectedItem;
            Page.Navigate(SelectedButton.NavLink);
        }

        private void BottomSidebarButtons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SidebarButton SelectedButton = (SidebarButton)BottomSidebarButtons.SelectedItem;
            switch (SelectedButton.Name)
            {
                case "BtnBack":
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
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
    }
}
