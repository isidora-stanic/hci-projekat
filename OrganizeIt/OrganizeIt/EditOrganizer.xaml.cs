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
        public new String Name = "fsdsd";
        public User User = new User {
            UserType = UserType.Client,

        Username = "fsddfs",
        Password = "fdsa",

        FirstName = "fsd",
        LastName = "",

        Address = null,
        Gender = Gender.Male,
        PhoneNumber = "gd",
        Email = "asgs"
    };
        public EditOrganizer()
        {
            InitializeComponent();
            this.DataContext = User;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(User.Username);
        }
    }
}
