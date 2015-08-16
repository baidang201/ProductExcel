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
                NameAfterTrim = value.Trim();
                NotifyPropertyChanged("Name");
            }
        }

        //名字去空格后
        protected string m_NameAfterTrime = "";
        public string NameAfterTrim
        {
            get { return this.m_NameAfterTrime; }
            set
            {
                this.m_NameAfterTrime = value;
                NotifyPropertyChanged("NameAfterTrime");
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
            this.BillDay = 0;
            this.PayDay = 0;
            this.PayLimit = 0.0;
            this.CostBase = 0.0;
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

    public class PreViewInfo : INotifyPropertyChanged
    {
        protected string m_One = "";
        public string One
        {
            get { return this.m_One; }
            set
            {
                this.m_One = value;
                NotifyPropertyChanged("One");
            }
        }


        protected string m_Two = "";
        public string Two
        {
            get { return this.m_Two; }
            set
            {
                this.m_Two = value;
                NotifyPropertyChanged("Two");
            }
        }


        protected string m_Three = "";
        public string Three
        {
            get { return this.m_Three; }
            set
            {
                this.m_Three = value;
                NotifyPropertyChanged("Three");
            }
        }


        protected string m_Four = "";
        public string Four
        {
            get { return this.m_Four; }
            set
            {
                this.m_Four = value;
                NotifyPropertyChanged("Four");
            }
        }


        protected string m_Five = "";
        public string Five
        {
            get { return this.m_Five; }
            set
            {
                this.m_Five = value;
                NotifyPropertyChanged("Five");
            }
        }
        

        public PreViewInfo()
        {
            One = "";
            Two = "";
            Three = "";
            Four = "";
            Five = "";
        }

        public PreViewInfo(string one,
            string two,
            string three,
            string four,
            string five
            )
        {
            One = one;
            Two = two;
            Three = three;
            Four = four;
            Five = five;
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
    public class OneDayPlanPayInfo
    {
        public List<double> listOneDayPlanPayInfo;

        public OneDayPlanPayInfo()
        {
            listOneDayPlanPayInfo = new List<double>();
        }

        public OneDayPlanPayInfo(int count)
        {
            listOneDayPlanPayInfo = new List<double>();
            for (int i = 0; i < count; i++)
            {
                listOneDayPlanPayInfo.Add(new double());
            }
        }

        public OneDayPlanPayInfo(List<double> listPay)
        {
            listOneDayPlanPayInfo = new List<double>();
            for (int i = 0; i < listPay.Count; i++)
            {
                if (i > 1)
                {
                    break;
                }

                listOneDayPlanPayInfo.Add(listPay[i]);
            }
        }
    }

    //一种天数单元的分布情况，即所有天数的民生、普通、高消费列表。可表示3天 4天 ....
    public class DaysPlanInfo
    {
        public int DayCount;
        public List<OneDayPlanPayInfo> listLiveliHoodPayInfoUnion;
        public List<OneDayPlanPayInfo> listNormalPayInfoUnion;
        public List<OneDayPlanPayInfo> listHightConsumptionPayInfoUnion;

        public DaysPlanInfo()
        {
            DayCount = 0;
            listLiveliHoodPayInfoUnion = new List<OneDayPlanPayInfo>();
            listNormalPayInfoUnion = new List<OneDayPlanPayInfo>();
            listHightConsumptionPayInfoUnion = new List<OneDayPlanPayInfo>();
        }

        public DaysPlanInfo(int count)
        {
            DayCount = count;
            listLiveliHoodPayInfoUnion = new List<OneDayPlanPayInfo>();
            listNormalPayInfoUnion = new List<OneDayPlanPayInfo>();
            listHightConsumptionPayInfoUnion = new List<OneDayPlanPayInfo>();

            for (int i = 0; i < count; i++)
            {
                listLiveliHoodPayInfoUnion.Add(new OneDayPlanPayInfo());
                listNormalPayInfoUnion.Add(new OneDayPlanPayInfo());
                listHightConsumptionPayInfoUnion.Add(new OneDayPlanPayInfo());
            }
        }

        public DaysPlanInfo(List<OneDayPlanPayInfo> listHoodPayInfo,
            List<OneDayPlanPayInfo> listNormalPayInfo,
            List<OneDayPlanPayInfo> listHightConsumptionPayInfo
            )
        {
            listLiveliHoodPayInfoUnion = listHoodPayInfo;
            listNormalPayInfoUnion = listNormalPayInfo;
            listHightConsumptionPayInfoUnion = listHightConsumptionPayInfo;

        }
    }

    //3 - 10天的分配类,如表示3~7天情况的分布情况
    public class AssignInfo
    {
        public Dictionary<int, DaysPlanInfo> dicDaysUnionAssignInfo = new Dictionary<int, DaysPlanInfo>();
        public Dictionary<double, List<double>> dicAmountAssign = new Dictionary<double, List<double>>();
        
        public AssignInfo()
        {
            int minDay = 3;
            int maxDay = 10;

            dicAmountAssign.Add(
                0.5,
                new List<double>() { 0.78, 0.17, 0.06 }
                );
            dicAmountAssign.Add(
                0.6,
                new List<double>() { 0.59, 0.31, 0.11 }
                );
            dicAmountAssign.Add(
                0.7,
                new List<double>() { 0.385, 0.475, 0.15 }
                );
            dicAmountAssign.Add(
                0.8,
                new List<double>() { 0.265, 0.485, 0.26 }
                );
            dicAmountAssign.Add(
                0.9,
                new List<double>() { 0.155, 0.475, 0.38 }
                );
            dicAmountAssign.Add(
                1.0,
                new List<double>() { 0.125, 0.32, 0.565 }
                );


            dicDaysUnionAssignInfo.Add(
                3,
                new DaysPlanInfo(
                    new List<OneDayPlanPayInfo>(){ 
                        new OneDayPlanPayInfo(new List<double> (){34}),//0
                        new OneDayPlanPayInfo(new List<double> (){30}),//1
                        new OneDayPlanPayInfo(new List<double> (){36}),//2
                    },
                    new List<OneDayPlanPayInfo>() { 
                        new OneDayPlanPayInfo(new List<double> (){28}),//0
                        new OneDayPlanPayInfo(new List<double> (){23,18}),//1
                        new OneDayPlanPayInfo(new List<double> (){31}),//2
                    },
                    new List<OneDayPlanPayInfo>() { 
                        new OneDayPlanPayInfo(new List<double> (){38}),//0
                        new OneDayPlanPayInfo(new List<double> (){27}),//1
                        new OneDayPlanPayInfo(new List<double> (){35}),//2
                    }
                )
            );

            dicDaysUnionAssignInfo.Add(
                4,
                new DaysPlanInfo(
                    new List<OneDayPlanPayInfo>(){ 
                        new OneDayPlanPayInfo(new List<double> (){20}),
                        new OneDayPlanPayInfo(new List<double> (){15, 17}),
                        new OneDayPlanPayInfo(new List<double> (){23}),
                        new OneDayPlanPayInfo(new List<double> (){25}),
                    },
                    new List<OneDayPlanPayInfo>()
                    {
                        new OneDayPlanPayInfo(new List<double> (){23,13}),
                        new OneDayPlanPayInfo(new List<double> (){17}),
                        new OneDayPlanPayInfo(new List<double> (){23}),
                        new OneDayPlanPayInfo(new List<double> (){24}),
                    },
                    new List<OneDayPlanPayInfo>()
                    {
                        new OneDayPlanPayInfo(new List<double> (){15}),
                        new OneDayPlanPayInfo(new List<double> (){26}),
                        new OneDayPlanPayInfo(new List<double> (){31}),
                        new OneDayPlanPayInfo(new List<double> (){16, 12}),
                    }
                )
            );

            dicDaysUnionAssignInfo.Add(
               5,
               new DaysPlanInfo(
                   new List<OneDayPlanPayInfo>(){ 
                        new OneDayPlanPayInfo(new List<double> (){18}),
                        new OneDayPlanPayInfo(new List<double> (){11, 12}),
                        new OneDayPlanPayInfo(new List<double> (){23}),
                        new OneDayPlanPayInfo(new List<double> (){19}),
                        new OneDayPlanPayInfo(new List<double> (){17}),
                    },
                   new List<OneDayPlanPayInfo>()
                   {
                       new OneDayPlanPayInfo(new List<double> (){6,19}),
                        new OneDayPlanPayInfo(new List<double> (){18}),
                        new OneDayPlanPayInfo(new List<double> (){13}),
                        new OneDayPlanPayInfo(new List<double> (){9,10}),
                        new OneDayPlanPayInfo(new List<double> (){25}),
                   },
                   new List<OneDayPlanPayInfo>()
                   {
                       new OneDayPlanPayInfo(new List<double> (){16}),
                        new OneDayPlanPayInfo(new List<double> (){20}),
                        new OneDayPlanPayInfo(new List<double> (){12, 13}),
                        new OneDayPlanPayInfo(new List<double> (){22}),
                        new OneDayPlanPayInfo(new List<double> (){17}),
                   }
               )
           );

            dicDaysUnionAssignInfo.Add(
              6,
              new DaysPlanInfo(
                  new List<OneDayPlanPayInfo>()
                  {
                      new OneDayPlanPayInfo(new List<double> (){15}),
                        new OneDayPlanPayInfo(new List<double> (){12, 6}),
                        new OneDayPlanPayInfo(new List<double> (){18}),
                        new OneDayPlanPayInfo(new List<double> (){17}),
                        new OneDayPlanPayInfo(new List<double> (){11, 5}),
                        new OneDayPlanPayInfo(new List<double> (){16}),
                  },
                  new List<OneDayPlanPayInfo>()
                  {
                      new OneDayPlanPayInfo(new List<double> (){18}),
                        new OneDayPlanPayInfo(new List<double> (){15}),
                        new OneDayPlanPayInfo(new List<double> (){10,7}),
                        new OneDayPlanPayInfo(new List<double> (){14}),
                        new OneDayPlanPayInfo(new List<double> (){11,9}),
                        new OneDayPlanPayInfo(new List<double> (){16}),
                  },
                  new List<OneDayPlanPayInfo>()
                  {
                      new OneDayPlanPayInfo(new List<double> (){24}),
                        new OneDayPlanPayInfo(new List<double> (){11, 12}),
                        new OneDayPlanPayInfo(new List<double> (){8}),
                        new OneDayPlanPayInfo(new List<double> (){13}),
                        new OneDayPlanPayInfo(new List<double> (){17}),
                        new OneDayPlanPayInfo(new List<double> (){6, 9}),
                  }
              )
          );

            dicDaysUnionAssignInfo.Add(
              7,
              new DaysPlanInfo(
                  new List<OneDayPlanPayInfo>()
                  {
                      new OneDayPlanPayInfo(new List<double> (){12.5}),
                        new OneDayPlanPayInfo(new List<double> (){17}),
                        new OneDayPlanPayInfo(new List<double> (){12}),
                        new OneDayPlanPayInfo(new List<double> (){13}),
                        new OneDayPlanPayInfo(new List<double> (){14}),
                        new OneDayPlanPayInfo(new List<double> (){16.5}),
                        new OneDayPlanPayInfo(new List<double> (){15}),
                  },
                  new List<OneDayPlanPayInfo>()
                  {
                      new OneDayPlanPayInfo(new List<double> (){15}),
                        new OneDayPlanPayInfo(new List<double> (){13}),
                        new OneDayPlanPayInfo(new List<double> (){8,7}),
                        new OneDayPlanPayInfo(new List<double> (){12}),
                        new OneDayPlanPayInfo(new List<double> (){11,6}),
                        new OneDayPlanPayInfo(new List<double> (){12}),
                        new OneDayPlanPayInfo(new List<double> (){16}),
                  },
                  new List<OneDayPlanPayInfo>()
                  {
                      new OneDayPlanPayInfo(new List<double> (){17.5}),
                        new OneDayPlanPayInfo(new List<double> (){16}),
                        new OneDayPlanPayInfo(new List<double> (){15}),
                        new OneDayPlanPayInfo(new List<double> (){9, 10.5}),
                        new OneDayPlanPayInfo(new List<double> (){11}),
                        new OneDayPlanPayInfo(new List<double> (){11.5}),
                        new OneDayPlanPayInfo(new List<double> (){9.5}),
                  }
              )
          );

            dicDaysUnionAssignInfo.Add(
              8,
              new DaysPlanInfo(
                  new List<OneDayPlanPayInfo>()
                  {
                      new OneDayPlanPayInfo(new List<double> (){13}),
                        new OneDayPlanPayInfo(new List<double> (){10}),
                        new OneDayPlanPayInfo(new List<double> (){12}),
                        new OneDayPlanPayInfo(new List<double> (){15}),
                        new OneDayPlanPayInfo(new List<double> (){7, 6}),
                        new OneDayPlanPayInfo(new List<double> (){11}),
                        new OneDayPlanPayInfo(new List<double> (){13.5}),
                        new OneDayPlanPayInfo(new List<double> (){12.5}),
                  },
                  new List<OneDayPlanPayInfo>()
                  {
                      new OneDayPlanPayInfo(new List<double> (){11}),
                        new OneDayPlanPayInfo(new List<double> (){8,5}),
                        new OneDayPlanPayInfo(new List<double> (){14.5}),
                        new OneDayPlanPayInfo(new List<double> (){9}),
                        new OneDayPlanPayInfo(new List<double> (){13}),
                        new OneDayPlanPayInfo(new List<double> (){13.5}),
                        new OneDayPlanPayInfo(new List<double> (){12}),
                        new OneDayPlanPayInfo(new List<double> (){14}),
                  },
                  new List<OneDayPlanPayInfo>()
                  {
                      new OneDayPlanPayInfo(new List<double> (){15}),
                        new OneDayPlanPayInfo(new List<double> (){13}),
                        new OneDayPlanPayInfo(new List<double> (){7, 5}),
                        new OneDayPlanPayInfo(new List<double> (){12}),
                        new OneDayPlanPayInfo(new List<double> (){12.5}),
                        new OneDayPlanPayInfo(new List<double> (){14}),
                        new OneDayPlanPayInfo(new List<double> (){10}),
                        new OneDayPlanPayInfo(new List<double> (){11.5}),
                  }
              )
          );

            dicDaysUnionAssignInfo.Add(
              9,
              new DaysPlanInfo(
                  new List<OneDayPlanPayInfo>()
                  {
                      new OneDayPlanPayInfo(new List<double> (){13}),
                        new OneDayPlanPayInfo(new List<double> (){10}),
                        new OneDayPlanPayInfo(new List<double> (){8}),
                        new OneDayPlanPayInfo(new List<double> (){11}),
                        new OneDayPlanPayInfo(new List<double> (){7, 9}),
                        new OneDayPlanPayInfo(new List<double> (){14}),
                        new OneDayPlanPayInfo(new List<double> (){7.5}),
                        new OneDayPlanPayInfo(new List<double> (){12}),
                        new OneDayPlanPayInfo(new List<double> (){8.5}),
                  },
                  new List<OneDayPlanPayInfo>()
                  {
                      new OneDayPlanPayInfo(new List<double> (){5,6}),
                        new OneDayPlanPayInfo(new List<double> (){12}),
                        new OneDayPlanPayInfo(new List<double> (){13}),
                        new OneDayPlanPayInfo(new List<double> (){10}),
                        new OneDayPlanPayInfo(new List<double> (){11}),
                        new OneDayPlanPayInfo(new List<double> (){9}),
                        new OneDayPlanPayInfo(new List<double> (){11.5}),
                        new OneDayPlanPayInfo(new List<double> (){12}),
                        new OneDayPlanPayInfo(new List<double> (){10.5}),
                  },
                  new List<OneDayPlanPayInfo>()
                  {
                      new OneDayPlanPayInfo(new List<double> (){12}),
                        new OneDayPlanPayInfo(new List<double> (){11}),
                        new OneDayPlanPayInfo(new List<double> (){10}),
                        new OneDayPlanPayInfo(new List<double> (){6, 5}),
                        new OneDayPlanPayInfo(new List<double> (){9}),
                        new OneDayPlanPayInfo(new List<double> (){12}),
                        new OneDayPlanPayInfo(new List<double> (){11.5}),
                        new OneDayPlanPayInfo(new List<double> (){10.5}),
                        new OneDayPlanPayInfo(new List<double> (){13}),
                  }
              )
          );

            dicDaysUnionAssignInfo.Add(
              10,
              new DaysPlanInfo(
                  new List<OneDayPlanPayInfo>()
                  {
                    new OneDayPlanPayInfo(new List<double> (){9.5}),
                    new OneDayPlanPayInfo(new List<double> (){6, 5}),
                    new OneDayPlanPayInfo(new List<double> (){11}),
                    new OneDayPlanPayInfo(new List<double> (){7}),
                    new OneDayPlanPayInfo(new List<double> (){4, 4.5}),
                    new OneDayPlanPayInfo(new List<double> (){12}),
                    new OneDayPlanPayInfo(new List<double> (){10}),
                    new OneDayPlanPayInfo(new List<double> (){11.5}),
                    new OneDayPlanPayInfo(new List<double> (){9}),
                    new OneDayPlanPayInfo(new List<double> (){10.5}),
                  },
                  new List<OneDayPlanPayInfo>()
                  {
                      new OneDayPlanPayInfo(new List<double> (){7}),
                        new OneDayPlanPayInfo(new List<double> (){8,3.5}),
                        new OneDayPlanPayInfo(new List<double> (){7.5}),
                        new OneDayPlanPayInfo(new List<double> (){10}),
                        new OneDayPlanPayInfo(new List<double> (){9}),
                        new OneDayPlanPayInfo(new List<double> (){11}),
                        new OneDayPlanPayInfo(new List<double> (){10}),
                        new OneDayPlanPayInfo(new List<double> (){12}),
                        new OneDayPlanPayInfo(new List<double> (){4}),
                        new OneDayPlanPayInfo(new List<double> (){9.5,8.5}),
                  },
                  new List<OneDayPlanPayInfo>()
                  {
                      new OneDayPlanPayInfo(new List<double> (){7.5, 8.5}),
                       new OneDayPlanPayInfo(new List<double> ()),
                        new OneDayPlanPayInfo(new List<double> (){13}),
                        new OneDayPlanPayInfo(new List<double> (){11.5}),
                        new OneDayPlanPayInfo(new List<double> (){11}),
                        new OneDayPlanPayInfo(new List<double> (){5, 7}),
                        new OneDayPlanPayInfo(new List<double> (){10}),
                        new OneDayPlanPayInfo(new List<double> (){8}),
                        new OneDayPlanPayInfo(new List<double> (){6.5}),
                        new OneDayPlanPayInfo(new List<double> (){12}),
                  }
              )
          );


        }
    }

    public class CostBaseForEnum : INotifyPropertyChanged
    {
        //成本
        protected string m_CostBaseDisplay = "";
        public string CostBaseDisplay
        {
            get { return this.m_CostBaseDisplay; }
            set
            {
                this.m_CostBaseDisplay = value;
                NotifyPropertyChanged("CostBaseDisplay");
            }
        }

        //成本
        protected double m_CostBaseValue = 0.0;
        public double CostBaseValue
        {
            get { return this.m_CostBaseValue; }
            set
            {
                this.m_CostBaseValue = value;
                if (0.0 == m_CostBaseValue)
                {
                    CostBaseDisplay = "未设置";
                }
                else
                {
                    CostBaseDisplay = CostBaseValue.ToString();
                }
                NotifyPropertyChanged("CostBaseValue");
            }
        }

        public CostBaseForEnum()
        {
            CostBaseDisplay ="未设置";
            CostBaseValue = 0.0;
        }

        public CostBaseForEnum(string display, double value)
        {
            CostBaseDisplay = display;
            CostBaseValue = value;
        }

        public CostBaseForEnum(double value)
        {
            CostBaseValue = value;
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
