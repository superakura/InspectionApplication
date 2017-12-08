namespace InspectionApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edit7 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Log",
                c => new
                    {
                        LogID = c.Int(nullable: false, identity: true),
                        LogInfo = c.String(nullable: false, maxLength: 200),
                        LogType = c.String(nullable: false, maxLength: 100),
                        LogInputDate = c.DateTime(nullable: false),
                        LogInputPerson = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LogID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Log");
        }
    }
}
