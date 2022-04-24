namespace Course_Store.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTrainer : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Trainers", "Image");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Trainers", "Image", c => c.String());
        }
    }
}
