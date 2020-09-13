using System;
using System.Linq;
using JWTWebAPI.DataAccess;

namespace JWTWebAPI.Businesslogic
{
    public class UserManagement : IUserManagement
    {
        private readonly APIDbContext _database;
        public UserManagement(APIDbContext context)
        {
            _database = context;
        }
        public void CreateUser(User user)
        {
            _database.Users.Add(user);
            _database.SaveChanges();
        }

        public User GetUser(string userName, string password)
        {
            User user = _database.Users.Where(u => u.Username == userName && u.Password == password).FirstOrDefault();
            return user;
        }
    }
}
