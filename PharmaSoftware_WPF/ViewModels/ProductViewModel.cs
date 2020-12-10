using GalaSoft.MvvmLight.Command;
using PharmaSoftware_DAL;
using PharmaSoftware_DAL.Data;
using PharmaSoftware_DAL.Data.UnitOfWork;
using PharmaSoftware_WPF.State.Authenticators;
using PharmaSoftware_WPF.State.ManageWIndows;
using PharmaSoftware_WPF.Views;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PharmaSoftware_WPF.ViewModels
{
    public class ProductViewModel : BaseViewModel, IDisposable
    {
        private readonly IUnitOfWork _uow = new UnitOfWork(new PharmaSoftwareEntities());

        public RelayCommand<IClosable> LogoutCommand { get; private set; }
        public RelayCommand<IClosable> CancelCommand { get; private set; }
        public RelayCommand<IClosable> ShowProfileViewCommand { get; private set; }
        public RelayCommand<IClosable> AddProductCommand { get; private set; }


        public Pharmacy Pharmacy { get; set; }
        public ProductCategory SelectedCategory { get; set; }
        public ProductSubcategory SelectedSubcategory { get; set; }
        public bool CmbIsEnabled { get; set; }

        public ObservableCollection<PharmacyProduct> PharmacyProducts { get; set; }
        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<Supplier> Suppliers { get; set; }
        public ObservableCollection<ProductPreparation> Preparations { get; set; }
        public ObservableCollection<ProductCategory> Categories { get; set; }
        public ObservableCollection<ProductSubcategory> Subcategories { get; set; }


        public Product Product { get; set; }
        public PharmacyProduct PharmacyProduct { get; set; }


        public string ValidInStorage { get; set; }
        public string ValidOrdered { get; set; }
        public string ValidPrice { get; set; }
        public string ValidCost { get; set; }


        public int QtyStockIssues { get; set; }


        public override string this[string columnName]
        {
            get
            {
                if (columnName == "ValidInStorage" && !int.TryParse(ValidInStorage, out int stock))
                {
                    return "\"Aantal in stock\" moet een numerieke waarde hebben!";
                }
                if (columnName == "ValidOrdered" && !string.IsNullOrWhiteSpace(ValidInStorage))
                {
                    if (!int.TryParse(ValidInStorage, out int ordered))
                    {
                        return "\"Aantal besteld\" moet een numerieke waarde hebben!";
                    }
                }
                if (columnName == "ValidPrice" && !decimal.TryParse(ValidPrice, out decimal price))
                {
                    return "Prijs moet een numerieke waarde zijn!";
                }
                if (columnName == "ValidCost" && !decimal.TryParse(ValidCost, out decimal cost))
                {
                    return "Kostprijs moet een nummerieke waarde zijn!";
                }
                return "";
            }
        }

        public ProductViewModel(int id)
        {
            this.CmbIsEnabled = false;

            this.LogoutCommand = new RelayCommand<IClosable>(this.Logout);
            this.CancelCommand = new RelayCommand<IClosable>(this.ShowStorageView);
            this.ShowProfileViewCommand = new RelayCommand<IClosable>(this.ShowProfileView);
            this.AddProductCommand = new RelayCommand<IClosable>(this.AddProduct);

            Pharmacy = _uow.PharmacyRepo.Get(p => p.PharmacyID == id,
                p => p.PharmacyProducts.Select(pp => pp.Product))
                .FirstOrDefault();

            PharmacyProduct = new PharmacyProduct()
            {
                PharmacyID = Pharmacy.PharmacyID,
                DateOfOrder = DateTime.Now,
                Product = new Product()
            };

            PharmacyProducts = new ObservableCollection<PharmacyProduct>(_uow.PharmacyProductRepo.Get(pp => pp.PharmacyID == Pharmacy.PharmacyID, pp => pp.Product));
            Products = new ObservableCollection<Product>(_uow.ProductRepo.Get());
            Suppliers = new ObservableCollection<Supplier>(_uow.SupplierRepo.Get().OrderBy(s => s.Name));
            Categories = new ObservableCollection<ProductCategory>(_uow.ProductCategoryRepo.Get().OrderBy(c => c.Name));
            Preparations = new ObservableCollection<ProductPreparation>(_uow.ProductPreparationRepo.Get()
                .OrderBy(p => p.Name == "Overige").ThenBy(p => p.Name));

            QtyStockIssues = CountStockIssues(10);
        }
        private int CountStockIssues(int minimumStock)
        {
            int issues = 0;
            foreach (PharmacyProduct product in PharmacyProducts)
            {
                if (product.QtyInStorage <= minimumStock)
                {
                    if (product.QtyOrdered < 20 || product.QtyOrdered == null)
                    {
                        issues++;
                    }
                }
            }
            return issues;
        }
        public void Dispose()
        {
            _uow?.Dispose();
        }

        private void ShowSubcategories(ProductCategory productCategory)
        {
            if (SelectedCategory != null)
            {
                this.CmbIsEnabled = true;
                Subcategories = new ObservableCollection<ProductSubcategory>(_uow.ProductSubcategoryRepo.Get(sc => sc.ProductCategoryID == productCategory.ProductCategoryID));
            }
            else
            {
                this.CmbIsEnabled = false;
            }
        }

        private void FillingPharmacyProduct(PharmacyProduct pharmacyProduct)
        {
            pharmacyProduct.Product.ProductCategory = SelectedCategory;
            pharmacyProduct.Product.ProductSubcategory = SelectedSubcategory;
            pharmacyProduct.DateOfOrder = DateTime.Now;
            pharmacyProduct.Product.Price = decimal.Parse(ValidPrice);
            pharmacyProduct.Product.Cost = decimal.Parse(ValidCost);
            pharmacyProduct.QtyInStorage = int.Parse(ValidInStorage);
            if (!string.IsNullOrWhiteSpace(ValidOrdered))
            {
                pharmacyProduct.QtyOrdered = int.Parse(ValidOrdered);
            }
        }

        private bool AddProduct()
        {
            string productError = ValidateInputFieldsProduct(SelectedCategory, SelectedSubcategory, PharmacyProduct.Product.SupplierID);
            if (string.IsNullOrWhiteSpace(productError))
            {
                if (PharmacyProduct.Product.IsValid() && PharmacyProduct.IsValid())
                {
                    if (! IsNumericValid())
                    {
                        MessageBox.Show(Error, "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }

                    FillingPharmacyProduct(PharmacyProduct);

                    if (!Products.Contains(PharmacyProduct.Product))
                    {
                        int okProduct = AddNewProduct(PharmacyProduct.Product);
                        if (okProduct == 0)
                        {
                            MessageBox.Show("Het product is niet toegevoegd!", "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Error);
                            return false;
                        }
                    }

                    Product KnownProduct = _uow.ProductRepo.Get(p => p.Code == PharmacyProduct.Product.Code).FirstOrDefault();
                    PharmacyProduct.ProductID = KnownProduct.ProductID;

                    if (!PharmacyProducts.Contains(PharmacyProduct))
                    {
                        int okPharmacyProduct = AddNewPharmacyProduct(PharmacyProduct);

                        if (okPharmacyProduct > 0)
                        {
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("Er is iets misgelopen bij de toevoeging van het product aan de stock", "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Error);
                            return false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Dit product zit al in uw stock!", "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                }
                else
                {
                    if (PharmacyProduct.Product.Error != "")
                    {
                        MessageBox.Show(PharmacyProduct.Product.Error, "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        MessageBox.Show(PharmacyProduct.Error, "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    return false;
                }
            }
            else
            {
                MessageBox.Show(productError, "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

        }

        private int AddNewProduct(Product product)
        {
                _uow.ProductRepo.Add(product);
                return _uow.Save();
        }

        private int AddNewPharmacyProduct(PharmacyProduct product)
        {
            _uow.PharmacyProductRepo.Add(product);
            return _uow.Save();
        }

        private string ValidateInputFieldsProduct(ProductCategory selectedCategory, ProductSubcategory selectedSubcategory,
            int? supplierID)
        {
            if (selectedCategory == null)
            {
                return "Selecteer een categorie!";
            }
            if (selectedSubcategory == null)
            {
                return "Selecteer een subcategorie!";

            }
            if (supplierID == 0)
            {
                return "Selecteer een leverancier!";
            }
            return "";
        }


        #region Commands
        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            switch (parameter.ToString())
            {
                case "CloseApp": Application.Current.Shutdown(); break;
                case "ShowSubcategories": ShowSubcategories(SelectedCategory); break;
            }
        }

        private void Logout(IClosable window)
        {
            if (window != null)
            {
                Authenticator.LogOut();
                window.Close();
            }
        }

        private void ShowProfileView(IClosable window)
        {
            if (window != null)
            {
                if (Authenticator.isLoggedIn)
                {
                    ShowProfileWindow(Authenticator.CurrentUser.PharmacyID);
                    window.Close();
                }
            }
        }

        private void AddProduct(IClosable window)
        {
            if (window != null)
            {
                if (Authenticator.isLoggedIn)
                {
                    if (AddProduct())
                    {
                        ShowStorageWindow(Authenticator.CurrentUser.PharmacyID);
                        window.Close();
                    }
                }
            }
        }

        private void ShowStorageView(IClosable window)
        {
            if (window != null)
            {
                if (Authenticator.isLoggedIn)
                {
                    ShowStorageWindow(Authenticator.CurrentUser.PharmacyID);
                    window.Close();
                }
            }
        }

        private void ShowProfileWindow(int id)
        {
            ProfileView profileView = new ProfileView();
            ProfileViewModel profileViewModel = new ProfileViewModel(id);
            profileView.DataContext = profileViewModel;
            profileView.Show();
        }

        private void ShowStorageWindow(int id)
        {
            StorageView storageView = new StorageView();
            StorageViewModel storageViewModel = new StorageViewModel(id);
            storageView.DataContext = storageViewModel;
            storageView.Show();
        }

        #endregion
    }
}
