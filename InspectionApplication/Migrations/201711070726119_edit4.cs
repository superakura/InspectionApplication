namespace InspectionApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edit4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.InspectionApplications", "DisposeDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.InspectionApplications", "DisposeDate", c => c.DateTime(nullable: false));
        }
    }
}
