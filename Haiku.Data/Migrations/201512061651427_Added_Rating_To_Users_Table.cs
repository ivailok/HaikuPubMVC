namespace Haiku.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Rating_To_Users_Table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Rating", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Rating");
        }
    }
}
