using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Signalway.CommThemes.Converters
{
    public class BoolToStrConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string strValue = value.ToString();
            if (strValue == "1")
                return true;
            else
                return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool? isCheck = (bool?)value;
            if (isCheck.HasValue == false || isCheck.Value == false)
                return "0";
            else
                return "1";
        }
    }
}
