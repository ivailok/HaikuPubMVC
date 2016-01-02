namespace Haiku.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Created_Reports_Table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Reason = c.String(),
                        DateSent = c.DateTime(nullable: false),
                        HaikuId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Haikus", t => t.HaikuId, cascadeDelete: true)
                .Index(t => t.HaikuId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reports", "HaikuId", "dbo.Haikus");
            DropIndex("dbo.Reports", new[] { "HaikuId" });
            DropTable("dbo.Reports");
        }
    }
}
