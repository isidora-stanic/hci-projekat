using OrganizeIt.backend.todo;
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
    /// Interaction logic for AddTodo.xaml
    /// </summary>
    public partial class AddTodo : Page
    {
        public AddTodo()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            string messageBoxText = "Da li zelite da otkazete dodavanje zadatka?";
            string caption = "Otkazivanje";
            MessageBoxButton btn = MessageBoxButton.YesNo;
            MessageBoxImage img = MessageBoxImage.Question;

            var result = MessageBox.Show(messageBoxText, caption, btn, img, MessageBoxResult.No);
            if (result == MessageBoxResult.No)
                return;

            NavigationService.Navigate(new OrganizerHomePage());
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            string name = NameBox.Text;
            string description = DescriptionBox.Text;
            string status = (StatusBox.SelectedItem as ComboBoxItem).Content as string;

            string messageBoxText = $"Da li zelite da dodate zadatak '{name}'?";
            string caption = "Dodavanje";
            MessageBoxButton btn = MessageBoxButton.YesNo;
            MessageBoxImage img = MessageBoxImage.Question;

            var result = MessageBox.Show(messageBoxText, caption, btn, img, MessageBoxResult.No);
            if (result == MessageBoxResult.No)
                return;

            ToDoStatus newStatus;

            switch (status)
            {
                case "Novo":
                    newStatus = ToDoStatus.ToDo;
                    break;
                case "U obradi":
                    newStatus = ToDoStatus.Processing;
                    break;
                case "Poslato":
                    newStatus = ToDoStatus.Sent;
                    break;
                case "Prihvaceno":
                    newStatus = ToDoStatus.Accepted;
                    break;
                case "Odbijeno":
                    newStatus = ToDoStatus.Rejected;
                    break;
                default:
                    newStatus = ToDoStatus.ToDo;
                    break;
            }

            ToDoCard newCard = new ToDoCard
            {
                Name = name,
                Description = description,
                Status = newStatus,
                Organizer = backend.Backend.LoggedInUser
            };

            var users = backend.Backend.LoadUsers();
            var todoList = backend.Backend.LoadTodoList(users);
            todoList.Add(newCard);
            backend.Backend.SaveTodoList(todoList);

            NavigationService.Navigate(new OrganizerHomePage());
        }
    }
}
