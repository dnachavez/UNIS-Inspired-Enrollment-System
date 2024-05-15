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
    /// Interaction logic for SubjectPage.xaml
    /// </summary>
    public partial class SubjectPage : Page
    {
        private int? selectedSubjectId = null;

        public SubjectPage()
        {
            InitializeComponent();

            LoadSubjects();
        }

        public void LoadSubjects()
        {
            Subject subject = new Subject();
            DgSubjects.ItemsSource = subject.GetSubjects();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtSubjectName.Text))
            {
                Dialog dialog = new Dialog();
                dialog.SetDialog("Error", "Please enter a subject name.");
                dialog.ShowDialog(Window.GetWindow(this));
            }
            else if (string.IsNullOrEmpty(TxtSubjectCode.Text))
            {
                Dialog dialog = new Dialog();
                dialog.SetDialog("Error", "Please enter a subject code.");
                dialog.ShowDialog(Window.GetWindow(this));
            }
            else
            {
                if (selectedSubjectId.HasValue)
                {
                    Subject subject = new Subject();
                    if (subject.UpdateSubject(selectedSubjectId.Value, TxtSubjectName.Text, TxtSubjectCode.Text))
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Success", "Subject updated successfully.");
                        dialog.ShowDialog(Window.GetWindow(this));
                        LoadSubjects();
                        TxtSubjectName.Text = "";
                        TxtSubjectCode.Text = "";
                        selectedSubjectId = null;
                        BtnAdd.Content = "Add";
                    }
                    else
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Error", "Failed to update subject.");
                        dialog.ShowDialog(Window.GetWindow(this));
                    }
                }
                else
                {
                    Subject subject = new Subject();
                    if (subject.AddSubject(TxtSubjectName.Text, TxtSubjectCode.Text))
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Success", "Subject added successfully.");
                        dialog.ShowDialog(Window.GetWindow(this));
                        LoadSubjects();
                        TxtSubjectName.Text = "";
                        TxtSubjectCode.Text = "";
                    }
                    else
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Error", "Subject already exists.");
                        dialog.ShowDialog(Window.GetWindow(this));
                    }
                }
            }
        }

        private void SearchSubjects(string search)
        {
            Subject subject = new Subject();

            var formattedSubjects = subject.GetSubjects().Select(s => new
            {
                s.Id,
                s.Name,
                s.Code
            }).ToList();

            var filteredSubjects = formattedSubjects.Where(s => 
                s.Id.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                s.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                s.Code.Contains(search, StringComparison.OrdinalIgnoreCase)
            ).ToList();

            DgSubjects.ItemsSource = filteredSubjects;
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchSubjects(TxtSearch.Text);
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchSubjects(TxtSearch.Text);
        }

        private void DeleteSubject()
        {
            if (DgSubjects.SelectedItem is Subject selectedSubject)
            {
                Prompt prompt = new Prompt();
                prompt.SetPrompt("Delete Subject", "Are you sure you want to delete this subject?");
                // Create the event handler for the Confirm event, which is to call the DeleteSubject method from the Subject class
                prompt.Confirm += (s, e) =>
                {
                    Subject subject = new Subject();
                    if (subject.DeleteSubject(selectedSubject.Id))
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Success", "Subject deleted successfully.");
                        dialog.ShowDialog(Window.GetWindow(this));
                        LoadSubjects();
                    }
                    else
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Error", "Failed to delete subject.");
                        dialog.ShowDialog(Window.GetWindow(this));
                    }
                };
                prompt.ShowPrompt(Window.GetWindow(this));
            }
        }

        private void DgSubjects_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                DeleteSubject();
            }
        }

        private void DgSubjects_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DgSubjects.SelectedItem is Subject selectedSubject)
            {
                TxtSubjectName.Text = selectedSubject.Name;
                TxtSubjectCode.Text = selectedSubject.Code;
                selectedSubjectId = selectedSubject.Id;
                BtnAdd.Content = "Update";
            }
        }

        private void BtnAdd2_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
