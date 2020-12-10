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
    public class IntToColorConverter: IValueConverter
    {
        public int QtyInStorage { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int QtyInStorage;
            Int32.TryParse(value.ToString(), out QtyInStorage);
            if (QtyInStorage < 5)
            {
                return Brushes.Red;
            }
            else if (QtyInStorage < 10)
            {
                return Brushes.Orange;
            }
            else
            {
                return Brushes.Green;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
