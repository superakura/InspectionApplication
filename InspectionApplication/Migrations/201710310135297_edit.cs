namespace InspectionApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DeptInfo",
                c => new
                    {
                        DeptID = c.Int(nullable: false, identity: true),
                        DeptName = c.String(nullable: false, maxLength: 30),
                        DeptFatherID = c.Int(nullable: false),
                        DeptState = c.Int(nullable: false),
                        DeptRemark = c.String(maxLength: 200),
                        DeptOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DeptID);
            
            CreateTable(
                "dbo.InspectionApplications",
                c => new
                    {
                        InspectionApplicationID = c.Int(nullable: false, identity: true),
                        InspectionApplicationNum = c.String(nullable: false, maxLength: 100),
                        ProductName = c.String(nullable: false, maxLength: 200),
                        ProductFactory = c.String(nullable: false, maxLength: 200),
                        ProductDealer = c.String(nullable: false, maxLength: 200),
                        ProductType = c.String(nullable: false, maxLength: 200),
                        ProductPackingType = c.String(nullable: false, maxLength: 200),
                        ProductCount = c.String(nullable: false, maxLength: 50),
                        ArrivalDate = c.DateTime(nullable: false),
                        InspectionDate = c.DateTime(nullable: false),
                        InspectionDeptName = c.String(maxLength: 100),
                        InspectionDeptID = c.Int(nullable: false),
                        InspectionPersonName = c.String(maxLength: 100),
                        InspectionPersonID = c.Int(nullable: false),
                        DisposePersonName = c.String(nullable: false, maxLength: 20),
                        DisposePersonID = c.Int(nullable: false),
                        DisposeDate = c.DateTime(nullable: false),
                        InspectionApplicationState = c.String(nullable: false, maxLength: 20),
                        DisposeRemark = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.InspectionApplicationID)
                .Index(t => t.InspectionApplicationNum, unique: true);
            
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
            
            CreateTable(
                "dbo.ProductUseDeptInfo",
                c => new
                    {
                        ProductUseDeptInfoID = c.Int(nullable: false, identity: true),
                        ProductUseDeptID = c.Int(nullable: false),
                        InspectionApplicationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProductUseDeptInfoID)
                .ForeignKey("dbo.DeptInfo", t => t.ProductUseDeptID, cascadeDelete: true)
                .ForeignKey("dbo.InspectionApplications", t => t.InspectionApplicationID, cascadeDelete: true)
                .Index(t => t.ProductUseDeptID)
                .Index(t => t.InspectionApplicationID);
            
            CreateTable(
                "dbo.UserInfo",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        UserNum = c.String(nullable: false, maxLength: 20),
                        UserName = c.String(nullable: false, maxLength: 20),
                        UserPassword = c.String(nullable: false, maxLength: 100),
                        UserPhone = c.String(maxLength: 50),
                        UserRole = c.String(nullable: false, maxLength: 30),
                        UserRemark = c.String(maxLength: 200),
                        UserState = c.Int(nullable: false),
                        UserDeptID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("dbo.DeptInfo", t => t.UserDeptID, cascadeDelete: true)
                .Index(t => t.UserDeptID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserInfo", "UserDeptID", "dbo.DeptInfo");
            DropForeignKey("dbo.ProductUseDeptInfo", "InspectionApplicationID", "dbo.InspectionApplications");
            DropForeignKey("dbo.ProductUseDeptInfo", "ProductUseDeptID", "dbo.DeptInfo");
            DropIndex("dbo.UserInfo", new[] { "UserDeptID" });
            DropIndex("dbo.ProductUseDeptInfo", new[] { "InspectionApplicationID" });
            DropIndex("dbo.ProductUseDeptInfo", new[] { "ProductUseDeptID" });
            DropIndex("dbo.InspectionApplications", new[] { "InspectionApplicationNum" });
            DropTable("dbo.UserInfo");
            DropTable("dbo.ProductUseDeptInfo");
            DropTable("dbo.Logs");
            DropTable("dbo.InspectionApplications");
            DropTable("dbo.DeptInfo");
        }
    }
}
