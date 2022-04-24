namespace Course_Store.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dropTrainerBirthday : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Trainers", "Birthday");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Trainers", "Birthday", c => c.DateTime(nullable: false));
        }
    }
}
