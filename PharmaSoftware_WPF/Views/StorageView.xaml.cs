using PharmaSoftware_WPF.State.ManageWIndows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PharmaSoftware_WPF.Views
{
    /// <summary>
    /// Interaction logic for StorageView.xaml
    /// </summary>
    public partial class StorageView : Window, IClosable
    {
        public StorageView()
        {
            InitializeComponent();
        }

        private void exp_Collapsed(object sender, RoutedEventArgs e)
        {
            this.dgdSupply.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed;
        }

        private void exp_Expanded(object sender, RoutedEventArgs e)
        {
            this.dgdSupply.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.VisibleWhenSelected;
        }

    }
}
