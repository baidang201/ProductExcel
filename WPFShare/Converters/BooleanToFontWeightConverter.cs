using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace Signalway.CommThemes.Converters
{
    /// <summary>
    /// 转换bool到字体权重 (false: normal, true: bold)
    /// </summary>
    public class BooleanToFontWeightConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool inverse = (parameter as string) == "inverse";

            var bold = value as bool?;
            if (bold.HasValue && bold.Value) {
                return inverse ? FontWeights.Normal : FontWeights.Bold;
            }
            return inverse ? FontWeights.Bold : FontWeights.Normal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
