using JwtAuthentication.application.Database.Model;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthentication.endpoints.EndPoint
{
    [Route("/newtoken","POST")]
    public class NewToken
    {
        public string RefreshToken { get; set; }
        public User User { get; set; }
    }
}
