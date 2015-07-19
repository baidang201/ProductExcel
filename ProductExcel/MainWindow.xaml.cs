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

namespace ProductExcel
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
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
            dataGridExcel.LoadingRow += new EventHandler<DataGridRowEventArgs>(dataGridExcel_LoadingRow);
            dataCompany.LoadingRow += new EventHandler<DataGridRowEventArgs>(dataGridExcel_LoadingRow);

            dataGridExcel.UnloadingRow += new EventHandler<DataGridRowEventArgs>(dataGridExcel_UnloadingRow);
            dataCompany.UnloadingRow += new EventHandler<DataGridRowEventArgs>(dataCompany_UnloadingRow);

            listPayInfo.Add(new PayInfo());
            listCompanyInfo.Add(new CompanyInfo());

            dataGridExcel.ItemsSource = listPayInfo;
            dataCompany.ItemsSource = listCompanyInfo;
            initGUI();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        void initGUI()
        {
            comboPayDayCount.ItemsSource = new int[] { 3, 4, 5, 6, 7, 8, 9 };
            comboPayMode.ItemsSource = new string[] { "模式1", "模式2", "模式3", "模式4" };

            comboPayDayCount.SelectedIndex = 0;
            comboPayMode.SelectedIndex = 0;
        }

        private void dataGridExcel_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        void dataGridExcel_UnloadingRow(object sender, DataGridRowEventArgs e)
        {
            dataGridExcel_LoadingRow(sender, e);
            if (dataGridExcel.Items != null)
            {
                for (int i = 0; i < dataGridExcel.Items.Count; i++)
                {
                    try
                    {
                        DataGridRow row = dataGridExcel.ItemContainerGenerator.ContainerFromIndex(i) as DataGridRow;
                        if (row != null)
                        {
                            row.Header = (i + 1).ToString();
                        }
                    }
                    catch { }
                }
            }

        }

        void dataCompany_UnloadingRow(object sender, DataGridRowEventArgs e)
        {
            dataGridExcel_LoadingRow(sender, e);
            if (dataCompany.Items != null)
            {
                for (int i = 0; i < dataCompany.Items.Count; i++)
                {
                    try
                    {
                        DataGridRow row = dataCompany.ItemContainerGenerator.ContainerFromIndex(i) as DataGridRow;
                        if (row != null)
                        {
                            row.Header = (i + 1).ToString();
                        }
                    }
                    catch { }
                }
            }

        }


        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = "Excel文件|*.xls|Excel文件|*.xlsx";
            openDlg.RestoreDirectory = true;
            bool? bResult = openDlg.ShowDialog();
            if (bResult.HasValue && bResult.Value == true)
            {
                strFullPath = openDlg.FileName;
                //tbFullPath.Text = strFullPath;
            }

        }

        private void btnAddARow_Click(object sender, RoutedEventArgs e)
        {
            listPayInfo.Add(new PayInfo());
            //dataGridExcel.Items.Add(new DataGridRow());
            dataGridExcel.ItemsSource = null;
            dataGridExcel.ItemsSource = listPayInfo;
        }

        private void dataGridExcel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridExcel.SelectedItems.Count == 1)
            {
                cbTrunCostBase.IsChecked = false;
                cbTrunCostExtForSafe.IsChecked = false;

                if ((dataGridExcel.SelectedItems[0] as PayInfo) != null)
                {
                    CurrentPayInfo = dataGridExcel.SelectedItems[0] as PayInfo;
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

        private void btOutPutExcel_Click(object sender, RoutedEventArgs e)
        {
            if (false == CheckPayInfoOK())
            {
                return;
            }

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
                using (FileStream fs = File.OpenWrite(fullName))
                {
                    workbook.Write(fs);                    
                } 
            }
        }
    }
}


