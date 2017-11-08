namespace InspectionApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edit1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductUseDeptInfo", "ProductUseFatherDeptID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductUseDeptInfo", "ProductUseFatherDeptID");
        }
    }
}
