﻿using GalaSoft.MvvmLight.Command;
using MaterialDesignThemes.Wpf;
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
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace PharmaSoftware_WPF.ViewModels
{
    public class StorageViewModel : BaseViewModel, IDisposable
    {
        private readonly IUnitOfWork _uow = new UnitOfWork(new PharmaSoftwareEntities());
        public RelayCommand<IClosable> LogoutCommand { get; private set; }
        public RelayCommand<IClosable> ShowProductViewCommand { get; private set; }
        public RelayCommand<IClosable> ShowProfileViewCommand { get; private set; }
        public RelayCommand<IClosable> EditPharmacyProductCommand { get; private set; }
        public RelayCommand<IClosable> DeleteProductsCommand { get; private set; }


        Pharmacy Pharmacy { get; set; }

        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<PharmacyProduct> PharmacyProducts { get; set; }
        public int QtyStockIssues { get; set; }

        public PharmacyProduct SelectedProduct { get; set; }
        public PharmacyProduct PharmacyProduct { get; set; }
        public bool IsSelecteed { get; set; }

        public StorageViewModel(int id)
        {
            this.LogoutCommand = new RelayCommand<IClosable>(this.Logout);
            this.ShowProductViewCommand = new RelayCommand<IClosable>(this.ShowProductView);
            this.ShowProfileViewCommand = new RelayCommand<IClosable>(this.ShowProfileView);
            this.EditPharmacyProductCommand = new RelayCommand<IClosable>(this.EditPharmacyProduct);
            this.DeleteProductsCommand = new RelayCommand<IClosable>(this.DeleteProducts);

            Pharmacy = _uow.PharmacyRepo.Get(p => p.PharmacyID == id,
                p => p.PharmacyProducts.Select(pp => pp.Product))
                .FirstOrDefault();

            Products = new ObservableCollection<Product>(_uow.ProductRepo.Get());
            PharmacyProducts = new ObservableCollection<PharmacyProduct>(_uow.PharmacyProductRepo.Get(pp => pp.PharmacyID == Pharmacy.PharmacyID,
                pp => pp.Product.ProductCategory));

            //PharmacyProducts = GetAllProductsFromPharmacy();
            QtyStockIssues = CountStockIssues(5);


        }

        private int CountStockIssues(int minimumStock)
        {
            int issues = 0;
            foreach (PharmacyProduct product in PharmacyProducts)
            {
                if ((product.QtyInStorage + product.QtyOrdered) <= minimumStock)
                {
                    issues++;
                }
            }
            return issues;
        }

        public override string this[string columnName] => throw new NotImplementedException();

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            switch (parameter.ToString())
            {
                case "CloseApp": Application.Current.Shutdown(); break;
                case "RowDetailChanged":; break;

            }
        }

        private void ShowProductWindow(int id)
        {
            ProductView productView = new ProductView();
            ProductViewModel productViewModel = new ProductViewModel(id);
            productView.DataContext = productViewModel;
            productView.Show();
        }

        private void ShowProfileWindow(int id)
        {
            ProfileView profileView = new ProfileView();
            ProfileViewModel profileViewModel = new ProfileViewModel(id);
            profileView.DataContext = profileViewModel;
            profileView.Show();
        }

        public void Dispose()
        {
            _uow?.Dispose();
        }


        #region Commands
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
        private void EditPharmacyProduct(IClosable window)
        {
            if (window != null)
            {
                if (Authenticator.isLoggedIn)
                {
                    bool productEdited = EditProduct();
                    if (productEdited)
                    {
                        ShowStorageWindow(Authenticator.CurrentUser.PharmacyID);
                        window.Close();
                    }

                }
            }
        }


        private bool EditProduct()
        {
            if (SelectedProduct != null)
            {
                if (SelectedProduct.IsValid())
                {
                    _uow.PharmacyProductRepo.Edit(SelectedProduct);
                    int ok = _uow.Save();
                    if (ok > 0)
                    {
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("De data is niet veranderd!", "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show(SelectedProduct.Error, "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        private void DeleteProducts(IClosable window)
        {
            if (window != null)
            {
                if (Authenticator.isLoggedIn)
                {
                    bool productDeleted = DeleteProducts();
                    if (productDeleted)
                    {
                        ShowStorageWindow(Authenticator.CurrentUser.PharmacyID);
                        window.Close();
                    }

                }
            }
        }

        private bool DeleteProducts()
        {
            int selectedItems = 0;

            foreach (PharmacyProduct product in PharmacyProducts)
            {
                if (product.IsSelected)
                {
                    selectedItems++;

                    _uow.PharmacyProductRepo.Delete(product);
                    List<PharmacyProduct> checkProductList = _uow.PharmacyProductRepo.Get(x => x.ProductID == product.ProductID).ToList();
                    if (checkProductList.Count <= 1)
                    {
                        _uow.ProductRepo.Delete<int>(product.ProductID);
                    }
                    else
                    {
                        MessageBox.Show($"Er is iets misgelopen tijdens het verwijderen!","Foutmelding",MessageBoxButton.OK,MessageBoxImage.Error);
                        return false;
                    }
                }
            }
            if (selectedItems == 0)
            {
                MessageBox.Show($"Selecteer eerst een product!", "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            int ok = _uow.Save();
            if (ok == 0)
            {
                MessageBox.Show($"Er is iets misgelopen tijdens het verwijderen!", "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void ShowStorageWindow(int id)
        {
            StorageView storageView = new StorageView();
            StorageViewModel storageViewModel = new StorageViewModel(id);
            storageView.DataContext = storageViewModel;
            storageView.Show();
        }
        private void ShowProductView(IClosable window)
        {
            if (window != null)
            {
                if (Authenticator.isLoggedIn)
                {
                    ShowProductWindow(Authenticator.CurrentUser.PharmacyID);
                    window.Close();
                }
            }
        }

        #endregion
    }
}
