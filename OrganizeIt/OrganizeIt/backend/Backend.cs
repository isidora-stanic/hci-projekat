using OrganizeIt.backend.social_gatherings;
using OrganizeIt.backend.users;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace OrganizeIt.backend
{
    public class Backend
    {
        private static string DataDir = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory())) +
                          Path.DirectorySeparatorChar + "backend" + Path.DirectorySeparatorChar + "data" +
                          Path.DirectorySeparatorChar;

        private static JsonSerializerOptions serializerOptions = new JsonSerializerOptions { WriteIndented = true };

        public static Dictionary<string, User> Users { get; set; }
        public static Dictionary<int, SocialGatheringCollaborator> Collaborators { get; set; }
        public static User LoggedInUser { get; set; }

        public Backend()
        {
            Users = LoadUsers();
            Collaborators = LoadCollaborators();
        }

        public static Dictionary<string, User> LoadUsers()
        {
            var usersDataDir = DataDir + "users.json";
            var jsonString = File.ReadAllText(usersDataDir);
            return JsonSerializer.Deserialize<Dictionary<string, User>>(jsonString);
        }

        public static void SaveUsers(Dictionary<string, User> usersDict)
        {
            var usersDataDir = DataDir + "users.json";
            var usersString = JsonSerializer.Serialize(usersDict, serializerOptions);
            File.WriteAllText(usersDataDir, usersString);
        }

        public static User LogIn(string username, string password, Dictionary<string, User> usersDict)
        {
            if (usersDict.ContainsKey(username))
            {
                var user = usersDict[username];
                if (user.Password == password)
                {
                    LoggedInUser = user;
                    return user;
                }
            }

            return null;
        }

        public static void SaveCollaborators(Dictionary<int, SocialGatheringCollaborator> collaboratorsDict)
        {
            var collaboratorsDataDir = DataDir + "collaborators.json";
            var collaboratorsString = JsonSerializer.Serialize(collaboratorsDict, serializerOptions);
            File.WriteAllText(collaboratorsDataDir, collaboratorsString);
        }

        public static Dictionary<int, SocialGatheringCollaborator> LoadCollaborators()
        {
            var collaboratorsDataDir = DataDir + "collaborators.json";
            var jsonString = File.ReadAllText(collaboratorsDataDir);
            return JsonSerializer.Deserialize<Dictionary<int, SocialGatheringCollaborator>>(jsonString);
        }

        static string GetUserImagePath(string userName)
        {
            return DataDir + "img" + Path.DirectorySeparatorChar + "users" + Path.DirectorySeparatorChar + userName + ".jpg";
        }

        static string GetUserImagePath(User user)
        {
            return DataDir + "img" + Path.DirectorySeparatorChar + "users" + Path.DirectorySeparatorChar + user.Username + ".jpg";
        }

        static string GetCollaboratorImagePath(int collaboratorID)
        {
            return DataDir + "img" + Path.DirectorySeparatorChar + "collaborators" + Path.DirectorySeparatorChar + collaboratorID + ".jpg";
        }

        static string GetCollaboratorImagePath(SocialGatheringCollaborator collaborator)
        {
            return DataDir + "img" + Path.DirectorySeparatorChar + "collaborators" + Path.DirectorySeparatorChar + collaborator.Id + ".jpg";
        }

        static int GenerateCollaboratorID()
        {
            return Collaborators.Count;
        }

        static List<User> searchUsers(string searchTerm)
        {
            var searchTerm2 = searchTerm.ToLower();
            var users = from user in Users.Values
                        where (user.Address.City.ToLower().Contains(searchTerm2)
                        || user.Address.StreetAddress.Contains(searchTerm2)
                        || user.BirthDate.ToString().ToLower().Contains(searchTerm2)
                        || user.PhoneNumber.ToLower().Contains(searchTerm2)
                        || user.FirstName.ToLower().Contains(searchTerm2)
                        || user.LastName.ToLower().Contains(searchTerm2)
                        || user.Username.ToLower().Contains(searchTerm2)
                        || user.Email.ToLower().Contains(searchTerm2)
                        )
                        select user;
            return new List<User>(users);
        }

        static List<SocialGatheringCollaborator> searchCollaborators(string searchTerm)
        {
            var searchTerm2 = searchTerm.ToLower();
            var collaborators = from collaborator in Collaborators.Values
                                where (collaborator.Name.ToLower().Contains(searchTerm2) || collaborator.Description.ToLower().Contains(searchTerm2))
                                select collaborator;
            return new List<SocialGatheringCollaborator>(collaborators);
        }

        static List<User> getUsersOfType(UserType userType)
        {
            var users = from user in Users.Values
                        where user.UserType == userType
                        select user;
            return new List<User>(users);
        }

        static void saveSocialGatherings(Dictionary<string, User> usersDict)
        {
            var clientsList = getUsersOfType(UserType.Client);
            var socialGatheringsDict = new Dictionary<int, SocialGathering>();
            var num = 0;
            foreach (var client in clientsList)
            {
                foreach (var socialGathering in client.SocialGatherings)
                {
                    socialGatheringsDict.Add(num, socialGathering);
                    num++;
                }
            }

            var socialGatheringsDataDir = DataDir + Path.DirectorySeparatorChar + "social_gatherings.json";
            var socialGatheringsString = JsonSerializer.Serialize(socialGatheringsDict, serializerOptions);
            File.WriteAllText(socialGatheringsDataDir, socialGatheringsString);
        }

        static void loadSocialGatherings(Dictionary<string, User> usersDict)
        {
            var socialGatheringsDataDir = DataDir + Path.DirectorySeparatorChar + "social_gatherings.json";
            var jsonString = File.ReadAllText(socialGatheringsDataDir);
            var socialGatheringsDict = JsonSerializer.Deserialize<Dictionary<int, SocialGathering>>(jsonString);

            foreach (var socialGathering in socialGatheringsDict.Values)
            {
                var client = usersDict[socialGathering.ClientUsername];
                socialGathering.Client = client;

                var organizer = usersDict[socialGathering.OrganizerUsername];
                socialGathering.Organizer = organizer;

                client.SocialGatherings.Add(socialGathering);
                organizer.SocialGatherings.Add(socialGathering);

                foreach (var suggestion in socialGathering.SocialGatheringSuggestions)
                {
                    suggestion.SocialGathering = socialGathering;
                    foreach (var reply in suggestion.SuggestionReplies)
                    {
                        reply.SocialGatheringSuggestion = suggestion;
                        client.SocialGatheringSuggestionReplies.Add(reply);
                        organizer.SocialGatheringSuggestionReplies.Add(reply);
                    }
                    client.SocialGatheringSuggestions.Add(suggestion);
                    organizer.SocialGatheringSuggestions.Add(suggestion);
                }
            }
        }
    }
}