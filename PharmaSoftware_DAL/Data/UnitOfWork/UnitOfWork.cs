using PharmaSoftware_DAL.Data.Repositories;
using PharmaSoftware_DAL.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaSoftware_DAL.Data.UnitOfWork
{
    public class UnitOfWork: IUnitOfWork
    {

        #region attributen
        private IRepository<OrderIntern> _orderInternRepo;
        private IRepository<Pharmacy> _pharmacyRepo;
        private IRepository<PharmacyProduct> _pharmacyProductRepo;
        private IRepository<Product> _productRepo;
        private IRepository<ProductCategory> _productCategoryRepo;
        private IRepository<ProductPreparation> _productPreparationRepo;
        private IRepository<ProductSubcategory> _productSubcategoryRepo;
        private IRepository<Supplier> _supplierRepo;
        #endregion

        private PharmaSoftwareEntities PharmaSoftwareEntities { get; }

        public UnitOfWork(PharmaSoftwareEntities pharmaSoftwareEntities)
        {
            this.PharmaSoftwareEntities = pharmaSoftwareEntities;
        }

        #region repositories
        public IRepository<OrderIntern> OrderInternRepo
        {
            get
            {
                if (_orderInternRepo == null)
                {
                    _orderInternRepo = new Repository<OrderIntern>(this.PharmaSoftwareEntities);
                }
                return _orderInternRepo;
            }
        }

        public IRepository<Pharmacy> PharmacyRepo
        {
            get
            {
                if (_pharmacyRepo == null)
                {
                    _pharmacyRepo = new Repository<Pharmacy>(this.PharmaSoftwareEntities);
                }
                return _pharmacyRepo;
            }
        }

        public IRepository<PharmacyProduct> PharmacyProductRepo
        {
            get
            {
                if (_pharmacyProductRepo == null)
                {
                    _pharmacyProductRepo = new Repository<PharmacyProduct>(this.PharmaSoftwareEntities);
                }
                return _pharmacyProductRepo;
            }
        }

        public IRepository<Product> ProductRepo
        {
            get
            {
                if (_productRepo == null)
                {
                    _productRepo = new Repository<Product>(this.PharmaSoftwareEntities);
                }
                return _productRepo;
            }
        }
        public IRepository<ProductCategory> ProductCategoryRepo
        {
            get
            {
                if (_productCategoryRepo == null)
                {
                    _productCategoryRepo = new Repository<ProductCategory>(this.PharmaSoftwareEntities);
                }
                return _productCategoryRepo;
            }
        }

        public IRepository<ProductPreparation> ProductPreparationRepo
        {
            get
            {
                if (_productPreparationRepo == null)
                {
                    _productPreparationRepo = new Repository<ProductPreparation>(this.PharmaSoftwareEntities);
                }
                return _productPreparationRepo;
            }
        }

        public IRepository<ProductSubcategory> ProductSubcategoryRepo
        {
            get
            {
                if (_productSubcategoryRepo == null)
                {
                    _productSubcategoryRepo = new Repository<ProductSubcategory>(this.PharmaSoftwareEntities);
                }
                return _productSubcategoryRepo;
            }
        }

        public IRepository<Supplier> SupplierRepo
        {
            get
            {
                if (_supplierRepo == null)
                {
                    _supplierRepo = new Repository<Supplier>(this.PharmaSoftwareEntities);
                }
                return _supplierRepo;
            }
        }
        #endregion

        public void Dispose()
        {
            PharmaSoftwareEntities.Dispose();
        }

        public int Save()
        {
            return PharmaSoftwareEntities.SaveChanges();
        }
    }
}
