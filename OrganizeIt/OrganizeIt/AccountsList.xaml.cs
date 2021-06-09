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
using System.Globalization;
using OrganizeIt.backend.social_gatherings;
using HelpSistem;

namespace OrganizeIt
{
    /// <summary>
    /// Interaction logic for AccountsList.xaml
    /// </summary>
    public partial class AccountsList : Page
    {
        private class UndoCommandObject : ICommand
        {
            private readonly AccountsList _target;

            public UndoCommandObject(AccountsList target)
            {
                _target = target;
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                _target.DoCommand();
            }
        }

        private readonly ICommand _undoCommand;
        public ICommand UndoCommand { get { return _undoCommand; } }

        public ObservableCollection<User> clients = new ObservableCollection<User>();
        public ObservableCollection<User> organizers = new ObservableCollection<User>();
        public ObservableCollection<SocialGatheringCollaborator> collaborators = new ObservableCollection<SocialGatheringCollaborator>();

        public Dictionary<string, User> allUsers;

        public Stack<User> deletedClients = new Stack<User>();
        public Stack<User> deletedOrganizers = new Stack<User>();
        public Stack<SocialGatheringCollaborator> deletedColaborators = new Stack<SocialGatheringCollaborator>();

        public AccountsList()
        {
            InitializeComponent();
            allUsers = backend.Backend.LoadUsers();
            var collabDict = backend.Backend.LoadCollaborators().Values.ToList();
            collaborators = new ObservableCollection<SocialGatheringCollaborator>(collabDict);
            this.DataContext = this;

            clients = new ObservableCollection<User>(allUsers.Values.Where(user => user.UserType == UserType.Client));
            organizers = new ObservableCollection<User>(allUsers.Values.Where(user => user.UserType == UserType.Organizer));

            ClientListView.ItemsSource = clients;
            OrganizerListView.ItemsSource = organizers;
            CollaboratorListView.ItemsSource = collaborators;

            _undoCommand = new UndoCommandObject(this);
        }

        /* Funkcija za promenu teksta pri pretrazi klijenata */

        private void ClientSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchClients(ClientSearch.Text);
        }

        /* Funkcija za promenu teksta pri pretrazi organizatora */

