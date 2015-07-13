using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Signalway.CommThemes.Modules
{
    public class LocalUsage : INotifyPropertyChanged
    {
        /// <summary>
        /// 属性变更通知事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChang(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        private int cpuUsage = 0;
        public int CPU
        {
            get { return this.cpuUsage; }
            set
            {
                this.cpuUsage = value;
                NotifyPropertyChang("CPU");
            }
        }

        private int memUsage = 0;
        public int MEM
        {
            get { return this.memUsage; }
            set
            {
                this.memUsage = value;
                NotifyPropertyChang("MEM");
            }
        }

        private string timeValue = DateTime.Now.ToString("MM/dd HH:mm");
        public string TIME
        {
            get { return this.timeValue; }
            set
            {
                this.timeValue = value;
                NotifyPropertyChang("TIME");
            }
        }

        private string verValue = "1.0.0.0";
        public string VER
        {
            get { return this.verValue; }
            set
            {
                this.verValue = value;
            }
        }

        private bool supportCPU = true;
        public bool SupportCPU
        {
            get { return this.supportCPU; }
            set
            {
                this.supportCPU = value;
                NotifyPropertyChang("SupportCPU");
            }
        }

        private bool supportMEM = true;
        public bool SupportMEM
        {
            get { return this.supportMEM; }
            set
            {
                this.supportMEM = value;
                NotifyPropertyChang("SupportMEM");
            }
        }

        private System.Timers.Timer clockTimer;
        private System.Diagnostics.PerformanceCounter pcCPU = null;
        private System.Diagnostics.PerformanceCounter pcMem = null;


        public LocalUsage()
        {
            clockTimer = new System.Timers.Timer();
            clockTimer.Interval = 1000;
            clockTimer.Elapsed += clockTimer_Tick;
            try
            {
                pcCPU = new System.Diagnostics.PerformanceCounter("Processor", "% Processor Time", "_Total");
            }
            catch
            {
                SupportCPU = false;
            }

            try
            {
                pcMem = new System.Diagnostics.PerformanceCounter("Memory", "% Committed Bytes In Use", "");
            }
            catch
            {
                SupportMEM = false;
            }

            //获取程序集版本
            VER = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(); 
        }

        public void StartUpdate(int Interval=1000)
        {
            clockTimer.Interval = Interval;
            clockTimer.Start();
        }

        public void StopUpdate()
        {
            clockTimer.Stop();
        }

        /// <summary>
        /// 本地定时器
        /// </summary>
        private void clockTimer_Tick(object sender, EventArgs e)
        {
            if(supportCPU) CPU = (int)pcCPU.NextValue();
            if(supportMEM) MEM = (int)pcMem.NextValue();
            TIME = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }
    }
}
