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
    /// Interaction logic for SeatingArrangement.xaml
    /// </summary>
    public partial class SeatingArrangement : Page
    {
        public SocialGatheringSuggestion GatheringSuggestion { get; set; }
        public string TableImage { get; set; }

        public ObservableCollection<string> Guests { get; set; }
        public ObservableCollection<string> Table1 { get; set; }
        public ObservableCollection<string> Table2 { get; set; }
        public ObservableCollection<string> Table3 { get; set; }
        public ObservableCollection<string> Table4 { get; set; }
        

        Point startPoint = new Point();
        ObservableCollection<string> sourceList;


        public SeatingArrangement(SocialGatheringSuggestion gatheringSuggestion)
        {
            InitializeComponent();
            this.DataContext = this;

            GatheringSuggestion = gatheringSuggestion;
            TableImage = backend.Backend.GetTableImagePath(); 
            
            if (gatheringSuggestion.SocialGatheringSeating == null)
            {
                Guests = new ObservableCollection<string>(gatheringSuggestion.SocialGathering.GuestList);
                Table1 = new ObservableCollection<string>();
                Table2 = new ObservableCollection<string>();
                Table3 = new ObservableCollection<string>();
                Table4 = new ObservableCollection<string>();
            }
            else
            {
                Guests = new ObservableCollection<string>();
                Table1 = new ObservableCollection<string>(gatheringSuggestion.SocialGatheringSeating.Table1);
                Table2 = new ObservableCollection<string>(gatheringSuggestion.SocialGatheringSeating.Table2);
                Table3 = new ObservableCollection<string>(gatheringSuggestion.SocialGatheringSeating.Table3);
                Table4 = new ObservableCollection<string>(gatheringSuggestion.SocialGatheringSeating.Table4);
            }
        }

        private void ListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
        }

        private void ListView_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                // Get the dragged ListViewItem
                ListView listView = sender as ListView;
                ListViewItem listViewItem =
                    FindAncestor<ListViewItem>((DependencyObject)e.OriginalSource);

                // Find the source list
                sourceList = listView.ItemsSource as ObservableCollection<string>;

                // Find the data behind the ListViewItem
                try 
                {
                    string guest = (string)listView.ItemContainerGenerator.
                  ItemFromContainer(listViewItem);
                    // Initialize the drag & drop operation
                    DataObject dragData = new DataObject("Format", guest);
                    DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);
                }
                catch
                {
                    return;
                }
                

                
            }
        }

        private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }

        private void ListView_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("Format") || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void ListView_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("Format"))
            {
                ListView lv = sender as ListView;
                string guest = e.Data.GetData("Format") as string;
                var src = e.Source;
                var droppedList = lv.ItemsSource as ObservableCollection<string>;

                if (droppedList != Guests && droppedList.Count >= 4)
                {
                    string messageBoxText = $"Ovaj sto je već popunjen";
                    string caption = "Popunjen sto";
                    MessageBoxButton btn = MessageBoxButton.OK;
                    MessageBoxImage img = MessageBoxImage.Information;

                    var result = MessageBox.Show(messageBoxText, caption, btn, img, MessageBoxResult.OK);
                    return;
                }

                sourceList.Remove(guest);
                droppedList.Add(guest);
            }
        }


        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {

            string messageBoxText = $"Da li ste sigurni da želite da otkažete raspoređivanje gostiju?";
            string caption = "Otkazivanje";
            MessageBoxButton btn = MessageBoxButton.YesNo;
            MessageBoxImage img = MessageBoxImage.Question;

            var result = MessageBox.Show(messageBoxText, caption, btn, img, MessageBoxResult.No);
            if (result == MessageBoxResult.Yes)
                NavigationService.GoBack();
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Guests.Count != 0)
            {
                MessageBox.Show("Niste rasporedili sve goste.", "Raspored");
                return;
            }

            string messageBoxText = "Da li ste sigurni da želite da sačuvate raspored gostiju?";
            string caption = "Završi raspoređivanje";
            MessageBoxButton btn = MessageBoxButton.YesNo;
            MessageBoxImage img = MessageBoxImage.Question;

            var result = MessageBox.Show(messageBoxText, caption, btn, img, MessageBoxResult.No);
            if (result == MessageBoxResult.No)
                return;

            SocialGatheringSeating seating = new SocialGatheringSeating
            {
                Table1 = new List<string>(Table1),
                Table2 = new List<string>(Table2),
                Table3 = new List<string>(Table3),
                Table4 = new List<string>(Table4)
            };
            GatheringSuggestion.SocialGatheringSeating = seating;
            NavigationService.Navigate(new EventSuggestionDraft(GatheringSuggestion));
        }
    }
}
