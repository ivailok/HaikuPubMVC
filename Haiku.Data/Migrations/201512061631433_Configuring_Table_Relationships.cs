namespace Haiku.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Configuring_Table_Relationships : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ratings", "HaikuId", c => c.Int(nullable: false));
            AlterColumn("dbo.Ratings", "Value", c => c.Int(nullable: false));
            CreateIndex("dbo.Ratings", "HaikuId");
            AddForeignKey("dbo.Ratings", "HaikuId", "dbo.Haikus", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ratings", "HaikuId", "dbo.Haikus");
            DropIndex("dbo.Ratings", new[] { "HaikuId" });
            AlterColumn("dbo.Ratings", "Value", c => c.Double(nullable: false));
            DropColumn("dbo.Ratings", "HaikuId");
        }
    }
}
