using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace login_system_2030.Models
{
    public class User // defines what makes a user (what a user is)
    {
        public string Username { get; set; }
        // username input

        public string Password { get; set; }
        // password input

        public string Email { get; set; }
        // email input

        public int Id { get; private set; }
        // id is used to navigate through the database (serves as the primary key)

        public User(string username, string password, string email) // a public users must have the properties(nocaps) in order to construct the given variables (caps)
        {
            Username = username;
            Password = password;
            Email = email;
        }
    }
}
