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
    /// Interaction logic for EditSaradnik.xaml
    /// </summary>
    public partial class EditSaradnik : Page
    {
        public SocialGatheringCollaborator Collab;
        public Dictionary<int, SocialGatheringCollaborator> Collaborators;

        public EditSaradnik(SocialGatheringCollaborator collab)
        {
            this.Collaborators = backend.Backend.LoadCollaborators();
            this.Collab = collab;
            this.DataContext = collab;

            InitializeComponent();
            this.name.Text = this.Collab.Name;
            this.description.Text = this.Collab.Description;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string messageBoxText = $"Da li ste sigurni da želite da izmenite saradnika {this.Collab.Name}?";
            string caption = "Izmena saradnika";
            this.Collab.Name = this.name.Text;
            this.Collab.Description = this.description.Text;

            BindingExpression binding = name.GetBindingExpression(TextBox.TextProperty);
            binding.UpdateSource();

            if (this.Collab.Name == "" || this.Collab.Name is null)
                return;
            MessageBoxButton btn = MessageBoxButton.YesNo;
            MessageBoxImage img = MessageBoxImage.Question;

            var result = MessageBox.Show(messageBoxText, caption, btn, img, MessageBoxResult.No);

            if (result == MessageBoxResult.Yes)
            {
                this.Collaborators[this.Collab.Id] = this.Collab;
                backend.Backend.SaveCollaborators(this.Collaborators);
            }

            NavigationService.Navigate(new AccountsList());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AccountsList());
        }
    }
}