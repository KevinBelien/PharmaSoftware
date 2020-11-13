using PharmaSoftware_DAL.Data.Repositories;
using PharmaSoftware_DAL.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaSoftware_DAL.Data.UnitOfWork
{
    public interface IUnitOfWork: IDisposable
    {
        IRepository<OrderIntern> OrderInternRepo { get; }
        IRepository<Pharmacy> PharmacyRepo { get; }
        IRepository<PharmacyProduct> PharmacyProductRepo { get; }
        IRepository<Product> ProductRepo { get; }
        IRepository<ProductCategory> ProductCategoryRepo { get; }
        IRepository<ProductPreparation> ProductPreparationRepo { get; }
        IRepository<ProductSubcategory> ProductSubcategoryRepo { get; }
        IRepository<Supplier> SupplierRepo { get; }
        int Save();
    }
}
