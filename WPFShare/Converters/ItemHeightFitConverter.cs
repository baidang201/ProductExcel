using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Signalway.CommThemes.Converters
{
    /// <summary>
    /// 按照条件比例计算合适宽度
    /// </summary>
    [ValueConversion(typeof(double), typeof(double))]
    public class ItemHeightFitConverter : IValueConverter
    {
        /*对ListBox的宽度进行计算，计算合适的Item宽度，铺满整个窗口*/
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double width = 0;
            if (value != null && double.TryParse(value.ToString(), out width))
            {
                double rate = 0.8;
                if (parameter == null || !double.TryParse(parameter.ToString(), out rate))
                {
                    rate = 0.8;
                }
                double calValue = Math.Floor(width * rate);
                return calValue;
            }

            return 60;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
