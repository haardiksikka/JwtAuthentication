using JwtAuthentication.application.Database.Repository;
using JwtAuthentication.application.JwtToken;
using JwtAuthentication.endpoints.EndPoint;
using ServiceStack.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JwtAuthentication.web.ServiceLayer
{
    public class GenerateNewToken : Service
    {
        private readonly UserDal userdal;
        private readonly TokenFactory token;
        public GenerateNewToken()
        {
            userdal = new UserDal();
            token = new TokenFactory();
        }
        public object POST(NewToken rtoken)
        {
            var oldRefreshToken = rtoken.RefreshToken;
            var isValid = token.ValidateAndDeleteRefreshToken(t => t.RefreshToken == oldRefreshToken);
            if (isValid)
            {

                var result = token.CreateJwtTokenAsync(rtoken.User);
                return new { access_token = result.AccessToken, refresh_token = result.RefreshToken };
            }
            else
            {
                return "Invalid Token";
            }
        }
    }
}