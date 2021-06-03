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
    /// Interaction logic for EditOrganizer.xaml
    /// </summary>
    public partial class EditOrganizer : Page
    {
        public User User { get; set; }
        public Dictionary<string, User> allUsers { get; set; }
        public EditOrganizer(User user, Dictionary<string, User> allUsers)
        {
            this.allUsers = allUsers;
            this.User = user;

            InitializeComponent();
            
            this.DataContext = User;

            if(this.User.Gender == Gender.Female)
                pol.SelectedItem = pol.Items.GetItemAt(0);
            else
                pol.SelectedItem = pol.Items.GetItemAt(1);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int selectedItem = pol.SelectedIndex;
            if (selectedItem == 0)
                this.User.Gender = Gender.Female;
            else
                this.User.Gender = Gender.Male;

            backend.Backend.SaveUsers(this.allUsers);
            NavigationService.Navigate(new AccountsList());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AccountsList());
        }
    }
}
