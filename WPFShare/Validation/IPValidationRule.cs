using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Signalway.CommThemes.Validation
{
    public class IPValidationRule : ValidationRule
    {
        public enum IPKind
        {
            IPaddr = 0,
            Mask = 1,
            Getway = 2
        }

        private IPKind _kind = IPKind.IPaddr;
        /// <summary>
        /// 是否掩码
        /// </summary>
        public IPKind Kind
        {
            get { return _kind; }
            set { _kind = value; }
        }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            /*原值验证*/
            if (value == null)
            {
                return new ValidationResult(false, "输入值无效！");
            }
            string strValue = value.ToString();
            string[] fields = strValue.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            if (fields.Length != 4)
            {
                return new ValidationResult(false, "无效IP输入！");
            }

            int nValue = 0;
            int[] Values = new int[4];
            for (int i = 0; i < 4; ++i)
            {
                string field = fields[i];
                if (int.TryParse(field, out nValue) == false)
                {
                    return new ValidationResult(false, "无效IP数值！");
                }
                else
                {
                    if (nValue > 255 || nValue < 0)
                        return new ValidationResult(false, "无效IP数值！");
                    else
                        Values[i] = nValue;
                }
            }

            if (_kind == IPKind.Mask)
            {
                if (Values[0] != 255)
                    return new ValidationResult(false, "无效子网掩码！");
                else if (Values[1] > Values[0] || Values[2] > Values[1] || Values[3] > Values[2])
                {
                    return new ValidationResult(false, "无效子网掩码！");
                }
            }
            else if (_kind == IPKind.IPaddr)
            {
                if (Values[0] < 1 || Values[0] > 233 || Values[3] < 1 || Values.Any(r => r == 255))
                    return new ValidationResult(false, "无效输入IP！");
            }
            else if (_kind == IPKind.Getway)
            {
                if (Values[0] == 0 && Values.Any(r => r != 0))
                    return new ValidationResult(false, "无效输入网关！");
                else if (Values.Any(r => r == 255))
                    return new ValidationResult(false, "无效输入网关！");
            }


            return new ValidationResult(true, null);
        }

        public bool ValidationIP(string ip, string mask, ref string gateway)
        {
            string[] ipstrs = ip.Split('.');
            if (ipstrs.Length != 4) return false;
            Byte[] nbuf = new Byte[4];
            for (int j = 0; j < 4; ++j)
            {
                nbuf[j] = Convert.ToByte(ipstrs[3 - j], 10);
            }
            uint dwIP = BitConverter.ToUInt32(nbuf, 0);

            string[] maskstrs = mask.Split('.');
            if (maskstrs.Length != 4) return false;
            for (int j = 0; j < 4; ++j)
            {
                nbuf[j] = Convert.ToByte(maskstrs[3 - j], 10);
            }
            uint dwMask = BitConverter.ToUInt32(nbuf, 0);

            if (!string.IsNullOrEmpty(gateway) && gateway.Trim() != "0.0.0.0")
            {
                string[] bstrs = gateway.Split('.');
                if (bstrs.Length != 4) return false;
                for (int j = 0; j < 4; ++j)
                {
                    nbuf[j] = Convert.ToByte(bstrs[3 - j], 10);
                }
                uint dwGateway = BitConverter.ToUInt32(nbuf, 0);

                if ((dwIP & dwMask) != (dwGateway & dwMask))
                {
                    return false;
                }
            }
            else
            {
                gateway = "";
                for (int i = 0; i < 4; ++i)
                {
                    string str = maskstrs[i];
                    if (!string.IsNullOrEmpty(gateway)) gateway += ".";

                    if (str == "255") gateway += ipstrs[i];
                    else gateway += "1";
                }
            }

            return true;
        }
    }
}
