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

namespace ProductExcel
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {        
        static private string strFullPath = "";
        static private int MaxLine = 100 - 3 + 1;
        static private List<PayInfo> listPayInfo = new List<PayInfo>();

        public MainWindow()
        {
            InitializeComponent();

            this.dataGridExcel.LoadingRow += new EventHandler<DataGridRowEventArgs>(this.dataGridExcel_LoadingRow);
            listPayInfo.Add(new PayInfo());

            dataGridExcel.ItemsSource = listPayInfo;
        }

        private void dataGridExcel_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
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

        private void btnProductThree_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (strFullPath.Length == 0)
                {
                    MessageBox.Show("未选中文件");
                    return;
                }

                // 返回对象
                object objRtn = new object();

                // 获得一个ExcelMacroHelper对象
                ExcelMacroHelper excelMacroHelper = new ExcelMacroHelper();

                // 执行指定Excel中的宏，执行时显示Excel
                excelMacroHelper.RunExcelMacro(
                                                    strFullPath,
                                                    "三天",
                                                    new Object[] {},
                                                    out objRtn,
                                                    true
                                              );
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnProductSix_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (strFullPath.Length == 0)
                {
                    MessageBox.Show("未选中文件");
                    return;
                }

                // 返回对象
                object objRtn = new object();

                // 获得一个ExcelMacroHelper对象
                ExcelMacroHelper excelMacroHelper = new ExcelMacroHelper();

                // 执行指定Excel中的宏，执行时显示Excel
                excelMacroHelper.RunExcelMacro(
                                                    strFullPath,
                                                    "六天",
                                                    new Object[] { },
                                                    out objRtn,
                                                    true
                                              );
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAddARow_Click(object sender, RoutedEventArgs e)
        {
            listPayInfo.Add(new PayInfo());
            //dataGridExcel.Items.Add(new DataGridRow());
            dataGridExcel.ItemsSource = null;
            dataGridExcel.ItemsSource = listPayInfo;
        }
        
    }

    public class PayInfo : INotifyPropertyChanged
    {
        protected string m_Name = "";
        public string Name
        {
            get { return this.m_Name; }
            set
            {
                this.m_Name = value;
                NotifyPropertyChanged("Name");
            }
        }

        protected double m_PayExt = 0;
        public double PayExt
        {
            get { return this.m_PayExt; }
            set
            {
                this.m_PayExt = value;
                NotifyPropertyChanged("PayExt");
            }
        }

        protected int m_BillDay = 1;
        public int BillDay
        {
            get { return this.m_BillDay; }
            set
            {
                this.m_BillDay = value;
                NotifyPropertyChanged("BillDay");
            }
        }

        protected int m_PayDay = 1;
        public int PayDay
        {
            get { return this.m_PayDay; }
            set
            {
                this.m_PayDay = value;
                NotifyPropertyChanged("PayDay");
            }
        }

        protected double m_PayLimit = 0;
        public double PayLimit
        {
            get { return this.m_PayLimit; }
            set
            {
                this.m_PayLimit = value;
                NotifyPropertyChanged("PayLimit");
            }
        }

        public PayInfo(string Name, double PayExt, int BillDay, int PayDay, double PayLimit)
        {
            this.Name = Name;
            this.PayExt = PayExt;
            this.BillDay = BillDay;
            this.PayDay = PayDay;
            this.PayLimit = PayLimit;
        }

        public PayInfo()
        {
            this.Name = "";
            this.PayExt = 0.0;
            this.BillDay = 1;
            this.PayDay = 1;
            this.PayLimit = 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
