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

        private User _organizer;
        private User _client;

        [JsonIgnore]
        public User Organizer { get { return _organizer; } set { _organizer = value; OrganizerUsername = _organizer.Username; } }

        [JsonIgnore]
        public User Client { get { return _client; } set { _client = value; ClientUsername = _client.Username; } }

        public List<SocialGatheringSuggestion> SocialGatheringSuggestions { get; set; }

        public bool AcceptedSuggestions { get; set; }
    }
}