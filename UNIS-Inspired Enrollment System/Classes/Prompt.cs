using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Effects;

namespace UNIS_Inspired_Enrollment_System.Classes
{
    internal class Prompt
    {
        private readonly PromptWindow _promptWindow;

        public event EventHandler Confirm;
        public event EventHandler Cancel;

        public Prompt()
        {
            _promptWindow = new PromptWindow();
            _promptWindow.Confirm += PromptWindow_Confirm;
            _promptWindow.Cancel += PromptWindow_Cancel;
        }

        public void SetPrompt(string title, string message)
        {
            _promptWindow.SetPrompt(title, message);
        }

        public void ShowPrompt(Window owner)
        {
            BlurEffect effect = new BlurEffect
            {
                Radius = 5
            };
            owner.Effect = effect;

            _promptWindow.Owner = owner;
            _promptWindow.ShowDialog();

            owner.Effect = null;
        }

        private void PromptWindow_Confirm(object sender, EventArgs e)
        {
            Confirm?.Invoke(this, EventArgs.Empty);
        }

        private void PromptWindow_Cancel(object sender, EventArgs e)
        {
            Cancel?.Invoke(this, EventArgs.Empty);
        }
    }
}
