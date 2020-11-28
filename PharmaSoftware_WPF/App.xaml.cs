using Microsoft.Extensions.DependencyInjection;
using PharmaSoftware_DAL.Services.HashingServices;
using PharmaSoftware_WPF.State.Authenticators;
using PharmaSoftware_WPF.ViewModels;
using PharmaSoftware_WPF.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PharmaSoftware_WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            RegisterView mainView = new RegisterView();
            RegisterViewModel mainViewModel = new RegisterViewModel();
            mainView.DataContext = mainViewModel;
            mainView.Show();
        }
    }
}
