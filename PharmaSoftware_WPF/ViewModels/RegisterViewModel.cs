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

namespace PharmaSoftware_WPF.ViewModels
{
    public class RegisterViewModel: BaseViewModel, IDisposable
    {
        IUnitOfWork _uow = new UnitOfWork(new PharmaSoftwareEntities());
        private readonly IHashingService passwordHasher = new HashingService();
        IAuthenticator _authenticator = new Authenticator();

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

        private string city { get; set; }

        public string City
        {
            get
            {
                return city;
            }
            set
            {
                city = value;
                NotifyPropertyChanged(nameof(City));
            }
        }

        private string district { get; set; }

        public string District
        {
            get
            {
                return district;
            }
            set
            {
                district = value;
                NotifyPropertyChanged(nameof(District));
            }
        }

        private string street{ get; set; }

        public string Street
        {
            get
            {
                return street;
            }
            set
            {
                street = value;
                NotifyPropertyChanged(nameof(Street));
            }
        }

        private string zip { get; set; }

        public string ZIP
        {
            get
            {
                return zip;
            }
            set
            {
                zip = value;
                NotifyPropertyChanged(nameof(ZIP));
            }
        }

        private string houseNr { get; set; }

        public string HouseNr
        {
            get
            {
                return houseNr;
            }
            set
            {
                houseNr= value;
                NotifyPropertyChanged(nameof(HouseNr));
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
        }

        private void AddPharmacy()
        {
            int validPhoneNr = 0;
            string errors = "";

            errors += ValidateInputFields(ConvertPhone,CopyPassword, ref validPhoneNr);
            if (string.IsNullOrWhiteSpace(errors))
            {
                string hashPass = passwordHasher.EncryptString(password);

                Pharmacy pharmacy = FillPharmacy(Username, hashPass, validPhoneNr, ZIP, Street, City, District, HouseNr);

                if (!GetAllPharmacies().Contains(pharmacy))
                {
                    if (pharmacy.IsValid())
                    {
                        _uow.PharmacyRepo.Add(pharmacy);
                        int ok = _uow.Save();
                        if (ok > 0)
                        {
                            _authenticator.CurrentUser = pharmacy;
                            ShowStorageView(_authenticator.CurrentUser.PharmacyID);

                        }
                        else
                        {
                            MessageBox.Show("Het registreren is niet gelukt!");
                        }
                    }
                    else
                    {
                        MessageBox.Show(pharmacy.Error);
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

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            switch (parameter.ToString())
            {
                case "CloseApp": Application.Current.Shutdown(); break;
                case "AddPharmacy": AddPharmacy(); break;
                case "ShowLoginView": ShowLoginView(); break;
            }
        }

        private void ShowStorageView(int id)
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

            if (password != confirmPassword)
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

        private Pharmacy FillPharmacy(string username, string password, int phoneNr, string zip, string street, string city, string district, string houseNr)
        {
            Pharmacy pharmacy = new Pharmacy()
            {
                Username = username,
                PasswordHash = password,
                PhoneNr = phoneNr,
                ZIP = zip,
                Street = street,
                City = city,
                District = district,
                HouseNr = houseNr
            };

            return pharmacy;
        }

        private void ShowLoginView()
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
    }
}
