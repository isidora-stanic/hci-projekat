using System;
using System.Collections.Generic;
using OrganizeIt.backend.users;

namespace OrganizeIt.backend.social_gatherings
{
    public class SocialGathering
    {
        public DateTime RequestDate { get; set; }

        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime DateTime { get; set; }
        public int NumberOfGuests { get; set; }
        public string Description { get; set; }

        // spisak imena gostiju koji nastaje prilikom importovanja CSV fajla
        // izmeniti po potrebi
        public List<string> GuestList { get; set; }

        public User Organizer { get; set; }
        public User Client { get; set; }

        public List<SocialGatheringSuggestion> EventSuggestions { get; set; }
    }
}