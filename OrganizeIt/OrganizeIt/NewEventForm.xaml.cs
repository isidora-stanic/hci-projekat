using Microsoft.Win32;
using OrganizeIt.backend.social_gatherings;
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

namespace OrganizeIt
{
    /// <summary>
    /// Interaction logic for NewEventForm.xaml
    /// </summary>
    public partial class NewEventForm : Page
    {
        public string filename { get; set; } = "";
        public SocialGathering Proslava { get; set; }
        public NewEventForm()
        {
            InitializeComponent();
            Proslava = new SocialGathering
            {
                AcceptedSuggestions = false,
                Client = backend.Backend.LoggedInUser,
                ClientUsername = backend.Backend.LoggedInUser.Username,
                RequestDate = DateTime.Now,
                DateTime = DateTime.Now,
                Description = "",
                Name = "",
                GuestList = new List<string>(),
                NumberOfGuests = 0,
                SocialGatheringSuggestions = new List<SocialGatheringSuggestion>(),
                Type = ""
            };
            // Organizer = null, OrganizerUsername = "",
            DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Proslava.Name.Length == 0)
                MessageBox.Show("Napisite naslov.");
            else
                NavigationService.Navigate(new OrganizersCheckList(Proslava));
        }

        private void CSVButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.FileName = "Document"; // Default file name
            dialog.DefaultExt = ".csv"; // Default file extension
            dialog.Filter = "Text documents (.csv)|*.csv"; // Filter files by extension
                                                           // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                filename = dialog.FileName;
                filenameTextBox.Text = filename;
            }
        }
    }
}
