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

        private void btOutPutExcel_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sd = new SaveFileDialog();
            sd.Filter = "Excel文档|*.xls";
            sd.Title = "导出excel";

            bool? bResult = sd.ShowDialog();
            if (bResult.HasValue && bResult.Value == true)
            {
                string fullName = sd.FileName;
                //string fileName = sd.FileName.Substring(0, sd.FileName.Length - ".xls".Length);
            }
        }        
    }

    public class PayInfo : INotifyPropertyChanged
    {
        //名字
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

        //手续费
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

        //账单日
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

        //还款日
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

        //额度
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

        //成本
        protected double m_CostBase = 0;
        public double CostBase
        {
            get { return this.m_CostBase; }
            set
            {
                this.m_CostBase = value;
                NotifyPropertyChanged("CostBase");
            }
        }

        //多还款
        protected double m_CostExtForSafe = 0;
        public double CostExtForSafe
        {
            get { return this.m_CostExtForSafe; }
            set
            {
                this.m_CostExtForSafe = value;
                NotifyPropertyChanged("CostExtForSafe");
            }
        }

        public PayInfo(string Name, double PayExt, int BillDay, int PayDay, double PayLimit, double CostBase, double CostExtForSafe)
        {
            this.Name = Name;
            this.PayExt = PayExt;
            this.BillDay = BillDay;
            this.PayDay = PayDay;
            this.PayLimit = PayLimit;
            this.CostBase = CostBase;
            this.CostExtForSafe = CostExtForSafe;
        }

        public PayInfo()
        {
            this.Name = "";
            this.PayExt = 0.0;
            this.BillDay = 1;
            this.PayDay = 1;
            this.PayLimit = 0;
            this.CostBase = 0.6;
            this.CostExtForSafe = 2;
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


    public class CompanyInfo : INotifyPropertyChanged
    {
        //民生
        protected string m_LiveliHood = "";
        public string LiveliHood
        {
            get { return this.m_LiveliHood; }
            set
            {
                this.m_LiveliHood = value;
                NotifyPropertyChanged("LiveliHood");
            }
        }

        //一般
        protected string m_Normal = "";
        public string Normal
        {
            get { return this.m_Normal; }
            set
            {
                this.m_Normal = value;
                NotifyPropertyChanged("Normal");
            }
        }

        //高消费
        protected string m_HightConsumption = "";
        public string HightConsumption
        {
            get { return this.m_HightConsumption; }
            set
            {
                this.m_HightConsumption = value;
                NotifyPropertyChanged("HightConsumption");
            }
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
