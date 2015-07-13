using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace UtilityTool
{
    public class LogToolsEx
    {
        /// <summary>
        /// 日志保存的基础路径，如果为空则保存到程序运行的路径
        /// </summary>
        public static string LogBasePath = "";
        /// <summary>
        /// 保存日志
        /// </summary>
        public static bool WriteLog = true;
        public static bool ShowDebug = false;
        #region 4Test
        public static void Warning(string _Content, params object[] _par)
        {
            System.Diagnostics.Debug.WriteLine(_Content, _par);
            if (WriteLog) CommonLogger.GetLogger(LogBasePath).Warn(string.Format(_Content, _par));
        }
        public static void Debug(string _Content, params object[] _par)
        {
            System.Diagnostics.Debug.WriteLine(_Content, _par);
            if (WriteLog) CommonLogger.GetLogger(LogBasePath).Debug(string.Format(_Content, _par));
        }
        public static void Info(string _Content, params object[] _par)
        {
            System.Diagnostics.Debug.WriteLine(_Content, _par);
            if (WriteLog) CommonLogger.GetLogger(LogBasePath).Info(string.Format(_Content, _par));
        }
        public static void Error(string _Content, params object[] _par)
        {
            System.Diagnostics.Debug.WriteLine(_Content, _par);
            if (WriteLog) CommonLogger.GetLogger(LogBasePath).Error(string.Format(_Content, _par));
        }
        #endregion

        public static void Warning2File(string _FileName, string _Content, params object[] _par)
        {
            if(ShowDebug) System.Diagnostics.Debug.WriteLine(_Content, _par);
            if (WriteLog) CommonLogger.GetLogger(LogBasePath, _FileName).Warn(string.Format(_Content, _par));
        }

        //[Conditional("TraceLog")]
        public static void Debug2File(string _FileName, string _Content, params object[] _par)
        {
            if (ShowDebug) System.Diagnostics.Debug.WriteLine(_Content, _par);
            if (WriteLog) CommonLogger.GetLogger(LogBasePath, _FileName).Debug(string.Format(_Content, _par));
        }

        //[Conditional("TraceLog")]
        public static void Info2File(string _FileName, string _Content, params object[] _par)
        {
            if (ShowDebug) System.Diagnostics.Debug.WriteLine(_Content, _par);
            if (WriteLog) CommonLogger.GetLogger(LogBasePath, _FileName).Info(string.Format(_Content, _par));
        }

        public static void Error2File(string _FileName, string _Content, params object[] _par)
        {
            if (ShowDebug) System.Diagnostics.Debug.WriteLine(_Content, _par);
            CommonLogger.GetLogger(LogBasePath, _FileName).Error(string.Format(_Content, _par));
        }

        //[Conditional("TraceLog")]
        public static void Info2File(string _FileName, int _StackBack, string _Content, params object[] _par)
        {
            if (ShowDebug) System.Diagnostics.Debug.WriteLine(_Content, _par);
            if (WriteLog)
            {
                StackFrame frame = new StackFrame(_StackBack + 1, true);
                CommonLogger.GetLogger(LogBasePath, _FileName).Info(string.Format(_Content, _par) + string.Format("文件:{0}; 代码行:{1};", frame.GetFileName(), frame.GetFileLineNumber()));
                System.Diagnostics.Debug.WriteLine("文件:{0}; 代码行:{1};", frame.GetFileName(), frame.GetFileLineNumber());
            }
        }

        //[Conditional("TraceLog")]
        public static void Write(string _FileName, string _Content, params object[] _par)
        {
            if (ShowDebug) System.Diagnostics.Debug.WriteLine(_Content, _par);
            if (WriteLog) ((CommonLogger)CommonLogger.GetLogger(LogBasePath, _FileName)).Write(string.Format(_Content, _par));
        }
    }

    /*
    public class CLoggerEx
    {
        public string Tag = "";
        public CLoggerEx Instance { get { return instance; } set { } }
        public static CLoggerEx instance = null;
        public static CLoggerEx GetLog(string tag)
        {
            if (instance == null)
            {
                instance = new CLoggerEx(tag);
            }
            return instance;
        }
        private CLoggerEx(string tag)
        {
            Tag = tag;
        }
        public static CLoggerEx operator +(CLoggerEx _UILog, string info)
        {
            CLoggerTools.Info2File(_UILog.Tag, info);
            return _UILog;
        }
    }
     */
}
