using System;
using System.Diagnostics;
using System.Reflection;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;

namespace UtilityTool  
{  
    public class CertificateHelper  
    {  
        /// <summary>  
        /// 安装资源文件中的证书  
        /// </summary>  
        public static string InstallCertificateFromResource(StoreName sn, byte[] certificatefile)  
        {
            try
            {
                StorePermission sp = new StorePermission(StorePermissionFlags.AllFlags);
                sp.Demand();
                X509Certificate2 certificate = new X509Certificate2(certificatefile);

                if (TryGetCertificate(sn, certificatefile) == null)
                {
                    X509Store AuthRoot = new X509Store(sn, StoreLocation.LocalMachine);
                    AuthRoot.Open(OpenFlags.ReadWrite);
                    //AuthRoot.Remove(certificate);
                    AuthRoot.Add(certificate);
                    AuthRoot.Close();
                }
                return string.Empty;
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }  
  
        /// <summary>  
        /// 获取资源文件中的证书  
        /// </summary>  
        public static X509Certificate2 GetCertificateFromResource(byte[] certificatefile)  
        {  
            //使用“Visual Studio命令提示”工具生成证书文件：  
            //makecert -r -pe -n "CN=CameraNavi" -b 01/01/2013 -e 01/01/2055 -sky exchange -sv CameraNavi.pvk CameraNavi.cer  
            //pvk2pfx.exe -pvk CameraNavi.pvk -spc CameraNavi.cer -pfx CameraNavi.pfx  
            //然后将CameraNavi.pfx导入成资源  
            //byte[] certificatefile = Certificateinstaller.Properties.Resources.CameraNavi;
            return new X509Certificate2(certificatefile); 
            //return new X509Certificate2(certificatefile,"",X509KeyStorageFlags.PersistKeySet);  
        }  
  
        /// <summary>  
        /// 尝试获取计算机中的证书  
        /// </summary>  
        public static X509Certificate2 TryGetCertificate(StoreName sn, byte[] certificatefile)  
        {  
            var store = new X509Store(sn, StoreLocation.LocalMachine);  
            store.Open(OpenFlags.ReadOnly);
            var certs = store.Certificates.Find(X509FindType.FindBySubjectDistinguishedName, GetCertificateFromResource(certificatefile).Subject, false);
            store.Close();
            return (certs.Count > 0) ? certs[0] : null;  
        }  
  
        /// <summary>  
        /// 通过另开进程运行本程序集来安装证书  
        /// </summary>  
        private static void InstallCertificate()  
        {  
            Process.Start(Assembly.GetExecutingAssembly().ManifestModule.Name).WaitForExit();  
        }   
    }  
}  

