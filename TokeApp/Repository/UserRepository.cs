using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TokeApp.Models;

namespace TokeApp.Repository
{
    public class UserRepository
    {        
        public List<User> users;

        public UserRepository()
        {
            users = new List<User>
            {
                new User() { Username = "john", Password = "john1234" },
                new User() { Username = "allen", Password = "allen1234" }
            };
        }
        public User GetUser(string username)
        {
            return users.FirstOrDefault(user => user.Username == username);
        }
    }
}