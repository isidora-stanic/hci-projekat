using HelpSistem;
using MaterialDesignThemes.Wpf;
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
        public ObservableCollection<SocialGatheringSuggestion> suggestions = new ObservableCollection<SocialGatheringSuggestion>();


        public ManifestationList()
        {
            InitializeComponent();
            var users =  backend.Backend.LoadUsers();
            backend.Backend.loadSocialGatherings(users);

            this.socialGatherings 
                = new ObservableCollection<SocialGathering> (
                    users[backend.Backend.LoggedInUser.Username].SocialGatherings
                    );

            foreach (SocialGathering sg in socialGatherings)
            {
                foreach (SocialGatheringSuggestion sgs in sg.SocialGatheringSuggestions)
                    suggestions.Add(sgs);
            }

            this.DataContext = this;
            ManifestationListView.ItemsSource = socialGatherings;
            SuggestionListView.ItemsSource = suggestions;
        }

        private void ManifestationSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchSocialGatherings(ManifestationSearch.Text);
        }

        private void SuggestionSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchSuggestions(SuggestionsSearch.Text);
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

        public void SearchSuggestions(string query)
        {
            var filteredSuggestions = 
                from suggestion in suggestions
                where suggestion.SocialGathering.Name.ToUpper().Contains(query.Trim().ToUpper())
                select suggestion;

            ObservableCollection<SocialGatheringSuggestion> filteredSuggestionsObservable 
                = new ObservableCollection<SocialGatheringSuggestion>(filteredSuggestions);
            SuggestionListView.ItemsSource = filteredSuggestionsObservable;
        }

        private void SuggestionListView_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SocialGatheringSuggestion selectedSuggestion = (sender as ListView).SelectedItem as SocialGatheringSuggestion;
            NavigationService.Navigate(new EventSuggestionView(selectedSuggestion));
        }

        private void DodajManifBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new NewEventForm());
        }

        

        private void NewLikeThis_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SocialGathering proslava = (SocialGathering)((MaterialDesignThemes.Wpf.PackIcon)sender).DataContext;
            NavigationService.Navigate(new NewEventForm(proslava));
        }

        private void OdjavaBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string messageBoxText = $"Da li želite da se odjavite";
            string caption = "Odjava";
            MessageBoxButton btn = MessageBoxButton.YesNo;
            MessageBoxImage img = MessageBoxImage.Question;

            var result = MessageBox.Show(messageBoxText, caption, btn, img, MessageBoxResult.No);
            if (result == MessageBoxResult.No)
                return;

            backend.Backend.LoggedInUser = null;
            backend.Backend.LogOut();
            NavigationService.Navigate(new Login());
        }

        private void Help_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string str = "ManifestationListPage";
            HelpProvider.ShowHelp(str, Window.GetWindow(this));
        }
    }
}
