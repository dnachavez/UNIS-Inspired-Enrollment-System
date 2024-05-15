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
using System.Windows.Threading;

namespace UNIS_Inspired_Enrollment_System
{
    /// <summary>
    /// Interaction logic for SplashWindow.xaml
    /// </summary>
    public partial class SplashWindow : Window
    {
        private readonly DispatcherTimer _timer;
        private readonly Random _random;
        private double _currentProgress;

        public SplashWindow()
        {
            InitializeComponent();

            _random = new Random();
            _currentProgress = 0;

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(100)
            };
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (_currentProgress < Loader.Maximum)
            {
                _currentProgress += _random.Next(1, 6);
                Loader.Value = _currentProgress;
            }
            else
            {
                _timer.Stop();

                if (Properties.Settings.Default.IsLoggedIn)
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                }
                else
                {
                    LoginWindow loginWindow = new LoginWindow();
                    loginWindow.Show();
                }

                Close();
            }
        }
    }
}
