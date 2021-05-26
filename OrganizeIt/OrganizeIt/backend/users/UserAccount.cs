using System;
using System.Text.Json.Serialization;

namespace OrganizeIt.backend.users
{
    public class UserAccount
    {
        // obrisati ako je visak
        public User AccountOwner { get; set; }
    }
}