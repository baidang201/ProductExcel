using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Windows;

namespace Signalway.CommThemes.Converters
{
    /// <summary>
    /// 转换bool 显示可见属性.
    /// </summary>
    public class BooleanToVisibilityConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool flag = false;
            if (value is bool) 
            {
                flag = (bool)value;
            }
            else if (value is bool?) 
            {
                bool? nullable = (bool?)value;
                flag = nullable.HasValue ? nullable.Value : false;
            }

            bool inverse = (parameter as string) == "inverse";

            if (inverse) 
            {
                return (flag ? Visibility.Collapsed : Visibility.Visible);
            }
            else 
            {
                return (flag ? Visibility.Visible : Visibility.Hidden);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
