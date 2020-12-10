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
    public class ProfileViewModel: BaseViewModel, IDisposable
    {
        private readonly IUnitOfWork _uow = new UnitOfWork(new PharmaSoftwareEntities());

        public RelayCommand<IClosable> LogoutCommand { get; private set; }
        public RelayCommand<IClosable> ShowProfileViewCommand { get; private set; }
        public RelayCommand<IClosable> ShowEditProfileViewCommand { get; private set; }
        public RelayCommand<IClosable> DeleteProfileCommand { get; private set; }

        public Pharmacy Pharmacy { get; set; }
        public ObservableCollection<PharmacyProduct> PharmacyProducts { get; set; }

        public bool DisplayControl { get; set; }


        public int QtyStockIssues { get; set; }

        public override string this[string columnName] => throw new NotImplementedException();

        public ProfileViewModel(int id)
        {
            this.LogoutCommand = new RelayCommand<IClosable>(this.Logout);
            this.ShowProfileViewCommand = new RelayCommand<IClosable>(this.ShowProfileView);
            this.ShowEditProfileViewCommand = new RelayCommand<IClosable>(this.ShowEditProfileView);
            this.DeleteProfileCommand = new RelayCommand<IClosable>(this.DeleteProfile);

            Pharmacy = _uow.PharmacyRepo.Get(p => p.PharmacyID == id).FirstOrDefault();
            DisplayControl = Pharmacy.District != null ? true : false;

            PharmacyProducts = new ObservableCollection<PharmacyProduct>(_uow.PharmacyProductRepo.Get(pp => pp.PharmacyID == Pharmacy.PharmacyID));

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

        private bool DeleteAccount()
        {
            if (MessageBox.Show("Bent u zeker dat u dit account wil verwijderen.", "Waarschuwing",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                 _uow.PharmacyRepo.Delete(Pharmacy);
                int ok = _uow.Save();
                if (ok > 0)
                {
                    return true;
                }
                return false;
            }
            return false;
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

        private void ShowProfileWindow(int id)
        {
            ProfileView profileView = new ProfileView();
            ProfileViewModel profileViewModel = new ProfileViewModel(id);
            profileView.DataContext = profileViewModel;
            profileView.Show();
        }

        private void ShowEditProfileView(IClosable window)
        {
            if (window != null)
            {
                if (Authenticator.isLoggedIn)
                {
                    ShowEditProfileWindow(Authenticator.CurrentUser.PharmacyID);
                    window.Close();
                }
            }
        }

        private void ShowEditProfileWindow(int id)
        {
            EditProfileView editProfileView = new EditProfileView();
            EditProfileViewModel editProfileViewModel = new EditProfileViewModel(id);
            editProfileView.DataContext = editProfileViewModel;
            editProfileView.Show();
        }

        private void DeleteProfile(IClosable window)
        {
            if (window != null)
            {
                if (DeleteAccount())
                {
                    Logout(window);
                    window.Close();
                }
            }
        }
        #endregion
    }
}
