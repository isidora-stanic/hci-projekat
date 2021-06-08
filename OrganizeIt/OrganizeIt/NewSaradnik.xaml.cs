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
    /// Interaction logic for NewSaradnik.xaml
    /// </summary>
    public partial class NewSaradnik : Page
    {
        public NewSaradnik()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(this.username.Text == "")
            {
                BindingExpression binding = username.GetBindingExpression(TextBox.TextProperty);
                binding.UpdateSource();
                return;
            }
            SocialGatheringCollaborator sgc = new SocialGatheringCollaborator();
            sgc.Description = this.description.Text;
            sgc.Name = this.username.Text;

            Dictionary<int, SocialGatheringCollaborator> collaborators = backend.Backend.LoadCollaborators();
            sgc.Id = collaborators.Last().Key + 1;
            collaborators.Add(sgc.Id, sgc);

            backend.Backend.Collaborators = collaborators;

            backend.Backend.SaveCollaborators();
            NavigationService.Navigate(new AccountsList());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AccountsList());
        }
    }
}
