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
    /// Interaction logic for ManifestationList.xaml
    /// </summary>
    public partial class ManifestationList : Page
    {
        public ObservableCollection<SocialGathering> socialGatherings = new ObservableCollection<SocialGathering>();

        public SocialGathering gath1 = new SocialGathering
        {
            Name = "Man1",
            Type = "Koncert",
            DateTime = new DateTime(),
            NumberOfGuests = 100,
            Description = "AAA"
        };

        public SocialGathering gath2 = new SocialGathering
        {
            Name = "Mangath2",
            Type = "Babine",
            DateTime = new DateTime(),
            NumberOfGuests = 100,
            Description = "AAA"
        };

        public SocialGathering gath3 = new SocialGathering
        {
            Name = "Mangath3",
            Type = "Rodjendan",
            DateTime = new DateTime(),
            NumberOfGuests = 100,
            Description = "AAA"
        };

        public SocialGathering gath4 = new SocialGathering
        {
            Name = "Mangath4",
            Type = "Koncert",
            DateTime = new DateTime(),
            NumberOfGuests = 100,
            Description = "AAA"
        };

        public SocialGathering gath5 = new SocialGathering
        {
            Name = "Mangath5",
            Type = "Zurka",
            DateTime = new DateTime(),
            NumberOfGuests = 100,
            Description = "AAA"
        };

        public SocialGathering gath6 = new SocialGathering
        {
            Name = "Man1gath6",
            Type = "Koncert",
            DateTime = new DateTime(),
            NumberOfGuests = 100,
            Description = "AAA"
        };

        public ManifestationList()
        {
            InitializeComponent();
            socialGatherings.Add(gath1);
            socialGatherings.Add(gath2);
            socialGatherings.Add(gath3);
            socialGatherings.Add(gath4);
            socialGatherings.Add(gath5);
            socialGatherings.Add(gath6);
            this.DataContext = this;
            ManifestationListView.ItemsSource = socialGatherings;
        }

        private void ManifestationSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchSocialGatherings(ManifestationSearch.Text);
        }

        public void SearchSocialGatherings(string query)
        {
            var filteredGatherings =
                from socialGathering in socialGatherings
                where socialGathering.Name.ToUpper().Contains(query.Trim().ToUpper())
                    || socialGathering.Type.ToUpper().Contains(query.Trim().ToUpper())
                select socialGathering;

            ObservableCollection<SocialGathering> filteredGatheringsObservable = new ObservableCollection<SocialGathering>(filteredGatherings);
            ManifestationListView.ItemsSource = filteredGatheringsObservable;
        }
    }
}
