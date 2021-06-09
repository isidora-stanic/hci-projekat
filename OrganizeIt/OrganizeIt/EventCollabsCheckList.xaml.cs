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
    /// Interaction logic for EventCollabsCheckList.xaml
    /// </summary>
    public partial class EventCollabsCheckList : Page
    {
        public List<SocialGatheringCollaborator> SviSaradniciNormalni { get; set; }
        public List<SaradnikSelectDTO> SviSaradnici { get; set; }
        //public List<SocialGatheringCollaborator> SelektovaniSaradnici { get; set; }
        public List<SaradnikSelectDTO> PretrazeniSaradnici { get; set; }
        public SocialGatheringCategorySuggestion Sekcija { get; set; }
        public EventSuggestionDraft Before { get; set; }
        public EventCollabsCheckList(EventSuggestionDraft before, SocialGatheringCategorySuggestion sekcija)
        {
            Sekcija = sekcija;
            Before = before;
            SviSaradniciNormalni = new List<SocialGatheringCollaborator>();
            SviSaradnici = new List<SaradnikSelectDTO>();
            //SelektovaniSaradnici = new List<SocialGatheringCollaborator>();
            PretrazeniSaradnici = new List<SaradnikSelectDTO>();
            DataContext = this;
            InitializeComponent();
            InitSaradnici();
            PretraziSaradnike("");
        }

        public void InitSaradnici()
        {
            SviSaradniciNormalni = backend.Backend.LoadCollaborators().Values.ToList();

            ProdjiListuINamestiBulove();

        }

        public void PretraziSaradnike(string ukucano)
        {
            PretrazeniSaradnici = new List<SaradnikSelectDTO>();
            foreach (SaradnikSelectDTO s in SviSaradnici)
            {
                if (s.Collaborator.Name.ToLower().Contains(ukucano.ToLower()) || s.Collaborator.Description.Contains(ukucano.ToLower()) || s.Collaborator.Id.ToString().Contains(ukucano.ToLower()))
                {
                    PretrazeniSaradnici.Add(s);
                }
            }
            listaSaradnika.ItemsSource = PretrazeniSaradnici;
        }

        private void Cancel_btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Ok_btn_Click(object sender, RoutedEventArgs e)
        {
            Sekcija.SuggestedCollaborators = new ObservableCollection<SocialGatheringCollaborator>(ProdjiKrozSveIIzvuciSelektoanoZaKraj());

            NavigationService.Navigate(new EventSuggestionDraft(Before.Predlog));
        }

        private void TbxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            PretraziSaradnike(TbxSearch.Text);
        }

        private void CheckBox_MouseDown(object sender, RoutedEventArgs e)
        {
            CheckBox check = (CheckBox)sender;
            SocialGatheringCollaborator saradnik = ((SaradnikSelectDTO)check.DataContext).Collaborator;
            NadjiSaradnikaIPostaviMuBool(saradnik, (bool)check.IsChecked);

        }

        private void NadjiSaradnikaIPostaviMuBool(SocialGatheringCollaborator saradnik, bool bul)
        {
            foreach (SaradnikSelectDTO dto in SviSaradnici)
            {
                if (dto.Collaborator.Id.Equals(saradnik.Id))
                {
                    dto.IsSelected = bul;
                }
            }
        }

        private void NadjiSaradnikaIToggleMuBool(SocialGatheringCollaborator saradnik)
        {
            foreach (SaradnikSelectDTO dto in SviSaradnici)
            {
                if (dto.Collaborator.Id.Equals(saradnik.Id))
                {
                    if (dto.IsSelected)
                        dto.IsSelected = false;
                    else
                        dto.IsSelected = true;
                }
            }
        }

        private void ProdjiListuINamestiBulove()
        {
            foreach (SocialGatheringCollaborator sar in SviSaradniciNormalni)
            {
                SaradnikSelectDTO dto = new SaradnikSelectDTO { Collaborator = sar, IsSelected = false };
                foreach (SocialGatheringCollaborator coll in Sekcija.SuggestedCollaborators)
                {
                    if (coll.Id.Equals(sar.Id))
                    {
                        dto.IsSelected = true;
                    }
                }
                SviSaradnici.Add(dto);
            }
        }

        private List<SocialGatheringCollaborator> ProdjiKrozSveIIzvuciSelektoanoZaKraj()
        {
            List<SocialGatheringCollaborator> SelektovaniSaradnici = new List<SocialGatheringCollaborator>();
            foreach (SaradnikSelectDTO dto in SviSaradnici)
            {
                if (dto.IsSelected)
                {
                    SelektovaniSaradnici.Add(dto.Collaborator);
                }
            }
            return SelektovaniSaradnici;
        }
    }

    public class SaradnikSelectDTO
    {
        public SocialGatheringCollaborator Collaborator { get; set; }
        public bool IsSelected { get; set; }
    }
}
