using PharmaSoftware_WPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaSoftware_WPF.ViewModels
{
    public class MenuButtonViewModel : BaseViewModel
    {

        public override string this[string columnName] => throw new NotImplementedException();

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            switch (parameter.ToString())
            {
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
