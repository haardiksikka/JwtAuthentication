using JwtAuthentication.application.Database.Context;
using JwtAuthentication.application.Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace JwtAuthentication.application.Database.Repository
{
    public class UserDal
    {
        public UserDal()
        {

        }

        public User FindUserAsync(string username, string password)
        {
            using(var context = new ApplicationContext())
            {
                return context.User.FirstOrDefault(x => x.Username == username && x.Password.Equals(password));
            }
        }
    }
}
