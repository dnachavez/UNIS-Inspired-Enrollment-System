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
    /// Interaction logic for DepartmentPage.xaml
    /// </summary>
    public partial class DepartmentPage : Page
    {
        private int? selectedDepartmentId = null;

        public DepartmentPage()
        {
            InitializeComponent();

            LoadDepartments();
        }

        public void LoadDepartments()
        {
            Department department = new Department();
            DgDepartments.ItemsSource = department.GetDepartments();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtDepartmentName.Text))
            {
                Dialog dialog = new Dialog();
                dialog.SetDialog("Error", "Please enter a department name.");
                dialog.ShowDialog(Window.GetWindow(this));
            }
            else
            {
                if (selectedDepartmentId.HasValue)
                {
                    Department department = new Department();
                    if (department.UpdateDepartment(selectedDepartmentId.Value, TxtDepartmentName.Text))
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Success", "Department updated successfully.");
                        dialog.ShowDialog(Window.GetWindow(this));
                        LoadDepartments();
                        TxtDepartmentName.Text = "";
                        selectedDepartmentId = null;
                        BtnAdd.Content = "Add";
                    }
                    else
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Error", "Failed to update department.");
                        dialog.ShowDialog(Window.GetWindow(this));
                    }
                }
                else
                {
                    Department department = new Department();
                    if (department.AddDepartment(TxtDepartmentName.Text))
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Success", "Department added successfully.");
                        dialog.ShowDialog(Window.GetWindow(this));
                        LoadDepartments();
                        TxtDepartmentName.Text = "";
                    }
                    else
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Error", "Department already exists.");
                        dialog.ShowDialog(Window.GetWindow(this));
                    }
                }
            }
        }

        private void SearchDepartments(string search)
        {
            Department department = new Department();

            var formattedDepartments = department.GetDepartments().Select(d => new
            {
                d.Id,
                d.Name
            }).ToList();

            var filteredDepartments = formattedDepartments.Where(ay =>
                ay.Id.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                ay.Name.Contains(search, StringComparison.OrdinalIgnoreCase)
            ).ToList();

            DgDepartments.ItemsSource = filteredDepartments;
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchDepartments(TxtSearch.Text);
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchDepartments(TxtSearch.Text);
        }

        private void DeleteDepartment()
        {
            if (DgDepartments.SelectedItem is Department selectedDepartment)
            {
                Prompt prompt = new Prompt();
                prompt.SetPrompt("Delete Department", "Are you sure you want to delete this department?");
                // Create the event handler for the Confirm event, which is to call the DeleteDepartment method from the Department class
                prompt.Confirm += (s, e) =>
                {
                    Department department = new Department();
                    if (department.DeleteDepartment(selectedDepartment.Id))
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Success", "Department deleted successfully.");
                        dialog.ShowDialog(Window.GetWindow(this));
                        LoadDepartments();
                    }
                    else
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Error", "Failed to delete department.");
                        dialog.ShowDialog(Window.GetWindow(this));
                    }
                };
                prompt.ShowPrompt(Window.GetWindow(this));
            }
        }

        private void DgDepartments_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                DeleteDepartment();
            }
        }

        private void DgDepartments_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Department department = (Department)DgDepartments.SelectedItem;
            if (department != null)
            {
                TxtDepartmentName.Text = department.Name;
                selectedDepartmentId = department.Id;
                BtnAdd.Content = "Update";
            }
        }
    }
}
