using GalaSoft.MvvmLight.Command;
using PharmaSoftware_DAL;
using PharmaSoftware_DAL.Data;
using PharmaSoftware_DAL.Data.UnitOfWork;
using PharmaSoftware_WPF.State.Authenticators;
using PharmaSoftware_WPF.State.ManageWIndows;
using PharmaSoftware_WPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PharmaSoftware_WPF.ViewModels
{
    public class ProductViewModel : BaseViewModel, IDisposable
    {
        private readonly IUnitOfWork _uow = new UnitOfWork(new PharmaSoftwareEntities());
        public RelayCommand<IClosable> LogoutCommand { get; private set; }
        public RelayCommand<IClosable> CancelCommand { get; private set; }
        public RelayCommand<IClosable> ShowStorageViewCommand { get; private set; }
        public RelayCommand<IClosable> ShowProfileViewCommand { get; private set; }

        Pharmacy Pharmacy { get; set; }

        Product Product { get; set; }

        public override string this[string columnName] => throw new NotImplementedException();

        public ProductViewModel(int id)
        {
            this.LogoutCommand = new RelayCommand<IClosable>(this.Logout);
            this.CancelCommand = new RelayCommand<IClosable>(this.ShowStorageView);
            this.ShowStorageViewCommand = new RelayCommand<IClosable>(this.ShowStorageView);
            this.ShowProfileViewCommand = new RelayCommand<IClosable>(this.ShowProfileView);

            Pharmacy = _uow.PharmacyRepo.Get(p => p.PharmacyID == id,
                p => p.PharmacyProducts)
                .FirstOrDefault();
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
