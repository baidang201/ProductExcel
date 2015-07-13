using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Windows.Forms;


namespace UtilityTool
{
    public interface ILog
    {
        void Debug(string msg);
        void Debug(Exception exp);
        void Debug(string logType, string msg);
        void Debug(string logType, Exception exp);

        void Info(string msg);
        void Info(string logType, string msg);

        void Warn(string msg);
        void Warn(Exception exp);
        void Warn(string msg, int errorCode);
        void Warn(string logType, string msg);
        void Warn(string logType, Exception exp);

        void Error(string msg);
        void Error(Exception exp);
        void Error(string msg, int errorCode);
        void Error(string logType, string msg);
        void Error(string logType, Exception exp);

        void Fatal(string msg);
        void Fatal(Exception exp);
        void Fatal(string msg, int errorCode);
        void Fatal(string logType, string msg);
        void Fatal(string logType, Exception exp);
    }
    public enum LoggerLevel
    {
        Debug = 100,
        Info = 200,
        Warn = 300,
        Error = 400,
        Fatal = 500
    }
    public class LoggerEntry
    {
        public string datetime;
        public string level;
        public string file;
        public int line;
        public int errorCode;
        public string msg;

        public LoggerEntry()
        {
            datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");
            level = string.Empty;
            file = "fileName";
            line = 0;
            errorCode = 0;
            msg = string.Empty;
        }
        public bool FromString(string log)
        {
            return false;
        }
    }
    public class CommonLogger : ILog
    {
        protected static List<CommonLogger> _instances = new List<CommonLogger>();
        protected LoggerLevel m_curLogLevel = LoggerLevel.Debug;

        //Tag
        private string _baseFileName = @"log";
        /// <summary>
        /// Tag
        /// </summary>
        public string BaseFileName { get { return _baseFileName; } set { _baseFileName = value; } }

        //Date
        private DateTime _baseFileDate = DateTime.Now;
        /// <summary>
        /// Date
        /// </summary>
        private DateTime BaseFileDate { get { return _baseFileDate; } set { _baseFileDate = value; } }

        //Index
        private int _baseFileIndex = 0;
        /// <summary>
        /// 今天的序号
        /// </summary>
        public int BaseFileIndex { get { return _baseFileIndex; } set { _baseFileIndex = value; } }

        //全路径
        private string _baseFileFullPath = @"";
        /// <summary>
        /// 全路径
        /// </summary>
        public string BaseFileFullPath { get { return _baseFileFullPath; } set { _baseFileFullPath = value; } }

        /// <summary>
        /// 单个文件的大小
        /// </summary>
        protected const int SINGLEFILE_MAX_SIZE = 5 * 1024 * 1024;
        protected const string BASE_FILETYPE = @".log";
        protected const string BASE_FILEPATH = @"Log/";
        protected StringBuilder m_sb = new StringBuilder();
        //protected object WriteLock = new object();
        protected StreamWriter m_logWriter = null;
        protected static string m_baseFilePath = string.Empty;

        private static object myLock = new object();

        public void SetLoggerLevel(LoggerLevel level)
        {
            m_curLogLevel = level;
        }
        public static ILog GetLogger(string baseFilePath,string baseFileName = @"Log")
        {
            //baseFileName = baseFileName.ToUpper();
            lock (myLock)
            {
                CommonLogger _instance = _instances.FirstOrDefault(instance => instance.BaseFileName.Equals(baseFileName));
                if (_instance != null && _instance.BaseFileDate.Date != DateTime.Now.Date)
                {
                    //日期变了
                    _instance.UnInitialize();
                    _instances.Remove(_instance);
                    _instance = null;
                }
                //判断文件大小
                if (_instance != null)
                {
                    FileInfo fi = new FileInfo(_instance.BaseFileFullPath);
                    if (fi.Exists && fi.Length > SINGLEFILE_MAX_SIZE)
                    {
                        _instance.UnInitialize();
                        _instances.Remove(_instance);
                        _instance = null;
                    }
                }
                
                if (_instance == null)
                {
                    _instance = new CommonLogger(baseFilePath);
                    _instance.BaseFileName = baseFileName;
                    _instance.Initialize();
                    _instances.Add(_instance);
                }

                DelLogFile("", DateTime.Now.AddHours(-24));
                return _instance;
            }
        }

        #region 增强方法
        /// <summary>
        /// 返回指定Tag和指定日志的日志文件路径
        /// </summary>
        public static string GetFileName(string _tag, DateTime _dt, int _index = 0)
        {
            string dateTime = _dt.ToString("yyyyMMdd");
            //string baseFolder = BASE_FILEPATH + (@"\" + dateTime + @"\");
            string fileName = ""  ;
            if (_index > 0)
            {
                fileName = string.Format("{0}_{1}_{3}{2}", _tag, dateTime, BASE_FILETYPE, _index);
            }
            else
            {
                fileName = string.Format("{0}_{1}{2}", _tag, dateTime, BASE_FILETYPE);
            }
            return fileName;
        }

