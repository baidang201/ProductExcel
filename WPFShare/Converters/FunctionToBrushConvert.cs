using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace Signalway.CommThemes.Converters
{
    public class FunctionToBrushConvert : IValueConverter
    {
        private string[] FNames = new string[] { "修改IP", "烧写加密", "生产自动化测试", "版本升级", "下载INI", "上传INI", "存储格式化" , "高温老化"};
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string fname = value as string;

            if (FNames.Contains(fname))
                return Brushes.Red;
            else
                return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return 0;
        }
    }
}
