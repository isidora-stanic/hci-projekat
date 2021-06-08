using OrganizeIt.backend;
using OrganizeIt.backend.social_gatherings;
using OrganizeIt.backend.users;
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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        private Backend Backend;

        public Login()
        {
            InitializeComponent();
            Backend = new Backend();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, User> users = Backend.LoadUsers();
            User user = Backend.LogIn(this.username.Text, this.password.Password, users);
            if (user == null)
                NavigationService.Navigate(new CreateAccount());  //NavigationService.Navigate(new EditOrganizer());
            else if (user.UserType.Equals(UserType.Administrator))
                NavigationService.Navigate(new AccountsList());
            else if (user.UserType.Equals(UserType.Organizer))
                NavigationService.Navigate(new OrganizerHomePage());
            else if (user.UserType.Equals(UserType.Client))
                NavigationService.Navigate(new ManifestationList());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            backend.Backend.LoadAll();
            SocialGatheringSuggestionReply odgovor = backend.Backend.Users["jadranka88"].SocialGatheringSuggestionReplies[4];

            NavigationService.Navigate(new EventSuggReplyView(odgovor));
        }
    }
}