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

namespace UNIS_Inspired_Enrollment_System
{
    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {
        public DialogWindow()
        {
            InitializeComponent();
        }

        public void SetDialog(string title, string description)
        {
            // Set the title and description of the dialog
            TbTitle.Text = title;
            TbDescription.Text = description;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            // Close the window
            Close();
        }
    }
}
