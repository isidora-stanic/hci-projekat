using System;
using System.Collections.Generic;

namespace OrganizeIt.backend.social_gatherings
{
    public class SocialGatheringSuggestionReply
    {
        public DateTime ReplyDate { get; set; }
        public SocialGatheringSuggestion SocialGatheringSuggestion { get; set; }

        public Dictionary<SocialGatheringCategorySuggestion, SocialGatheringCollaborator> AcceptedCollaborators
        {
            get;
            set;
        }

        public Dictionary<SocialGatheringCategorySuggestion, string> CategoryComments { get; set; }


        // true ako su prihvaceni svi predlozi
        public bool AllAccepted { get; set; }
    }
}