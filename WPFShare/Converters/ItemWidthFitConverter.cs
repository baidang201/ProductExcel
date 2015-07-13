using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Signalway.CommThemes.Converters
{
    /// <summary>
    /// 按照条件计算合适宽度,最小宽度160
    /// </summary>
    [ValueConversion(typeof(double), typeof(double))]
    public class ItemWidthFitConverter : IValueConverter
    {
        /*对ListBox的宽度进行计算，计算合适的Item宽度，铺满整个窗口*/
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double minValue = 180D;
            double width = 0;
            if (value != null && double.TryParse(value.ToString(), out width))
            {
                int itemCount = 0;
                if (parameter == null || !int.TryParse(parameter.ToString(), out itemCount))
                {
                    itemCount = 6;
                }
                double calValue = Math.Floor((width - 10 * itemCount) / itemCount * 0.85D);
                return calValue;
            }

            return minValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
