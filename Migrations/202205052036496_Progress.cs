namespace Course_Store.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Progress : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Progresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StaretdTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        ProgressStatus = c.Int(nullable: false),
                        CourseDetail_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CourseDetails", t => t.CourseDetail_Id, cascadeDelete: true)
                .Index(t => t.CourseDetail_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Progresses", "CourseDetail_Id", "dbo.CourseDetails");
            DropIndex("dbo.Progresses", new[] { "CourseDetail_Id" });
            DropTable("dbo.Progresses");
        }
    }
}
