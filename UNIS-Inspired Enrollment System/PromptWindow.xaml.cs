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
    /// Interaction logic for PromptWindow.xaml
    /// </summary>
    public partial class PromptWindow : Window
    {
        public event EventHandler Confirm;
        public event EventHandler Cancel;

        public PromptWindow()
        {
            InitializeComponent();
        }

        public void SetPrompt(string title, string message)
        {
            TbTitle.Text = title;
            TbDescription.Text = message;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Cancel?.Invoke(this, EventArgs.Empty);
            Close();
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            Confirm?.Invoke(this, EventArgs.Empty);
            Close();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            // Close the window
            Close();
        }
    }
}
