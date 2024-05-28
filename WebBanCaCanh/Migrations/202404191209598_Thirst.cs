namespace WebBanCaCanh.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Thirst : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "IsVisible", c => c.Boolean(nullable: false));
            AddColumn("dbo.Banners", "IsVisible", c => c.Boolean(nullable: false));
            AddColumn("dbo.News", "IsVisible", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Promotions", "PromotionName", c => c.String(nullable: false));
            AlterColumn("dbo.Banners", "Content", c => c.String(nullable: false, maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Banners", "Content", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Promotions", "PromotionName", c => c.String(nullable: false, maxLength: 100));
            DropColumn("dbo.News", "IsVisible");
            DropColumn("dbo.Banners", "IsVisible");
            DropColumn("dbo.Products", "IsVisible");
        }
    }
}
