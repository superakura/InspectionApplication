namespace InspectionApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edit2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.InspectionApplications", new[] { "InspectionApplicationNum" });
            AlterColumn("dbo.InspectionApplications", "InspectionApplicationNum", c => c.String(maxLength: 100));
            AlterColumn("dbo.InspectionApplications", "DisposePersonName", c => c.String(maxLength: 20));
            CreateIndex("dbo.InspectionApplications", "InspectionApplicationNum", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.InspectionApplications", new[] { "InspectionApplicationNum" });
            AlterColumn("dbo.InspectionApplications", "DisposePersonName", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.InspectionApplications", "InspectionApplicationNum", c => c.String(nullable: false, maxLength: 100));
            CreateIndex("dbo.InspectionApplications", "InspectionApplicationNum", unique: true);
        }
    }
}
