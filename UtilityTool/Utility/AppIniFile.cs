using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Utility
{
    public class AppIniFile
    {
        public static bool UpdateOption(ref List<Option> lstOptions, bool Load, string optionFileName)
        {
            string opFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,optionFileName);
            if (Load)
            {
                lstOptions = new List<Option>();
                StreamReader sr = null;
                try
                {
                    sr = new StreamReader(opFile);
                    string rtext = "";
                    while ((rtext = sr.ReadLine()) != null)
                    {
                        rtext = rtext.Trim();

                        string optionName = "";
                        string optionValue = "";
                        if (rtext.Length > 0)
                        {
                            int pos = rtext.IndexOf("=");
                            if (pos != -1)
                            {
                                optionName = rtext.Substring(0, pos);
                                optionValue = rtext.Substring(pos + 1, rtext.Length - pos - 1);
                            }
                        }
                        Option op = new Option();
                        lstOptions.Add(op);
                        op.OptionName = optionName;
                        op.OptionValue = optionValue;
                    }
                    sr.Close();
                    sr.Dispose();
                }
                catch
                {
                    if (sr != null) sr.Dispose();
                    return false;
                }
            }
            else
            {
                StreamWriter sw = null;
                try
                {
                    sw = new StreamWriter(opFile);
                    foreach (var op in lstOptions)
                    {
                        string wtext = string.Format("{0}={1}", op.OptionName, op.OptionValue);
                        sw.WriteLine(wtext);
                    }
                    sw.Close();
                    sw.Dispose();
                }
                catch
                {
                    if (sw != null) sw.Dispose();
                    return false;
                }
            }
            return true;
        }
    }

    public class Option
    {
        public int OptionID { get; set; }
        public string OptionName { get; set; }
        public string OptionValue { get; set; }
    }
}
