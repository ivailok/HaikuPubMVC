namespace Haiku.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_User_AccessToken_Length_To_128 : DbMigration
    {
        public override void Up()
        {
            DropIndex("Users", "IX_User_AccessToken");
            AlterColumn("dbo.Users", "AccessToken", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("Users", "AccessToken", true, "IX_User_AccessToken");
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "AccessToken", c => c.String(nullable: false, maxLength: 512));
        }
    }
}
