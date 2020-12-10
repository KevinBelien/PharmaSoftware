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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PharmaSoftware_WPF.ViewModels
{
    public class EditProfileViewModel : BaseViewModel, IDisposable, INotifyPropertyChanged
    {
        private readonly IUnitOfWork _uow = new UnitOfWork(new PharmaSoftwareEntities());

        public RelayCommand<IClosable> LogoutCommand { get; private set; }
        public RelayCommand<IClosable> ShowProfileViewCommand { get; private set; }
        public RelayCommand<IClosable> EditProfileCommand { get; private set; }

        public Pharmacy Pharmacy { get; set; }
        public ObservableCollection<PharmacyProduct> PharmacyProducts { get; set; }

        //public string ConvertPhone { get; set; }
        private string _convertPhone;

        public string ConvertPhone
        {
            get { return this._convertPhone; }
            set { _convertPhone = value;
                NotifyPropertyChanged(ConvertPhone);
            }
        }
        public int QtyStockIssues { get; set; }

        public override string this[string columnName] => throw new NotImplementedException();

        public EditProfileViewModel(int id)
        {
            this.LogoutCommand = new RelayCommand<IClosable>(this.Logout);
            this.ShowProfileViewCommand = new RelayCommand<IClosable>(this.ShowProfileView);
            this.EditProfileCommand = new RelayCommand<IClosable>(this.EditProfile);

            Pharmacy = _uow.PharmacyRepo.Get(p => p.PharmacyID == id).FirstOrDefault();
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
        private ObservableCollection<Pharmacy> GetAllPharmacies()
        {
            return new ObservableCollection<Pharmacy>(_uow.PharmacyRepo.Get());
        }

        private bool EditAccount()
        {
                if (!GetAllPharmacies().Where(p => p.PharmacyID != Authenticator.CurrentUser.PharmacyID).Contains(Pharmacy))
                {
                    if (Pharmacy.IsValid())
                    {
                        _uow.PharmacyRepo.Edit(Pharmacy);
                        int ok = _uow.Save();
                        if (ok > 0)
                        {
                            Authenticator.CurrentUser = Pharmacy;
                            return true;
                        }
                        MessageBox.Show("Er is iets misgegaan bij het wijzigen van je profiel!", "Foutmelding",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                    else
                    {
                        MessageBox.Show(Pharmacy.Error, "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                }
                MessageBox.Show("Deze gebruikersnaam bestaat al!", "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void EditProfile(IClosable window)
        {
            if (window != null)
            {
                if (EditAccount())
                {
                    ShowProfileWindow(Authenticator.CurrentUser.PharmacyID);
                    window.Close();
                }
            }
        }

        #endregion
    }
}
