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
    /// Interaction logic for AcademicYearPage.xaml
    /// </summary>
    public partial class AcademicYearPage : Page
    {
        private int? selectedAcademicYearId = null;

        public AcademicYearPage()
        {
            InitializeComponent();

            LoadAcademicYears();
        }

        private void LoadAcademicYears()
        {
            AcademicYear academicYear = new AcademicYear();
            var academicYears = academicYear.GetAcademicYears();
            var formattedAcademicYears = academicYears.Select(a => new
            {
                a.Id,
                a.Name,
                Status = a.Status == 1 ? "Enabled" : "Disabled"
            }).ToList();
            DgAcademicYears.ItemsSource = formattedAcademicYears;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtAcademicYear.Text))
            {
                Dialog dialog = new Dialog();
                dialog.SetDialog("Error", "Please enter an academic year name.");
                dialog.ShowDialog(Window.GetWindow(this));
            }
            else if (RadioEnabled.IsChecked == false && RadioDisabled.IsChecked == false)
            {
                Dialog dialog = new Dialog();
                dialog.SetDialog("Error", "Please select a status.");
                dialog.ShowDialog(Window.GetWindow(this));
            }
            else
            {
                if (selectedAcademicYearId.HasValue)
                {
                    int status = RadioEnabled.IsChecked == true ? 1 : 0;
                    AcademicYear academicYear = new AcademicYear();
                    if (academicYear.UpdateAcademicYear(selectedAcademicYearId.Value, TxtAcademicYear.Text, status))
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Success", "Academic year updated successfully.");
                        dialog.ShowDialog(Window.GetWindow(this));
                        LoadAcademicYears();
                        TxtAcademicYear.Text = "";
                        selectedAcademicYearId = null;
                        RadioEnabled.IsChecked = false;
                        RadioDisabled.IsChecked = false;
                        BtnAdd.Content = "Add";
                    }
                    else
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Error", "Failed to update academic year");
                        dialog.ShowDialog(Window.GetWindow(this));
                    }
                }
                else
                {
                    int status = RadioEnabled.IsChecked == true ? 1 : 0;
                    AcademicYear academicYear = new AcademicYear();
                    if (academicYear.AddAcademicYear(TxtAcademicYear.Text, status))
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Success", "Academic year added successfully.");
                        dialog.ShowDialog(Window.GetWindow(this));
                        LoadAcademicYears();
                        TxtAcademicYear.Text = "";
                        RadioEnabled.IsChecked = false;
                        RadioDisabled.IsChecked = false;
                    }
                    else
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Error", "Academic year already exists.");
                        dialog.ShowDialog(Window.GetWindow(this));
                    }
                }
            }
        }

        private void SearchAcademicYear(string search)
        {
            AcademicYear academicYear = new AcademicYear();

            var formattedAcademicYears = academicYear.GetAcademicYears().Select(a => new
            {
                a.Id,
                a.Name,
                Status = a.Status == 1 ? "Enabled" : "Disabled"
            }).ToList();

            var filteredAcademicYears = formattedAcademicYears.Where(ay =>
                ay.Id.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                ay.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                ay.Status.Contains(search, StringComparison.OrdinalIgnoreCase)
            ).ToList();

            DgAcademicYears.ItemsSource = filteredAcademicYears;
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchAcademicYear(TxtSearch.Text);
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchAcademicYear(TxtSearch.Text);
        }

        private void DeleteAcademicYear()
        {
            if (DgAcademicYears.SelectedItem is not null)
            {
                var selectedAcademicYear = (dynamic)DgAcademicYears.SelectedItem;
                Prompt prompt = new Prompt();
                prompt.SetPrompt("Delete Academic Year", "Are you sure you want to delete this academic year?");
                // Create the event handler for the Confirm event, which is to call the DeleteAcademicYear method from the AcademicYear class
                prompt.Confirm += (s, e) =>
                {
                    Department department = new Department();
                    if (department.DeleteDepartment(selectedAcademicYear.Id))
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Success", "Academic year deleted successfully.");
                        dialog.ShowDialog(Window.GetWindow(this));
                        LoadAcademicYears();
                    }
                    else
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Error", "Failed to delete academic year.");
                        dialog.ShowDialog(Window.GetWindow(this));
                    }
                };
                prompt.ShowPrompt(Window.GetWindow(this));
            }
        }

        private void DgAcademicYears_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                DeleteAcademicYear();
            }
        }

        private void DgAcademicYears_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DgAcademicYears.SelectedItem is not null)
            {
                var selectedAcademicYear = (dynamic)DgAcademicYears.SelectedItem;
                TxtAcademicYear.Text = selectedAcademicYear.Name;
                if (selectedAcademicYear.Status == "Enabled")
                {
                    RadioEnabled.IsChecked = true;
                }
                else
                {
                    RadioDisabled.IsChecked = true;
                }
                selectedAcademicYearId = selectedAcademicYear.Id;
                BtnAdd.Content = "Update";
            }
        }
    }
}
