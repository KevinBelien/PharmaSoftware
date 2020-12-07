using MaterialDesignThemes.Wpf;
using PharmaSoftware_WPF.State.ManageWIndows;
using PharmaSoftware_WPF.ViewModels;
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

        /*private void btnRowDetailChanged_Click(object sender, RoutedEventArgs e)
        {
            DependencyObject obj = (DependencyObject)e.OriginalSource;
            while (!(obj is DataGridRow) && obj != null) obj = VisualTreeHelper.GetParent(obj);

            if (obj is DataGridRow)
            {
                if ((obj as DataGridRow).DetailsVisibility == Visibility.Visible)
                {
                    (obj as DataGridRow).DetailsVisibility = Visibility.Collapsed;

                }
                else
                {
                    (obj as DataGridRow).DetailsVisibility = Visibility.Visible;            
                }
            }
        }*/



        /*private void exp_Collapsed(object sender, RoutedEventArgs e)
        {
            this.dgdSupply.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed;
        }

        private void exp_Expanded(object sender, RoutedEventArgs e)
        {
            this.dgdSupply.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.VisibleWhenSelected;
        }*/

    }
}
