using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Signalway.CommThemes.Converters
{
    public class StrToDoubleConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return 0D;

            double dValue = 0;
            double.TryParse(value.ToString(), out dValue);

            return dValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return "0";

            double dValue = 0;
            double.TryParse(value.ToString(), out dValue);

            return dValue.ToString("F4");
        }
    }
}
