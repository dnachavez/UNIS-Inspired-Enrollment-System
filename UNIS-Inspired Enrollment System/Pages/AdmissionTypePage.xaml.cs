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
    /// Interaction logic for AdmissionTypePage.xaml
    /// </summary>
    public partial class AdmissionTypePage : Page
    {
        private int? selectedAdmissionTypeId = null;

        public AdmissionTypePage()
        {
            InitializeComponent();

            LoadAdmissionTypes();
        }

        public void LoadAdmissionTypes()
        {
            AdmissionType admissionType = new AdmissionType();
            DgAdmissionTypes.ItemsSource = admissionType.GetAdmissionTypes();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtAdmissionType.Text))
            {
                Dialog dialog = new Dialog();
                dialog.SetDialog("Error", "Please enter an admission type.");
                dialog.ShowDialog(Window.GetWindow(this));
            }
            else
            {
                if (selectedAdmissionTypeId.HasValue)
                {
                    AdmissionType admissionType = new AdmissionType();
                    if (admissionType.UpdateAdmissionType(selectedAdmissionTypeId.Value, TxtAdmissionType.Text))
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Success", "Admission type updated successfully.");
                        dialog.ShowDialog(Window.GetWindow(this));
                        LoadAdmissionTypes();
                        TxtAdmissionType.Text = "";
                        selectedAdmissionTypeId = null;
                        BtnAdd.Content = "Add";
                    }
                    else
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Error", "Failed to update admission type.");
                        dialog.ShowDialog(Window.GetWindow(this));
                    }
                }
                else
                {
                    AdmissionType admissionType = new AdmissionType();
                    if (admissionType.AddAdmissionType(TxtAdmissionType.Text))
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Success", "Admission type added successfully.");
                        dialog.ShowDialog(Window.GetWindow(this));
                        LoadAdmissionTypes();
                        TxtAdmissionType.Text = "";
                    }
                    else
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Error", "Failed to add admission type.");
                        dialog.ShowDialog(Window.GetWindow(this));
                    }
                }
            }
        }

        private void SearchAdmissionTypes(string search)
        {
            AdmissionType admissionType = new AdmissionType();

            var formattedAdmissionTypes = admissionType.GetAdmissionTypes().Select(ay => new
            {
                ay.Id,
                ay.Name
            }).ToList();

            var filteredAdmissionTypes = formattedAdmissionTypes.Where(ay => 
                ay.Id.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                ay.Name.Contains(search, StringComparison.OrdinalIgnoreCase)
            ).ToList();

            DgAdmissionTypes.ItemsSource = filteredAdmissionTypes;
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchAdmissionTypes(TxtSearch.Text);
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchAdmissionTypes(TxtSearch.Text);
        }

        private void DeleteAdmissionType()
        {
            if (DgAdmissionTypes.SelectedItem is AdmissionType selectedType)
            {
                Prompt prompt = new Prompt();
                prompt.SetPrompt("Delete Admission Type", "Are you sure you want to delete this admission type?");
                // Create the event handler for the Confirm event, which is to call the DeleteAdmissionType method from the AdmissionType class
                prompt.Confirm += (s, e) =>
                {
                    AdmissionType admissionType = new AdmissionType();
                    if (admissionType.DeleteAdmissionType(selectedType.Id))
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Success", "Admission type deleted successfully.");
                        dialog.ShowDialog(Window.GetWindow(this));
                        LoadAdmissionTypes();
                    }
                    else
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Error", "Failed to delete admission type.");
                        dialog.ShowDialog(Window.GetWindow(this));
                    }
                };
                prompt.ShowPrompt(Window.GetWindow(this));
            }
            else
            {
                Dialog dialog = new Dialog();
                dialog.SetDialog("Error", "Please select an admission type to delete.");
                dialog.ShowDialog(Window.GetWindow(this));
            }
        }

        private void DgAdmissionTypes_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                DeleteAdmissionType();
            }
        }

        private void DgAdmissionTypes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AdmissionType admissionType = (AdmissionType)DgAdmissionTypes.SelectedItem;
            if (admissionType != null)
            {
                TxtAdmissionType.Text = admissionType.Name;
                selectedAdmissionTypeId = admissionType.Id;
                BtnAdd.Content = "Update";
            }
        }
    }
}
