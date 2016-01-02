namespace Haiku.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IX_User_AccessToken : DbMigration
    {
        public override void Up()
        {
            CreateIndex("Users", "AccessToken", true, "IX_User_AccessToken");
        }
        
        public override void Down()
        {
            DropIndex("Users", "IX_User_AccessToken");
        }
    }
}
