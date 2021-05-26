using OrganizeIt.backend.users;

namespace OrganizeIt.backend.todo
{
    public class ToDoCard
    {
        public ToDoStatus Status { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        //obrisati ako je viska
        public User Organizer { get; set; }
    }
}