        /// <summary>
        /// 给定日志的路径（和文件名），找出他所属的Tag
        /// </summary>
        public static string GetFileNameTag(string filePath)
        {
            string filename = Path.GetFileName(filePath);
            return filename.Substring(0, filename.Length - 13);
        }

        /// <summary>
        /// 获取日志文件的日期
        /// </summary>
        public static DateTime GetFileNameDateTime(string filePath)
        {
            string filename = Path.GetFileName(filePath);
            string datetimeval = filename.Substring(filename.Length - 12, 8);
            DateTime val = DateTime.MinValue;
            try
            {
                val = new DateTime(int.Parse(datetimeval.Substring(0, 4)), int.Parse(datetimeval.Substring(4, 2)), int.Parse(datetimeval.Substring(6, 2)));
            }
            catch { val = DateTime.MinValue; }
            return val;
        }

        //删除指定日期之前的日志
        public static void DelLogFile(string tag, DateTime dt)
        {
            //if ((DateTime.Now - dt).Days < 3)
            //{
            //    throw new Exception("不允许删除三天之内的日志");
            //}
            //else
            {
                if (string.IsNullOrWhiteSpace(tag))
                {
                    string[] logPaths = Directory.GetDirectories(GetFilePath(BASE_FILEPATH));
                    foreach (string logPath in logPaths)
                    {
                        string pathName = new DirectoryInfo(logPath).Name;
                        DateTime dtHas = new DateTime(int.Parse(pathName.Substring(0, 4)), int.Parse(pathName.Substring(4, 2)), int.Parse(pathName.Substring(6, 2)));
                        if (dtHas <= dt)
                        {
                            try
                            {
                                Directory.Delete(logPath, true);
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                else
                {
                    string[] logs = Directory.GetFiles(GetFilePath(BASE_FILEPATH), tag + "*.csv", SearchOption.AllDirectories);
                    foreach (string log in logs)
                    {
                        DateTime _dt = GetFileNameDateTime(log);
                        if (_dt <= dt)
                        {
                            bool success = false;
                            try
                            {
                                File.Delete(log);
                                success = true;
                            }
                            catch { success = false; }
                        }
                    }
                }
            }
        }

        private static string GetFilePath(string BASE_FILEPATH)
        {
            if (string.IsNullOrEmpty(m_baseFilePath))
            {
                return Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, BASE_FILEPATH);
            }
            else
                return Path.Combine(m_baseFilePath, BASE_FILEPATH);
        }

        /// <summary>
        /// 原样输出文本
        /// </summary>
        public void Write(string msg)
        {
            lock (myLock)
            {
                m_logWriter.Write(msg);
                m_logWriter.Flush();
            }
        }
        #endregion

        protected CommonLogger(string baseFilePath)
        {
            m_baseFilePath = baseFilePath;
            //Initialize();
        }

        protected virtual int Initialize()
        {
            string filepath = Path.Combine(GetFilePath(BASE_FILEPATH), DateTime.Now.ToString("yyyyMMdd"));
            string filename = GetFileName(_baseFileName, DateTime.Now, 0);
            //寻找空闲的Index
            int _index = 0;
            while (_index < 10000)  //真的给他超过10000个日志就让他关小黑屋去！
            {
                _baseFileFullPath = Path.Combine(filepath, filename);
                FileInfo fi = new FileInfo(_baseFileFullPath);
                if (fi.Exists && fi.Length > SINGLEFILE_MAX_SIZE)
                {
                    _index++;
                    filename = GetFileName(_baseFileName, DateTime.Now, _index);
                }
                else
                {
                    _baseFileIndex = _index;
                    break;
                }
            }
            try
            {
                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                Stream fileStream = new FileStream(_baseFileFullPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                m_logWriter = new StreamWriter(fileStream, Encoding.Unicode);
            }
            catch (Exception exp)
            {
                System.Diagnostics.Trace.WriteLine(exp.ToString());
            }
            return 0;
        }

        private void UnInitialize()
        {
            if (m_logWriter != null)
            {
                try { m_logWriter.Flush(); }
                catch { }
                try { m_logWriter.Close(); }
                catch { }
                try { m_logWriter.Dispose(); }
                catch { }
            }
        }

        protected virtual void WriteLog(LoggerEntry log)
        {
            lock (myLock)
            {
                m_sb.Length = 0;
                m_sb.Append(log.datetime);
                m_sb.Append(",");
                m_sb.Append(log.level);
                m_sb.Append(",");
                m_sb.Append(log.errorCode);
                m_sb.Append(",");
                m_sb.Append(log.msg);
                if (BaseFileName == "LOGICLOG")
                {
                    if (string.IsNullOrWhiteSpace(log.file) == false)
                    {
                        m_sb.Append(",");
                        m_sb.Append(log.file);
                    }
                    if (log.line > 0)
                    {
                        m_sb.Append(",");
                        m_sb.Append(log.line);
                    }
                }
                try
                {
                    if (m_logWriter != null)
                    {
                        m_logWriter.WriteLine(m_sb.ToString());
                        m_logWriter.Flush();
                    }
                }
                catch (Exception ex)
                {
                    LogToolsEx.Error2File("DebugFile", "WriteLog Fail: Error={0}", ex); 
                }
                
            }
        }
        public string GetLevelName(LoggerLevel loggerLevel)
        {
            switch (loggerLevel)
            {
                case LoggerLevel.Debug:
                    return "Debug";
                case LoggerLevel.Info:
                    return "Info";
                case LoggerLevel.Warn:
                    return "Warn";
                case LoggerLevel.Error:
                    return "Error";
                case LoggerLevel.Fatal:
                    return "Fatal";
                default:
                    return "Unknow";
            }
        }

        #region ILog Members

        public void Debug(string msg)
        {
            if (LoggerLevel.Debug >= m_curLogLevel)
            {
                LoggerEntry log = new LoggerEntry();
                StackFrame frame = new StackFrame(2, true);
                log.file = frame.GetFileName();
                log.line = frame.GetFileLineNumber();
                log.level = GetLevelName(LoggerLevel.Debug);
                log.msg = msg;
                this.WriteLog(log);
            }
            System.Diagnostics.Trace.WriteLine(msg);
        }

        public void Debug(Exception exp)
        {
            if (LoggerLevel.Debug >= m_curLogLevel)
            {
                LoggerEntry log = new LoggerEntry();

                StackFrame frame = new StackFrame(2, true);
                log.file = frame.GetFileName();
                log.line = frame.GetFileLineNumber();
                log.level = GetLevelName(LoggerLevel.Debug);
                log.msg = exp.ToString();

                this.WriteLog(log);
            }
            System.Diagnostics.Trace.WriteLine(exp.ToString());
        }

        public void Debug(string logType, string msg)
        {
            if (LoggerLevel.Debug >= m_curLogLevel)
            {
                LoggerEntry log = new LoggerEntry();
                log.file = logType;
                log.level = GetLevelName(LoggerLevel.Debug);
                log.msg = msg;

                this.WriteLog(log);
            }
        }

        public void Debug(string logType, Exception exp)
        {
            if (LoggerLevel.Debug >= m_curLogLevel)
            {
                LoggerEntry log = new LoggerEntry();
                log.file = logType;
                log.level = GetLevelName(LoggerLevel.Debug);
                log.msg = exp.ToString();

                this.WriteLog(log);
            }
        }

        public void Info(string msg)
        {
            if (LoggerLevel.Info >= m_curLogLevel)
            {
                LoggerEntry log = new LoggerEntry();

                StackFrame frame = new StackFrame(2, true);
                log.file = frame.GetFileName();
                log.line = frame.GetFileLineNumber();
                log.level = GetLevelName(LoggerLevel.Info);
                log.msg = msg;

                this.WriteLog(log);
            }
        }

        public void Info(string logType, string msg)
        {
            if (LoggerLevel.Info >= m_curLogLevel)
            {
                LoggerEntry log = new LoggerEntry();
                log.file = logType;
                log.level = GetLevelName(LoggerLevel.Info);
                log.msg = msg;

                this.WriteLog(log);
            }
        }

        public void Warn(string msg)
        {
            if (LoggerLevel.Warn >= m_curLogLevel)
            {
                LoggerEntry log = new LoggerEntry();

                StackFrame frame = new StackFrame(2, true);
                log.file = frame.GetFileName();
                log.line = frame.GetFileLineNumber();
                log.level = GetLevelName(LoggerLevel.Warn);
                log.msg = msg;
                this.WriteLog(log);
            }
        }

        public void Warn(Exception exp)
        {
            if (LoggerLevel.Warn >= m_curLogLevel)
            {
                LoggerEntry log = new LoggerEntry();

                StackFrame frame = new StackFrame(2, true);
                log.file = frame.GetFileName();
                log.line = frame.GetFileLineNumber();
                log.level = GetLevelName(LoggerLevel.Warn);
                log.msg = exp.ToString();
                this.WriteLog(log);
            }
        }

        public void Warn(string msg, int errorCode)
        {
            if (LoggerLevel.Warn >= m_curLogLevel)
            {
                LoggerEntry log = new LoggerEntry();

                StackFrame frame = new StackFrame(2, true);
                log.file = frame.GetFileName();
                log.line = frame.GetFileLineNumber();
                log.level = GetLevelName(LoggerLevel.Warn);
                log.msg = msg;
                log.errorCode = errorCode;
                this.WriteLog(log);
            }
        }
        public void Warn(string logType, string msg)
        {
            if (LoggerLevel.Warn >= m_curLogLevel)
            {
                LoggerEntry log = new LoggerEntry();
                log.file = logType;
                log.level = GetLevelName(LoggerLevel.Warn);
                log.msg = msg;

                this.WriteLog(log);
            }
        }
        public void Warn(string logType, Exception exp)
        {
            if (LoggerLevel.Warn >= m_curLogLevel)
            {
                LoggerEntry log = new LoggerEntry();
                log.file = logType;
                log.level = GetLevelName(LoggerLevel.Warn);
                log.msg = exp.ToString();

                this.WriteLog(log);
            }
        }

        public void Error(string msg)
        {
            if (LoggerLevel.Error >= m_curLogLevel)
            {
                LoggerEntry log = new LoggerEntry();

                StackFrame frame = new StackFrame(2, true);
                log.file = frame.GetFileName();
                log.line = frame.GetFileLineNumber();
                log.level = GetLevelName(LoggerLevel.Error);
                log.msg = msg;
                this.WriteLog(log);
            }
        }

        public void Error(Exception exp)
        {
            if (LoggerLevel.Error >= m_curLogLevel)
            {
                LoggerEntry log = new LoggerEntry();

                StackFrame frame = new StackFrame(2, true);
                log.file = frame.GetFileName();
                log.line = frame.GetFileLineNumber();
                log.level = GetLevelName(LoggerLevel.Error);
                log.msg = exp.ToString();
                this.WriteLog(log);
            }
        }

        public void Error(string msg, int errorCode)
        {
            if (LoggerLevel.Error >= m_curLogLevel)
            {
                LoggerEntry log = new LoggerEntry();

                StackFrame frame = new StackFrame(2, true);
                log.file = frame.GetFileName();
                log.line = frame.GetFileLineNumber();
                log.level = GetLevelName(LoggerLevel.Error);
                log.msg = msg;
                log.errorCode = errorCode;
                this.WriteLog(log);
            }
        }

        public void Error(string logType, string msg)
        {
            if (LoggerLevel.Error >= m_curLogLevel)
            {
                LoggerEntry log = new LoggerEntry();
                log.file = logType;
                log.level = GetLevelName(LoggerLevel.Error);
                log.msg = msg;

                this.WriteLog(log);
            }
        }

        public void Error(string logType, Exception exp)
        {
            if (LoggerLevel.Error >= m_curLogLevel)
            {
                LoggerEntry log = new LoggerEntry();
                log.file = logType;
                log.level = GetLevelName(LoggerLevel.Error);
                log.msg = exp.ToString();

                this.WriteLog(log);
            }
        }

        public void Fatal(string msg)
        {
            if (LoggerLevel.Fatal >= m_curLogLevel)
            {
                LoggerEntry log = new LoggerEntry();

                StackFrame frame = new StackFrame(2, true);
                log.file = frame.GetFileName();
                log.line = frame.GetFileLineNumber();
                log.level = GetLevelName(LoggerLevel.Fatal);
                log.msg = msg;
                this.WriteLog(log);
            }
        }

        public void Fatal(Exception exp)
        {
            if (LoggerLevel.Fatal >= m_curLogLevel)
            {
                LoggerEntry log = new LoggerEntry();

                StackFrame frame = new StackFrame(2, true);
                log.file = frame.GetFileName();
                log.line = frame.GetFileLineNumber();
                log.level = GetLevelName(LoggerLevel.Fatal);
                log.msg = exp.ToString();
                this.WriteLog(log);
            }
        }

        public void Fatal(string msg, int errorCode)
        {
            if (LoggerLevel.Fatal >= m_curLogLevel)
            {
                LoggerEntry log = new LoggerEntry();

                StackFrame frame = new StackFrame(2, true);
                log.file = frame.GetFileName();
                log.line = frame.GetFileLineNumber();
                log.level = GetLevelName(LoggerLevel.Fatal);
                log.msg = msg;
                log.errorCode = errorCode;
                this.WriteLog(log);
            }
        }

        public void Fatal(string logType, string msg)
        {
            if (LoggerLevel.Fatal >= m_curLogLevel)
            {
                LoggerEntry log = new LoggerEntry();
                log.file = logType;
                log.level = GetLevelName(LoggerLevel.Fatal);
                log.msg = msg;

                this.WriteLog(log);
            }
        }

        public void Fatal(string logType, Exception exp)
        {
            if (LoggerLevel.Fatal >= m_curLogLevel)
            {
                LoggerEntry log = new LoggerEntry();
                log.file = logType;
                log.level = GetLevelName(LoggerLevel.Fatal);
                log.msg = exp.ToString();

                this.WriteLog(log);
            }
        }
        #endregion
    }
}
