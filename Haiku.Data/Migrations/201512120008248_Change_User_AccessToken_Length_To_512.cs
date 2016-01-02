namespace Haiku.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_User_AccessToken_Length_To_512 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "AccessToken", c => c.String(nullable: false, maxLength: 512));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "AccessToken", c => c.String(nullable: false, maxLength: 30));
        }
    }
}
