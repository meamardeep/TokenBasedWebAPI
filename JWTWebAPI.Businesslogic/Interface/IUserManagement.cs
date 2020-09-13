using System;
using System.Collections.Generic;
using System.Text;
using JWTWebAPI.DataAccess;

namespace JWTWebAPI.Businesslogic
{
    public interface IUserManagement
    {
        void CreateUser(User user);
        User GetUser(string userName, string Password);
    }
}
