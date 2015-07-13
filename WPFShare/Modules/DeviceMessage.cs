using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Signalway.CommThemes.Modules
{
    public class DeviceMessage : INotifyPropertyChanged
    {
        /// <summary>
        /// 属性变更通知事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChang(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChangedEventArgs e = new PropertyChangedEventArgs(info);
                PropertyChanged(this, e);
                e = null;
            }
        }


        private DateTime MessageTimeValue;
        /// <summary>
        /// 消息时间
        /// </summary>
        public DateTime MessageTime
        {
            get { return this.MessageTimeValue; }
            set
            {
                this.MessageTimeValue = value;
                NotifyPropertyChang("MessageTime");
            }
        }

        private string MessageContentValue;
        /// <summary>
        /// 消息内容
        /// </summary>
        public string MessageContent
        {
            get { return this.MessageContentValue; }
            set
            {
                this.MessageContentValue = value;
                NotifyPropertyChang("MessageContent");
            }
        }

        public DeviceMessage(string content)
        {
            this.MessageContentValue = content;
            this.MessageTimeValue = DateTime.Now;
        }

        public override string ToString()
        {
            return string.Format("{0}  {1}", MessageTimeValue.ToString("HH:mm:ss"), MessageContentValue);
        } 
    }
}
