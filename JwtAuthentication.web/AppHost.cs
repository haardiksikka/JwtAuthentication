using ServiceStack.Text;
using ServiceStack.WebHost.Endpoints;
using System;
using System.Collections.Generic;
using Funq;
using System.Linq;
using System.Web;
using JwtAuthentication.web.ServiceLayer;

namespace JwtAuthentication.web
{
    public class AppHost : AppHostBase
    {
        public AppHost() : base("JwtAuthentication.web", typeof(UserLogin).Assembly) { }
        public override void Configure(Container container)
        {
            JsConfig.EmitCamelCaseNames = true;
        }
    }
}