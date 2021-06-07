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
    /// Interaction logic for OrganizersCheckList.xaml
    /// </summary>

    public partial class OrganizersCheckList : Page
    {
        public ObservableCollection<User> organizers = new ObservableCollection<User>();
        public List<OrganizatorDTO> organizatori = new List<OrganizatorDTO>();
        public Dictionary<string, User> allUsers { get; set; }
        public SocialGathering Proslava { get; set; }
        public User odabranOrganizator { get; set; }

        public OrganizersCheckList(SocialGathering proslava)
        {
            InitializeComponent();
            this.DataContext = this;
            Proslava = proslava;
            allUsers = backend.Backend.LoadUsers();
            backend.Backend.loadSocialGatherings(allUsers);
            organizers = new ObservableCollection<User>(allUsers.Values.Where(user => user.UserType == UserType.Organizer));

            var selektovanOrganizator = Proslava.Organizer;
            if (selektovanOrganizator != null)
            {
                foreach (User o in organizers)
                {
                    OrganizatorDTO dto;
                    if (o.Username.Equals(selektovanOrganizator.Username))
                        dto = new OrganizatorDTO { Organizator = o, SlikaPutanja = backend.Backend.GetUserImagePath(o.Username), IsSelected = true };
                    else
                        dto = new OrganizatorDTO { Organizator = o, SlikaPutanja = backend.Backend.GetUserImagePath(o.Username), IsSelected = false };
                    organizatori.Add(dto);
                }
            }
            else 
            {
                foreach (User o in organizers)
                {
                    var dto = new OrganizatorDTO { Organizator = o, SlikaPutanja = backend.Backend.GetUserImagePath(o.Username), IsSelected = false };
                    organizatori.Add(dto);
                }
            }

            OrganizerListView.ItemsSource = organizatori;
        }

        /* Funkcija za promenu teksta pri pretrazi organizatora */
        private void OrganizerSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchOrganizers(OrganizerSearch.Text);
        }

        /* Ova funkcija se poziva pri pozivu pretrage */
        public void SearchOrganizers(string query)
        {
            var filteredOrganizers =
                from organizer in organizatori
                where organizer.Organizator.FirstName.ToUpper().Contains(query.ToUpper().Trim())
                    || organizer.Organizator.LastName.ToUpper().Contains(query.ToUpper().Trim())
                    || organizer.Organizator.Email.ToUpper().Contains(query.ToUpper().Trim())
                    || organizer.Organizator.Username.ToUpper().Contains(query.ToUpper().Trim())
                select organizer;

            List<OrganizatorDTO> filteredOrganizersObservable = new List<OrganizatorDTO>(filteredOrganizers);

            OrganizerListView.ItemsSource = filteredOrganizersObservable;
        }

        private void LogoutIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string messageBoxText = $"Da li zelite da se odjavite";
            string caption = "Odjava";
            MessageBoxButton btn = MessageBoxButton.YesNo;
            MessageBoxImage img = MessageBoxImage.Question;

            var result = MessageBox.Show(messageBoxText, caption, btn, img, MessageBoxResult.No);
            if (result == MessageBoxResult.No)
                return;

            backend.Backend.LoggedInUser = null;
            NavigationService.Navigate(new Login());
        }

        private void CheckBox_MouseDown(object sender, RoutedEventArgs e)
        {
            RadioButton check = (RadioButton)sender;
            User kliknutiOrganizator = ((OrganizatorDTO)check.DataContext).Organizator;
            NadjiOrganizatoraIPostaviMuBool(kliknutiOrganizator, (bool)check.IsChecked);
        }

        private void NadjiOrganizatoraIPostaviMuBool(User org, bool selektovan)
        {
            if (selektovan)
            {
                odabranOrganizator = org; // ovo je dobro
                // ovo dole ne radi kako treba
                foreach (var orgDto in organizatori)
                {
                    
                    if (orgDto.Organizator.Username.Equals(org.Username))
                    {
                        orgDto.IsSelected = true;
                    }
                    else if (orgDto.IsSelected)
                    {
                        orgDto.IsSelected = false;
                    }
                }
            }
            else
            {
                foreach (var orgDto in organizatori)
                {
                    if (orgDto.IsSelected && orgDto.Organizator.Username.Equals(org.Username))
                    {
                        orgDto.IsSelected = false;
                    }
                }
            }
        }

        private void NazadBtn_Click(object sender, RoutedEventArgs e)
        {
            if (odabranOrganizator != null)
                Proslava.Organizer = odabranOrganizator;
            
            NavigationService.GoBack();
        }

        private void ZakaziBtn_Click(object sender, RoutedEventArgs e)
        {
            if (odabranOrganizator != null)
                Proslava.Organizer = odabranOrganizator;
            else if (Proslava.Organizer == null)
            {
                MessageBox.Show("Niste odabrali organizatora!");
                return;
            }
            if (Proslava.Client == null)
                Proslava.Client = allUsers["jadranka88"];

            backend.Backend.AddGathering(Proslava);

            NavigationService.Navigate(new ManifestationList());
        }
    }

    public class OrganizatorDTO
    {
        public string SlikaPutanja { get; set; }
        public User Organizator { get; set; }
        public bool IsSelected { get; set; }
    }
}
