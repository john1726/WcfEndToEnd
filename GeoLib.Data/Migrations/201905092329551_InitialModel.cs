namespace GeoLib.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.State",
                c => new
                    {
                        StateId = c.Int(nullable: false, identity: true),
                        Abbreviation = c.String(),
                        Name = c.String(),
                        IsPrimaryState = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.StateId);
            
            CreateTable(
                "dbo.ZipCode",
                c => new
                    {
                        ZipCodeId = c.Int(nullable: false, identity: true),
                        City = c.String(),
                        StateId = c.Int(nullable: false),
                        Zip = c.String(),
                        County = c.String(),
                        AreaCode = c.Int(nullable: false),
                        Fips = c.Int(nullable: false),
                        TimeZone = c.String(),
                        ObservesDST = c.Boolean(nullable: false),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ZipCodeId)
                .ForeignKey("dbo.State", t => t.StateId, cascadeDelete: true)
                .Index(t => t.StateId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ZipCode", "StateId", "dbo.State");
            DropIndex("dbo.ZipCode", new[] { "StateId" });
            DropTable("dbo.ZipCode");
            DropTable("dbo.State");
        }
    }
}
