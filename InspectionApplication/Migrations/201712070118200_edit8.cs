namespace InspectionApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edit8 : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Logs");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        LogsID = c.Int(nullable: false, identity: true),
                        InputPersonID = c.Int(nullable: false),
                        InputDate = c.DateTime(nullable: false),
                        LogInfo = c.String(nullable: false, maxLength: 500),
                    })
                .PrimaryKey(t => t.LogsID);
            
        }
    }
}
