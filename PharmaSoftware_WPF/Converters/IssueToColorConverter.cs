using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace PharmaSoftware_WPF.Converters
{
    [ValueConversion(typeof(int), typeof(Brush))]
    public class IssueToColorConverter : IValueConverter
    {
        public int QtyStockIssues { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int QtyStockIssues;
            int.TryParse(value.ToString(), out QtyStockIssues);
            if (QtyStockIssues > 0)
            {
                return Brushes.Red;
            }
            else
            {
                return Brushes.Black;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
