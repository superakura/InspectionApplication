namespace InspectionApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edit6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InspectionApplications", "ProductBatchNum", c => c.String(nullable: false, maxLength: 200));
            AddColumn("dbo.InspectionApplications", "InputDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.InspectionApplications", "SamplePlace", c => c.String(maxLength: 200));
            AlterColumn("dbo.InspectionApplications", "ProductType", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.InspectionApplications", "ProductType", c => c.String(nullable: false, maxLength: 200));
            DropColumn("dbo.InspectionApplications", "SamplePlace");
            DropColumn("dbo.InspectionApplications", "InputDate");
            DropColumn("dbo.InspectionApplications", "ProductBatchNum");
        }
    }
}
