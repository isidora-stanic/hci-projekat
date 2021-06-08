using OrganizeIt.backend.social_gatherings;
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
    /// Interaction logic for EventSuggestionDraft.xaml
    /// </summary>
    public partial class EventSuggestionDraft : Page
    {
        public ObservableCollection<SocialGatheringCategorySuggestion> MainWindowSekcije { get; set; }
        public SocialGatheringSuggestion Predlog { get; set; }
        public ListToStringConverter ListToStringConverter { get; set; }
        public int counterForIds { get; set; }

        public EventSuggestionDraft(SocialGathering proslava)
        {
            ListToStringConverter = new ListToStringConverter();
            counterForIds = 0;
            InitializeComponent();

            //InitializeMainWindowSekcije();
            MainWindowSekcije = new ObservableCollection<SocialGatheringCategorySuggestion>();

            Predlog = new SocialGatheringSuggestion 
            { 
                CategorySuggestions = new List<SocialGatheringCategorySuggestion>(),
                SocialGathering = proslava,
                Client = proslava.Client,
                Organizer = proslava.Organizer,
                SuggestionDate = DateTime.Now
            };

            //this.DataContext = MainWindowSekcije;
            DataContext = this;
        }

        public EventSuggestionDraft(SocialGatheringSuggestion predlog)
        {
            MainWindowSekcije = new ObservableCollection<SocialGatheringCategorySuggestion>(predlog.CategorySuggestions);

            Predlog = new SocialGatheringSuggestion 
            { 
                CategorySuggestions = new List<SocialGatheringCategorySuggestion>(),
                SocialGathering = predlog.SocialGathering,
                Client = predlog.Client,
                Organizer = predlog.Organizer,
                SuggestionDate = DateTime.Now,
                SocialGatheringSeating = predlog.SocialGatheringSeating
            };

            ListToStringConverter = new ListToStringConverter();
            InitializeComponent();

            DataContext = this;
        }

        public void DodajSekciju()
        {
            MainWindowSekcije.Add(new SocialGatheringCategorySuggestion
            {
                Id = counterForIds++.ToString(),
                CategoryTitle = "",
                SuggestedCollaborators = new ObservableCollection<SocialGatheringCollaborator>()
            });
        }

        public void UkloniSekciju(SocialGatheringCategorySuggestion sc)
        {
            MainWindowSekcije.Remove(sc);
        }

        private void Plus_btn_Click(object sender, RoutedEventArgs e)
        {
            DodajSekciju();
        }

        private void Minus_btn_Click(object sender, RoutedEventArgs e)
        {
            SocialGatheringCategorySuggestion sc = (SocialGatheringCategorySuggestion)((MaterialDesignThemes.Wpf.PackIcon)sender).DataContext;
            UkloniSekciju(sc);
        }

        private void Dodaj_saradnike_btn_Click(object sender, RoutedEventArgs e)
        {
            SocialGatheringCategorySuggestion sekcija = (SocialGatheringCategorySuggestion)((Button)sender).DataContext;
            Predlog.CategorySuggestions = MainWindowSekcije.ToList();
            NavigationService.Navigate(new EventCollabsCheckList(this, sekcija));
        }

        private void PosaljiBtn_Click(object sender, RoutedEventArgs e)
        {
            Predlog.CategorySuggestions = MainWindowSekcije.ToList();


            backend.Backend.LoadAll();


            var users = backend.Backend.LoadUsers();
            var user = backend.Backend.LoggedInUser;
            //user = users["mmartinovic"];
            var cl = Predlog.Client;
            //MessageBox.Show(cl.Username);
            //MessageBox.Show(user.UserType.ToString());
            //var manif = new SocialGathering { Client = cl, Organizer = user, DateTime = DateTime.Now, Description = "blabla opis", Name = "Proslava bree", NumberOfGuests = 3, RequestDate = DateTime.Now, SocialGatheringSuggestions = new List<SocialGatheringSuggestion>() };
            /*manif.SocialGatheringSuggestions.Add(Predlog);
            backend.Backend.loadSocialGatherings(users);
            cl.SocialGatherings.Add(manif);
            Predlog.SocialGathering = manif;
            Predlog.Organizer = user;
            Predlog.Client = cl;

            backend.Backend.saveSocialGatherings(users);

            // ovo ce ici u drugi prozor--------------
            backend.Backend.loadSocialGatherings(users);
            cl = users["jadranka88"];*/

            //backend.Backend.AddGathering(manif);
            //Predlog.SocialGathering = manif;
            var manif = Predlog.SocialGathering; // mora proslava biti dodata predlogu u konstruktoru
            backend.Backend.AddSuggestion(Predlog, manif);

            //NavigationService.Navigate(new EventSuggestionView(Predlog));
            NavigationService.Navigate(new OrganizerHomePage());
        }

        private void OtkaziBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void SacuvajNeSalji_Click(object sender, RoutedEventArgs e)
        {
            // ako implementiramo draft (nije obavezno, vrv necemo)
        }

        private void RasporedBtn_Click(object sender, RoutedEventArgs e)
        {
            Predlog.CategorySuggestions = MainWindowSekcije.ToList();
            NavigationService.Navigate(new SeatingArrangement(Predlog));
        }
    }

    [ValueConversion(typeof(ObservableCollection<SocialGatheringCollaborator>), typeof(string))]
    public class ListToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(string))
                throw new InvalidOperationException("The target must be a String");
            List<string> nazivi = new List<string>();
            foreach (SocialGatheringCollaborator saradnik in (ObservableCollection<SocialGatheringCollaborator>)value)
            {
                nazivi.Add(saradnik.Name);
            }

            return String.Join(", ", nazivi.ToArray());
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}