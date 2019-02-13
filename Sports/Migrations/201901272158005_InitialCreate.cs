namespace Sports.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Challenges",
                c => new
                    {
                        ChallengeID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        Head = c.String(),
                        Description1 = c.String(),
                    })
                .PrimaryKey(t => t.ChallengeID);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        ClientID = c.Int(nullable: false, identity: true),
                        Head = c.String(),
                        Image = c.String(),
                        Description = c.String(),
                        PricingPlanID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ClientID)
                .ForeignKey("dbo.PricingPlans", t => t.PricingPlanID, cascadeDelete: true)
                .Index(t => t.PricingPlanID);
            
            CreateTable(
                "dbo.Hobbies",
                c => new
                    {
                        HobbyID = c.Int(nullable: false, identity: true),
                        TypeOfSportID = c.Int(nullable: false),
                        ClientID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.HobbyID)
                .ForeignKey("dbo.Clients", t => t.ClientID, cascadeDelete: true)
                .ForeignKey("dbo.TypeOfSports", t => t.TypeOfSportID, cascadeDelete: true)
                .Index(t => t.TypeOfSportID)
                .Index(t => t.ClientID);
            
            CreateTable(
                "dbo.TypeOfSports",
                c => new
                    {
                        TypeOfSportID = c.Int(nullable: false, identity: true),
                        Head = c.String(),
                        Description = c.String(),
                        Image = c.String(),
                        Image1 = c.String(),
                        SportID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TypeOfSportID)
                .ForeignKey("dbo.Sports", t => t.SportID, cascadeDelete: true)
                .Index(t => t.SportID);
            
            CreateTable(
                "dbo.Sports",
                c => new
                    {
                        SportID = c.Int(nullable: false, identity: true),
                        Head1 = c.String(),
                        Description = c.String(),
                        Head2 = c.String(),
                        Image = c.String(),
                    })
                .PrimaryKey(t => t.SportID);
            
            CreateTable(
                "dbo.PricingPlans",
                c => new
                    {
                        PricingPlanID = c.Int(nullable: false, identity: true),
                        Head = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Head1 = c.String(),
                        Head2 = c.String(),
                    })
                .PrimaryKey(t => t.PricingPlanID);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        ImageID = c.Int(nullable: false, identity: true),
                        Image1 = c.String(),
                    })
                .PrimaryKey(t => t.ImageID);
            
            CreateTable(
                "dbo.Subscribes",
                c => new
                    {
                        SubscribeID = c.Int(nullable: false, identity: true),
                        Head = c.String(),
                    })
                .PrimaryKey(t => t.SubscribeID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Clients", "PricingPlanID", "dbo.PricingPlans");
            DropForeignKey("dbo.TypeOfSports", "SportID", "dbo.Sports");
            DropForeignKey("dbo.Hobbies", "TypeOfSportID", "dbo.TypeOfSports");
            DropForeignKey("dbo.Hobbies", "ClientID", "dbo.Clients");
            DropIndex("dbo.TypeOfSports", new[] { "SportID" });
            DropIndex("dbo.Hobbies", new[] { "ClientID" });
            DropIndex("dbo.Hobbies", new[] { "TypeOfSportID" });
            DropIndex("dbo.Clients", new[] { "PricingPlanID" });
            DropTable("dbo.Subscribes");
            DropTable("dbo.Images");
            DropTable("dbo.PricingPlans");
            DropTable("dbo.Sports");
            DropTable("dbo.TypeOfSports");
            DropTable("dbo.Hobbies");
            DropTable("dbo.Clients");
            DropTable("dbo.Challenges");
        }
    }
}
