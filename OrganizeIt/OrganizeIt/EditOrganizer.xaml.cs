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

            //this.DataContext = User;

            this.City.Text = user.Address.City;
            this.Address.Text = user.Address.StreetAddress;
            this.Phone.Text = user.PhoneNumber;
            this.Firstname.Text = user.FirstName;
            this.Lastname.Text = user.LastName;
            this.Email.Text = user.Email;
            this.username.Text = user.Username;
            this.BirthDate.SelectedDate = user.BirthDate;

            if (this.User.Gender == Gender.Female)
                pol.SelectedItem = pol.Items.GetItemAt(0);
            else
                pol.SelectedItem = pol.Items.GetItemAt(1);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.User.Address.City = this.City.Text;
            this.User.Address.StreetAddress = this.Address.Text;
            this.User.PhoneNumber = this.Phone.Text;
            this.User.FirstName = this.Firstname.Text;
            this.User.LastName = this.Lastname.Text;
            BindingExpression binding = username.GetBindingExpression(TextBox.TextProperty);
            binding.UpdateSource();

            BindingExpression binding1 = Firstname.GetBindingExpression(TextBox.TextProperty);
            binding1.UpdateSource();

            BindingExpression binding2 = Lastname.GetBindingExpression(TextBox.TextProperty);
            binding2.UpdateSource();

            BindingExpression binding3 = Phone.GetBindingExpression(TextBox.TextProperty);
            binding3.UpdateSource();

            BindingExpression binding4 = Email.GetBindingExpression(TextBox.TextProperty);
            binding4.UpdateSource();

            BindingExpression binding5 = Address.GetBindingExpression(TextBox.TextProperty);
            binding5.UpdateSource();

            BindingExpression binding6 = City.GetBindingExpression(TextBox.TextProperty);
            binding6.UpdateSource();

            string messageBoxText = $"Da li ste sigurni da želite da izmenite korisnika {this.User.Username}?";
            string caption = "Izmena korisnika";
            MessageBoxButton btn = MessageBoxButton.YesNo;
            MessageBoxImage img = MessageBoxImage.Question;

            if (User.Username.Length < 5 || this.City.Text == "" || this.Address.Text == "" || User.Email == "" || User.PhoneNumber == "" || this.Phone.Text == ""
                || this.Firstname.Text == "" || this.Lastname.Text == "")
            {
                return;
            }
            var result = MessageBox.Show(messageBoxText, caption, btn, img, MessageBoxResult.No);
            if (result == MessageBoxResult.Yes)
            {
                int selectedItem = pol.SelectedIndex;
                if (selectedItem == 0)
                    this.User.Gender = Gender.Female;
                else
                    this.User.Gender = Gender.Male;

                backend.Backend.SaveUsers(this.allUsers);

            }
            NavigationService.Navigate(new AccountsList());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AccountsList());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ChangePassword cp = new ChangePassword(this.User);
            cp.Show();
        }
    }
}