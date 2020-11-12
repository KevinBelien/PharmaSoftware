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
            StorageView mainView = new StorageView();
            StorageViewModel mainViewModel = new StorageViewModel();
            mainView.DataContext = mainViewModel;
            mainView.Show();
        }
    }
}
