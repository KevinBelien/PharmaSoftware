using PharmaSoftware_DAL;
using PharmaSoftware_WPF.ViewModels;
using PharmaSoftware_WPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaSoftware_WPF.State.Authenticators
{
    public static class Authenticator
    {

        public static Pharmacy CurrentUser { get; set; }

        public static bool isLoggedIn => CurrentUser != null;
        public static void LogOut()
        {
            CurrentUser = null;
            ShowLoginView();
        }

        private static void ShowLoginView()
        {
            LoginView login = new LoginView();
            LoginViewModel loginViewModel = new LoginViewModel();
            login.DataContext = loginViewModel;
            login.Show();
        }

    }
}
