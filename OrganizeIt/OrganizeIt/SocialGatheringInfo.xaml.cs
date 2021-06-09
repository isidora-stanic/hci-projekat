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
    /// Interaction logic for SocialGatheringInfo.xaml
    /// </summary>
    public partial class SocialGatheringInfo : Page
    {
        public SocialGathering Proslava { get; set; }
        public string SlikaKlijenta { get; set; }
        public SocialGatheringInfo(SocialGathering proslava)
        {
            Proslava = proslava;
            SlikaKlijenta = backend.Backend.GetUserImagePath(Proslava.Client.Username);
            InitializeComponent();
            DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EventSuggestionDraft(Proslava));
        }

        private void NazadBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrganizerHomePage());
        }
    }
}
