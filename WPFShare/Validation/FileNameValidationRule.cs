using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Signalway.CommThemes.Validation
{
    public class FileNameValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if(value == null)
            {
                 return new ValidationResult(false, "无效文件名称！"); 
            }

            string filename = value.ToString().Trim();
            if (string.IsNullOrEmpty(filename))
            {
                return new ValidationResult(false, "无效文件名称！"); 
            }

            char c = filename.FirstOrDefault(r => r == '｛' || r == '｝'　|| r == '＄');
            if (c != '\0')
            {
                return new ValidationResult(false, "非法字符｛｝＄！"); 
            }

            string[] dollars = filename.Split(new string[]{"${"},StringSplitOptions.None);
            if (dollars.Length >= 2)
            {
                for (int i = 1; i < dollars.Length;++i )
                {
                    string oneVar = dollars[i];
                    if(oneVar[0] == '{') 
                    {
                        //连续 {{
                        return new ValidationResult(false, "无效通配符！"); 
                    }
                    int Index = oneVar.IndexOf('}');
                    if (Index == -1)
                    {
                        //不出现 }
                        return new ValidationResult(false, "无效通配符！"); 
                    }

                    string varName = oneVar.Substring(0, Index).Trim();
                    if (string.IsNullOrEmpty(varName))
                    {
                        //{}中间无内容
                        return new ValidationResult(false, "无效通配符！"); 
                    }

                    varName = varName.ToUpper();
                    if (varName != "YEAR"
                        && varName != "MONTH"
                        && varName != "DAY"
                        && varName != "HOUR"
                        && varName != "MINUTE"
                        && varName != "SECOND"
                        && varName != "MILLISECOND"
                        && varName != "CARDID"
                        && varName != "PLATECOLOR"
                        && varName != "PLATENAME"
                        && varName != "IMAGENO"
                        && varName != "SPEED"
                        && varName != "IP"
                        && varName != "DEVICE"
                        && varName != "PECCANCY"
                        && varName != "WAY")
                    {
                        return new ValidationResult(false, "无效变量名称！"); 
                    }
                }
            }

            return new ValidationResult(true,""); 
        }
    }
}
