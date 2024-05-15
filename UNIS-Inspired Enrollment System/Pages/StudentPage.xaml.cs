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
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.IO;
using PdfSharp.Fonts;

namespace UNIS_Inspired_Enrollment_System.Pages
{
    /// <summary>
    /// Interaction logic for StudentPage.xaml
    /// </summary>
    public partial class StudentPage : Page
    {
        static StudentPage()
        {
            // Register the custom font resolver once
            GlobalFontSettings.FontResolver = new FileFontResolver();
        }

        public StudentPage()
        {
            InitializeComponent();

            LoadStudents();
        }

        private void FetchAndGeneratePDF(Admission selectedAdmission)
        {
            // Fetch subject schedules for the selected admission
            List<SubjectSchedule> subjectSchedules = SubjectSchedule.GetSubjectSchedulesBySemesterAndAcademicYear(selectedAdmission.SemesterName, selectedAdmission.AcademicYearName);

            // Generate PDF
            GeneratePDF(selectedAdmission, subjectSchedules);
        }

        public class FileFontResolver : IFontResolver // FontResolverBase
        {
            public string DefaultFontName => throw new NotImplementedException();

            public byte[] GetFont(string faceName)
            {
                using (var ms = new MemoryStream())
                {
                    using (var fs = File.Open(faceName, FileMode.Open))
                    {
                        fs.CopyTo(ms);
                        ms.Position = 0;
                        return ms.ToArray();
                    }
                }
            }

            public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
            {
                if (familyName.Equals("Cormorant Garamond", StringComparison.CurrentCultureIgnoreCase))
                {
                    return new FontResolverInfo("C:\\Users\\DAN\\source\\repos\\UNIS-Inspired Enrollment System\\UNIS-Inspired Enrollment System\\Fonts\\CormorantGaramond-Regular.ttf");
                }
                return null;
            }
        }

        private void OpenPDF(string filePath)
        {
            try
            {
                // Use ProcessStartInfo to specify the file to open and the working directory
                var processStartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true,
                    WorkingDirectory = System.IO.Path.GetDirectoryName(filePath) // Fully qualified
                };
                System.Diagnostics.Process.Start(processStartInfo);
            }
            catch (Exception ex)
            {
                // Handle the exception if the PDF cannot be opened
                MessageBox.Show($"An error occurred while trying to open the PDF file: {ex.Message}");
            }
        }

        private void GeneratePDF(Admission admission, List<SubjectSchedule> subjectSchedules)
        {
            string filePath = "SubjectSchedules.pdf";

            // Create a new PDF document
            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();

            // Create a font using Cormorant Garamond
            XFont font = new XFont("Cormorant Garamond", 12);

            // Create a graphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Set the initial Y position for drawing
            double y = 20;

            // Add student name and admission details to the document
            string studentInfo = $"Student Name: {admission.FirstName} {admission.LastName}\n" +
                                 $"Course: {admission.CourseName}\n" +
                                 $"Year Level: {admission.YearLevelName}\n" +
                                 $"Academic Year: {admission.AcademicYearName}\n" +
                                 $"Semester: {admission.SemesterName}\n";

            gfx.DrawString(studentInfo, font, XBrushes.Black, 20, y);
            y += 80; // Adjust the Y position to add space after the student info

            // Add subject schedule details to the document
            foreach (SubjectSchedule schedule in subjectSchedules)
            {
                string scheduleInfo = $"{schedule.Day}: {schedule.SubjectCode} - {schedule.SubjectName} ({schedule.TimeStart:hh\\:mm} - {schedule.TimeEnd:hh\\:mm})";
                gfx.DrawString(scheduleInfo, font, XBrushes.Black, 20, y);
                y += 20;
            }

            // Save the document to a file
            document.Save(filePath);

            // Open the PDF in the default PDF viewer
            OpenPDF(filePath);
        }

        private void LoadStudents()
        {
            Admission admission = new Admission();
            DgListOfStudents.ItemsSource = admission.GetAdmissions();
        }

        private void SearchStudents(string search)
        {
            Admission admission = new Admission();

            var filteredAdmissions = admission.GetAdmissions().Where(a =>
                a.AdmissionID.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                a.AcademicYearName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                a.AdmissionDate.ToString("d").Contains(search, StringComparison.OrdinalIgnoreCase) ||
                a.CourseName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                a.YearLevelName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                a.AdmissionTypeName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                a.SemesterName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                a.FirstName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                a.MiddleName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                a.LastName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                a.GenderName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                a.DateOfBirth.ToString("d").Contains(search, StringComparison.OrdinalIgnoreCase) ||
                a.MobileNo.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                a.Email.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                a.City.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                a.State.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                a.PermanentAddress.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                a.CurrentAddress.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

            DgListOfStudents.ItemsSource = filteredAdmissions;
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchStudents(TxtSearch.Text);
        }

        // Create the method to delete a student and his/her admission by calling the deleteadmission in the admission class
        private void DeleteStudent()
        {
            if (DgListOfStudents.SelectedItem is Admission selectedAdmission)
            {
                Prompt prompt = new Prompt();
                prompt.SetPrompt("Delete Student", "Are you sure you want to delete this student?");
                // Create the event handler for the Confirm event, which is to call the DeleteAdmission method from the Admission class
                prompt.Confirm += (s, e) =>
                {
                    Admission admission = new Admission();
                    if (admission.DeleteAdmission(selectedAdmission.AdmissionID))
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Success", "Student deleted successfully.");
                        dialog.ShowDialog(Window.GetWindow(this));
                        LoadStudents();
                    }
                    else
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Error", "Failed to delete student.");
                        dialog.ShowDialog(Window.GetWindow(this));
                    }
                };
                prompt.ShowPrompt(Window.GetWindow(this));
            }
        }

        private void DgListOfStudents_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                DeleteStudent();
            }

            if (e.Key == Key.P && Keyboard.Modifiers == (ModifierKeys.Control | ModifierKeys.Shift))
            {
                var selectedAdmission = DgListOfStudents.SelectedItem as Admission;
                if (selectedAdmission != null)
                {
                    // Fetch and generate PDF
                    FetchAndGeneratePDF(selectedAdmission);
                }
            }
        }

        private void DgListOfStudents_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = DgListOfStudents.SelectedItem as Admission;
            if (selectedItem != null)
            {
                AdmissionWindow admissionWindow = new AdmissionWindow();
                admissionWindow.Show();

                var frame = (Frame)admissionWindow.FindName("Page");
                frame.Navigate(new CreateAdmissionPage(selectedItem));
            }
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchStudents(TxtSearch.Text);
        }
    }
}
