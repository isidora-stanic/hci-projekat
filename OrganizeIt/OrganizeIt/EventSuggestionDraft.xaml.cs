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

        public EventSuggestionDraft()
        {
            ListToStringConverter = new ListToStringConverter();
            counterForIds = 0;
            InitializeComponent();

            

            //InitializeMainWindowSekcije();
            MainWindowSekcije = new ObservableCollection<SocialGatheringCategorySuggestion>();

            Predlog = new SocialGatheringSuggestion { CategorySuggestions = new List<SocialGatheringCategorySuggestion>() };

            //this.DataContext = MainWindowSekcije;
            DataContext = this;
        }

        public EventSuggestionDraft(SocialGatheringSuggestion predlog)
        {
            MainWindowSekcije = new ObservableCollection<SocialGatheringCategorySuggestion>(predlog.CategorySuggestions);

            Predlog = new SocialGatheringSuggestion { CategorySuggestions = new List<SocialGatheringCategorySuggestion>() };

            ListToStringConverter = new ListToStringConverter();
            InitializeComponent();

            DataContext = this;
        }

        public ObservableCollection<SocialGatheringCategorySuggestion> InitializeMainWindowSekcije()
        {
            MainWindowSekcije = new ObservableCollection<SocialGatheringCategorySuggestion>();
            SocialGatheringCategorySuggestion muzika = new SocialGatheringCategorySuggestion { Id=counterForIds++.ToString(), CategoryTitle="Muzika", SuggestedCollaborators=new ObservableCollection<SocialGatheringCollaborator>() };
            SocialGatheringCollaborator sc1 = new SocialGatheringCollaborator { Id = "saradnik1", Name = "MC Stojan", Description = "smesan DJ" };
            SocialGatheringCollaborator sc2 = new SocialGatheringCollaborator { Id = "saradnik2", Name = "DJ Stasa", Description = "dobar DJ" };
            muzika.SuggestedCollaborators.Add(sc1);
            muzika.SuggestedCollaborators.Add(sc2);
            MainWindowSekcije.Add(muzika);
            MainWindowSekcije.Add(new SocialGatheringCategorySuggestion { Id = counterForIds++.ToString(), CategoryTitle ="Prostor", SuggestedCollaborators = new ObservableCollection<SocialGatheringCollaborator>() });
            MainWindowSekcije.Add(new SocialGatheringCategorySuggestion { Id = counterForIds++.ToString(), CategoryTitle = "Dekoracija", SuggestedCollaborators = new ObservableCollection<SocialGatheringCollaborator>() });
            return MainWindowSekcije;
        }

        public void DodajSekciju()
        {
            MainWindowSekcije.Add(new SocialGatheringCategorySuggestion { Id = counterForIds++.ToString(), CategoryTitle = "", SuggestedCollaborators = new ObservableCollection<SocialGatheringCollaborator>() });
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
