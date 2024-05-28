namespace WebBanCaCanh.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Order : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "CustomerName", c => c.String(nullable: false));
            AddColumn("dbo.Orders", "PhoneNumber", c => c.String(nullable: false));
            AddColumn("dbo.Orders", "City", c => c.String(nullable: false));
            AddColumn("dbo.Orders", "District", c => c.String(nullable: false));
            AddColumn("dbo.Orders", "Address", c => c.String(nullable: false));
            AddColumn("dbo.Orders", "Note", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Note");
            DropColumn("dbo.Orders", "Address");
            DropColumn("dbo.Orders", "District");
            DropColumn("dbo.Orders", "City");
            DropColumn("dbo.Orders", "PhoneNumber");
            DropColumn("dbo.Orders", "CustomerName");
        }
    }
}
