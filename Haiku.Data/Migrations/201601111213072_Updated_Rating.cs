namespace Haiku.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Updated_Rating : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Ratings");
            AddColumn("dbo.Ratings", "UserId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Ratings", new[] { "UserId", "HaikuId" });
            DropColumn("dbo.Ratings", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Ratings", "Id", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.Ratings");
            DropColumn("dbo.Ratings", "UserId");
            AddPrimaryKey("dbo.Ratings", "Id");
        }
    }
}
