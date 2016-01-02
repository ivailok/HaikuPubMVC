namespace Haiku.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IX_User_Nickname : DbMigration
    {
        public override void Up()
        {
            CreateIndex("Users", "Nickname", true, "IX_User_Nickname");
        }
        
        public override void Down()
        {
            DropIndex("Users", "IX_User_Nickname");
        }
    }
}
