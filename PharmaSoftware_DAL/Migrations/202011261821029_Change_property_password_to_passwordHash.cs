namespace PharmaSoftware_DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_property_password_to_passwordHash : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pharmacy", "PasswordHash", c => c.String(nullable: false));
            DropColumn("dbo.Pharmacy", "Password");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pharmacy", "Password", c => c.String(nullable: false));
            DropColumn("dbo.Pharmacy", "PasswordHash");
        }
    }
}
