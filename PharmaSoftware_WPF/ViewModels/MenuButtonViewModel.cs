using GalaSoft.MvvmLight.Command;
using PharmaSoftware_WPF.State.Authenticators;
using PharmaSoftware_WPF.State.ManageWIndows;
using PharmaSoftware_WPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaSoftware_WPF.ViewModels
{
    public class MenuButtonViewModel
    {
        public RelayCommand<IClosable> ShowStorageViewCommand { get; private set; }

        public MenuButtonViewModel()
        {
            this.ShowStorageViewCommand = new RelayCommand<IClosable>(this.ShowStorageView);

        }
        
        private void ShowStorageView(IClosable window)
        {
            if (window != null)
            {

                if (Authenticator.isLoggedIn)
                {
                    ShowStorageWindow(Authenticator.CurrentUser.PharmacyID);
                    window.Close();
                }
            }
        }

        private void ShowStorageWindow(int id)
        {
            StorageView storageView = new StorageView();
            StorageViewModel storageViewModel = new StorageViewModel(id);
            storageView.DataContext = storageViewModel;
            storageView.Show();
        }
    }
}
