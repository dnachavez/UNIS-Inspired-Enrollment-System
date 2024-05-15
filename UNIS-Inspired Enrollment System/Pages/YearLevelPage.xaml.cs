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
    /// Interaction logic for YearLevelPage.xaml
    /// </summary>
    public partial class YearLevelPage : Page
    {
        private int? selectedYearLevelId = null;

        public YearLevelPage()
        {
            InitializeComponent();

            LoadYearLevels();
        }

        public void LoadYearLevels()
        {
            YearLevel yearLevel = new YearLevel();
            DgYearLevels.ItemsSource = yearLevel.GetYearLevels();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtYearLevel.Text))
            {
                Dialog dialog = new Dialog();
                dialog.SetDialog("Error", "Please enter a year level name.");
                dialog.ShowDialog(Window.GetWindow(this));
            }
            else
            {
                if (selectedYearLevelId.HasValue)
                {
                    YearLevel yearLevel = new YearLevel();
                    if (yearLevel.UpdateYearLevel(selectedYearLevelId.Value, TxtYearLevel.Text))
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Success", "Year level updated successfully.");
                        dialog.ShowDialog(Window.GetWindow(this));
                        LoadYearLevels();
                        TxtYearLevel.Text = "";
                        selectedYearLevelId = null;
                        BtnAdd.Content = "Add";
                    }
                    else
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Error", "Year level already exists.");
                        dialog.ShowDialog(Window.GetWindow(this));
                    }
                }
                else
                {
                    YearLevel yearLevel = new YearLevel();
                    if (yearLevel.AddYearLevel(TxtYearLevel.Text))
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Success", "Year level added successfully.");
                        dialog.ShowDialog(Window.GetWindow(this));
                        LoadYearLevels();
                        TxtYearLevel.Text = "";
                    }
                    else
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Error", "Year level already exists.");
                        dialog.ShowDialog(Window.GetWindow(this));
                    }
                }
            }
        }

        public void SearchYearLevels(string search)
        {
            YearLevel yearLevel = new YearLevel();

            var formattedYearLevels = yearLevel.GetYearLevels().Select(yl => new
            {
                yl.Id,
                yl.Name
            }).ToList();

            var filteredYearLevels = formattedYearLevels.Where(yl => 
                yl.Id.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                yl.Name.Contains(search, StringComparison.OrdinalIgnoreCase)
            ).ToList();

            DgYearLevels.ItemsSource = filteredYearLevels;
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchYearLevels(TxtSearch.Text);
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchYearLevels(TxtSearch.Text);
        }

        public void DeleteYearLevel()
        {
            if (DgYearLevels.SelectedItem is YearLevel selectedYearLevel)
            {
                Prompt prompt = new Prompt();
                prompt.SetPrompt("Delete Year Level", "Are you sure you want to delete this year level?");
                // Create the event handler for the Confirm event, which is to call the DeleteYearLevel method from the YearLevel class
                prompt.Confirm += (s, e) =>
                {
                    YearLevel yearLevel = new YearLevel();
                    if (yearLevel.DeleteYearLevel(selectedYearLevel.Id))
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Success", "Year level deleted successfully.");
                        dialog.ShowDialog(Window.GetWindow(this));
                        LoadYearLevels();
                    }
                    else
                    {
                        Dialog dialog = new Dialog();
                        dialog.SetDialog("Error", "Failed to delete year level.");
                        dialog.ShowDialog(Window.GetWindow(this));
                    }
                };
                prompt.ShowPrompt(Window.GetWindow(this));
            }
        }

        private void DgYearLevels_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                DeleteYearLevel();
            }
        }

        private void DgYearLevels_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DgYearLevels.SelectedItem is YearLevel selectedYearLevel)
            {
                TxtYearLevel.Text = selectedYearLevel.Name;
                selectedYearLevelId = selectedYearLevel.Id;
                BtnAdd.Content = "Update";
            }
        }
    }
}
