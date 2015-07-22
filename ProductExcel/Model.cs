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
            this.BillDay = BillDay;
            this.PayDay = PayDay;
            this.PayLimit = PayLimit;
            this.CostBase = CostBase;
            this.CostExtForSafe = CostExtForSafe;
        }

        public PayInfo()
        {
            this.Name = "";
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

    //一天内的消费，第一笔 第二笔。一天内的分布
    public class FirstSecondPayInfoUnion
    {
        List<double> listFirstSecondPayInfo;

        public FirstSecondPayInfoUnion()
        {
            listFirstSecondPayInfo = new List<double>();
        }

        public FirstSecondPayInfoUnion(int count)
        {
            listFirstSecondPayInfo = new List<double>();
              for(int i=0; i<count; i++)
            {
                listFirstSecondPayInfo.Add(new double());
            }
        }
    }

    //一个单元的分布情况，
    public class DaysUnionAssignInfo
    {
        public int DayCount;
        public List<FirstSecondPayInfoUnion> listFirstSecondPayInfoUnion;

        public DaysUnionAssignInfo()
        {
            DayCount = 0;
            listFirstSecondPayInfoUnion = new List<FirstSecondPayInfoUnion>();
        }

        public DaysUnionAssignInfo(int count)
        {
            DayCount = count;
            listFirstSecondPayInfoUnion = new List<FirstSecondPayInfoUnion>();
            for(int i=0; i<count; i++)
            {
                listFirstSecondPayInfoUnion.Add(new FirstSecondPayInfoUnion());
            }
        }
    }

    //3 - 10天的分配类,如表示3~7天情况的分布情况
    public class AssignInfo
    {
        Dictionary<int, DaysUnionAssignInfo> dicDaysUnionAssignInfo = new Dictionary<int, DaysUnionAssignInfo>();

        public AssignInfo()
        {
            int minDay = 3;
            int maxDay = 10;
            for (int i = minDay; i < maxDay; i++)
            {
                dicDaysUnionAssignInfo.Add(i, new DaysUnionAssignInfo());
            }
        }
    }

    public class PayAssignModeInfo
    {
        public int ModeDayCount = 0;
        public List<int> listDaysAssign;

        public PayAssignModeInfo()
        {
            ModeDayCount = 0;
            listDaysAssign = new List<int>();
        }

    }

}
