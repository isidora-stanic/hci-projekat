using OrganizeIt.backend.users;
using System.Text.Json.Serialization;

namespace OrganizeIt.backend.todo
{
    public class ToDoCard
    {
        public ToDoStatus Status { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        //obrisati ako je viska
        [JsonIgnore]
        public User Organizer { get; set; }
    }
}