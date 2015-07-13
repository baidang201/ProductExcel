using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace UtilityTool
{
    public class WindowsApi
    {
        private static DateTime tick = DateTime.Now;
        /// <summary>
        /// 在控制台输出以传入字符串命名的，距离上次调用本函数相隔的时间值
        /// </summary>
        public static double ElapsedTime(string _str)
        {
            double diff = (DateTime.Now - tick).TotalMilliseconds;
            Debug.WriteLine(_str + ":" + diff.ToString() + "毫秒");
            tick = DateTime.Now;
            return diff;
        }

        /// <summary>
        /// 内存拷贝
        /// </summary>
        [DllImport("Kernel32.dll", EntryPoint = "RtlMoveMemory", SetLastError = true)]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, int size);

        /// <summary>
        /// 释放句柄
        /// </summary>
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject", SetLastError = true)]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("user32.dll")]
        private static extern int ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern int SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int IsIconic(IntPtr hWnd);

        [DllImport("kernel32.dll", EntryPoint = "GetDiskFreeSpaceExA")]
        public static extern int GetDiskFreeSpaceEx(string lpRootPathName, out long lpFreeBytesAvailable, out long lpTotalNumberOfBytes, out long lpTotalNumberOfFreeBytes);

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        public const int WM_COPYDATA = 0x004A;
        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpData;
        }

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, ref COPYDATASTRUCT lParam);

        /// <summary>
        /// 发送数据到其他窗口进程
        /// </summary>
        /// <param name="ProcessName">进程名称</param>
        /// <param name="Data">数据</param>
        /// <returns>发送结果</returns>
        public static int SendDataToProcess(string ProcessName, string WinName, string Data)
        {
            IntPtr hWnd = IntPtr.Zero;
            Process[] processes = Process.GetProcessesByName(ProcessName);
            foreach (Process _process in processes)
            {
                if (_process.ProcessName == ProcessName)
                {
                    hWnd = _process.MainWindowHandle;
                    break;
                }
            }
            processes = null;

            if (hWnd == IntPtr.Zero)
            {
                hWnd = FindWindow(null, WinName);
            }

            if (hWnd != IntPtr.Zero)
            {
                byte[] buffer = System.Text.Encoding.Default.GetBytes(Data);
                int len = buffer.Length;
                buffer = null;

                COPYDATASTRUCT cds;
                cds.dwData = (IntPtr)100;
                cds.lpData = Data;
                cds.cbData = len + 1;
                return SendMessage(hWnd, WM_COPYDATA, 0, ref cds);
            }
            else
            {
                return 0;
            }

        }

        /// <summary>
        /// 获取当前进行主窗口句柄
        /// </summary>
        /// <returns></returns>
        public static IntPtr GetOtherInstanceWindowHandle()
        {
            IntPtr hWnd = IntPtr.Zero;
            Process process = Process.GetCurrentProcess();

            Process[] processes = Process.GetProcessesByName(process.ProcessName);
            foreach (Process _process in processes)
            {
                //遍历进程，找到那个进程路径一样，但是ID不一样，句柄不为空的，就是已经运行的程序
                if (_process.Id != process.Id &&
                    _process.ProcessName == process.ProcessName)
                {
                    hWnd = _process.MainWindowHandle;
                    break;
                }
            }
            return hWnd;
        }

        const int SW_RESTORE = 9;
        /// <summary>
        /// 激活已经运行的程序窗口
        /// </summary>
        public static void SwitchToCurrentInstance()
        {
            IntPtr hWnd = GetOtherInstanceWindowHandle();
            if (hWnd != IntPtr.Zero)
            {
                if (IsIconic(hWnd) != 0)
                {
                    ShowWindow(hWnd, SW_RESTORE);
                }

                // 前置窗口.
                SetForegroundWindow(hWnd);
            }
        }

        /// <summary>
        /// 判断程序是否运行
        /// </summary>
        /// <returns></returns>
        public static bool IsAlreadyRunning()
        {
            Process process = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(process.ProcessName);
            foreach (Process _process in processes)
            {
                //遍历进程，找到那个进程路径一样，但是ID不一样，句柄不为空的，就是已经运行的程序
                if (_process.Id != process.Id /*&&
                    _process.MainModule.FileName == process.MainModule.FileName*/)
                {
                    return true;
                }
            }
            return false;
        }

        public static void KillOtherInstance()
        {
            Process process = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(process.ProcessName);
            foreach (Process _process in processes)
            {
                //遍历进程，找到那个进程路径一样，但是ID不一样，句柄不为空的，就是已经运行的程序
                if (_process.Id != process.Id)
                {
                    _process.Kill();
                }
            }
        }

        static Mutex mutex;
        static DateTime dt1970 = new System.DateTime(1970, 1, 1, 0, 0, 0);
        public static System.DateTime ConvertLongToDateTime(long d)
        {
            System.DateTime time = System.DateTime.MinValue;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(dt1970);
            time = startTime.AddMilliseconds(d);
            return time;
        }

        public static string GetLocalIP()
        {
            try
            {
                string HostName = Dns.GetHostName(); //得到主机名
                IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
                for (int i = 0; i < IpEntry.AddressList.Length; i++)
                {
                    //从IP地址列表中筛选出IPv4类型的IP地址
                    //AddressFamily.InterNetwork表示此IP为IPv4,
                    //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                    if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        return IpEntry.AddressList[i].ToString();
                    }
                }
                return "0.0.0.0";
            }
            catch
            {
                return "0.0.0.0";
            }
        }

        public static bool CheckPort(string tempPort)
        {
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo("netstat", "-an");
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.RedirectStandardOutput = true;
            p.Start();
            string result = p.StandardOutput.ReadToEnd().ToLower();//最后都转换成小写字母  
            System.Net.IPAddress[] addressList = Dns.GetHostByName(Dns.GetHostName()).AddressList;
            List<string> ipList = new List<string>();
            ipList.Add("127.0.0.1");
            ipList.Add("0.0.0.0");
            for (int i = 0; i < addressList.Length; i++)
            {
                ipList.Add(addressList[i].ToString());
            }
            bool use = false;
            for (int i = 0; i < ipList.Count; i++)
            {
                if (result.IndexOf("tcp    " + ipList[i] + ":" + tempPort) >= 0 || result.IndexOf("udp    " + ipList[i] + ":" + tempPort) >= 0)
                {
                    use = true;
                    break;
                }
            }
            p.Close();
            return use;
        }

        public static bool DrawRectInJpeg(string FileName, Rectangle rect)
        {
            try
            {
                bool isOk = false;
                string tmpName = FileName + "_bak";
                using (Bitmap bmp = new Bitmap(FileName))
                {
                    Graphics bmpGrh = Graphics.FromImage(bmp);
                    try
                    {
                        using (Pen pen = new Pen(Brushes.Red, 6f))
                        {
                            bmpGrh.DrawRectangle(pen, rect);
                        }


                        bmp.Save(tmpName, System.Drawing.Imaging.ImageFormat.Jpeg);
                        isOk = true;
                    }
                    finally
                    {
                        bmpGrh.Dispose();
                        bmpGrh = null;
                    }
                }

                if (isOk)
                {
                    File.Delete(FileName);
                    File.Move(tmpName, FileName);
                }
            }
            catch
            {
                return false;
            }


            return true;
        }
    }
}
