using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UNIS_Inspired_Enrollment_System.Classes;

namespace UNIS_Inspired_Enrollment_System.Pages
{
    /// <summary>
    /// Interaction logic for SubjectSchedulePage.xaml
    /// </summary>
    public partial class SubjectSchedulePage : Page
    {
        private int? selectedSubjectScheduleId = null;

        public SubjectSchedulePage()
        {
            InitializeComponent();

            LoadSubjectSchedules();
            LoadSubjects();
            LoadDays();
            LoadAcademicYears(); // Ensure this is called
        }

        public void LoadSubjectSchedules()
        {
            DgSubjectSchedules.ItemsSource = SubjectSchedule.GetSubjectSchedules();
        }

        public void LoadSubjects()
        {
            Subject subject = new Subject();
            CmbSubjects.ItemsSource = subject.GetSubjectsConnectedToCourses();
            CmbSubjects.DisplayMemberPath = "Name";
            CmbSubjects.SelectedValuePath = "Id";
        }

        public void LoadAcademicYears()
        {
            AcademicYear academicYear = new AcademicYear();
            CmbAcademicYears.ItemsSource = academicYear.GetAcademicYears();
            CmbAcademicYears.DisplayMemberPath = "Name";
            CmbAcademicYears.SelectedValuePath = "Id";
        }

        private void LoadDays()
        {
            List<string> days = new List<string> { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            CmbDays.ItemsSource = days;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (CmbSubjects.SelectedItem == null)
            {
                Dialog dialog = new Dialog();
                dialog.SetDialog("Error", "Please select a subject.");
                dialog.ShowDialog(Window.GetWindow(this));
            }
            else if (CmbDays.SelectedItem == null)
            {
                Dialog dialog = new Dialog();
                dialog.SetDialog("Error", "Please select a day.");
                dialog.ShowDialog(Window.GetWindow(this));
            }
            else if (CmbAcademicYears.SelectedItem == null)
            {
                Dialog dialog = new Dialog();
                dialog.SetDialog("Error", "Please select an academic year.");
                dialog.ShowDialog(Window.GetWindow(this));
            }
            else if (string.IsNullOrEmpty(TxtTimeStart.Text))
            {
                Dialog dialog = new Dialog();
                dialog.SetDialog("Error", "Please enter a start time.");
                dialog.ShowDialog(Window.GetWindow(this));
            }
            else if (string.IsNullOrEmpty(TxtTimeEnd.Text))
            {
                Dialog dialog = new Dialog();
                dialog.SetDialog("Error", "Please enter an end time.");
                dialog.ShowDialog(Window.GetWindow(this));
            }
            else
            {
                int academicYearId = (int)CmbAcademicYears.SelectedValue;
                if (selectedSubjectScheduleId.HasValue)
                {
                    if (SubjectSchedule.UpdateSubjectSchedule(selectedSubjectScheduleId.Value, (int)CmbSubjects.SelectedValue, academicYearId, CmbDays.SelectedItem.ToString(), TimeSpan.Parse(TxtTimeStart.Text), TimeSpan.Parse(TxtTimeEnd.Text)))
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Success", "Subject schedule updated successfully.");
                        dialog.ShowDialog(Window.GetWindow(this));
                        LoadSubjectSchedules();
                        ClearForm();
                    }
                    else
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Error", "Failed to update subject schedule.");
                        dialog.ShowDialog(Window.GetWindow(this));
                    }
                }
                else
                {
                    if (SubjectSchedule.AddSubjectSchedule((int)CmbSubjects.SelectedValue, academicYearId, CmbDays.SelectedItem.ToString(), TimeSpan.Parse(TxtTimeStart.Text), TimeSpan.Parse(TxtTimeEnd.Text)))
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Success", "Subject schedule added successfully.");
                        dialog.ShowDialog(Window.GetWindow(this));
                        LoadSubjectSchedules();
                        ClearForm();
                    }
                    else
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Error", "Failed to add subject schedule.");
                        dialog.ShowDialog(Window.GetWindow(this));
                    }
                }
            }
        }

        private void ClearForm()
        {
            CmbSubjects.SelectedItem = null;
            CmbDays.SelectedItem = null;
            CmbAcademicYears.SelectedItem = null; // Clear the selected academic year
            TxtTimeStart.Text = "";
            TxtTimeEnd.Text = "";
            selectedSubjectScheduleId = null;
            BtnAdd.Content = "Add";
        }

        public void SearchSubjectSchedules(string search)
        {
            var subjectSchedules = SubjectSchedule.GetSubjectSchedules();

            var formattedSubjectSchedules = subjectSchedules.Select(s => new
            {
                s.Id,
                s.SubjectCode,
                s.SubjectName,
                s.AcademicYearName,
                s.Day,
                s.TimeStart,
                s.TimeEnd
            }).ToList();

            var filteredSubjectSchedules = formattedSubjectSchedules.Where(s =>
                s.Id.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                s.SubjectCode.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                s.SubjectName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                s.AcademicYearName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                s.Day.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                s.TimeStart.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                s.TimeEnd.ToString().Contains(search, StringComparison.OrdinalIgnoreCase)
            ).ToList();

            DgSubjectSchedules.ItemsSource = filteredSubjectSchedules;
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchSubjectSchedules(TxtSearch.Text);
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchSubjectSchedules(TxtSearch.Text);
        }

        private void DeleteSubjectSchedule()
        {
            if (DgSubjectSchedules.SelectedItem is SubjectSchedule selectedSubjectSchedule)
            {
                Prompt prompt = new Prompt();
                prompt.SetPrompt("Delete Subject Schedule", "Are you sure you want to delete this subject schedule?");
                // Create the event handler for the Confirm event, which is to call the DeleteSubjectSchedule method from the SubjectSchedule class
                prompt.Confirm += (s, e) =>
                {
                    if (SubjectSchedule.DeleteSubjectSchedule(selectedSubjectSchedule.Id))
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Success", "Subject schedule deleted successfully.");
                        dialog.ShowDialog(Window.GetWindow(this));
                        LoadSubjectSchedules();
                    }
                    else
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Error", "Failed to delete subject schedule.");
                        dialog.ShowDialog(Window.GetWindow(this));
                    }
                };
                prompt.ShowPrompt(Window.GetWindow(this));
            }
            else
            {
                Dialog dialog = new Dialog();
                dialog.SetDialog("Error", "Please select a subject schedule.");
                dialog.ShowDialog(Window.GetWindow(this));
            }
        }

        private void DgSubjectSchedules_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                DeleteSubjectSchedule();
            }
        }

        private void DgSubjectSchedules_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DgSubjectSchedules.SelectedItem is SubjectSchedule selectedSubjectSchedule)
            {
                CmbSubjects.SelectedValue = selectedSubjectSchedule.SubjectId;
                CmbDays.SelectedItem = selectedSubjectSchedule.Day;
                CmbAcademicYears.SelectedValue = selectedSubjectSchedule.AcademicYearId; // Set the academic year
                TxtTimeStart.Text = selectedSubjectSchedule.TimeStart.ToString();
                TxtTimeEnd.Text = selectedSubjectSchedule.TimeEnd.ToString();
                selectedSubjectScheduleId = selectedSubjectSchedule.Id;
                BtnAdd.Content = "Update";
            }
        }
    }
}
