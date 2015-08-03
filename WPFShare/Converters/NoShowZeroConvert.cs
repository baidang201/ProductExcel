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

            int outValue = 0;
            if (!int.TryParse(value.ToString(), out outValue))
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
                return 0;
            }

            if ("" == value)
            {
                return 0;
            }
            else
            {
                int outValue =0;
                if (int.TryParse(value.ToString(), out outValue))
                {
                    return outValue;
                }
                else
                {
                    return 0;
                }
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

            double outValue = 0;
            if (!double.TryParse(value.ToString(), out outValue))
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
                return 0.0;
            }

            if ("" == value)
            {
                return 0.0;
            }
            else
            {
                double outValue = 0.0;
                if (double.TryParse(value.ToString(), out outValue))
                {
                    return outValue;
                }
                else
                {
                    return 0.0;
                }
            }
        }
    }
}

   
