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
    public partial class NewOrganizer : Page, Saveable
    {
        private Boolean IsClient { get; set; }

        public NewOrganizer(Boolean isClient)
        {
            this.DataContext = new RegistrationVM(this);
            InitializeComponent();
            this.IsClient = isClient;
            if (this.IsClient)
                this.Btn2.Content = "Dodaj korisnika";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SaveCommand();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AccountsList());
        }

        public void SaveCommand()
        {
            backend.Backend bb = new backend.Backend();

            BindingExpression binding = username.GetBindingExpression(TextBox.TextProperty);
            binding.UpdateSource();

            BindingExpression binding1 = name.GetBindingExpression(TextBox.TextProperty);
            binding1.UpdateSource();

            BindingExpression binding2 = lastname.GetBindingExpression(TextBox.TextProperty);
            binding2.UpdateSource();

            BindingExpression binding3 = phone.GetBindingExpression(TextBox.TextProperty);
            binding3.UpdateSource();

            BindingExpression binding4 = email.GetBindingExpression(TextBox.TextProperty);
            binding4.UpdateSource();

            BindingExpression binding5 = address.GetBindingExpression(TextBox.TextProperty);
            binding5.UpdateSource();

            BindingExpression binding6 = city.GetBindingExpression(TextBox.TextProperty);
            binding6.UpdateSource();

            BindingExpression binding7 = date.GetBindingExpression(DatePicker.SelectedDateProperty);
            binding7.UpdateSource();

            BindingExpression binding8 = password.GetBindingExpression(PasswordBoxAssistant.BoundPassword);
            binding8.UpdateSource();

            try { DateTime oDate = Convert.ToDateTime(this.date.Text); }
            catch (Exception) { return; }

            if (this.username.Text == "" || this.password.Password == "" || this.address.Text == "" || this.city.Text == ""
                || this.name.Text == "" || this.lastname.Text == "" || this.phone.Text == "" || this.email.Text == "")
            {
                return;
            }

            string messageBoxText = $"Da li ste sigurni da želite da dodate novog korisnika?";
            string caption = "Dodavanje korisnika";
            MessageBoxButton btn = MessageBoxButton.YesNo;
            MessageBoxImage img = MessageBoxImage.Question;


            var result = MessageBox.Show(messageBoxText, caption, btn, img, MessageBoxResult.No);
            if (result == MessageBoxResult.No)
                return;

            User user = new User();
            if (this.IsClient)
                user.UserType = UserType.Client;
            else
                user.UserType = UserType.Organizer;
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
            NavigationService.Navigate(new AccountsList());
        }
    }
}
