namespace InspectionApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edit5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InspectionApplications", "InspectionFatherDeptName", c => c.String(maxLength: 100));
            AddColumn("dbo.InspectionApplications", "InspectionFartherDeptID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InspectionApplications", "InspectionFartherDeptID");
            DropColumn("dbo.InspectionApplications", "InspectionFatherDeptName");
        }
    }
}
