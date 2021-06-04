using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using OrganizeIt.backend.users;

namespace OrganizeIt.backend.social_gatherings
{
    public class SocialGatheringSuggestion
    {
        public DateTime SuggestionDate { get; set; }

        [JsonIgnore]
        public SocialGathering SocialGathering { get; set; }

        public List<SocialGatheringCategorySuggestion> CategorySuggestions { get; set; }
        public SocialGatheringSeating SocialGatheringSeating { get; set; } // prazna klasa za sad

        // brisati ako je visak
        public User Organizer { get; set; }

        public User Client { get; set; }

        public List<SocialGatheringSuggestionReply> SuggestionReplies { get; set; }
    }
}