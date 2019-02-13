namespace Sports.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RowVersion1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Challenges", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Challenges", "RowVersion");
        }
    }
}
