namespace WebBanCaCanh.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserIdNull : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Orders", new[] { "UserId" });
            AlterColumn("dbo.Orders", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Orders", "UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Orders", new[] { "UserId" });
            AlterColumn("dbo.Orders", "UserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Orders", "UserId");
        }
    }
}
