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

        private string copyPassword;

        public string CopyPassword
        {
            get
            {
                return copyPassword;
            }
            set
            {
                copyPassword = value;
                NotifyPropertyChanged(CopyPassword);
            }
        }
        private string phone { get; set; }

        public string ConvertPhone
        {
            get
            {
                return phone;
            }
            set
            {
                phone = value;
                NotifyPropertyChanged(nameof(ConvertPhone));
            }
        }


        /*private SecureString encryptedPassword { get; set; }
        public SecureString EncryptedPassword
        {
            get
            {
                return encryptedPassword;
            }
            set
            {
                encryptedPassword = value;
                NotifyPropertyChanged(Password);
            }
        }*/

        public RegisterViewModel()
        {
            this.Pharmacy = new Pharmacy();
            this.ShowLoginViewCommand = new RelayCommand<IClosable>(this.ShowLoginView);
            this.ShowStorageViewCommand = new RelayCommand<IClosable>(this.ShowStorageView);
        }

        private void AddPharmacy()
        {
            int validPhoneNr = 0;
            string errors = "";

            errors += ValidateInputFields(ConvertPhone,CopyPassword, ref validPhoneNr);
            if (string.IsNullOrWhiteSpace(errors))
            {
                Pharmacy.PhoneNr = validPhoneNr;
                Pharmacy.PasswordHash = Password;
                //string hashPass = passwordHasher.EncryptString(password);

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
                            MessageBox.Show("Het registreren is niet gelukt!");
                        }
                    }
                    else
                    {
                        MessageBox.Show(Pharmacy.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Deze gebruiker bestaat al!");

                }
            }
            else
            {
                MessageBox.Show(errors);
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

        public string ValidateInputFields(string phoneNr, string confirmPassword, ref int validPhoneNr)
        {
            if (!int.TryParse(phoneNr, out validPhoneNr))
            {
                return "Telefoonnummer moet een numerieke waarde hebben!";
            }

            if (passwordHasher.DecryptString(password) != passwordHasher.DecryptString(confirmPassword))
            {
                return "Wachtwoorden komen niet overeen!";
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return "Wachtwoord is niet ingevuld!";
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