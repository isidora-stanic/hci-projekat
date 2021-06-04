using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OrganizeIt.backend.social_gatherings
{
    public class SocialGatheringSuggestionReply
    {
        public DateTime ReplyDate { get; set; }

        [JsonIgnore]
        public SocialGatheringSuggestion SocialGatheringSuggestion { get; set; }

        public Dictionary<SocialGatheringCategorySuggestion, string> CategoryComments { get; set; }

        // true ako su prihvaceni svi predlozi
        public bool SuggestionsAccepted { get; set; }
    }
}