namespace InspectionApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edit10 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserInfo", "UserPassword", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserInfo", "UserPassword", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
