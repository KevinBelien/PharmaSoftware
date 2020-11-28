using PharmaSoftware_DAL;
using PharmaSoftware_WPF.State.Authenticators;
using PharmaSoftware_WPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PharmaSoftware_WPF.ViewModels
{
    public class StorageViewModel : BaseViewModel, IDisposable
    {
        private readonly IAuthenticator _authenticator = new Authenticator();

        public StorageViewModel(int id)
        {
            
        }

        public override string this[string columnName] => throw new NotImplementedException();

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public override void Execute(object parameter) {
            switch (parameter.ToString())
            {
                case "CloseApp": Application.Current.Shutdown(); break;
                case "ShowLoginView": ShowLoginView(); break;
            }
        }

        private void ShowLoginView()
        {
            LoginView login = new LoginView();
            LoginViewModel loginViewModel = new LoginViewModel();
            login.DataContext = loginViewModel;
            login.Show();
        }
    }
}
