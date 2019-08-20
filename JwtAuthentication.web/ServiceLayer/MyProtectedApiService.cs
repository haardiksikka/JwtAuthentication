using JwtAuthentication.endpoints.EndPoint;
using JwtAuthentication.web.Filters;
using ServiceStack.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JwtAuthentication.web.ServiceLayer
{
    [AuthenticationFilter]
    public class MyProtectedApiService : Service
    {
        public string GET(MyProtectedApi api)
        {
            return "Hello from protected api";
        }
    }
}