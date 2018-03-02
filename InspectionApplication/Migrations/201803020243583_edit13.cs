namespace InspectionApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edit13 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NumCheck", "InspectionNum", c => c.String(maxLength: 200));
            AlterColumn("dbo.NumCheck", "TypeOneString", c => c.String(maxLength: 200));
            AlterColumn("dbo.NumCheck", "TypeTwoString", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.NumCheck", "TypeTwoString", c => c.String());
            AlterColumn("dbo.NumCheck", "TypeOneString", c => c.String());
            DropColumn("dbo.NumCheck", "InspectionNum");
        }
    }
}
