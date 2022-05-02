namespace Course_Store.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedOrderRelationship : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.OrderDetails", "CourseId");
            CreateIndex("dbo.Orders", "UserId");
            AddForeignKey("dbo.OrderDetails", "CourseId", "dbo.Courses", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Orders", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.OrderDetails", "CourseId", "dbo.Courses");
            DropIndex("dbo.Orders", new[] { "UserId" });
            DropIndex("dbo.OrderDetails", new[] { "CourseId" });
            AlterColumn("dbo.Orders", "UserId", c => c.String());
        }
    }
}
