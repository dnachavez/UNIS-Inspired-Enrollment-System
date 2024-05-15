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
    /// Interaction logic for CourseSubjectPage.xaml
    /// </summary>
    public partial class CourseSubjectPage : Page
    {
        private int? selectedCourseSubjectId = null;

        public CourseSubjectPage()
        {
            InitializeComponent();

            LoadCourseSubjects();
            LoadSubjects();
            LoadCourses();
            LoadYearLevels();
            LoadSemesters();
        }

        public void LoadCourseSubjects()
        {
            CourseSubject courseSubject = new CourseSubject();
            DgCourseSubjects.ItemsSource = courseSubject.GetCourseSubjects();
        }

        public void LoadSubjects()
        {
            Subject subject = new Subject();
            CmbSubjects.ItemsSource = subject.GetSubjects();
            CmbSubjects.DisplayMemberPath = "Name";
            CmbSubjects.SelectedValuePath = "Id";
        }

        public void LoadCourses()
        {
            Course course = new Course();
            CmbCourses.ItemsSource = course.GetCourses();
            CmbCourses.DisplayMemberPath = "Name";
            CmbCourses.SelectedValuePath = "Id";
        }

        public void LoadYearLevels()
        {
            YearLevel yearLevel = new YearLevel();
            CmbYearLevels.ItemsSource = yearLevel.GetYearLevels();
            CmbYearLevels.DisplayMemberPath = "Name";
            CmbYearLevels.SelectedValuePath = "Id";
        }

        public void LoadSemesters()
        {
            Semester semester = new Semester();
            CmbSemesters.ItemsSource = semester.GetSemesters();
            CmbSemesters.DisplayMemberPath = "Name";
            CmbSemesters.SelectedValuePath = "Id";
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (CmbSubjects.SelectedItem == null)
            {
                Dialog dialog = new Dialog();
                dialog.SetDialog("Error", "Please select a subject.");
                dialog.ShowDialog(Window.GetWindow(this));
            }
            else if (CmbCourses.SelectedItem == null)
            {
                Dialog dialog = new Dialog();
                dialog.SetDialog("Error", "Please select a course.");
                dialog.ShowDialog(Window.GetWindow(this));
            }
            else if (CmbYearLevels.SelectedItem == null)
            {
                Dialog dialog = new Dialog();
                dialog.SetDialog("Error", "Please select a year level.");
                dialog.ShowDialog(Window.GetWindow(this));
            }
            else if (CmbSemesters.SelectedItem == null)
            {
                Dialog dialog = new Dialog();
                dialog.SetDialog("Error", "Please select a semester.");
                dialog.ShowDialog(Window.GetWindow(this));
            }
            else
            {
                if (selectedCourseSubjectId.HasValue)
                {
                    CourseSubject courseSubject = new CourseSubject();
                    if (courseSubject.UpdateCourseSubject(selectedCourseSubjectId.Value, (int)CmbSubjects.SelectedValue, (int)CmbCourses.SelectedValue, (int)CmbYearLevels.SelectedValue, (int)CmbSemesters.SelectedValue))
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Success", "Course subject updated successfully.");
                        dialog.ShowDialog(Window.GetWindow(this));
                        LoadCourseSubjects();
                        CmbSubjects.SelectedItem = null;
                        CmbCourses.SelectedItem = null;
                        CmbYearLevels.SelectedItem = null;
                        CmbSemesters.SelectedItem = null;
                        selectedCourseSubjectId = null;
                        BtnAdd.Content = "Add";
                    }
                    else
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Error", "Failed to update course subject.");
                        dialog.ShowDialog(Window.GetWindow(this));
                    }
                }
                else
                {
                    CourseSubject courseSubject = new CourseSubject();
                    if (courseSubject.AddCourseSubject((int)CmbSubjects.SelectedValue, (int)CmbCourses.SelectedValue, (int)CmbYearLevels.SelectedValue, (int)CmbSemesters.SelectedValue))
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Success", "Course subject added successfully.");
                        dialog.ShowDialog(Window.GetWindow(this));
                        LoadCourseSubjects();
                        CmbSubjects.SelectedItem = null;
                        CmbCourses.SelectedItem = null;
                        CmbYearLevels.SelectedItem = null;
                        CmbSemesters.SelectedItem = null;
                    }
                    else
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Error", "Course subject already exists.");
                        dialog.ShowDialog(Window.GetWindow(this));
                    }
                }
            }
        }

        private void SearchCourseSubjects(string search)
        {
            CourseSubject courseSubject = new CourseSubject();

            var formattedCourseSubjects = courseSubject.GetCourseSubjects().Select(cs => new
            {
                cs.Id,
                cs.SubjectId,
                cs.CourseId,
                cs.YearLevelId,
                cs.SemesterId,
                cs.SubjectCode,
                cs.SubjectName,
                cs.CourseName,
                cs.YearLevelName,
                cs.SemesterName
            }).ToList();

            var filteredCourseSubjects = formattedCourseSubjects.Where(cs => 
                cs.SubjectCode.ToLower().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                cs.SubjectName.ToLower().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                cs.CourseName.ToLower().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                cs.YearLevelName.ToLower().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                cs.SemesterName.ToLower().Contains(search, StringComparison.OrdinalIgnoreCase)
            ).ToList();

            DgCourseSubjects.ItemsSource = filteredCourseSubjects;
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchCourseSubjects(TxtSearch.Text);
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchCourseSubjects(TxtSearch.Text);
        }

        private void DeleteCourseSubject()
        {
            if (DgCourseSubjects.SelectedItem is CourseSubject selectedCourseSubject)
            {
                Prompt prompt = new Prompt();
                prompt.SetPrompt("Delete Course Subject", "Are you sure you want to delete this course subject?");
                // Create the event handler for the Confirm event, which is to call the DeleteCourseSubject method from the CourseSubject class
                prompt.Confirm += (s, e) =>
                {
                    CourseSubject courseSubject = new CourseSubject();
                    if (courseSubject.DeleteCourseSubject(selectedCourseSubject.Id))
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Success", "Course subject deleted successfully.");
                        dialog.ShowDialog(Window.GetWindow(this));
                        LoadCourseSubjects();
                    }
                    else
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Error", "Failed to delete course subject.");
                        dialog.ShowDialog(Window.GetWindow(this));
                    }
                };
                prompt.ShowPrompt(Window.GetWindow(this));
            }
        }

        private void DgCourseSubjects_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                DeleteCourseSubject();
            }
        }

        private void DgCourseSubjects_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DgCourseSubjects.SelectedItem is CourseSubject selectedCourseSubject)
            {
                selectedCourseSubjectId = selectedCourseSubject.Id;
                CmbSubjects.SelectedValue = selectedCourseSubject.SubjectId;
                CmbCourses.SelectedValue = selectedCourseSubject.CourseId;
                CmbYearLevels.SelectedValue = selectedCourseSubject.YearLevelId;
                CmbSemesters.SelectedValue = selectedCourseSubject.SemesterId;
                BtnAdd.Content = "Update";
            }
        }
    }
}
