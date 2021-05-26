using System.Collections.ObjectModel;

namespace OrganizeIt.backend.social_gatherings
{
    public class SocialGatheringCategorySuggestion
    {
        public SocialGathering SocialGathering { get; set; }

        public string CategoryTitle { get; set; }
        public ObservableCollection<SocialGatheringCollaborator> SuggestedCollaborators { get; set; }


        // dodato po isidorinoj sugestiji
        public string Id { get; set; }
    }
}