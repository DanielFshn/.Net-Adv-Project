namespace Course_Store.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateProgress1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Progresses", "Points", c => c.Int(nullable: false));
            AddColumn("dbo.Progresses", "User_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Progresses", "User_Id");
            AddForeignKey("dbo.Progresses", "User_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Progresses", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Progresses", new[] { "User_Id" });
            DropColumn("dbo.Progresses", "User_Id");
            DropColumn("dbo.Progresses", "Points");
        }
    }
}
