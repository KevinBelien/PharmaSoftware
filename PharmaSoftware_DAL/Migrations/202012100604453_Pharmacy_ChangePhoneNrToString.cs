namespace PharmaSoftware_DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pharmacy_ChangePhoneNrToString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Pharmacy", "PhoneNr", c => c.String(nullable: false));
            AlterColumn("dbo.Supplier", "PhoneNr", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Supplier", "PhoneNr", c => c.Int());
            AlterColumn("dbo.Pharmacy", "PhoneNr", c => c.Int(nullable: false));
        }
    }
}
