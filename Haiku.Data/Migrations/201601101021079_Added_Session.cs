namespace Haiku.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Session : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Password", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Users", "Salt", c => c.String(nullable: false, maxLength: 24));
            DropIndex("dbo.Users", "IX_User_AccessToken");
            DropColumn("dbo.Users", "AccessToken");
            DropColumn("dbo.Users", "Role");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Role", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "AccessToken", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Users", "AccessToken", true, "IX_User_AccessToken");
            DropColumn("dbo.Users", "Salt");
            DropColumn("dbo.Users", "Password");
        }
    }
}
