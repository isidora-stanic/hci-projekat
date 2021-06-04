using OrganizeIt.backend.users;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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

        // potrebno zbog ucitavanja
        public string OrganizerUsername { get; set; }

        public string ClientUsername { get; set; }

        [JsonIgnore]
        public User Organizer { get { return Organizer; } set { Organizer = value; OrganizerUsername = value.Username; } }

        [JsonIgnore]
        public User Client { get { return Client; } set { Client = value; OrganizerUsername = value.Username; } }

        public List<SocialGatheringSuggestion> SocialGatheringSuggestions { get; set; }
    }
}