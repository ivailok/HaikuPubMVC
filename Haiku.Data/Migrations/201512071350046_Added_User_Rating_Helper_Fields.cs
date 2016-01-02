namespace Haiku.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_User_Rating_Helper_Fields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "HaikusRatingSum", c => c.Double(nullable: false));
            AddColumn("dbo.Users", "HaikusCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "HaikusCount");
            DropColumn("dbo.Users", "HaikusRatingSum");
        }
    }
}
