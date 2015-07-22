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

using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

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
        int companyInfoNum = 3;
        int payInfoNum = 6;

        static private string strFullPath = "";
        static private int MaxLine = 100;
        static private List<PayInfo> listPayInfo = new List<PayInfo>();
        static private List<CompanyInfo> listCompanyInfo = new List<CompanyInfo>();

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
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        void initGUI()
        {
            comboPayDayCount.ItemsSource = new int[] { 3, 4, 5, 6, 7, 8, 9, 10 };
            comboPayMode.ItemsSource = new string[] { "模式1", "模式2", "模式3", "模式4" };

            comboPayDayCount.SelectedIndex = 0;
            comboPayMode.SelectedIndex = 0;
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

                        if (pms.Length < companyInfoNum)
                        {
                            break;
                        }

                        CompanyInfo companyInfo = new CompanyInfo();
                        companyInfo.LiveliHood = pms[0];
                        companyInfo.Normal = pms[1];
                        companyInfo.HightConsumption = pms[2];

                        listCompanyInfo.Add(companyInfo);

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

                        if (pms.Length < payInfoNum)
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
                    }
                }
            }
            #endregion
            
        }

        private void dataGridPayInfo_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        void dataGridPayInfo_UnloadingRow(object sender, DataGridRowEventArgs e)
        {
            if (dataGridPayInfo.Items != null)
            {
                for (int i = 0; i < dataGridPayInfo.Items.Count; i++)
                {
                    try
                    {
                        DataGridRow row = dataGridPayInfo.ItemContainerGenerator.ContainerFromIndex(i) as DataGridRow;
                        if (row != null)
                        {
                            row.Header = (i + 1).ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        LogToolsEx.Error2File(EORLog, "dataPayInfo_UnloadingRow ex={0}", ex);
                    }

                }
            }

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
                cbTrunCostBase.IsChecked = false;
                cbTrunCostExtForSafe.IsChecked = false;

                if ((dataGridPayInfo.SelectedItems[0] as PayInfo) != null)
                {
                    CurrentPayInfo = dataGridPayInfo.SelectedItems[0] as PayInfo;
                    tbCostBase.Text = CurrentPayInfo.CostBase.ToString();
                    tbCostExtForSafe.Text = CurrentPayInfo.CostExtForSafe.ToString();
                }
            }
            else
            {
                CurrentPayInfo = null;
            }
        }

        private void tbCostBase_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (null != CurrentPayInfo)
            {
                if (!string.IsNullOrEmpty(tbCostBase.Text.Trim()))
                {
                    CurrentPayInfo.CostBase = Convert.ToDouble(tbCostBase.Text);
                }
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
            return true;
        }

        private void SavePayInfo()
        {

            using (StreamWriter SW = new StreamWriter(iniPayFileName))
            {
                string line = null;

                for (int i = 0; i < dataGridPayInfo.Items.Count; i++ )
                {
                    if (dataGridPayInfo.Items[i] is PayInfo)
                    {
                        PayInfo payInfo = (PayInfo)dataGridPayInfo.Items[i];
                        string[] rgPayInfo = new string[payInfoNum];
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

                for (int i = 0; i < dataGridCompanyInfo.Items.Count; i++ )
                {
                    if (dataGridCompanyInfo.Items[i] is CompanyInfo)
                    {
                        CompanyInfo companyInfo = (CompanyInfo)dataGridCompanyInfo.Items[i];
                        string[] rgComanyInfo = new string[companyInfoNum];
                        rgComanyInfo[0] = companyInfo.LiveliHood;
                        rgComanyInfo[1] = companyInfo.Normal;
                        rgComanyInfo[2] = companyInfo.HightConsumption;
                        line = string.Join(",", rgComanyInfo);
                        SW.WriteLine(line);
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

            SaveFileDialog sd = new SaveFileDialog();
            sd.Filter = "Excel文档|*.xls";
            sd.Title = "导出excel";

            bool? bResult = sd.ShowDialog();
            if (bResult.HasValue && bResult.Value == true)
            {
                string fullName = sd.FileName;
                //string fileName = sd.FileName.Substring(0, sd.FileName.Length - ".xls".Length);
                string excelTemp = @"temp.ini";

                Helper.CopyFileTo(excelTemp, fullName);

                if (System.IO.File.Exists(fullName) == false)//该批任务中，文件已解压，则不需要解密
                {
                    return;
                }

                FileStream file = File.OpenRead(fullName);
                IWorkbook workbook = new HSSFWorkbook(file);

                //保存信用卡信息
                int PayDayCount = Convert.ToInt32(comboPayDayCount.SelectedValue);
                for (int i = 1; i <= PayDayCount; i++)//遍历页数
                {
                    ISheet sheet = workbook.GetSheet(i.ToString());

                    int beginRow = 4;
                    int beginCol = 0;
                    int row = beginRow;
                    int col = beginCol;

                    foreach (PayInfo payInfo in listPayInfo)
                    {
                        sheet.GetRow(row).GetCell(col).SetCellValue(payInfo.Name);
                        sheet.GetRow(row).GetCell(col + 1).SetCellValue(payInfo.BillDay);
                        sheet.GetRow(row).GetCell(col + 2).SetCellValue(payInfo.PayDay);
                        sheet.GetRow(row).GetCell(col + 3).SetCellValue(payInfo.PayLimit);
                        row++;
                    }

                }

                //保存商户
                {
                    int row = 1;
                    int col = 0;
                    ISheet sheet = workbook.GetSheet("库");
                    foreach(CompanyInfo companyInfo in listCompanyInfo)
                    {
                        sheet.GetRow(row).GetCell(col).SetCellValue(companyInfo.LiveliHood);
                        sheet.GetRow(row).GetCell(col + 1).SetCellValue(companyInfo.Normal);
                        sheet.GetRow(row).GetCell(col + 2).SetCellValue(companyInfo.HightConsumption);
                        row++;
                    }

                }
                using (FileStream fs = File.OpenWrite(fullName))
                {
                    workbook.Write(fs);
                }
            }
        }

      
    }
}


