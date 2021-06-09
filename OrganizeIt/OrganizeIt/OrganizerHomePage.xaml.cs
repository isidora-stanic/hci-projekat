using OrganizeIt.backend;
using OrganizeIt.backend.social_gatherings;
using OrganizeIt.backend.todo;
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
using System.Globalization;

namespace OrganizeIt
{
    /// <summary>
    /// Interaction logic for OrganizerHomePage.xaml
    /// </summary>
    public partial class OrganizerHomePage : Page
    {

        private class UndoCommandObject : ICommand
        {
            private readonly OrganizerHomePage _target;

            public UndoCommandObject(OrganizerHomePage target)
            {
                _target = target;
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                _target.DoCommand();
            }
        }



        public ObservableCollection<SocialGathering> requests { get; set; }
        public ObservableCollection<SocialGatheringSuggestionReply> responses { get; set; }
        public ObservableCollection<ToDoCard> todosToDo { get; set; } 
        public ObservableCollection<ToDoCard> todosProcessing { get; set; }
        public ObservableCollection<ToDoCard> todosSent { get; set; }
        public ObservableCollection<ToDoCard> todosAccepted { get; set; }
        public ObservableCollection<ToDoCard> todosRejected { get; set; }

        private readonly ICommand _undoCommand;
        public ICommand UndoCommand { get { return _undoCommand; } }

        public Stack<ToDoCard> deletedToDos = new Stack<ToDoCard>();


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
            List<ToDoCard> allTodoCards = backend.Backend.LoadTodoList(users);


            responses = new ObservableCollection<SocialGatheringSuggestionReply>
                 (users[loggedIn.Username].SocialGatheringSuggestionReplies);
            requests = new ObservableCollection<SocialGathering>(filteredGatherings);

            todosToDo = new ObservableCollection<ToDoCard>(backend.Backend.GetTodoCardByStatusForOrganizer(allTodoCards, ToDoStatus.ToDo, loggedIn.Username));
            todosProcessing = new ObservableCollection<ToDoCard>(backend.Backend.GetTodoCardByStatusForOrganizer(allTodoCards, ToDoStatus.Processing, loggedIn.Username));
            todosSent = new ObservableCollection<ToDoCard>(backend.Backend.GetTodoCardByStatusForOrganizer(allTodoCards, ToDoStatus.Sent, loggedIn.Username));
            todosAccepted = new ObservableCollection<ToDoCard>(backend.Backend.GetTodoCardByStatusForOrganizer(allTodoCards, ToDoStatus.Accepted, loggedIn.Username));
            todosRejected = new ObservableCollection<ToDoCard>(backend.Backend.GetTodoCardByStatusForOrganizer(allTodoCards, ToDoStatus.Rejected, loggedIn.Username));

            RequestListView.ItemsSource = requests;
            ResponseListView.ItemsSource = responses;
            this.DataContext = this;

            _undoCommand = new UndoCommandObject(this);
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
            //MessageBox.Show($"Showing data for request for {selectedRequest.Name}");
            
            NavigationService.Navigate(new SocialGatheringInfo(selectedRequest));
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
            ResponseListView.ItemsSource = filteredResponsesObservable;
        }

        private void ResponseListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedResponse = (e.OriginalSource as FrameworkElement).DataContext as SocialGatheringSuggestionReply;
            //MessageBox.Show($"Showing data for request for {selectedResponse.SocialGatheringSuggestion.SocialGathering.Name}");

