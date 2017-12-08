namespace InspectionApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edit9 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Log", "InspectionID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Log", "InspectionID");
        }
    }
}
