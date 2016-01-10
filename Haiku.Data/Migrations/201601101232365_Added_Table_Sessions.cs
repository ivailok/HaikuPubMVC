namespace Haiku.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Table_Sessions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sessions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Token = c.String(nullable: false, maxLength: 88),
                        Nickname = c.String(nullable: false, maxLength: 20),
                        From = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Token, unique: true);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Sessions", new[] { "Token" });
            DropTable("dbo.Sessions");
        }
    }
}
