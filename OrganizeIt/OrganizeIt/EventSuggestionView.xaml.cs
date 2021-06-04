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
    /// Interaction logic for EventSuggestionView.xaml
    /// </summary>
    public partial class EventSuggestionView : Page
    {
        public ObservableCollection<SekcijaKomentarDTO> SekcijeDTOsKojeSeVide { get; set; }
        public ObservableCollection<SocialGatheringCategorySuggestion> MainWindowSekcije { get; set; }
        public SocialGatheringSuggestion Predlog { get; set; }
        public ListToStringConverter ListToStringConverter { get; set; }

        //public Dictionary<SocialGatheringCategorySuggestion, List<SaradnikSelectDTO>> Sekcije { get; set; }
        public SocialGatheringSuggestionReply Odgovor { get; set; }
        public int counterForIds { get; set; }

        public EventSuggestionView(SocialGatheringSuggestion predlog)
        {
            ListToStringConverter = new ListToStringConverter();
            counterForIds = 0;
            
            InitializeComponent();

            MainWindowSekcije = new ObservableCollection<SocialGatheringCategorySuggestion>();

            Predlog = predlog;
            Odgovor = new SocialGatheringSuggestionReply {
                CategoryComments = new Dictionary<SocialGatheringCategorySuggestion, string>(),
                SuggestionsAccepted = false,
                ReplyDate = DateTime.Now,
                SocialGatheringSuggestion = Predlog
            };

            InitSekcijeDTOs();

            DataContext = this;
        }

        public EventSuggestionView(SocialGatheringSuggestion predlog, List<SaradnikSelectDTO> saradniciDto)
        {
            MainWindowSekcije = new ObservableCollection<SocialGatheringCategorySuggestion>(predlog.CategorySuggestions);

            Predlog = predlog;

            Odgovor = new SocialGatheringSuggestionReply
            {
                CategoryComments = new Dictionary<SocialGatheringCategorySuggestion, string>(),
                SuggestionsAccepted = true,
                ReplyDate = DateTime.Now,
                SocialGatheringSuggestion = Predlog
            };
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
                var dto = new SekcijaKomentarDTO { Sekcija = sekcija, Komentar = "" };
                SekcijeDTOsKojeSeVide.Add(dto);
            }
        }

        public void NamestiOdgovor()
        {
            foreach (SekcijaKomentarDTO sekcijaKom in SekcijeDTOsKojeSeVide)
                Odgovor.CategoryComments[sekcijaKom.Sekcija] = sekcijaKom.Komentar;
        }

        private void PosaljiBtn_Click(object sender, RoutedEventArgs e)
        {
            // cuvanje odgovora i navigacija do sledeceg prozora
            NamestiOdgovor();
            MessageBox.Show("Slanje (cuvanje) odgovora");
        }

        private void PrihvatiBtn_Click(object sender, RoutedEventArgs e)
        {
            // cuvanje odgovora i navigacija do sledeceg prozora
            NamestiOdgovor();
            Odgovor.SuggestionsAccepted = true;
            MessageBox.Show("Slanje (cuvanje) odgovora i prihvatanje");
        }

        private void OtkaziBtn_Click(object sender, RoutedEventArgs e)
        {
            // samo vracanje na prethodni prozor bez odgovaranja
            NavigationService.GoBack();
        }
    }

    public class SekcijaKomentarDTO
    {
        public SocialGatheringCategorySuggestion Sekcija { get; set; }
        public string Komentar { get; set; }
    }
}
