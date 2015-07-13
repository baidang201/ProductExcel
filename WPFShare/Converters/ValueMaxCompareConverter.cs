using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Signalway.CommThemes.Converters
{
    /// <summary>
    /// 比较值是否大于门槛值,返回bool
    /// </summary>
    public class MaxValueCompareConverter : IValueConverter
    {
        /*比较转换值是否大于设定值*/
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double condition = 50D;
            if (!double.TryParse(parameter.ToString(), out condition))
            {
                condition = 0D;
            }

            double dval = 0;
            double.TryParse(value.ToString(), out dval);

            return dval > condition;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
