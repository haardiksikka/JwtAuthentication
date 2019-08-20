using Microsoft.IdentityModel.Tokens;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace JwtAuthentication.web.Filters
{
    public class AuthenticationFilter : RequestFilterAttribute
    {
        private static string secret = "XCAP05H6LoKvbRRa/QkqLNMI7cOHguaRyHzyg7n5qEkGjQmtBhz4SzYh4Fqwjyi3KJHlSXKPwVu2+bXr6CtpgQ==";
        public override void Execute(IHttpRequest req, IHttpResponse res, object requestDto)
        {
            var token = req.Headers.Get("Authorize");
            if (string.IsNullOrEmpty(token))
            {
                res.ReturnAuthRequired("The user is not authorize");
                res.Close();
                return;
            }
            try
            {
                ClaimsPrincipal principal = GetPrincipal(token);
                if (principal == null)
                    return;
                ClaimsIdentity identity = null;
                identity = (ClaimsIdentity)principal.Identity;

                Claim roleClaim = identity.FindFirst(ClaimTypes.Role);
                var isAuthorized = false;
                if (roleClaim.Value == "Admin")
                {
                    isAuthorized = true;
                }
                if (!isAuthorized)
                {
                    res.ReturnAuthRequired("You are not authorized");
                    res.Close();
                    return;
                }
            }
            catch(SecurityTokenExpiredException e)
            {
                res.Write("Token expired");
                res.Close();
                return;
            }
            catch (Exception)
            {
                res.Write("Invalid Token");
                res.Close();
                return;
            }
        }
        public ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                if (jwtToken == null)
                    return null;
                byte[] key = Convert.FromBase64String(secret);
                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                SecurityToken securityToken = new JwtSecurityToken();
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, parameters, out securityToken);
                return principal;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}