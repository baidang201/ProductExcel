using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Signalway.CommThemes.Validation
{
    public class IntValidationRule : ValidationRule
    {
        public int Max { get; set; }
        public int Min { get; set; }
        public int Default { get; set; }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            /*原值验证*/
            if (value == null)
            {
                return new ValidationResult(false, "输入值无效！");
            }
            string strValue = value.ToString();
            int nValue = Default;

            //值验证
            if (!int.TryParse(strValue, out nValue))
            {
                return new ValidationResult(false, string.Format("输入值:{0}", strValue)
                    + System.Environment.NewLine
                    + "不是一个有效数值！");
            }

            /*范围验证*/
            if (nValue < Min || nValue > Max)
            {
                return new ValidationResult(false,
                    string.Format("输入值:{0}", nValue)
                    + System.Environment.NewLine
                    + string.Format("不在有效范围【{0},{1}】！", Min, Max));
            }

            return new ValidationResult(true, null);
        }
    }
}
