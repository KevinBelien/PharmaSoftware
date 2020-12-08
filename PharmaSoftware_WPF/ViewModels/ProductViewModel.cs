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
        public RelayCommand<IClosable> ShowStorageViewCommand { get; private set; }
        public RelayCommand<IClosable> ShowProfileViewCommand { get; private set; }
        public RelayCommand<IClosable> AddProductCommand { get; private set; }


        public Pharmacy Pharmacy { get; set; }
        public ProductCategory SelectedCategory { get; set; }
        public ProductSubcategory SelectedSubcategory { get; set; }
        public bool CmbIsEnabled { get; set; }

        public ObservableCollection<PharmacyProduct> PharmacyProducts { get; set; }
        public ObservableCollection<Supplier> Suppliers { get; set; }
        public ObservableCollection<ProductPreparation> Preparations { get; set; }
        public ObservableCollection<ProductCategory> Categories { get; set; }
        public ObservableCollection<ProductSubcategory> Subcategories { get; set; }


        public Product Product { get; set; }
        public PharmacyProduct PharmacyProduct { get; set; }

        //public Product KnownProduct { get; set; }
        public string QtyInStorage { get; set; }
        public string QtyOrdered { get; set; }



        public override string this[string columnName] => throw new NotImplementedException();

        public ProductViewModel(int id)
        {
            this.CmbIsEnabled = false;

            this.LogoutCommand = new RelayCommand<IClosable>(this.Logout);
            this.CancelCommand = new RelayCommand<IClosable>(this.ShowStorageView);
            this.ShowStorageViewCommand = new RelayCommand<IClosable>(this.ShowStorageView);
            this.ShowProfileViewCommand = new RelayCommand<IClosable>(this.ShowProfileView);
            this.AddProductCommand = new RelayCommand<IClosable>(this.AddProduct);

            Product = new Product();

            Pharmacy = _uow.PharmacyRepo.Get(p => p.PharmacyID == id,
                p => p.PharmacyProducts.Select(pp => pp.Product))
                .FirstOrDefault();

            PharmacyProducts = new ObservableCollection<PharmacyProduct>(_uow.PharmacyProductRepo.Get(pp => pp.PharmacyID == Pharmacy.PharmacyID, pp => pp.Product));
            Suppliers = new ObservableCollection<Supplier>(_uow.SupplierRepo.Get().OrderBy(s => s.Name));
            Categories = new ObservableCollection<ProductCategory>(_uow.ProductCategoryRepo.Get().OrderBy(c => c.Name));
            Preparations = new ObservableCollection<ProductPreparation>(_uow.ProductPreparationRepo.Get()
                .OrderBy(p => p.Name == "Overige").ThenBy(p => p.Name));
        }

        public void Dispose()
        {
            _uow?.Dispose();
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

        private void AddProduct(IClosable window)
        {
            if (window != null)
            {
                if (Authenticator.isLoggedIn)
                {
                    bool productAdded = AddProductToPharmacy();
                    if (productAdded)
                    {
                        ShowStorageWindow(Authenticator.CurrentUser.PharmacyID);
                        window.Close();
                    }
                }
            }
        }

        private bool AddProductToPharmacy()
        {
            //checks whether the product failed when it doesn't exist yet
            bool succesProduct = true;
            //return value
            bool succesProductToPharmacy = false;

            string errorsProduct = ValidateInputFieldsProduct(SelectedCategory, SelectedSubcategory, Product.SupplierID);
            if (string.IsNullOrWhiteSpace(errorsProduct))
            {
                Product.ProductCategory = SelectedCategory;
                Product.ProductSubcategory = SelectedSubcategory;

                Product KnownProduct = _uow.ProductRepo.Get(p => p.Code == Product.Code).FirstOrDefault();
                if (KnownProduct == null)
                {
                    succesProduct = AddNewProduct(Product);
                    KnownProduct = new Product (){ProductID = Product.ProductID};
                }

                //Checking if this product already exists and add it if not
                if (succesProduct)
                {
                    string errorsPharmacyProduct = ValidateInputFieldsPharmacyProduct(QtyInStorage, QtyOrdered);

                    if (string.IsNullOrWhiteSpace(errorsPharmacyProduct))
                    {
                        PharmacyProduct = new PharmacyProduct()
                        {
                            PharmacyID = Pharmacy.PharmacyID,
                            ProductID = KnownProduct.ProductID,
                            DateOfOrder = DateTime.Now,
                            QtyOrdered = int.Parse(this.QtyOrdered),
                            QtyInStorage = int.Parse(this.QtyInStorage)
                        };
                        if (!PharmacyProducts.Contains(PharmacyProduct))
                        {
                            if (PharmacyProduct.IsValid())
                            {
                                _uow.PharmacyProductRepo.Add(PharmacyProduct);
                                int ok = _uow.Save();
                                if (ok > 0)
                                {
                                    succesProductToPharmacy = true;
                                }
                                else
                                {
                                    MessageBox.Show("Er is iets misgegaan bij het toevoegen van het product aan de lijst!", "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show(PharmacyProduct.Error, "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show($"Product met code {Product.Code} bestaat al in uw apotheek!", "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Error);

                        }
                    }
                    else
                    {
                        MessageBox.Show(errorsPharmacyProduct, "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    if (Product.Error != "")
                    {
                        MessageBox.Show(Product.Error, "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        MessageBox.Show("Er is iets misgegaan bij het toevoegen van het product!", "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show(errorsProduct, "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return succesProductToPharmacy;
        }

        private string ValidateInputFieldsPharmacyProduct(string qtyInStorage, string qtyOrdered)
        {
            if (!int.TryParse(qtyInStorage, out int validQtyInStorage))
            {
                return "\"Aantal in stock\" moet een numerieke waarde hebben!";
            }
            if (!int.TryParse(qtyOrdered, out int validQtyOrdered))
            {
                return "\"Aantal besteld\" moet een numerieke waarde hebben!";
            }
            return "";
        }

        private bool AddNewProduct(Product product)
        {
            if (product.IsValid())
            {
                _uow.ProductRepo.Add(product);
                int ok = _uow.Save();
                if (ok > 0)
                {
                    return true;
                }
            }
            return false;
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


        #endregion
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


    }
}
