using GalaSoft.MvvmLight.Command;
using PharmaSoftware_DAL;
using PharmaSoftware_DAL.Data;
using PharmaSoftware_DAL.Data.UnitOfWork;
using PharmaSoftware_WPF.State.Authenticators;
using PharmaSoftware_WPF.State.ManageWIndows;
using PharmaSoftware_WPF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PharmaSoftware_WPF.ViewModels
{
    public class StorageViewModel : BaseViewModel, IDisposable
    {
        private readonly IUnitOfWork _uow = new UnitOfWork(new PharmaSoftwareEntities());
        public RelayCommand<IClosable> LogoutCommand { get; private set; }
        public RelayCommand<IClosable> ShowProductViewCommand { get; private set; }
        public RelayCommand<IClosable> ShowProfileViewCommand { get; private set; }


        Pharmacy Pharmacy { get; set; }
        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<Product> PharmacyProducts { get; set; } 


        public StorageViewModel(int id)
        {
            this.LogoutCommand = new RelayCommand<IClosable>(this.Logout);
            this.ShowProductViewCommand = new RelayCommand<IClosable>(this.ShowProductView);
            this.ShowProfileViewCommand = new RelayCommand<IClosable>(this.ShowProfileView);

            Pharmacy = _uow.PharmacyRepo.Get(p => p.PharmacyID == id, 
                p => p.PharmacyProducts)
                .FirstOrDefault();

            Products = new ObservableCollection<Product>(_uow.ProductRepo.Get());

            PharmacyProducts = GetAllProductsFromPharmacy();

        }

        private ObservableCollection<Product> GetAllProductsFromPharmacy()
        {
            ObservableCollection<Product> productList = new ObservableCollection<Product>();

            foreach (var pp in Pharmacy.PharmacyProducts)
            {
                foreach (var product in Products)
                    if (pp.ProductID == product.ProductID)
                    {
                        productList.Add(product);
                    }
            }
            return productList;
        }

        public override string this[string columnName] => throw new NotImplementedException();

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter) {
            switch (parameter.ToString())
            {
                case "CloseApp": Application.Current.Shutdown(); break;
            }
        }

        private void ShowProductWindow(int id)
        {
            ProductView productView= new ProductView();
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
