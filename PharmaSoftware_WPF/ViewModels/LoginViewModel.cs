using System;
using PharmaSoftware_WPF.Views;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PharmaSoftware_DAL.Data.UnitOfWork;
using PharmaSoftware_DAL.Data;
using PharmaSoftware_DAL;
using PharmaSoftware_DAL.Services.HashingServices;
using PharmaSoftware_WPF.State.Authenticators;
using Prism.Commands;
using GalaSoft.MvvmLight.Command;
using PharmaSoftware_WPF.State.ManageWIndows;

namespace PharmaSoftware_WPF.ViewModels
{
    public class LoginViewModel: BaseViewModel, IDisposable
    {
        private readonly IUnitOfWork _uow = new UnitOfWork(new PharmaSoftwareEntities());
        private readonly IHashingService passwordHasher = new HashingService();


        public Pharmacy Pharmacy { get; set; }

        public override string this[string columnName] => throw new NotImplementedException();

        private void Login()
        {
            string errors = ValidateInputFields();
            if (errors == "")
            {
                /*Pharmacy checkPharm = new Pharmacy
                {
                    Username = Username,
                    PasswordHash = Password
                };*/

                Pharmacy pharm = _uow.PharmacyRepo.Get(x => x.Username == Pharmacy.Username).FirstOrDefault();
                if (pharm != null)
                {
                    if (passwordHasher.DecryptString(pharm.PasswordHash) == passwordHasher.DecryptString(Pharmacy.PasswordHash))
                    {
                        Authenticator.CurrentUser = pharm;
                    }
                    else
                    {
                        Pharmacy.PasswordHash = "";
                        MessageBox.Show($"Ongeldig wachtwoord!", "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show($"Gebruiker met gebruikersnaam {Pharmacy.Username} bestaat niet!", "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
            else
            {
                MessageBox.Show(Pharmacy.Error, "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        private string ValidateInputFields()
        {
            if (string.IsNullOrWhiteSpace(Pharmacy.Username))
            {
                return "Gebruikersnaam is niet ingevuld!";
            }
            if (string.IsNullOrWhiteSpace(Pharmacy.PasswordHash))
            {
                return "Wachtwoord is niet ingevuld!";
            }
            return "";
        }

        private void ShowRegisterWindow()
        {
            RegisterView registerView = new RegisterView();
            RegisterViewModel registerViewModel = new RegisterViewModel();
            registerView.DataContext = registerViewModel;
            registerView.Show();
        }

        private void ShowStorageWindow(int id)
        {
            StorageView storageView = new StorageView();
            StorageViewModel storageViewModel = new StorageViewModel(id);
            storageView.DataContext = storageViewModel;
            storageView.Show();
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
        public RelayCommand<IClosable> ShowRegisterViewCommand { get; private set; }
        public RelayCommand<IClosable> ShowStorageViewCommand { get; private set; }

        public LoginViewModel()
        {
            Pharmacy = new Pharmacy();
            this.ShowRegisterViewCommand = new RelayCommand<IClosable>(this.ShowRegisterView);
            this.ShowStorageViewCommand = new RelayCommand<IClosable>(this.ShowStorageView);

        }

        private void ShowRegisterView(IClosable window)
        {
            if (window != null)
            {
                ShowRegisterWindow();
                window.Close();
            }
        }

        private void ShowStorageView(IClosable window)
        {
            Login();
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
