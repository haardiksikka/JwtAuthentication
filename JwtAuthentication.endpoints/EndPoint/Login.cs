using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JwtAuthentication.application.Database.Model;

namespace JwtAuthentication.endpoints.EndPoint
{
    [Route("/login","POST")]
    public class Login
    {
        public User User { get; set; }
    }
}
