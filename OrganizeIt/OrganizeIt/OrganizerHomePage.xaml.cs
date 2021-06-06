using OrganizeIt.backend.social_gatherings;
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

namespace OrganizeIt
{
    /// <summary>
    /// Interaction logic for OrganizerHomePage.xaml
    /// </summary>
    public partial class OrganizerHomePage : Page
    {
        public ObservableCollection<SocialGathering> requests { get; set; }
        public ObservableCollection<SocialGatheringSuggestionReply> responses { get; set; }

        public OrganizerHomePage()
        {
            InitializeComponent();
            var users = backend.Backend.LoadUsers();
            User loggedIn = backend.Backend.LoggedInUser;

            List<SocialGathering> allGatherings = backend.Backend.loadSocialGatherings(users).Values.ToList();
            List<SocialGathering> filteredGatherings 
                = allGatherings /* Only not accepted gatherings, for this organizer are new requests*/ 
                    .Where(x => !x.AcceptedSuggestions && x.OrganizerUsername == loggedIn.Username)
                    .ToList();

            responses = new ObservableCollection<SocialGatheringSuggestionReply>(loggedIn.SocialGatheringSuggestionReplies);
            requests = new ObservableCollection<SocialGathering>(filteredGatherings);

            RequestListView.ItemsSource = requests;
            ResponseListView.ItemsSource = responses;
            this.DataContext = this;
        }

        private void RequestSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchRequests(RequestSearch.Text);
        }

        private void SearchRequests(string query)
        {
            var filteredRequests =
                from request in requests
                where request.ClientUsername.ToUpper().Contains(query.Trim().ToUpper())
                    || request.Name.ToUpper().Contains(query.Trim().ToUpper())
                    || request.Type.ToUpper().Contains(query.Trim().ToUpper())
                select request;

            ObservableCollection<SocialGathering> filteredRequestsObservable 
                = new ObservableCollection<SocialGathering>(filteredRequests);
            RequestListView.ItemsSource = filteredRequestsObservable;
        }

        private void RequestListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            /* TODO: Izmeniti da se prikaze stvarni prozor */
            var selectedRequest = (e.OriginalSource as FrameworkElement).DataContext as SocialGathering;
            MessageBox.Show($"Showing data for request for {selectedRequest.Name}");
        }

        private void ResponseSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchResponses(ResponseSearch.Text);
        }

        private void SearchResponses(string query)
        {
            var filteredResponses =
               from response in responses
               where response.SocialGatheringSuggestion.SocialGathering.Name.ToUpper().Contains(query.Trim().ToUpper())
                    || response.SocialGatheringSuggestion.SocialGathering.ClientUsername.ToUpper().Contains(query.Trim().ToUpper())
               select response;

            ObservableCollection<SocialGatheringSuggestionReply> filteredResponsesObservable
               = new ObservableCollection<SocialGatheringSuggestionReply>(filteredResponses);
            RequestListView.ItemsSource = filteredResponsesObservable;
        }

        private void ResponseListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            /* TODO: Izmeniti da se prikaze stvarni prozor */
            var selectedResponse = (e.OriginalSource as FrameworkElement).DataContext as SocialGatheringSuggestionReply;
            MessageBox.Show($"Showing data for request for {selectedResponse.SocialGatheringSuggestion.SocialGathering.Name}");
        }
    }
}
