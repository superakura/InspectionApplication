namespace InspectionApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edit3 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.InspectionApplications", new[] { "InspectionApplicationNum" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.InspectionApplications", "InspectionApplicationNum", unique: true);
        }
    }
}
