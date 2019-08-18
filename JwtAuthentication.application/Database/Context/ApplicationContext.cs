using JwtAuthentication.application.Database.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace JwtAuthentication.application.Database.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext() : base("ApplicationContext")
        {

        }

        public DbSet<User> User { get; set; }
        public DbSet<UserToken> UserToken { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.UserId);
            modelBuilder.Entity<User>().Property(d => d.UserId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<UserToken>().HasKey(d => d.TokenId);
            modelBuilder.Entity<UserToken>().Property(d => d.TokenId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }

}
