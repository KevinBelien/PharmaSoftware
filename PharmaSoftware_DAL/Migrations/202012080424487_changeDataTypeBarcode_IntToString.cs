namespace PharmaSoftware_DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeDataTypeBarcode_IntToString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Product", "Barcode", c => c.String(nullable: false, maxLength: 13));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Product", "Barcode", c => c.Int(nullable: false));
        }
    }
}
