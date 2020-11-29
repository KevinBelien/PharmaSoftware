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

namespace PharmaSoftware_WPF.ViewModels
{
    public class LoginViewModel: BaseViewModel, IDisposable
    {
        private readonly IUnitOfWork _uow = new UnitOfWork(new PharmaSoftwareEntities());
        private readonly IHashingService passwordHasher = new HashingService();
        private readonly Authenticator _authenticator = new Authenticator();

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

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            switch (parameter.ToString())
            {
                case "Login": Login(); break;
                case "CloseApp": Application.Current.Shutdown(); break;
                case "ShowRegisterView": ShowRegisterView(); break;
            }
        }

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
                string testpass = passwordHasher.DecryptString(pharm.PasswordHash);
                if (pharm != null)
                {
                    if (passwordHasher.DecryptString(pharm.PasswordHash) == checkPharm.PasswordHash)
                    {
                        _authenticator.CurrentUser = pharm;
                        ShowStorageView(pharm.PharmacyID);
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
            if (string.IsNullOrWhiteSpace(password))
            {
                return "Wachtwoord is niet ingevuld!";
            }
            return "";
        }

        private void ShowRegisterView()
        {
            RegisterView registerView = new RegisterView();
            RegisterViewModel registerViewModel = new RegisterViewModel();
            registerView.DataContext = registerViewModel;
            registerView.Show();
        }

        private void ShowStorageView(int id)
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
    }
}