            NavigationService.Navigate(new EventSuggReplyView(selectedResponse));
        }

        private void AddTodoIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new AddTodo());
        }

        private void LogoutIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string messageBoxText = $"Da li zelite da se odjavite";
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

        private void processingListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = (e.OriginalSource as FrameworkElement).DataContext as ToDoCard;
            TodoEdit edit = new TodoEdit(selectedItem);
            if (edit.ShowDialog() == true)
            {
                this.todosProcessing.Remove(edit.Answer);

                if (edit.Answer.Status == ToDoStatus.Deleted)
                {
                    edit.Answer.Status = ToDoStatus.Processing;
                    deletedToDos.Push(edit.Answer);
                    return;
                }

                putAppropriate(edit.Answer);
            }

        }

        private void todoListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = (e.OriginalSource as FrameworkElement).DataContext as ToDoCard;
            TodoEdit edit = new TodoEdit(selectedItem);
            if (edit.ShowDialog() == true)
            {
                this.todosToDo.Remove(edit.Answer);

                if (edit.Answer.Status == ToDoStatus.Deleted)
                {
                    edit.Answer.Status = ToDoStatus.ToDo;
                    deletedToDos.Push(edit.Answer);
                    return;
                }

                putAppropriate(edit.Answer);
            }
        }

        private void sentListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = (e.OriginalSource as FrameworkElement).DataContext as ToDoCard;
            TodoEdit edit = new TodoEdit(selectedItem);
            if (edit.ShowDialog() == true)
            {
                this.todosSent.Remove(edit.Answer);

                if (edit.Answer.Status == ToDoStatus.Deleted)
                {
                    edit.Answer.Status = ToDoStatus.Sent;
                    deletedToDos.Push(edit.Answer);
                    return;
                }

                putAppropriate(edit.Answer);
            }
        }

        private void acceptedListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = (e.OriginalSource as FrameworkElement).DataContext as ToDoCard;
            TodoEdit edit = new TodoEdit(selectedItem);
            if (edit.ShowDialog() == true)
            {
                this.todosAccepted.Remove(edit.Answer);

                if (edit.Answer.Status == ToDoStatus.Deleted)
                {
                    edit.Answer.Status = ToDoStatus.Accepted;
                    deletedToDos.Push(edit.Answer);
                    return;
                }

                putAppropriate(edit.Answer);
            }
        }

        private void declinedListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = (e.OriginalSource as FrameworkElement).DataContext as ToDoCard;
            TodoEdit edit = new TodoEdit(selectedItem);
            if (edit.ShowDialog() == true)
            {
                this.todosRejected.Remove(edit.Answer);


                if (edit.Answer.Status == ToDoStatus.Deleted)
                {
                    edit.Answer.Status = ToDoStatus.Rejected;
                    deletedToDos.Push(edit.Answer);
                    return;
                }

                putAppropriate(edit.Answer);
            }
        }

        private void putAppropriate(ToDoCard card)
        {
            if (card.Status == ToDoStatus.ToDo)
                todosToDo.Add(card);
            else if (card.Status == ToDoStatus.Sent)
                todosSent.Add(card);
            else if (card.Status == ToDoStatus.Processing)
                todosProcessing.Add(card);
            else if (card.Status == ToDoStatus.Accepted)
                todosAccepted.Add(card);
            else if (card.Status == ToDoStatus.Rejected)
                todosRejected.Add(card);

            saveToDos();
        }

        private void DoCommand()
        {
            putAppropriate(deletedToDos.Pop());
        }

        private void saveToDos()
        {
            List<ToDoCard> allTodos = new List<ToDoCard>();
            foreach (ToDoCard card in this.todosToDo)
                allTodos.Add(card);
            foreach (ToDoCard card in this.todosSent)
                allTodos.Add(card);
            foreach (ToDoCard card in this.todosProcessing)
                allTodos.Add(card);
            foreach (ToDoCard card in this.todosAccepted)
                allTodos.Add(card);
            foreach (ToDoCard card in this.todosRejected)
                allTodos.Add(card);
            int len = allTodos.Count;

            backend.Backend.SaveTodoList(allTodos);
        }

        /* funkcija za otvaranje detalja o proslavi */
        private void PackIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SocialGathering proslava = (SocialGathering)((MaterialDesignThemes.Wpf.PackIcon)sender).DataContext;
            NavigationService.Navigate(new SocialGatheringInfo(proslava));
        }


    }

    public class BooleanConverter : IValueConverter
    {
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value.ToString().ToUpper())
            {
                case "DA":
                    return true;
                case "NE":
                    return false;
                default:
                    return false;
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                if ((bool)value == true)
                    return "DA";
                else
                    return "NE";
            }
            return "NE";
        }
    }
}
