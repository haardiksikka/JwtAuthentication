namespace JwtAuthentication.application.Migrations
{
    using JwtAuthentication.application.Database.Model;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<JwtAuthentication.application.Database.Context.ApplicationContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(JwtAuthentication.application.Database.Context.ApplicationContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            var adminUser = new User
            {
                Username = "Vahid",
                Displayname = "Vaid",
                IsActive = true,
                Email = "vahid@jwt.com",
                Password = "1234",
            };
            context.User.Add(adminUser);
            context.SaveChanges();
        }
    }
}
