using HelpSistem;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[0]);
            if (focusedControl is DependencyObject)
            {
                string str = HelpProvider.GetHelpKey((DependencyObject)focusedControl);
                if (str.Equals("index"))
                {
                    str = frame.Content.ToString().Split('.')[1] + "Page";
                }
                HelpProvider.ShowHelp(str, this);
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            backend.Backend.LoadAll();
            var user = backend.Backend.LoggedInUser;
            if (user == null)
            {
                frame.NavigationService.Navigate(new Login());
            }
            else
            {
                if (user.UserType == backend.users.UserType.Administrator)
                {
                    frame.NavigationService.Navigate(new AccountsList());
                }
                else if (user.UserType == backend.users.UserType.Organizer)
                {
                    frame.NavigationService.Navigate(new OrganizerHomePage());
                }
                else if (user.UserType == backend.users.UserType.Client)
                {
                    frame.NavigationService.Navigate(new ManifestationList());
                }
            }
        }
    }
}