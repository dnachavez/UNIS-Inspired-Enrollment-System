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
    /// Interaction logic for SemesterPage.xaml
    /// </summary>
    public partial class SemesterPage : Page
    {
        private int? selectedSemesterId = null;

        public SemesterPage()
        {
            InitializeComponent();

            LoadSemesters();
        }

        public void LoadSemesters()
        {
            Semester semester = new Semester();
            DgSemesters.ItemsSource = semester.GetSemesters();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtSemester.Text))
            {
                Dialog dialog = new Dialog();
                dialog.SetDialog("Error", "Please enter a semester name.");
                dialog.ShowDialog(Window.GetWindow(this));
            }
            else
            {
                if (selectedSemesterId.HasValue)
                {
                    Semester semester = new Semester();
                    if (semester.UpdateSemester(selectedSemesterId.Value, TxtSemester.Text))
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Success", "Semester updated successfully.");
                        dialog.ShowDialog(Window.GetWindow(this));
                        LoadSemesters();
                        TxtSemester.Text = "";
                        selectedSemesterId = null;
                        BtnAdd.Content = "Add";
                    }
                    else
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Error", "Failed to update semester.");
                        dialog.ShowDialog(Window.GetWindow(this));
                    }
                }
                else
                {
                    Semester semester = new Semester();
                    if (semester.AddSemester(TxtSemester.Text))
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Success", "Semester added successfully.");
                        dialog.ShowDialog(Window.GetWindow(this));
                        LoadSemesters();
                        TxtSemester.Text = "";
                    }
                    else
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Error", "Semester already exists.");
                        dialog.ShowDialog(Window.GetWindow(this));
                    }
                }
            }
        }

        private void SearchSemesters(string search)
        {
            Semester semester = new Semester();

            var formattedSemesters = semester.GetSemesters().Select(s => new
            {
                s.Id,
                s.Name
            }).ToList();

            var filteredSemesters = formattedSemesters.Where(s =>
                s.Id.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                s.Name.Contains(search, StringComparison.OrdinalIgnoreCase)
            ).ToList();

            DgSemesters.ItemsSource = filteredSemesters;
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchSemesters(TxtSearch.Text);
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchSemesters(TxtSearch.Text);
        }

        private void DeleteSemester()
        {
            if (DgSemesters.SelectedItem is Semester selectedSemester)
            {
                Prompt prompt = new Prompt();
                prompt.SetPrompt("Delete Semester", "Are you sure you want to delete this semester?");
                // Create the event handler for the Confirm event, which is to call the DeleteSemester method from the Semester class
                prompt.Confirm += (s, e) =>
                {
                    Semester semester = new Semester();
                    if (semester.DeleteSemester(selectedSemester.Id))
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Success", "Semester deleted successfully.");
                        dialog.ShowDialog(Window.GetWindow(this));
                        LoadSemesters();
                    }
                    else
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Error", "Failed to delete semester.");
                        dialog.ShowDialog(Window.GetWindow(this));
                    }
                };
                prompt.ShowPrompt(Window.GetWindow(this));
            }
        }

        private void DgSemesters_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                DeleteSemester();
            }
        }

        private void DgSemesters_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DgSemesters.SelectedItem is Semester selectedSemester)
            {
                TxtSemester.Text = selectedSemester.Name;
                selectedSemesterId = selectedSemester.Id;
                BtnAdd.Content = "Update";
            }
        }
    }
}
