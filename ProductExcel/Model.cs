using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductExcel
{
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


    public class PayModeInfo
    {
        public int ModeDayCount = 0;
        public List<int> listDaysAssign;

        public PayModeInfo()
        {
            ModeDayCount = 0;
            listDaysAssign = new List<int>();
        }

    }

}
