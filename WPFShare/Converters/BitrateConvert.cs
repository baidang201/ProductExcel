using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Signalway.CommThemes.Converters
{
    public class BitrateConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return 0;

            int nValue = 0;
            int.TryParse(value.ToString(), out nValue);
            int pmValue = 1024 * 8;
            if (parameter != null) int.TryParse(parameter.ToString(), out pmValue);
            return nValue / pmValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return "0";
            int nValue = 0;
            int.TryParse(value.ToString(), out nValue);
            int pmValue = 1024 * 8;
            if (parameter != null) int.TryParse(parameter.ToString(), out pmValue);

            return (nValue * pmValue).ToString();
        }
    }
}
