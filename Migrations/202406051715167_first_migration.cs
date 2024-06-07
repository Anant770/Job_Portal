namespace Job_Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first_migration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.CompanyId);
            
            CreateTable(
                "dbo.Jobs",
                c => new
                    {
                        JobId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        Location = c.String(),
                        PostedDate = c.DateTime(nullable: false),
                        CompanyId = c.Int(nullable: false),
                        JobCategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.JobId)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .ForeignKey("dbo.JobCategories", t => t.JobCategoryId, cascadeDelete: true)
                .Index(t => t.CompanyId)
                .Index(t => t.JobCategoryId);
            
            CreateTable(
                "dbo.JobCategories",
                c => new
                    {
                        JobCategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.JobCategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Jobs", "JobCategoryId", "dbo.JobCategories");
            DropForeignKey("dbo.Jobs", "CompanyId", "dbo.Companies");
            DropIndex("dbo.Jobs", new[] { "JobCategoryId" });
            DropIndex("dbo.Jobs", new[] { "CompanyId" });
            DropTable("dbo.JobCategories");
            DropTable("dbo.Jobs");
            DropTable("dbo.Companies");
        }
    }
}
