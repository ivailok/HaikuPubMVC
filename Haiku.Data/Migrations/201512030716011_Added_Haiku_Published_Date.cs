namespace Haiku.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Haiku_Published_Date : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Haikus", "DatePublished", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Haikus", "DatePublished");
        }
    }
}
