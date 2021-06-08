using OrganizeIt.backend.social_gatherings;
using OrganizeIt.backend.todo;
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
            loadSocialGatherings();
        }

        public static void LoadAll()
        {
            Users = LoadUsers();
            Collaborators = LoadCollaborators();
            loadSocialGatherings();
        }

        public static void SaveAll()
        {
            SaveUsers();
            SaveCollaborators();
            saveSocialGatherings();
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

        public static void SaveUsers()
        {
            var usersDataDir = DataDir + "users.json";
            var usersString = JsonSerializer.Serialize(Users, serializerOptions);
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
                    var loggedInUserDataDir = DataDir + "logged_in_user.txt";
                    File.WriteAllText(loggedInUserDataDir, username);
                    return user;
                }
            }

            return null;
        }

        public static User GetLoggedInUser(Dictionary<string, User> usersDict)
        {
            var loggedInUserDataDir = DataDir + "logged_in_user.txt";
            var username = File.ReadAllText(loggedInUserDataDir);
            return usersDict[username];
        }

        public static void SaveCollaborators(Dictionary<int, SocialGatheringCollaborator> collaboratorsDict)
        {
            var collaboratorsDataDir = DataDir + "collaborators.json";
            var collaboratorsString = JsonSerializer.Serialize(collaboratorsDict, serializerOptions);
            File.WriteAllText(collaboratorsDataDir, collaboratorsString);
        }

        public static void SaveCollaborators()
        {
            var collaboratorsDataDir = DataDir + "collaborators.json";
            var collaboratorsString = JsonSerializer.Serialize(Collaborators, serializerOptions);
            File.WriteAllText(collaboratorsDataDir, collaboratorsString);
        }

        public static Dictionary<int, SocialGatheringCollaborator> LoadCollaborators()
        {
            var collaboratorsDataDir = DataDir + "collaborators.json";
            var jsonString = File.ReadAllText(collaboratorsDataDir);
            return JsonSerializer.Deserialize<Dictionary<int, SocialGatheringCollaborator>>(jsonString);
        }

        public static string GetUserImagePath(string userName)
        {
            return DataDir + "img" + Path.DirectorySeparatorChar + "users" + Path.DirectorySeparatorChar + userName + ".jpg";
        }

        public static string GetUserImagePath(User user)
        {
            return DataDir + "img" + Path.DirectorySeparatorChar + "users" + Path.DirectorySeparatorChar + user.Username + ".jpg";
        }

        public static string GetCollaboratorImagePath(int collaboratorID)
        {
            return DataDir + "img" + Path.DirectorySeparatorChar + "collaborators" + Path.DirectorySeparatorChar + collaboratorID + ".jpg";
        }

        public static string GetCollaboratorImagePath(SocialGatheringCollaborator collaborator)
        {
            return DataDir + "img" + Path.DirectorySeparatorChar + "collaborators" + Path.DirectorySeparatorChar + collaborator.Id + ".jpg";
        }

        public static string GetTableImagePath()
        {
            return DataDir + "img" + Path.DirectorySeparatorChar + "table" + Path.DirectorySeparatorChar + "astal.jpg";
        }

        public static int GenerateCollaboratorID()
        {
            return Collaborators.Count;
        }

        public static List<User> searchUsers(string searchTerm)
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

        public static List<SocialGatheringCollaborator> searchCollaborators(string searchTerm)
        {
            var searchTerm2 = searchTerm.ToLower();
            var collaborators = from collaborator in Collaborators.Values
                                where (collaborator.Name.ToLower().Contains(searchTerm2) || collaborator.Description.ToLower().Contains(searchTerm2))
                                select collaborator;
            return new List<SocialGatheringCollaborator>(collaborators);
        }

        public static List<User> getUsersOfType(UserType userType, Dictionary<string, User> usersDict)
        {
            var users = from user in usersDict.Values
                        where user.UserType == userType
                        select user;
            return new List<User>(users);
        }

        public static void saveSocialGatherings(Dictionary<string, User> usersDict)
        {
            var clientsList = getUsersOfType(UserType.Client, usersDict);
            var socialGatheringsDict = new Dictionary<int, SocialGathering>();
            var num = 0;
            foreach (var client in clientsList)
            {
                if (client.SocialGatherings == null)
                {
                    continue;
                }
                foreach (var socialGathering in client.SocialGatherings)
                {
                    socialGatheringsDict.Add(num, socialGathering);
                    num++;
                }
            }

            var socialGatheringsDataDir = DataDir + "social_gatherings.json";
            var socialGatheringsString = JsonSerializer.Serialize(socialGatheringsDict, serializerOptions);
            File.WriteAllText(socialGatheringsDataDir, socialGatheringsString);
        }

        public static void saveSocialGatherings()
        {
            var clientsList = getUsersOfType(UserType.Client, Users);
            var socialGatheringsDict = new Dictionary<int, SocialGathering>();
            var num = 0;
            foreach (var client in clientsList)
            {
                if (client.SocialGatherings == null)
                {
                    continue;
                }
                foreach (var socialGathering in client.SocialGatherings)
                {
                    socialGatheringsDict.Add(num, socialGathering);
                    num++;
                }
            }

            var socialGatheringsDataDir = DataDir + "social_gatherings.json";
            var socialGatheringsString = JsonSerializer.Serialize(socialGatheringsDict, serializerOptions);
            File.WriteAllText(socialGatheringsDataDir, socialGatheringsString);
        }

        public static Dictionary<int, SocialGathering> loadSocialGatherings(Dictionary<string, User> usersDict)
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
                    suggestion.Client = client;
                    suggestion.Organizer = organizer;

                    socialGathering.AcceptedSuggestions = false;

                    foreach (var reply in suggestion.SuggestionReplies)
                    {
                        reply.SocialGatheringSuggestion = suggestion;
                        client.SocialGatheringSuggestionReplies.Add(reply);
                        organizer.SocialGatheringSuggestionReplies.Add(reply);

                        if (reply.SuggestionsAccepted)
                            socialGathering.AcceptedSuggestions = reply.SuggestionsAccepted;
                    }
                    client.SocialGatheringSuggestions.Add(suggestion);
                    organizer.SocialGatheringSuggestions.Add(suggestion);
                }
            }

            return socialGatheringsDict;
        }

        public static void loadSocialGatherings()
        {
            var socialGatheringsDataDir = DataDir + Path.DirectorySeparatorChar + "social_gatherings.json";
            var jsonString = File.ReadAllText(socialGatheringsDataDir);
            var socialGatheringsDict = JsonSerializer.Deserialize<Dictionary<int, SocialGathering>>(jsonString);

            foreach (var socialGathering in socialGatheringsDict.Values)
            {
                var client = Users[socialGathering.ClientUsername];
                socialGathering.Client = client;

                var organizer = Users[socialGathering.OrganizerUsername];
                socialGathering.Organizer = organizer;

                client.SocialGatherings.Add(socialGathering);
                organizer.SocialGatherings.Add(socialGathering);

                foreach (var suggestion in socialGathering.SocialGatheringSuggestions)
                {
                    suggestion.SocialGathering = socialGathering;
                    suggestion.Client = client;
                    suggestion.Organizer = organizer;
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

        public static void AddGathering(SocialGathering socialGathering)
        {
            LoadAll();
            Users[socialGathering.Client.Username].SocialGatherings.Add(socialGathering);
            Users[socialGathering.Organizer.Username].SocialGatherings.Add(socialGathering);
            SaveAll();
        }

        public static void AddSuggestion(SocialGatheringSuggestion socialGatheringSuggestion, SocialGathering socialGathering)
        {
            LoadAll();
            var clientUsername = socialGathering.Client.Username;
            var organizerUsername = socialGathering.Organizer.Username;

            var gatheringsClient = Users[clientUsername].SocialGatherings;

            foreach (var gathering in gatheringsClient)
            {
                if (gathering.RequestDate == socialGathering.RequestDate)
                {
                    gathering.SocialGatheringSuggestions.Add(socialGatheringSuggestion);
                }
            }

            var gatheringsOrganizer = Users[organizerUsername].SocialGatherings;

            /*foreach (var gathering in gatheringsOrganizer)
            {
                if (gathering.RequestDate == socialGathering.RequestDate)
                {
                    gathering.SocialGatheringSuggestions.Add(socialGatheringSuggestion);
                }
            }*/

            Users[clientUsername].SocialGatherings = gatheringsClient;
            Users[organizerUsername].SocialGatherings = gatheringsOrganizer;
            SaveAll();
        }

        public static void AddSuggestionReply(SocialGatheringSuggestionReply socialGatheringSuggestionReply, SocialGatheringSuggestion socialGatheringSuggestion)
        {
            LoadAll();
            var clientUsername = socialGatheringSuggestion.SocialGathering.Client.Username;
            var organizerUsername = socialGatheringSuggestion.SocialGathering.Organizer.Username;

            var gatheringsClient = Users[clientUsername].SocialGatherings;

            foreach (var gathering in gatheringsClient)
            {
                if (gathering.RequestDate == socialGatheringSuggestion.SocialGathering.RequestDate)
                {
                    foreach (var sugggestion in gathering.SocialGatheringSuggestions)
                    {
                        if (sugggestion.SuggestionDate == socialGatheringSuggestion.SuggestionDate)
                        {
                            sugggestion.SuggestionReplies.Add(socialGatheringSuggestionReply);
                        }
                    }
                }
            }

            var gatheringsOrganizer = Users[organizerUsername].SocialGatherings;

            /*foreach (var gathering in gatheringsOrganizer)
            {
                if (gathering.RequestDate == socialGatheringSuggestion.SocialGathering.RequestDate)
                {
                    foreach (var sugggestion in gathering.SocialGatheringSuggestions)
                    {
                        if (sugggestion.SuggestionDate == socialGatheringSuggestion.SuggestionDate)
                        {
                            sugggestion.SuggestionReplies.Add(socialGatheringSuggestionReply);
                        }
                    }
                }
            }*/

            Users[clientUsername].SocialGatherings = gatheringsClient;
            Users[organizerUsername].SocialGatherings = gatheringsOrganizer;
            SaveAll();
        }

        public static List<SocialGathering> GetGatheringsByStatus(List<SocialGathering> socialGatherings, bool acceptedSuggestions)
        {
            var gatherings = from gathering in socialGatherings
                             where gathering.AcceptedSuggestions == acceptedSuggestions
                             select gathering;
            return new List<SocialGathering>(gatherings);
        }

        public static void SaveTodoList(List<ToDoCard> toDoCards)
        {
            var dict = new Dictionary<string, List<ToDoCard>>();
            foreach (var card in toDoCards)
            {
                if (!dict.ContainsKey(card.Organizer.Username))
                    dict[card.Organizer.Username] = new List<ToDoCard>();
                dict[card.Organizer.Username].Add(card);
            }
            var todoDataDir = DataDir + "todo.json";
            var toDoString = JsonSerializer.Serialize(dict, serializerOptions);
            File.WriteAllText(todoDataDir, toDoString);
        }

        public static List<ToDoCard> LoadTodoList(Dictionary<string, User> usersDict)
        {
            var todoDataDir = DataDir + "todo.json";
            var jsonString = File.ReadAllText(todoDataDir);
            var todoCardDict = JsonSerializer.Deserialize<Dictionary<string, List<ToDoCard>>>(jsonString);

            var allTodos = new List<ToDoCard>();

            foreach (var organizerUsername in todoCardDict.Keys)
            {
                foreach (var todoCard in todoCardDict[organizerUsername])
                {
                    todoCard.Organizer = usersDict[organizerUsername];
                    allTodos.Add(todoCard);
                }
            }

            return allTodos;
        }

        public static List<ToDoCard> GetTodoCardByStatus(List<ToDoCard> todoCards, ToDoStatus status)
        {
            var toDoCards = from card in todoCards
                            where card.Status == status
                            select card;
            return new List<ToDoCard>(toDoCards);
        }

        public static List<ToDoCard> GetTodoCardByStatusForOrganizer(List<ToDoCard> todoCards, ToDoStatus status, string organizerUsername)
        {
            var toDoCards = from card in todoCards
                            where card.Status == status && card.Organizer.Username == organizerUsername
                            select card;
            return new List<ToDoCard>(toDoCards);
        }

        public static List<ToDoCard> GetToDoCardsForOrganizer(List<ToDoCard> todoCards, User organizer)
        {
            var toDoCards = from card in todoCards
                            where card.Organizer.Username == organizer.Username
                            select card;
            return new List<ToDoCard>(toDoCards);
        }

        public static List<ToDoCard> GetToDoCardsForOrganizer(List<ToDoCard> todoCards, string organizerUsername)
        {
            var toDoCards = from card in todoCards
                            where card.Organizer.Username == organizerUsername
                            select card;
            return new List<ToDoCard>(toDoCards);
        }

        public static List<string> LoadGuestsFromCSV(string csv_path)
        {
            var lines = File.ReadAllLines(csv_path);
            var guests = lines[0].Split(',');
            return new List<string>(guests);
        }
    }
}