using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Signalway.CommThemes.Converters
{
    public class PlateWidthConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int nValue = 0;
            if(int.TryParse(value.ToString(),out nValue))
            {
                nValue = (int)Math.Round(Math.Pow(1.1D,nValue) * 56D);
            }

            return nValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int nValue = 0;
            if(int.TryParse(value.ToString(),out nValue))
            {
                nValue = (int)Math.Round(Math.Log(nValue / 56D,1.1D));
            }

            return nValue;
        }
    }
}
