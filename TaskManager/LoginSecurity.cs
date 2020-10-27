using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskManager_DAL;

namespace TaskManager
{
    public class LoginSecurity
    {
        public static bool Login(string username, string password)
        {
            using (TaskManagerEntities entities = new TaskManagerEntities())
            {
                return entities.Users.Any(user =>
                       user.username.Equals(username, StringComparison.OrdinalIgnoreCase)
                                          && user.password == password);
            }
        }
    }
}



