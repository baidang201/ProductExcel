using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Signalway.CommThemes.Converters
{
    public class IntNoShowZeroConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (null == value)
            {
                return "";
            }
            if (0 == int.Parse(value.ToString()))
            {
                return "";
            }
            else
            {
                return value.ToString();
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (null == value)
            {
                return null;
            }

            if ("" == value)
            {
                return 0;
            }
            else
            {
                return int.Parse(value.ToString());
            }
        }
    }

    public class DoubleNoShowZeroConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (null == value)
            {
                return "";
            }
            if (0.0 == System.Convert.ToDouble(value.ToString()))
            {
                return "";
            }
            else
            {
                return value.ToString();
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (null == value)
            {
                return null;
            }

            if ("" == value)
            {
                return 0.0;
            }
            else
            {
                return int.Parse(value.ToString());
            }
        }
    }
}

   
