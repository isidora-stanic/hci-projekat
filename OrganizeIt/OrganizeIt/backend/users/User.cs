using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using OrganizeIt.backend.social_gatherings;
using OrganizeIt.backend.todo;

namespace OrganizeIt.backend.users
{
    public class User
    {
        public UserType UserType { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Location Address { get; set; }
        public Gender Gender { get; set; }
        public DateTime BirthDate { get; set; }

        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        [JsonIgnore]
        public List<SocialGathering> SocialGatherings { get; set; }

        [JsonIgnore]
        public List<SocialGatheringSuggestion> SocialGatheringSuggestions { get; set; }

        [JsonIgnore]
        public List<SocialGatheringSuggestionReply> SocialGatheringSuggestionReplies { get; set; }

        // samo za organizatore, null ako je client
        public List<ToDoCard> ToDoCards { get; set; }
    }
}