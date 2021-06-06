using OrganizeIt.backend.social_gatherings;
using OrganizeIt.backend.users;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for OrganizerHomePage.xaml
    /// </summary>
    public partial class OrganizerHomePage : Page
    {
        public ObservableCollection<SocialGathering> requests { get; set; }

        public OrganizerHomePage()
        {
            List<SocialGathering> newGatheringsByOrganizer = new List<SocialGathering>();
            User loggedIn = backend.Backend.LoggedInUser;

            InitializeComponent();
        }

        private void RequestSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
