namespace Course_Store.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialSetup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CourseCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategotyType = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Image = c.String(),
                        UpdatedById = c.Int(),
                        DeletedById = c.Int(),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(),
                        DeletedOn = c.DateTime(),
                        IsPublish = c.Boolean(nullable: false),
                        Points = c.Int(),
                        Objectives = c.String(),
                        CategoryId = c.Int(nullable: false),
                        TrainerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CourseCategories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Trainers", t => t.TrainerId, cascadeDelete: true)
                .Index(t => t.CategoryId)
                .Index(t => t.TrainerId);
            
            CreateTable(
                "dbo.Trainers",
                c => new
                    {
                        TrainderId = c.Int(nullable: false, identity: true),
                        Skills = c.String(),
                        YearOfExperience = c.Byte(nullable: false),
                        Image = c.String(),
                        Birthday = c.DateTime(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.TrainderId)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Surname = c.String(),
                        UpdatedById = c.String(),
                        Photo = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(),
                        Birthday = c.DateTime(nullable: false),
                        Points = c.Int(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.CourseDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VideoPath = c.String(),
                        Progress_Id = c.Int(),
                        Course_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Progresses", t => t.Progress_Id)
                .ForeignKey("dbo.Courses", t => t.Course_Id)
                .Index(t => t.Progress_Id)
                .Index(t => t.Course_Id);
            
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
            
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CourseId = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsHidden = c.Boolean(nullable: false),
                        OrderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        AdminId = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(),
                        DeletedOn = c.DateTime(),
                        PaymentMethod = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.OrderDetails", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.CourseDetails", "Course_Id", "dbo.Courses");
            DropForeignKey("dbo.CourseDetails", "Progress_Id", "dbo.Progresses");
            DropForeignKey("dbo.Courses", "TrainerId", "dbo.Trainers");
            DropForeignKey("dbo.Trainers", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Courses", "CategoryId", "dbo.CourseCategories");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.OrderDetails", new[] { "OrderId" });
            DropIndex("dbo.CourseDetails", new[] { "Course_Id" });
            DropIndex("dbo.CourseDetails", new[] { "Progress_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Trainers", new[] { "User_Id" });
            DropIndex("dbo.Courses", new[] { "TrainerId" });
            DropIndex("dbo.Courses", new[] { "CategoryId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderDetails");
            DropTable("dbo.Progresses");
            DropTable("dbo.CourseDetails");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Trainers");
            DropTable("dbo.Courses");
            DropTable("dbo.CourseCategories");
        }
    }
}
