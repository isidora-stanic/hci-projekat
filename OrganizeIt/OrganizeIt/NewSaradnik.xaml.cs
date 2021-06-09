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
    /// Interaction logic for NewSaradnik.xaml
    /// </summary>
    public partial class NewSaradnik : Page, Saveable
    {
        public NewSaradnik()
        {
            this.DataContext = new RegistrationVM(this);
            InitializeComponent();
        }

        public void SaveCommand()
        {
            if (this.username.Text == "")
            {
                BindingExpression binding = username.GetBindingExpression(TextBox.TextProperty);
                binding.UpdateSource();
                return;
            }

            string messageBoxText = $"Da li ste sigurni da želite da dodate novog saradnika?";
            string caption = "Dodavanje saradnika";
            MessageBoxButton btn = MessageBoxButton.YesNo;
            MessageBoxImage img = MessageBoxImage.Question;


            var result = MessageBox.Show(messageBoxText, caption, btn, img, MessageBoxResult.No);
            if (result == MessageBoxResult.No)
                return;

            SocialGatheringCollaborator sgc = new SocialGatheringCollaborator();
            sgc.Description = this.description.Text;
            sgc.Name = this.username.Text;

            Dictionary<int, SocialGatheringCollaborator> collaborators = backend.Backend.LoadCollaborators();
            sgc.Id = collaborators.Last().Key + 1;
            collaborators.Add(sgc.Id, sgc);

            backend.Backend.Collaborators = collaborators;

            backend.Backend.SaveCollaborators();

            if (backend.Backend.LoggedInUser.UserType == backend.users.UserType.Administrator)
                NavigationService.Navigate(new AccountsList());
            else
                NavigationService.Navigate(new OrganizerHomePage());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SaveCommand();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (backend.Backend.LoggedInUser.UserType == backend.users.UserType.Administrator)
                NavigationService.Navigate(new AccountsList());
            else
                NavigationService.Navigate(new OrganizerHomePage());
        }
    }
}