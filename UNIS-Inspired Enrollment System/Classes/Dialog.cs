using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Effects;

namespace UNIS_Inspired_Enrollment_System.Classes
{
    internal class Dialog
    {
        private readonly DialogWindow _dialogWindow;

        public Dialog()
        {
            _dialogWindow = new DialogWindow();
        }

        public void SetDialog(string title, string description)
        {
            _dialogWindow.SetDialog(title, description);
        }

        public void ShowDialog(Window owner)
        {
            BlurEffect blurEffect = new BlurEffect
            {
                Radius = 5
            };
            owner.Effect = blurEffect;

            _dialogWindow.Owner = owner;

            _dialogWindow.ShowDialog();

            owner.Effect = null;
        }
    }
}
