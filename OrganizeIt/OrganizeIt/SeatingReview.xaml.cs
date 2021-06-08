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
    /// Interaction logic for SeatingReview.xaml
    /// </summary>
    public partial class SeatingReview : Page
    {
        public string TableImage { get; set; }

        public List<string> Table1 { get; set; }
        public List<string> Table2 { get; set; }
        public List<string> Table3 { get; set; }
        public List<string> Table4 { get; set; }

        public SeatingReview(SocialGatheringSeating seating)
        {
            InitializeComponent();
            TableImage = backend.Backend.GetTableImagePath();
            Table1 = seating.Table1;
            Table2 = seating.Table2;
            Table3 = seating.Table3;
            Table4 = seating.Table4;
            this.DataContext = this;
        }

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
