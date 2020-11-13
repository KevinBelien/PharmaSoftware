namespace PharmaSoftware_DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderIntern",
                c => new
                    {
                        OrderInternID = c.Int(nullable: false, identity: true),
                        DateOfOrder = c.DateTime(nullable: false),
                        DateOfDelivery = c.DateTime(),
                        Quantity = c.Int(nullable: false),
                        IsSend = c.Boolean(nullable: false),
                        ProductID = c.Int(nullable: false),
                        PharmacyBuyID = c.Int(nullable: false),
                        PharmacySellID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OrderInternID)
                .ForeignKey("dbo.Pharmacy", t => t.PharmacyBuyID)
                .ForeignKey("dbo.Pharmacy", t => t.PharmacySellID)
                .ForeignKey("dbo.Product", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.ProductID)
                .Index(t => t.PharmacyBuyID)
                .Index(t => t.PharmacySellID);
            
            CreateTable(
                "dbo.Pharmacy",
                c => new
                    {
                        PharmacyID = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        City = c.String(nullable: false),
                        ZIP = c.String(nullable: false, maxLength: 10),
                        Street = c.String(nullable: false),
                        HouseNr = c.String(nullable: false, maxLength: 10),
                        PhoneNr = c.Int(nullable: false),
                        District = c.String(),
                    })
                .PrimaryKey(t => t.PharmacyID);
            
            CreateTable(
                "dbo.PharmacyProduct",
                c => new
                    {
                        PharmacyProductID = c.Int(nullable: false, identity: true),
                        QtyInStorage = c.Int(nullable: false),
                        QtyOrdered = c.Int(),
                        DateOfOrder = c.DateTime(nullable: false),
                        DateOfArrival = c.DateTime(),
                        PharmacyID = c.Int(nullable: false),
                        ProductID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PharmacyProductID)
                .ForeignKey("dbo.Pharmacy", t => t.PharmacyID, cascadeDelete: true)
                .ForeignKey("dbo.Product", t => t.ProductID, cascadeDelete: true)
                .Index(t => new { t.PharmacyID, t.ProductID }, unique: true, name: "IX_Product_Pharmacy");
            
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        ProductID = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Barcode = c.Int(nullable: false),
                        Content = c.String(nullable: false, maxLength: 20),
                        Price = c.Decimal(nullable: false, storeType: "money"),
                        Cost = c.Decimal(nullable: false, storeType: "money"),
                        Brand = c.Int(nullable: false),
                        ProductCategoryID = c.Int(nullable: false),
                        ProductPreparationID = c.Int(nullable: false),
                        SupplierID = c.Int(nullable: false),
                        Product_ProductID = c.Int(),
                    })
                .PrimaryKey(t => t.ProductID)
                .ForeignKey("dbo.Product", t => t.Product_ProductID)
                .ForeignKey("dbo.ProductCategory", t => t.ProductCategoryID, cascadeDelete: true)
                .ForeignKey("dbo.ProductPreparation", t => t.ProductPreparationID, cascadeDelete: true)
                .ForeignKey("dbo.Supplier", t => t.SupplierID, cascadeDelete: true)
                .Index(t => t.ProductCategoryID)
                .Index(t => t.ProductPreparationID)
                .Index(t => t.SupplierID)
                .Index(t => t.Product_ProductID);
            
            CreateTable(
                "dbo.ProductCategory",
                c => new
                    {
                        ProductCategoryID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ProductCategoryID);
            
            CreateTable(
                "dbo.ProductSubcategories",
                c => new
                    {
                        ProductSubcategoryID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ProductCategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProductSubcategoryID)
                .ForeignKey("dbo.ProductCategory", t => t.ProductCategoryID, cascadeDelete: true)
                .Index(t => t.ProductCategoryID);
            
            CreateTable(
                "dbo.ProductPreparation",
                c => new
                    {
                        ProductPreparationID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ProductPreparationID);
            
            CreateTable(
                "dbo.Supplier",
                c => new
                    {
                        SupplierID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Mail = c.String(),
                        PhoneNr = c.Int(),
                        Country = c.String(),
                        City = c.String(),
                        ZIP = c.String(nullable: false, maxLength: 10),
                        Street = c.String(nullable: false),
                        HouseNr = c.String(nullable: false, maxLength: 10),
                        Website = c.String(),
                    })
                .PrimaryKey(t => t.SupplierID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderIntern", "ProductID", "dbo.Product");
            DropForeignKey("dbo.OrderIntern", "PharmacySellID", "dbo.Pharmacy");
            DropForeignKey("dbo.OrderIntern", "PharmacyBuyID", "dbo.Pharmacy");
            DropForeignKey("dbo.Product", "SupplierID", "dbo.Supplier");
            DropForeignKey("dbo.Product", "ProductPreparationID", "dbo.ProductPreparation");
            DropForeignKey("dbo.ProductSubcategories", "ProductCategoryID", "dbo.ProductCategory");
            DropForeignKey("dbo.Product", "ProductCategoryID", "dbo.ProductCategory");
            DropForeignKey("dbo.PharmacyProduct", "ProductID", "dbo.Product");
            DropForeignKey("dbo.Product", "Product_ProductID", "dbo.Product");
            DropForeignKey("dbo.PharmacyProduct", "PharmacyID", "dbo.Pharmacy");
            DropIndex("dbo.ProductSubcategories", new[] { "ProductCategoryID" });
            DropIndex("dbo.Product", new[] { "Product_ProductID" });
            DropIndex("dbo.Product", new[] { "SupplierID" });
            DropIndex("dbo.Product", new[] { "ProductPreparationID" });
            DropIndex("dbo.Product", new[] { "ProductCategoryID" });
            DropIndex("dbo.PharmacyProduct", "IX_Product_Pharmacy");
            DropIndex("dbo.OrderIntern", new[] { "PharmacySellID" });
            DropIndex("dbo.OrderIntern", new[] { "PharmacyBuyID" });
            DropIndex("dbo.OrderIntern", new[] { "ProductID" });
            DropTable("dbo.Supplier");
            DropTable("dbo.ProductPreparation");
            DropTable("dbo.ProductSubcategories");
            DropTable("dbo.ProductCategory");
            DropTable("dbo.Product");
            DropTable("dbo.PharmacyProduct");
            DropTable("dbo.Pharmacy");
            DropTable("dbo.OrderIntern");
        }
    }
}
