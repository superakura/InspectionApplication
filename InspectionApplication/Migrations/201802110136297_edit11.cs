namespace InspectionApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edit11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InspectionApplications", "IsMaterialName", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InspectionApplications", "IsMaterialName");
        }
    }
}
