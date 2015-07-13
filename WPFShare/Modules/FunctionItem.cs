/*-------------------------------------------------
 * FunctionItem.cs
 * 文件描述：简历设备功能的导航模型
 * 
 * 日志:
 *     2013/7/3 建立   Feng.ZY
 *     
 * ----------------------------------------------*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Signalway.CommThemes.Modules
{
    public class FunctionItem : INotifyPropertyChanged
    {
        private string function;
        private string imageIcon;
        private string imageIcon_enable;
        private string imageIcon_disable;
        private string comment;
        private string pageTitle;
        private string pageUrl;
        private bool isCommand;
        private bool isEnable = true;
        private Object pageObj = null;

        /// <summary>
        /// 属性变更通知事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public FunctionItem(string fn, string enableImg, string disableImg, string comm, string title, string url, bool isCmd = false, bool enable= true)
        {
            function = fn;
            imageIcon = enableImg;
            imageIcon_enable = enableImg;
            comment = comm;
            pageTitle = title;
            pageUrl = url;
            imageIcon_disable = disableImg;
            isCommand = isCmd;
            isEnable = enable;
        }

        public override string ToString()
        {
            return "【" + PageTitle + "】" + System.Environment.NewLine + Comment;
        }
        
        /// <summary>
        /// 功能名
        /// </summary>
        public string Function
        {
            get { return this.function; }
            set
            {
                this.function = value;
                NotifyPropertyChanged("Function");
            }
        }
        
        /// <summary>
        /// 关联图标
        /// </summary>
        public string ImageIcon
        {
            get { return this.imageIcon; }
            set
            {
                this.imageIcon = value;
                NotifyPropertyChanged("ImageIcon");
            }
        }

        /// <summary>
        /// 使能图标
        /// </summary>
        public string ImageIcon_Enable
        {
            get { return this.imageIcon_enable; }
            set
            {
                this.imageIcon_enable = value;
                NotifyPropertyChanged("ImageIcon_Enable");
            }
        }

        /// <summary>
        /// 不使能图标
        /// </summary>
        public string ImageIcon_Disable
        {
            get { return this.imageIcon_disable; }
            set
            {
                this.imageIcon_disable = value;
                NotifyPropertyChanged("ImageIcon_Disable");
            }
        }

        /// <summary>
        /// 功能描述
        /// </summary>
        public string Comment
        {
            get { return this.comment; }
            set
            {
                this.comment = value;
                NotifyPropertyChanged("Comment");
            }
        }

        /// <summary>
        /// 页面标题
        /// </summary>
        public string PageTitle
        {
            get { return this.pageTitle; }
            set
            {
                this.pageTitle = value;
                NotifyPropertyChanged("PageTitle");
            }
        }

        /// <summary>
        /// 页面Url
        /// </summary>
        public string PageUrl
        {
            get { return this.pageUrl; }
            set
            {
                this.pageUrl = value;
                NotifyPropertyChanged("PageUrl");
            }
        }

        /// <summary>
        /// 功能命令按钮
        /// </summary>
        public bool IsCommand
        {
            get { return this.isCommand; }
        }

        /// <summary>
        /// 使能
        /// </summary>
        public bool IsEnable
        {
            get { return this.isEnable; }
            set
            {
                this.isEnable = value;
                if (value)
                    ImageIcon = imageIcon_enable;
                else
                    ImageIcon = imageIcon_disable;
                NotifyPropertyChanged("IsEnable");
            }
        }

        /// <summary>
        /// 功能页面参数
        /// </summary>
        public object PageObject
        {
            get { return this.pageObj; }
            set
            {
                this.pageObj = value;
                NotifyPropertyChanged("PageObject");
            }
        }
    }
}
