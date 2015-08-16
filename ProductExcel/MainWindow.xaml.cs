using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Signalway.CommThemes;


using System.IO;
using UtilityTool;

namespace ProductExcel
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        string EORLog = @"EORLog.txt";
        string iniCompanyFileName = @"Company.ini";
        string iniPayFileName = @"PayFile.ini";
        string excelTempFieName = @"temp.ini";
        int companyInfoMemberCount = 3;
        int payInfoMemberCount = 6;

        static private int MaxLinePayInfo = 100;
        static private int MaxLineCompanyInfo = 40;
        static private List<PayInfo> listPayInfo = new List<PayInfo>();
        static private List<CompanyInfo> listCompanyInfo = new List<CompanyInfo>();
        static private Dictionary<int, int> dicPayModeCount = new Dictionary<int, int>() 
        { 
            {3, 6},
            {4, 10},
            {5, 10},
            {6, 10},
            {7, 10},
            {8, 10},
            {9, 10},
            {10, 10},
        };
        static private List<PreViewInfo> listPreViewInfo = new List<PreViewInfo>();

        static RadomHelper radomHelper = new RadomHelper();


        public PayInfo CurrentPayInfo = null;

        public MainWindow()
        {
            InitializeComponent();            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dataGridPayInfo.LoadingRow += new EventHandler<DataGridRowEventArgs>(dataGridPayInfo_LoadingRow);
            dataGridCompanyInfo.LoadingRow += new EventHandler<DataGridRowEventArgs>(dataGridPayInfo_LoadingRow);

            dataGridPayInfo.UnloadingRow += new EventHandler<DataGridRowEventArgs>(dataGridPayInfo_UnloadingRow);
            dataGridCompanyInfo.UnloadingRow += new EventHandler<DataGridRowEventArgs>(dataGridCompany_UnloadingRow);

            initINI();
            initGUI();

            dataGridPayInfo.ItemsSource = listPayInfo;
            dataGridCompanyInfo.ItemsSource = listCompanyInfo;
            dataGridPreView.ItemsSource = listPreViewInfo;
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            SavePayInfo();
        }

        void initGUI()
        {
            comboPayDayCount.ItemsSource = new int[] { 3, 4, 5, 6, 7, 8, 9, 10 };//天数
            comboPayDayCount.SelectedIndex = 0;

            comboCostBase.ItemsSource = new double[] { 0.5, 0.6, 0.7, 0.8, 0.9, 1.0 };//成本
            comboPayMode.SelectedIndex = 0;

            changePayModeItemSource(Convert.ToInt32(comboPayDayCount.SelectedValue));
        }

        void initINI()
        {

            #region 公司文件存在
            if (System.IO.File.Exists(iniCompanyFileName) == true)
            {
                using (StreamReader SR = new StreamReader(iniCompanyFileName))
                {
                    string line = null;
                    while ((line = SR.ReadLine()) != null)
                    {
                        string[] pms = line.Split(',');

                        if (pms.Length < companyInfoMemberCount)
                        {
                            break;
                        }

                        CompanyInfo companyInfo = new CompanyInfo();
                        companyInfo.LiveliHood = pms[0];
                        companyInfo.Normal = pms[1];
                        companyInfo.HightConsumption = pms[2];

                        listCompanyInfo.Add(companyInfo);

                        if (listCompanyInfo.Count == MaxLineCompanyInfo)
                        {
                            break;
                        }
                    }
                }
            }
            #endregion

            #region 信用卡信息存在
            if (System.IO.File.Exists(iniPayFileName) == true)
            {
                using (StreamReader SR = new StreamReader(iniPayFileName))
                {
                    string line = null;

                    while ((line = SR.ReadLine()) != null)
                    {
                        string[] pms = line.Split(',');

                        if (pms.Length < payInfoMemberCount)
                        {
                            break;
                        }

                        PayInfo payInfo = new PayInfo();
                        payInfo.Name = pms[0];
                        payInfo.BillDay = Convert.ToInt32(pms[1]);
                        payInfo.PayDay = Convert.ToInt32(pms[2]);
                        payInfo.PayLimit = Convert.ToDouble(pms[3]);
                        payInfo.CostBase = Convert.ToDouble(pms[4]);
                        payInfo.CostExtForSafe = Convert.ToDouble(pms[5]);

                        listPayInfo.Add(payInfo);

                        if (listPayInfo.Count == MaxLinePayInfo)
                        {
                            break;
                        }
                    }
                }
            }
            #endregion

            #region 补充空行
            while (listPayInfo.Count < MaxLinePayInfo)
            {
                PayInfo payInfo = new PayInfo();
                listPayInfo.Add(payInfo);
            }
            while (listCompanyInfo.Count < MaxLineCompanyInfo)
            {
                CompanyInfo companyInfo = new CompanyInfo();
                listCompanyInfo.Add(companyInfo);
            }
            #endregion

        }

        void initLogicParam()
        {

        }

        void changePayModeItemSource(int day)
        {
            List<string> listPayMode = new List<string>();

            if (false == dicPayModeCount.Any(r => r.Key == day))
            {
                comboPayMode.ItemsSource = null;
                return;
            }


            for (int i = 0; i < dicPayModeCount[day]; i++)
            {
                listPayMode.Add("模式" + (i + 1).ToString());
            }
            comboPayMode.ItemsSource = listPayMode;
            comboPayMode.SelectedIndex = 0;
        }

        private void dataGridPayInfo_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        void dataGridPayInfo_UnloadingRow(object sender, DataGridRowEventArgs e)
        {
            //if (dataGridPayInfo.Items != null)
            //{
            //    for (int i = 0; i < dataGridPayInfo.Items.Count; i++)
            //    {
            //        try
            //        {
            //            DataGridRow row = dataGridPayInfo.ItemContainerGenerator.ContainerFromIndex(i) as DataGridRow;
            //            if (row != null)
            //            {
            //                row.Header = (i + 1).ToString();
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            LogToolsEx.Error2File(EORLog, "dataPayInfo_UnloadingRow ex={0}", ex);
            //        }

            //    }
            //}

        }

        void dataGridCompany_UnloadingRow(object sender, DataGridRowEventArgs e)
        {
            if (dataGridCompanyInfo.Items != null)
            {
                for (int i = 0; i < dataGridCompanyInfo.Items.Count; i++)
                {
                    try
                    {
                        DataGridRow row = dataGridCompanyInfo.ItemContainerGenerator.ContainerFromIndex(i) as DataGridRow;
                        if (row != null)
                        {
                            row.Header = (i + 1).ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        LogToolsEx.Error2File(EORLog, "dataCompany_UnloadingRow ex={0}", ex);
                    }
                }
            }

        }



        private void btnAddARow_Click(object sender, RoutedEventArgs e)
        {
            listPayInfo.Add(new PayInfo());
            dataGridPayInfo.ItemsSource = null;
            dataGridPayInfo.ItemsSource = listPayInfo;
        }

        private void dataGridExcel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridPayInfo.SelectedItems.Count == 1)
            {
                cbTrunCostBase.IsChecked = true;
                cbTrunCostExtForSafe.IsChecked = false;

                if ((dataGridPayInfo.SelectedItems[0] as PayInfo) != null)
                {
                    CurrentPayInfo = dataGridPayInfo.SelectedItems[0] as PayInfo;
                    comboCostBase.SelectedIndex = comboCostBase.Items.IndexOf(CurrentPayInfo.CostBase);
                    tbCostExtForSafe.Text = CurrentPayInfo.CostExtForSafe.ToString();
                }
            }
            else
            {
                CurrentPayInfo = null;
            }
        }


        private void tbCostExtForSafe_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (null != CurrentPayInfo)
            {
                if (!string.IsNullOrEmpty(tbCostExtForSafe.Text.Trim()))
                {
                    CurrentPayInfo.CostExtForSafe = Convert.ToDouble(tbCostExtForSafe.Text);
                }
            }
        }

        private bool CheckPayInfoOK()
        {
            //foreach (PayInfo payInfo in listPayInfo)
            //{
            //    if (string.IsNullOrEmpty(payInfo.Name.Trim()))
            //    {
            //        MessageBox.Show("有信息为空，请检查");
            //        return false;
            //    }
            //}
            return true;
        }

        private bool HasNullPayInfo(PayInfo payInfo)
        {
            if (string.IsNullOrEmpty(payInfo.Name.Trim()))
            {
                return true;
            }
            if (0 == payInfo.PayDay)
            {
                return true;
            }
            if (0 == payInfo.BillDay)
            {
                return true;
            }
            if (0.0 == payInfo.PayLimit)
            {
                return true;
            }
            if (0.0 == payInfo.CostBase)
            {
                return true;
            }

            return false;
        }

        private void SavePayInfo()
        {

            using (StreamWriter SW = new StreamWriter(iniPayFileName))
            {
                string line = null;

                for (int i = 0; i < dataGridPayInfo.Items.Count; i++)
                {
                    if (dataGridPayInfo.Items[i] is PayInfo)
                    {
                        PayInfo payInfo = (PayInfo)dataGridPayInfo.Items[i];
                        string[] rgPayInfo = new string[payInfoMemberCount];
                        rgPayInfo[0] = payInfo.Name;
                        rgPayInfo[1] = payInfo.BillDay.ToString();
                        rgPayInfo[2] = payInfo.PayDay.ToString();
                        rgPayInfo[3] = payInfo.PayLimit.ToString();
                        rgPayInfo[4] = payInfo.CostBase.ToString();
                        rgPayInfo[5] = payInfo.CostExtForSafe.ToString();
                        line = string.Join(",", rgPayInfo);
                        SW.WriteLine(line);
                    }
                }
            }
        }

        private void btSaveComany_Click(object sender, RoutedEventArgs e)
        {
            using (StreamWriter SW = new StreamWriter(iniCompanyFileName))
            {
                string line = null;

                for (int i = 0; i < dataGridCompanyInfo.Items.Count; i++)
                {
                    if (dataGridCompanyInfo.Items[i] is CompanyInfo)
                    {
                        CompanyInfo companyInfo = (CompanyInfo)dataGridCompanyInfo.Items[i];
                        string[] rgComanyInfo = new string[companyInfoMemberCount];
                        rgComanyInfo[0] = companyInfo.LiveliHood;
                        rgComanyInfo[1] = companyInfo.Normal;
                        rgComanyInfo[2] = companyInfo.HightConsumption;
                        line = string.Join(",", rgComanyInfo);
                        SW.WriteLine(line);
                    }
                }
            }
        }

        private void comboPayDayCount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changePayModeItemSource(Convert.ToInt32(comboPayDayCount.SelectedValue));
        }

        private void comboCostBase_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (null != CurrentPayInfo)
            {
                if (true == cbTrunCostBase.IsChecked)
                {
                    CurrentPayInfo.CostBase = Convert.ToDouble(comboCostBase.SelectedValue);
                }
                else
                {
                    for (int i = 0; i < dataGridPayInfo.Items.Count; i++)
                    {
                        if (dataGridPayInfo.Items[i] is PayInfo)
                        {
                            PayInfo payInfo = (PayInfo)dataGridPayInfo.Items[i];
                            if (0.0 != payInfo.CostBase)
                            {
                                payInfo.CostBase = Convert.ToDouble(comboCostBase.SelectedValue);
                            }
                        }
                    }
                }
            }
        }

        private void btOutPutExcel_Click(object sender, RoutedEventArgs e)
        {
            if (false == CheckPayInfoOK())
            {
                return;
            }

            SavePayInfo();

            List<PayInfo> listNotNullPayInfo = new List<PayInfo>();
            foreach (object item in dataGridPayInfo.Items)
            {
                if ((item as PayInfo) != null)
                {
                    PayInfo payInfo = item as PayInfo;
                    if (false == HasNullPayInfo(payInfo))
                    {
                        listNotNullPayInfo.Add(payInfo);
                    }
                }
            }
            if (listNotNullPayInfo.Count == 0)
            {
                MessageBox.Show("没有可导出的记录");
                return;
            }

            string singleName = @"z整月安排.xls";
            string fullName = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), 
                singleName);

            int index = 1;
            while(true)
            {
                 if (System.IO.File.Exists(fullName) == true)
                 {
                     singleName = string.Format("z整月安排{0}.xls", index);
                     fullName = System.IO.Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                        singleName);
                     index++;
                 }
                 else
                 {
                     break;
                 }
            }

            string strFailReason = "";
            if (ExcelHelper.OutPutExcel(excelTempFieName,
                fullName,
                Convert.ToInt32(comboPayDayCount.SelectedValue),
                Convert.ToInt32(comboPayMode.SelectedIndex),
                listNotNullPayInfo,
                listCompanyInfo,
                radomHelper,
                ref strFailReason))
            {
                MessageBox.Show("导出完成");
            }
            else
            {
                MessageBox.Show(string.Format("导出失败。原因为：{0}", strFailReason));
            }

        }

        private void btPreView_Click(object sender, RoutedEventArgs e)
        {
            if (false == CheckPayInfoOK())
            {
                return;
            }

            SavePayInfo();

            List<PayInfo> listNotNullPayInfo = new List<PayInfo>();
            foreach (object item in dataGridPayInfo.Items)
            {
                if ((item as PayInfo) != null)
                {
                    PayInfo payInfo = item as PayInfo;
                    if (false == HasNullPayInfo(payInfo))
                    {
                        listNotNullPayInfo.Add(payInfo);
                    }
                }
            }
            if (listNotNullPayInfo.Count == 0)
            {
                MessageBox.Show("没有可预览的记录");
                return;
            }

           
           
            string strFailReason = "";
            List<double> listSum = PreViewHelper.GetSumList(
                Convert.ToInt32(comboPayDayCount.SelectedValue),
                Convert.ToInt32(comboPayMode.SelectedIndex),
                listNotNullPayInfo,
                listCompanyInfo,
                radomHelper,
                ref strFailReason);


            List<double> listCard = new List<double>();
            foreach(PayInfo payInfo in listNotNullPayInfo)
            {
                listCard.Add(payInfo.PayLimit);
            }
            
            listPreViewInfo = PreViewHelper.ProductPreViewList(listSum, listCard);
            dataGridPreView.ItemsSource = listPreViewInfo;
        }


        private void btTesting_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btClean_Click(object sender, RoutedEventArgs e)
        {     
            if (dataGridPayInfo.SelectedItems.Count > 0)
            {
                foreach (object item in dataGridPayInfo.SelectedItems)
                {
                    if ((item as PayInfo) != null)
                    {
                        PayInfo payInfo = item as PayInfo;
                        payInfo.Name = "";
                        payInfo.PayDay = 0;
                        payInfo.BillDay = 0;
                        payInfo.PayLimit = 0.0;
                        payInfo.CostBase = 0.0;
                    }
                }
                SavePayInfo();
            }
            else
            {
                return;
            }
        }

        
        private void dataGridPayInfo_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (
                e.Key == Key.V
                && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control
                )
            {

                IDataObject ido = Clipboard.GetDataObject();

                if (ido == null)
                {
                    return;

                }
                    string[] formats = ido.GetFormats();
                    string format = formats[0].ToString();
                    object data = ido.GetData(format);

                    string strPaste = data.ToString();
                    string[] rows = strPaste.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    if (rows.Length <= 0)
                    {
                        return;
                    }

                    int startIndex = dataGridPayInfo.SelectedIndex;
                    if (-1 == startIndex)//未选中
                    {
                        return;
                    }

                    int index = 0;
                    for (int i = 0; i < rows.Count(); i++)
                    {
                        if (index + startIndex >= MaxLinePayInfo)
                        {
                            return;
                        }

                        string[] rgParam = rows[i].Split(','); ;
                        int paramCount = 5;
                        if (rgParam.Length != paramCount)
                        {
                            return;
                        }

                        int billDay = 0;
                        int payDay = 0;
                        double payLimit = 0.0;
                        double costBase = 0.0;


                        bool fBillDay =int.TryParse(rgParam[1], out billDay);
                        bool fPayDay =int.TryParse(rgParam[2], out payDay);
                        bool fPayLimit =double.TryParse(rgParam[3], out payLimit);
                        bool fCostBase = double.TryParse(rgParam[4], out costBase);
                        if (
                            fBillDay
                            && fPayDay
                            && fPayLimit
                            && fCostBase
                            )
                        {//全部可解析
                            if (dataGridPayInfo.Items[index + startIndex] is PayInfo)
                            {
                                int sub = index + startIndex;
                                PayInfo payInfo = dataGridPayInfo.Items[sub] as PayInfo;
                                if (null != payInfo)
                                {
                                    payInfo.Name = rgParam[0];
                                    payInfo.BillDay = billDay;
                                    payInfo.PayDay = payDay;
                                    payInfo.PayLimit = payLimit;
                                    payInfo.CostBase = costBase;
                                    index++;
                                }
                            }
                        }
                        else//部分可解析
                        {
                            if (dataGridPayInfo.Items[index + startIndex] is PayInfo)
                            {
                                int sub = index + startIndex;
                                PayInfo payInfo = dataGridPayInfo.Items[sub] as PayInfo;
                                if (null != payInfo)
                                {
                                    payInfo.Name = rgParam[0];
                                    payInfo.BillDay = billDay;
                                    payInfo.PayDay = payDay;
                                    payInfo.PayLimit = payLimit;
                                    payInfo.CostBase = costBase;
                                    index++;
                                }
                            }
                        }
                    }

            }

        }
        
    }

}


