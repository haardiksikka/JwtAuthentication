using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JwtAuthentication.application.Database.Model;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using JwtAuthentication.application.Database.Context;

namespace JwtAuthentication.application.JwtToken
{
    public class TokenFactory
    {
        public class JwtTokensData
        {
            public string AccessToken { get; set; }
            public string RefreshToken { get; set; }
            public string RefreshTokenSerial { get; set; }
            public IEnumerable<Claim> Claims { get; set; }
        }
        public class BearerTokensOptions
        {
            public string Key { set; get; }
            public string Issuer { set; get; }
            public string Audience { set; get; }
            public int AccessTokenExpirationMinutes { set; get; }
            public int RefreshTokenExpirationMinutes { set; get; }
            public bool AllowMultipleLoginsFromTheSameUser { set; get; }
            public bool AllowSignoutAllUserActiveClients { set; get; }
        }
        public BearerTokensOptions configuration;
        private static string secret = "XCAP05H6LoKvbRRa/QkqLNMI7cOHguaRyHzyg7n5qEkGjQmtBhz4SzYh4Fqwjyi3KJHlSXKPwVu2+bXr6CtpgQ==";

        public TokenFactory()
        {
            configuration = new BearerTokensOptions();
        }

        public JwtTokensData CreateJwtTokenAsync(User user)
        {
            var (accessToken, claims) =  createAccessTokenAsync(user);
            var (refreshTokenValue, refreshTokenSerial) = createRefreshToken();
            var now = DateTimeOffset.UtcNow;
            var token = new UserToken
            {
                UserId = user.UserId,
                // Refresh token handles should be treated as secrets and should be stored hashed
              //  RefreshTokenIdHash = _securityService.GetSha256Hash(refreshTokenSerial),
                RefreshToken = refreshTokenValue,
                AccessToken = accessToken,
                RefreshTokenExpirationTime = now.AddMinutes(60),
                AccessTokenExpirationTime = now.AddMinutes(15)
            };
            using(var context = new ApplicationContext())
            {
                context.UserToken.Add(token);
                context.SaveChanges();
            }
            return new JwtTokensData
            {
                AccessToken = accessToken,
                RefreshToken = refreshTokenValue,
                RefreshTokenSerial = refreshTokenSerial,
                Claims = claims
            };
        }
        public Guid CreateCryptographicallySecureGuid()
        {
            RandomNumberGenerator _rand = RandomNumberGenerator.Create();
            var bytes = new byte[16];
            _rand.GetBytes(bytes);
            return new Guid(bytes);
        }
        private (string RefreshTokenValue, string RefreshTokenSerial) createRefreshToken()
        {
              var refreshTokenSerial = CreateCryptographicallySecureGuid().ToString().Replace("-", "");
            var Issuer = "Haardik Sikka";
            var claims = new List<Claim>
            {
                // Unique Id for all Jwt tokes
                new Claim(JwtRegisteredClaimNames.Jti, ClaimValueTypes.String, Issuer),
                // Issuer
                new Claim(JwtRegisteredClaimNames.Iss,Issuer, ClaimValueTypes.String),
                // Issued at
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64,Issuer),
                //// for invalidation
                //new Claim(ClaimTypes.SerialNumber, refreshTokenSerial, ClaimValueTypes.String,Issuer)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var now = DateTime.UtcNow;
            var token = new JwtSecurityToken(
                issuer: Issuer,
                audience: "My Audience",
                claims: claims,
                notBefore: now,
                expires: now.AddMinutes(60),
                signingCredentials: creds);
            var refreshTokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            return (refreshTokenValue, refreshTokenSerial);
        }
        private (string AccessToken, IEnumerable<Claim> Claims) createAccessTokenAsync(User user)
        {
            var Issuer = "Haardik Sikka";
            var claims = new List<Claim>
            {
                // Unique Id for all Jwt tokes
                //new Claim(JwtRegisteredClaimNames.Jti, ClaimValueTypes.String, Issuer),
                //// Issuer
                //new Claim(JwtRegisteredClaimNames.Iss, ClaimValueTypes.String, Issuer),
                //// Issued at
                //new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64, Issuer),
                //new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString(), ClaimValueTypes.String, configuration.Issuer),
                new Claim(ClaimTypes.Email, user.Email, ClaimValueTypes.String, Issuer),
                new Claim(ClaimTypes.Role,"Admin")
                
            };
            var key = new SymmetricSecurityKey(Convert.FromBase64String(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var now = DateTime.UtcNow;
            var token = new JwtSecurityToken(
                issuer: "Haardik Sikka",
                audience: "My Audience",
                claims: claims,
                notBefore: now,
                expires: now.AddMinutes(15),
                signingCredentials: creds);
            return (new JwtSecurityTokenHandler().WriteToken(token), claims);
        }

        public bool ValidateAndDeleteRefreshToken(Func<UserToken, bool> predicate)
        {
            var isValid = false;
            using(var context = new ApplicationContext())
            {
                var result = context.UserToken.FirstOrDefault(predicate);
                if(result != null)
                {
                    context.UserToken.Remove(result);
                    isValid = true;
                }
            }
            return isValid;
        }

       

        }
    }
