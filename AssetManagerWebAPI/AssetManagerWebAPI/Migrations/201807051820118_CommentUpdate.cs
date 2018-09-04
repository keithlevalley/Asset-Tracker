namespace AssetManagerWebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommentUpdate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DBComments",
                c => new
                    {
                        DBCommentID = c.Int(nullable: false, identity: true),
                        DBAssetID = c.String(maxLength: 128),
                        Comment = c.String(),
                    })
                .PrimaryKey(t => t.DBCommentID)
                .ForeignKey("dbo.DBAssets", t => t.DBAssetID)
                .Index(t => t.DBAssetID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DBComments", "DBAssetID", "dbo.DBAssets");
            DropIndex("dbo.DBComments", new[] { "DBAssetID" });
            DropTable("dbo.DBComments");
        }
    }
}
