using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JwtAuthentication.application.Database.Model
{
    public class UserToken
    {
        [Key]
        public Guid TokenId { get; set; }
        public string AccessToken { get; set; }
        public DateTimeOffset AccessTokenExpirationTime { get; set; }
        public string RefreshToken { get; set; }
        public DateTimeOffset RefreshTokenExpirationTime { get; set; }

       
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
