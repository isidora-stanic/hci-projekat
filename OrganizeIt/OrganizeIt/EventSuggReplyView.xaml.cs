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
    /// Interaction logic for EventSuggReplyView.xaml
    /// </summary>
    public partial class EventSuggReplyView : Page
    {
        public ObservableCollection<SekcijaKomentarDTO> SekcijeDTOsKojeSeVide { get; set; }
        public ObservableCollection<SocialGatheringCategorySuggestion> MainWindowSekcije { get; set; }
        public SocialGatheringSuggestion Predlog { get; set; }
        public ListToStringConverter ListToStringConverter { get; set; }

        //public Dictionary<SocialGatheringCategorySuggestion, List<SaradnikSelectDTO>> Sekcije { get; set; }
        public SocialGatheringSuggestionReply Odgovor { get; set; }
        public int counterForIds { get; set; }

        /*public EventSuggReplyView(SocialGatheringSuggestion predlog)
        {
            ListToStringConverter = new ListToStringConverter();
            counterForIds = 0;

            InitializeComponent();

            MainWindowSekcije = new ObservableCollection<SocialGatheringCategorySuggestion>();

            Predlog = predlog;
            Odgovor = new SocialGatheringSuggestionReply
            {
                CategoryComments = new Dictionary<string, string>(),
                ReplyDate = DateTime.Now,
                SocialGatheringSuggestion = Predlog
            };
            Odgovor.SuggestionsAccepted = false;

            InitSekcijeDTOs();

            DataContext = this;
        }*/

        public EventSuggReplyView(SocialGatheringSuggestionReply odgovor)
        {
            Odgovor = odgovor;
            Predlog = Odgovor.SocialGatheringSuggestion;

            MainWindowSekcije = new ObservableCollection<SocialGatheringCategorySuggestion>(Predlog.CategorySuggestions);
            InitSekcijeDTOs();

            ListToStringConverter = new ListToStringConverter();
            InitializeComponent();

            DataContext = this;
        }

        public void InitSekcijeDTOs()
        {

            SekcijeDTOsKojeSeVide = new ObservableCollection<SekcijaKomentarDTO>();
            foreach (var sekcija in Predlog.CategorySuggestions)
            {
                var dto = new SekcijaKomentarDTO { Sekcija = sekcija, Komentar = Odgovor.CategoryComments[sekcija.CategoryTitle] };
                SekcijeDTOsKojeSeVide.Add(dto);
            }
        }

        public void NamestiOdgovor()
        {
            foreach (SekcijaKomentarDTO sekcijaKom in SekcijeDTOsKojeSeVide)
                Odgovor.CategoryComments[sekcijaKom.Sekcija.CategoryTitle] = sekcijaKom.Komentar;
        }

        private void NoviPredlogBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EventSuggestionDraft(Predlog.SocialGathering));
        }

        private void IzmeniPredlogBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EventSuggestionDraft(Predlog));
        }

        private void NazadBtn_Click(object sender, RoutedEventArgs e)
        {
            // samo vracanje na prethodni prozor bez odgovaranja
            NavigationService.GoBack();
        }
    }
}
