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
using System.Windows.Shapes;

namespace OrganizeIt
{
    /// <summary>
    /// Interaction logic for ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : Window
    {
        private User User;
        public ChangePassword(User user)
        {
            InitializeComponent();
            this.User = user;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.User.Password = this.password.Password;
            Dictionary<string, User> users = backend.Backend.LoadUsers();
            users[this.User.Username].Password = this.password.Password;
            backend.Backend.Users = users;
            backend.Backend.SaveUsers();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
