using OrganizeIt.backend.users;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace OrganizeIt.backend
{
    public class Backend
    {
        public static Dictionary<string, User> LoadUsers()
        {
            var dataDir = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory())) +
                          Path.DirectorySeparatorChar + "backend" + Path.DirectorySeparatorChar + "data" +
                          Path.DirectorySeparatorChar;

            var usersDataDir = dataDir + "users.json";
            var jsonString = File.ReadAllText(usersDataDir);
            return JsonSerializer.Deserialize<Dictionary<string, User>>(jsonString);
        }

        public static void SaveUsers(Dictionary<string, User> usersDict)
        {
            var dataDir = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory())) +
                          Path.DirectorySeparatorChar + "backend" + Path.DirectorySeparatorChar + "data" +
                          Path.DirectorySeparatorChar;
            var usersDataDir = dataDir + "users.json";
            var usersString = JsonSerializer.Serialize(usersDict);
            File.WriteAllText(usersDataDir, usersString);
        }

        public static User LogIn(string username, string password, Dictionary<string, User> usersDict)
        {
            if (usersDict.ContainsKey(username))
            {
                var user = usersDict[username];
                if (user.Password == password)
                {
                    return user;
                }
            }

            return null;
        }
    }
}