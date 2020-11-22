using PharmaSoftware_DAL.DomainModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaSoftware_DAL.Data
{
    public class PharmaSoftwareEntities : DbContext
    {
        public PharmaSoftwareEntities(): base("name=PharmaSoftwareConnectionString")
        {

        }

        public DbSet<OrderIntern> OrdersIntern { get; set; }
        public DbSet<Pharmacy> Pharmacies { get; set; }
        public DbSet<PharmacyProduct> PharmacyProducts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductPreparation> ProductPreparations { get; set; }
        public DbSet<ProductSubcategory> ProductSubcategories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderIntern>()
                .HasRequired(o => o.PharmacyBuy)
                .WithMany(p => p.InternBought)
                .HasForeignKey(o => o.PharmacyBuyID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OrderIntern>()
                .HasRequired(o => o.PharmacySell)
                .WithMany(p => p.InternSold)
                .HasForeignKey(o => o.PharmacySellID)
                .WillCascadeOnDelete(false);
        }
    }
}
