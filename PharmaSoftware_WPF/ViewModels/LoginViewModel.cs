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

        private string username;
        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
                NotifyPropertyChanged(nameof(Username));
            }
        }

        private string password;
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
                NotifyPropertyChanged(Password);
            }
        }

        public override string this[string columnName] => throw new NotImplementedException();

        private void Login()
        {
            string errors = ValidateInputFields(Password);
            if (errors == "")
            {
                Pharmacy checkPharm = new Pharmacy
                {
                    Username = Username,
                    PasswordHash = Password
                };

                Pharmacy pharm = _uow.PharmacyRepo.Get(x => x.Username == checkPharm.Username).FirstOrDefault();
                if (pharm != null)
                {
                    if (passwordHasher.DecryptString(pharm.PasswordHash) == checkPharm.PasswordHash)
                    {
                        Authenticator.CurrentUser = pharm;
                    }
                    else
                    {
                        Password = "";
                        MessageBox.Show($"Ongeldig wachtwoord!", "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show($"Gebruiker met gebruikersnaam {checkPharm.Username} bestaat niet!", "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
            else
            {
                MessageBox.Show(errors, "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        private string ValidateInputFields(string password)
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                return "Gebruikersnaam is niet ingevuld!";
            }
            if (string.IsNullOrWhiteSpace(password))
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