        private void OrganizerSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchOrganizers(OrganizerSearch.Text);
        }

        /* Funkija za promenu teksta pri pretrazi klijenata */

        private void CollaboratorSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchCollaborators(CollaboratorSearch.Text);
        }

        /* Funkcija za brisanje korisnika, poziva se pri klikom na ikonicu */
        /* Gleda tip korisnika, i na osnovu toga brise iz odredjene liste */
        /* Na osnovu tipa ubacuje u stack obrisanih, kako bi mogao undo */

        private void DeleteButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            User u = (User)((MaterialDesignThemes.Wpf.PackIcon)sender).DataContext;

            string messageBoxText = $"Da li želite da izbrišete korisnika {u.Username}?";
            string caption = "Brisanje korisnika";
            MessageBoxButton btn = MessageBoxButton.YesNo;
            MessageBoxImage img = MessageBoxImage.Question;

            var result = MessageBox.Show(messageBoxText, caption, btn, img, MessageBoxResult.No);
            if (result == MessageBoxResult.Yes)
            {
                if (u.UserType == UserType.Client)
                {
                    foreach (User usr in clients)
                    {
                        if (usr.Username == u.Username)
                        {
                            clients.Remove(usr);
                            ClientListView.ItemsSource = clients;
                            deletedClients.Push(u);
                            break;
                        }
                    }
                }
                else if (u.UserType == UserType.Organizer)
                {
                    foreach (User usr in organizers)
                    {
                        if (usr.Username == u.Username)
                        {
                            organizers.Remove(usr);
                            OrganizerListView.ItemsSource = organizers;
                            deletedOrganizers.Push(u);
                            break;
                        }
                    }
                }
            }
        }

        /* Doscstring
         * Funkcija za brisanje saradnika iz liste saradnika
         */

        private void DeleteCollabButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SocialGatheringCollaborator u = (SocialGatheringCollaborator)
                ((MaterialDesignThemes.Wpf.PackIcon)sender).DataContext;

            string messageBoxText = $"Da li želite da izbrišete saradnika {u.Name}?";
            string caption = "Brisanje saradnika";
            MessageBoxButton btn = MessageBoxButton.YesNo;
            MessageBoxImage img = MessageBoxImage.Question;

            var result = MessageBox.Show(messageBoxText, caption, btn, img, MessageBoxResult.No);

            if (result == MessageBoxResult.Yes)
            {
                foreach (SocialGatheringCollaborator collab in collaborators)
                {
                    if (collab.Id == u.Id)
                    {
                        collaborators.Remove(collab);
                        CollaboratorListView.ItemsSource = collaborators;
                        deletedColaborators.Push(u);
                        break;
                    }
                }
            }
        }

        /* Funkcija koja se poziva kad se odabere izmena korisnika */

        private void EditButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            User v = (User)((MaterialDesignThemes.Wpf.PackIcon)sender).DataContext;
            NavigationService.Navigate(new EditOrganizer(v, allUsers));
        }

        /* Funkcija koja se pozove kad se odabere izmena saradnika */

        private void EditCollabButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SocialGatheringCollaborator collab
                = (SocialGatheringCollaborator)((MaterialDesignThemes.Wpf.PackIcon)sender).DataContext;
            NavigationService.Navigate(new EditSaradnik(collab)); //collab
        }

        /* Ova funkcija se poziva kada se pozove Undo komanda */
        /* Pozivom ove komande se sa steka vraca u listu korisnika */
        /* poslednji obrisani. Stack se bira u zavisnosti od aktivnog taba */

        private void DoCommand()
        {
            TabItem ti = TabCtrl.SelectedItem as TabItem;
            if (ti.Header as string == "Klijenti")
            {
                if (deletedClients.Count != 0)
                {
                    clients.Add(deletedClients.Pop());
                }
            }
            else if (ti.Header as string == "Organizatori")
            {
                if (deletedOrganizers.Count != 0)
                {
                    organizers.Add(deletedOrganizers.Pop());
                }
            }
            else if (ti.Header as string == "Saradnici")
            {
                if (deletedColaborators.Count != 0)
                {
                    collaborators.Add(deletedColaborators.Pop());
                }
            }
        }

        /* Ova funkcija se poziva pri pozivu pretrage */

        public void SearchClients(string query)
        {
            var filteredClients =
                from client in clients
                where client.FirstName.ToUpper().Contains(query.ToUpper().Trim())
                    || client.LastName.ToUpper().Contains(query.ToUpper().Trim())
                    || client.Email.ToUpper().Contains(query.ToUpper().Trim())
                    || client.Username.ToUpper().Contains(query.ToUpper().Trim())
                select client;

            ObservableCollection<User> filteredClientsObservable = new ObservableCollection<User>(filteredClients);
            ClientListView.ItemsSource = filteredClientsObservable;
        }

        /* Ova funkcija se poziva pri pozivu pretrage */

        public void SearchOrganizers(string query)
        {
            var filteredOrganizers =
                from organizer in organizers
                where organizer.FirstName.ToUpper().Contains(query.ToUpper().Trim())
                    || organizer.LastName.ToUpper().Contains(query.ToUpper().Trim())
                    || organizer.Email.ToUpper().Contains(query.ToUpper().Trim())
                    || organizer.Username.ToUpper().Contains(query.ToUpper().Trim())
                select organizer;

            ObservableCollection<User> filteredOrganizersObservable = new ObservableCollection<User>(filteredOrganizers);
            OrganizerListView.ItemsSource = filteredOrganizersObservable;
        }

        public void SearchCollaborators(string query)
        {
            var filteredCollaborators =
                from collaborator in collaborators
                where collaborator.Name.ToUpper().Contains(query.ToUpper().Trim())
                select collaborator;

            ObservableCollection<SocialGatheringCollaborator> filteredCollaboratorsObservable
                = new ObservableCollection<SocialGatheringCollaborator>(filteredCollaborators);
            CollaboratorListView.ItemsSource = filteredCollaboratorsObservable;
        }

        private void AddClientIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new NewOrganizer(true));
        }

        private void AddOrganizerIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new NewOrganizer(false));
        }

        private void AddCollaboratorIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new NewSaradnik());
        }

        private void LogoutIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string messageBoxText = $"Da li želite da se odjavite";
            string caption = "Odjava";
            MessageBoxButton btn = MessageBoxButton.YesNo;
            MessageBoxImage img = MessageBoxImage.Question;

            var result = MessageBox.Show(messageBoxText, caption, btn, img, MessageBoxResult.No);
            if (result == MessageBoxResult.No)
                return;

            backend.Backend.LoggedInUser = null;
            backend.Backend.LogOut();
            NavigationService.Navigate(new Login());
        }

        private void Help_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string str = "AccountsListPage";
            HelpProvider.ShowHelp(str, Window.GetWindow(this));
        }
    }

    public class GenderToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (((Gender)value) == Gender.Male)
                return "M";
            return "F";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString() == "M")
                return Gender.Male;
            return Gender.Female;
        }
    }
}