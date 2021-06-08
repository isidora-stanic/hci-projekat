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
    /// Interaction logic for NewOrganizer.xaml
    /// </summary>
    public partial class NewOrganizer : Page
    {
        public NewOrganizer()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            backend.Backend bb = new backend.Backend();

            BindingExpression binding = username.GetBindingExpression(TextBox.TextProperty);
            binding.UpdateSource();

            if (this.username.Text == "" || this.password.Password == "" 
                || this.name.Text == "" || this.lastname.Text == "" || this.phone.Text == "" || this.email.Text == "")
            {
                //this.username.BorderBrush = Brushes.Red;
                //this.username.Text = "";
                //this.username.Foreground = Brushes.Red;
                //MessageBox.Show("Nijedno polje ne smije biti null");
                return;
            }
            User user = new User();
            user.Username = this.username.Text;
            user.Password = this.password.Password;
            user.FirstName = this.name.Text;
            user.LastName = this.lastname.Text;
            user.PhoneNumber = this.phone.Text;
            user.Email = this.email.Text;

            user.Address = new Location();
            user.Address.City = this.city.Text;
            user.Address.StreetAddress = this.address.Text;
            user.BirthDate = DateTime.Parse(this.date.Text);

            if (this.gender.SelectedIndex == 0)
                user.Gender = Gender.Female;
            else
                user.Gender = Gender.Male;

            var allUsers = backend.Backend.LoadUsers();
            allUsers.Add(user.Username, user);
            backend.Backend.SaveUsers(allUsers);

            //this.username.BorderBrush = Brushes.Red;
            //this.username.Text = "";
            //this.username.Foreground = Brushes.Red;

        }
    }
}
