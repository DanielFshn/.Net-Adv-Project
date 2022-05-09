namespace Course_Store.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateProgress : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CourseDetails", "Progress_Id", "dbo.Progresses");
            DropForeignKey("dbo.CourseDetails", "Course_Id", "dbo.Courses");
            DropIndex("dbo.CourseDetails", new[] { "Progress_Id" });
            DropIndex("dbo.CourseDetails", new[] { "Course_Id" });
            AlterColumn("dbo.CourseDetails", "Course_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.CourseDetails", "Course_Id");
            AddForeignKey("dbo.CourseDetails", "Course_Id", "dbo.Courses", "Id", cascadeDelete: true);
            DropColumn("dbo.CourseDetails", "Progress_Id");
            DropTable("dbo.Progresses");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Progresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StaretdTime = c.DateTime(nullable: false),
                        StopedTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        ProgressStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.CourseDetails", "Progress_Id", c => c.Int());
            DropForeignKey("dbo.CourseDetails", "Course_Id", "dbo.Courses");
            DropIndex("dbo.CourseDetails", new[] { "Course_Id" });
            AlterColumn("dbo.CourseDetails", "Course_Id", c => c.Int());
            CreateIndex("dbo.CourseDetails", "Course_Id");
            CreateIndex("dbo.CourseDetails", "Progress_Id");
            AddForeignKey("dbo.CourseDetails", "Course_Id", "dbo.Courses", "Id");
            AddForeignKey("dbo.CourseDetails", "Progress_Id", "dbo.Progresses", "Id");
        }
    }
}
