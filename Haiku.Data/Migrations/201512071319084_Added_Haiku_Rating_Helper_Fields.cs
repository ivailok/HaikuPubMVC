namespace Haiku.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Haiku_Rating_Helper_Fields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Haikus", "RatingsSum", c => c.Int(nullable: false));
            AddColumn("dbo.Haikus", "RatingsCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Haikus", "RatingsCount");
            DropColumn("dbo.Haikus", "RatingsSum");
        }
    }
}
