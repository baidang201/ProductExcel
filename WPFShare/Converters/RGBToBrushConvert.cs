using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace Signalway.CommThemes.Converters
{
    public class RGBToBrushConvert:IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            byte R=0, G=0, B=0;
            byte.TryParse(values[0].ToString(), out R);
            byte.TryParse(values[1].ToString(), out G);
            byte.TryParse(values[2].ToString(), out B);

            Color color = Color.FromRgb(R, G, B);

            return color;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            Color color = (Color)value;

            object[] RGBValues = new object[3];
            RGBValues[0] = color.R.ToString();
            RGBValues[1] = color.G.ToString();
            RGBValues[2] = color.B.ToString();

            return RGBValues;
        }
    }
}
