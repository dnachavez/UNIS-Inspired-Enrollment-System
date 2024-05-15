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
    /// Interaction logic for CoursePage.xaml
    /// </summary>
    public partial class CoursePage : Page
    {
        private int? selectedCourseId = null;

        public CoursePage()
        {
            InitializeComponent();

            LoadCourses();
            LoadDepartments();
        }

        public void LoadCourses()
        {
            Course course = new Course();
            DgCourses.ItemsSource = course.GetCourses();
        }

        public void LoadDepartments()
        {
            Department department = new Department();
            CmbDepartments.ItemsSource = department.GetDepartments();
            CmbDepartments.DisplayMemberPath = "Name";
            CmbDepartments.SelectedValuePath = "Id";
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtCourseName.Text))
            {
                Dialog dialog = new Dialog();
                dialog.SetDialog("Error", "Please enter a course name.");
                dialog.ShowDialog(Window.GetWindow(this));
            }
            else if (CmbDepartments.SelectedItem == null)
            {
                Dialog dialog = new Dialog();
                dialog.SetDialog("Error", "Please select a department.");
                dialog.ShowDialog(Window.GetWindow(this));
            }
            else
            {
                if (selectedCourseId.HasValue)
                {
                    Course course = new Course();
                    if (course.UpdateCourse(selectedCourseId.Value, TxtCourseName.Text, (int)CmbDepartments.SelectedValue))
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Success", "Course updated successfully.");
                        dialog.ShowDialog(Window.GetWindow(this));
                        LoadCourses();
                        TxtCourseName.Text = "";
                        CmbDepartments.SelectedItem = null;
                        selectedCourseId = null;
                        BtnAdd.Content = "Add";
                    }
                    else
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Error", "Failed to update course.");
                        dialog.ShowDialog(Window.GetWindow(this));
                    }
                }
                else
                {
                    Course course = new Course();
                    if (course.AddCourse(TxtCourseName.Text, (int)CmbDepartments.SelectedValue))
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Success", "Course added successfully.");
                        dialog.ShowDialog(Window.GetWindow(this));
                        LoadCourses();
                        TxtCourseName.Text = "";
                        CmbDepartments.SelectedItem = null;
                    }
                    else
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Error", "Course already exists.");
                        dialog.ShowDialog(Window.GetWindow(this));
                    }
                }
            }
        }

        public void SearchCourses(string search)
        {
            Course course = new Course();

            var formattedCourses = course.GetCourses().Select(c => new
            {
                c.Id,
                c.Name,
                c.DepartmentName
            }).ToList();

            var filteredCourses = formattedCourses.Where(c => 
                c.Id.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                c.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                c.DepartmentName.Contains(search, StringComparison.OrdinalIgnoreCase)
            ).ToList();

            DgCourses.ItemsSource = filteredCourses;
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchCourses(TxtSearch.Text);
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchCourses(TxtSearch.Text);
        }

        private void DeleteCourse()
        {
            if (DgCourses.SelectedItem is Course selectedCourse)
            {
                Prompt prompt = new Prompt();
                prompt.SetPrompt("Delete Course", "Are you sure you want to delete this course?");
                // Create the event handler for the Confirm event, which is to call the DeleteCourse method from the Course class
                prompt.Confirm += (s, e) =>
                {
                    Course course = new Course();
                    if (course.DeleteCourse(selectedCourse.Id))
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Success", "Course deleted successfully.");
                        dialog.ShowDialog(Window.GetWindow(this));
                        LoadCourses();
                    }
                    else
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Error", "Failed to delete course.");
                        dialog.ShowDialog(Window.GetWindow(this));
                    }
                };
                prompt.ShowPrompt(Window.GetWindow(this));
            }
        }

        private void DgCourses_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                DeleteCourse();
            }
        }

        private void DgCourses_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DgCourses.SelectedItem is Course selectedCourse)
            {
                TxtCourseName.Text = selectedCourse.Name;
                CmbDepartments.SelectedValue = selectedCourse.DeparmentId;
                selectedCourseId = selectedCourse.Id;
                BtnAdd.Content = "Update";
            }
        }
    }
}
