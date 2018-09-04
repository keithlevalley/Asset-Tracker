namespace AssetManagerWebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedComment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DBComments", "AddedBy", c => c.String());
            AddColumn("dbo.DBComments", "DateAdded", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DBComments", "DateAdded");
            DropColumn("dbo.DBComments", "AddedBy");
        }
    }
}
