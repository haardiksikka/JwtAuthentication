namespace JwtAuthentication.application.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Jwt100 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Guid(nullable: false, identity: true),
                        Username = c.String(),
                        Displayname = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.UserTokens",
                c => new
                    {
                        TokenId = c.Guid(nullable: false, identity: true),
                        AccessToken = c.String(),
                        AccessTokenExpirationTime = c.DateTimeOffset(nullable: false, precision: 7),
                        RefreshToken = c.String(),
                        RefreshTokenExpirationTime = c.DateTimeOffset(nullable: false, precision: 7),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.TokenId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserTokens", "UserId", "dbo.Users");
            DropIndex("dbo.UserTokens", new[] { "UserId" });
            DropTable("dbo.UserTokens");
            DropTable("dbo.Users");
        }
    }
}
