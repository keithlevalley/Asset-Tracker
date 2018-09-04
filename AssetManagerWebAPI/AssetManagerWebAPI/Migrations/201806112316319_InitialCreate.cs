namespace AssetManagerWebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DBAssets",
                c => new
                    {
                        SerialNumber = c.String(nullable: false, maxLength: 128),
                        LastCheckin = c.DateTime(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        Managed = c.Boolean(nullable: false),
                        Storage = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        Name = c.String(),
                        Model = c.String(),
                        MACAddress = c.String(),
                        IpAddress = c.String(),
                    })
                .PrimaryKey(t => t.SerialNumber);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DBAssets");
        }
    }
}
