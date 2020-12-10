using PharmaSoftware_DAL.Data;
using PharmaSoftware_DAL.Data.UnitOfWork;
using PharmaSoftware_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Security;
using PharmaSoftware_WPF.State.Authenticators;
using PharmaSoftware_WPF.Views;
using System.Collections.ObjectModel;
using PharmaSoftware_DAL.Services.HashingServices;
using PharmaSoftware_WPF.State.ManageWIndows;
using GalaSoft.MvvmLight.Command;

namespace PharmaSoftware_WPF.ViewModels
{
    public class RegisterViewModel: BaseViewModel, IDisposable
    {
        private readonly IUnitOfWork _uow = new UnitOfWork(new PharmaSoftwareEntities());
        private readonly IHashingService passwordHasher = new HashingService();

        public RelayCommand<IClosable> ShowLoginViewCommand { get; private set; }
        public RelayCommand<IClosable> ShowStorageViewCommand { get; private set; }

        public Pharmacy Pharmacy { get; set; }
        public string CopyPassword { get; set; }
        public string ConvertPhone { get; set; }

        public RegisterViewModel()
        {
            this.Pharmacy = new Pharmacy();
            this.ShowLoginViewCommand = new RelayCommand<IClosable>(this.ShowLoginView);
            this.ShowStorageViewCommand = new RelayCommand<IClosable>(this.ShowStorageView);
        }

        private void AddPharmacy()
        {
            string errors = "";

            errors += ValidateInputFields(CopyPassword);
            if (string.IsNullOrWhiteSpace(errors))
            {
                if (!GetAllPharmacies().Contains(Pharmacy))
                {
                    if (Pharmacy.IsValid())
                    {
                        _uow.PharmacyRepo.Add(Pharmacy);
                        int ok = _uow.Save();
                        if (ok > 0)
                        {
                            Authenticator.CurrentUser = Pharmacy;
                        }
                        else
                        {
                            MessageBox.Show("Het registreren is niet gelukt!", "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show(Pharmacy.Error, "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Deze gebruiker bestaat al!", "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
            else
            {
                MessageBox.Show(errors, "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public override string this[string columnName] => throw new NotImplementedException();

        private void ShowStorageWindow(int id)
        {
            StorageView storageView = new StorageView();
            StorageViewModel storageViewModel = new StorageViewModel(id);
            storageView.DataContext = storageViewModel;
            storageView.Show();
        }

        public string ValidateInputFields(string confirmPassword)
        {

            if (string.IsNullOrWhiteSpace(Pharmacy.PasswordHash))
            {
                return "Wachtwoord is niet ingevuld!";
            }
            if (passwordHasher.DecryptString(Pharmacy.PasswordHash) != passwordHasher.DecryptString(confirmPassword))
            {
                return "Wachtwoorden komen niet overeen!";
            }
         
            return "";
        }

        private ObservableCollection<Pharmacy> GetAllPharmacies()
        {
            return new ObservableCollection<Pharmacy>(_uow.PharmacyRepo.Get());
        }

        private void ShowLoginWindow()
        {
            LoginView login = new LoginView();
            LoginViewModel loginViewModel = new LoginViewModel();
            login.DataContext = loginViewModel;
            login.Show();
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

        private void ShowLoginView(IClosable window)
        {
            if (window != null)
            {
                ShowLoginWindow();
                window.Close();
            }
        }

        private void ShowStorageView(IClosable window)
        {
            AddPharmacy();
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
    }
}