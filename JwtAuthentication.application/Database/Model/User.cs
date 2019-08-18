using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthentication.application.Database.Model
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Displayname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public bool IsActive { get; set; }

    }
}
