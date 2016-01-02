namespace Haiku.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRatingToHaikuEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Haikus", "Rating", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Haikus", "Rating");
        }
    }
}
