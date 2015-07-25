using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductExcel
{
    public class FileHelper
    {
        // <summary>
        /// 从一个目录将其内容移动到另一目录
        /// </summary>
        /// <param name="p">源目录</param>
        /// <param name="p_2">目的目录</param>
        static public void MoveFolderTo(string p, string p_2)
        {
            //检查是否存在目的目录
            if (!Directory.Exists(p_2))
                Directory.CreateDirectory(p_2);
            //先来移动文件
            DirectoryInfo info = new DirectoryInfo(p);
            FileInfo[] files = info.GetFiles();
            foreach (FileInfo file in files)
            {
                File.Copy(p, Path.Combine(p_2, file.Name), true); //复制文件
            }
        }

        // <summary>
        /// 从一个文件将其内容复制到另一目录
        /// </summary>
        /// <param name="p">源目录</param>
        /// <param name="p_2">目的目录</param>
        static public void CopyFileTo(string fileFullName, string dstDir, string newFileName)
        {
            if (System.IO.File.Exists(fileFullName) == false)//该批任务中，文件已解压，则不需要解密
            {
                return;
            }

            //检查是否存在目的目录
            if (!Directory.Exists(dstDir))
                Directory.CreateDirectory(dstDir);

            File.Copy(fileFullName, Path.Combine(dstDir, newFileName), true); //复制文件
            
        }

        // <summary>
        /// 从一个文件将其内容复制到另一目录
        /// </summary>
        /// <param name="p">源目录</param>
        /// <param name="p_2">目的目录</param>
        static public void CopyFileTo(string fileFullName, string dstFull)
        {
            if (System.IO.File.Exists(fileFullName) == false)//该批任务中，文件已解压，则不需要解密
            {
                return;
            }


            File.Copy(fileFullName, dstFull, true); //复制文件

        }
    }
}
