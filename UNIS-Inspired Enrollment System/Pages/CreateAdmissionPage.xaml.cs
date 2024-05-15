using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for CreateAdmissionPage.xaml
    /// </summary>
    public partial class CreateAdmissionPage : Page
    {
        private int? selectedAdmissionId = null;

        public CreateAdmissionPage()
        {
            InitializeComponent();

            LoadAcademicYears();
            LoadCourses();
            LoadYearLevels();
            LoadAdmissionTypes();
            LoadSemesters();
            LoadGenders();
        }

        public CreateAdmissionPage(Admission admission) : this()
        {
            if (admission != null)
            {
                PopulateFields(admission);
            }
        }

        private void PopulateFields(Admission admission)
        {
            selectedAdmissionId = admission.AdmissionID;
            CmbAcademicYears.SelectedValue = admission.AcademicYearId;
            DpAdmissionDate.SelectedDate = admission.AdmissionDate;
            CmbCourses.SelectedValue = admission.CourseId;
            CmbYearLevels.SelectedValue = admission.YearLevelId;
            CmbAdmissionTypes.SelectedValue = admission.AdmissionTypeId;
            CmbSemesters.SelectedValue = admission.SemesterId;
            TxtFirstName.Text = admission.FirstName;
            TxtMiddleName.Text = admission.MiddleName;
            TxtLastName.Text = admission.LastName;
            CmbGenders.SelectedValue = admission.Gender;
            DpDateOfBirth.SelectedDate = admission.DateOfBirth;
            TxtMobileNo.Text = admission.MobileNo;
            TxtEmail.Text = admission.Email;
            TxtCity.Text = admission.City;
            TxtState.Text = admission.State;
            TxtPermanentAddress.Text = admission.PermanentAddress;
            TxtCurrentAddress.Text = admission.CurrentAddress;
            BtnAdd.Content = "Update";
        }

        private void LoadAcademicYears()
        {
            AcademicYear academicYear = new AcademicYear();
            CmbAcademicYears.ItemsSource = academicYear.GetAcademicYears();
            CmbAcademicYears.DisplayMemberPath = "Name";
            CmbAcademicYears.SelectedValuePath = "Id";
        }

        private void LoadCourses()
        {
            Course course = new Course();
            CmbCourses.ItemsSource = course.GetCourses();
            CmbCourses.DisplayMemberPath = "Name";
            CmbCourses.SelectedValuePath = "Id";
        }

        private void LoadYearLevels()
        {
            YearLevel yearLevel = new YearLevel();
            CmbYearLevels.ItemsSource = yearLevel.GetYearLevels();
            CmbYearLevels.DisplayMemberPath = "Name";
            CmbYearLevels.SelectedValuePath = "Id";
        }

        private void LoadAdmissionTypes()
        {
            AdmissionType admissionType = new AdmissionType();
            CmbAdmissionTypes.ItemsSource = admissionType.GetAdmissionTypes();
            CmbAdmissionTypes.DisplayMemberPath = "Name";
            CmbAdmissionTypes.SelectedValuePath = "Id";
        }

        private void LoadSemesters()
        {
            Semester semester = new Semester();
            CmbSemesters.ItemsSource = semester.GetSemesters();
            CmbSemesters.DisplayMemberPath = "Name";
            CmbSemesters.SelectedValuePath = "Id";
        }

        private void LoadGenders()
        {
            var genders = new List<KeyValuePair<int, string>>
            {
                new KeyValuePair<int, string>(0, "Male"),
                new KeyValuePair<int, string>(1, "Female")
            };

            CmbGenders.ItemsSource = genders;
            CmbGenders.DisplayMemberPath = "Value";
            CmbGenders.SelectedValuePath = "Key";
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (CmbAcademicYears.SelectedItem == null)
            {
                ShowDialog("Error", "Please select an academic year.");
            }
            else if (DpAdmissionDate.SelectedDate == null)
            {
                ShowDialog("Error", "Please select an admission date.");
            }
            else if (CmbCourses.SelectedItem == null)
            {
                ShowDialog("Error", "Please select a course.");
            }
            else if (CmbYearLevels.SelectedItem == null)
            {
                ShowDialog("Error", "Please select a year level.");
            }
            else if (CmbAdmissionTypes.SelectedItem == null)
            {
                ShowDialog("Error", "Please select an admission type.");
            }
            else if (CmbSemesters.SelectedItem == null)
            {
                ShowDialog("Error", "Please select a semester.");
            }
            else if (string.IsNullOrEmpty(TxtFirstName.Text))
            {
                ShowDialog("Error", "Please enter a first name.");
            }
            else if (string.IsNullOrEmpty(TxtMiddleName.Text))
            {
                ShowDialog("Error", "Please enter a middle name.");
            }
            else if (string.IsNullOrEmpty(TxtLastName.Text))
            {
                ShowDialog("Error", "Please enter a last name.");
            }
            else if (CmbGenders.SelectedItem == null)
            {
                ShowDialog("Error", "Please select a gender.");
            }
            else if (DpDateOfBirth.SelectedDate == null)
            {
                ShowDialog("Error", "Please select a date of birth.");
            }
            else if (string.IsNullOrEmpty(TxtMobileNo.Text))
            {
                ShowDialog("Error", "Please enter a mobile number.");
            }
            else if (string.IsNullOrEmpty(TxtEmail.Text))
            {
                ShowDialog("Error", "Please enter an email address.");
            }
            else if (string.IsNullOrEmpty(TxtCity.Text))
            {
                ShowDialog("Error", "Please enter a city.");
            }
            else if (string.IsNullOrEmpty(TxtState.Text))
            {
                ShowDialog("Error", "Please enter a state.");
            }
            else if (string.IsNullOrEmpty(TxtPermanentAddress.Text))
            {
                ShowDialog("Error", "Please enter a permanent address.");
            }
            else if (string.IsNullOrEmpty(TxtCurrentAddress.Text))
            {
                ShowDialog("Error", "Please enter a current address.");
            }
            else
            {
                Admission admission = new Admission
                {
                    AdmissionID = selectedAdmissionId ?? 0, // Use 0 for new admission
                    AcademicYearId = (int)CmbAcademicYears.SelectedValue,
                    AdmissionDate = DpAdmissionDate.SelectedDate.Value,
                    CourseId = (int)CmbCourses.SelectedValue,
                    YearLevelId = (int)CmbYearLevels.SelectedValue,
                    AdmissionTypeId = (int)CmbAdmissionTypes.SelectedValue,
                    SemesterId = (int)CmbSemesters.SelectedValue,
                    FirstName = TxtFirstName.Text,
                    MiddleName = TxtMiddleName.Text,
                    LastName = TxtLastName.Text,
                    Gender = (int)CmbGenders.SelectedValue,
                    DateOfBirth = DpDateOfBirth.SelectedDate.Value,
                    MobileNo = TxtMobileNo.Text,
                    Email = TxtEmail.Text,
                    City = TxtCity.Text,
                    State = TxtState.Text,
                    PermanentAddress = TxtPermanentAddress.Text,
                    CurrentAddress = TxtCurrentAddress.Text
                };

                bool isSuccess;
                if (selectedAdmissionId.HasValue)
                {
                    isSuccess = admission.UpdateAdmission();
                    ShowDialog(isSuccess ? "Success" : "Error", isSuccess ? "Admission updated successfully." : "Failed to update admission.");
                }
                else
                {
                    isSuccess = admission.AddAdmission();
                    ShowDialog(isSuccess ? "Success" : "Error", isSuccess ? "Admission added successfully." : "Failed to add admission.");
                }

                if (isSuccess)
                {
                    ClearFields();
                    selectedAdmissionId = null;
                    BtnAdd.Content = "Add";
                }
            }
        }

        private void ShowDialog(string title, string message)
        {
            Dialog dialog = new Dialog();
            dialog.SetDialog(title, message);
            dialog.ShowDialog(Window.GetWindow(this));
        }

        private void ClearFields()
        {
            CmbAcademicYears.SelectedItem = null;
            DpAdmissionDate.SelectedDate = null;
            CmbCourses.SelectedItem = null;
            CmbYearLevels.SelectedItem = null;
            CmbAdmissionTypes.SelectedItem = null;
            CmbSemesters.SelectedItem = null;
            TxtFirstName.Text = "";
            TxtMiddleName.Text = "";
            TxtLastName.Text = "";
            CmbGenders.SelectedItem = null;
            DpDateOfBirth.SelectedDate = null;
            TxtMobileNo.Text = "";
            TxtEmail.Text = "";
            TxtCity.Text = "";
            TxtState.Text = "";
            TxtPermanentAddress.Text = "";
            TxtCurrentAddress.Text = "";
        }
    }
}
