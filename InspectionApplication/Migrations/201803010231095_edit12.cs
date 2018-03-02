namespace InspectionApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edit12 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NumCheck",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InspectionApplicationID = c.Int(nullable: false),
                        TypeOneString = c.String(),
                        TypeTwoString = c.String(),
                        TypeOneInt = c.Int(nullable: false),
                        TypeTwoInt = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.NumCheck");
        }
    }
}
