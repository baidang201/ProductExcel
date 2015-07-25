using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;

namespace ProductExcel
{
    class ExcelHelper
    {
        /*
         * 模板名称
         * 目标文件夹全名
         * 付款天数
         * 消费模式下标（用于随机数确定模式分配）
         * 信用卡信息
         * 公司信息
         * 失败原因
         */

        public static bool OutPutExcel(string tempFile, 
            string fullName, 
            int PayDayCount, 
            int PayModeIndex,
            List<PayInfo> listPayInfo, 
            List<CompanyInfo> listCompanyInfo,
            RadomHelper randomHelper,
            ref string  strFailReason)
        {
            int[] rgRadomNumIndex = new int[PayDayCount];
            RadomHelper.invKT(PayDayCount, randomHelper.dictRadom[PayDayCount][PayModeIndex], rgRadomNumIndex);


            FileHelper.CopyFileTo(tempFile, fullName);            

            if (System.IO.File.Exists(fullName) == false)//该批任务中，文件已解压，则不需要解密
            {
                strFailReason = "无法新建excel到目标目录";
                return false;
            }

            IWorkbook workbook = null;
            using (FileStream file = File.OpenRead(fullName))
            {
                workbook = new HSSFWorkbook(file);
            }

            //保存信用卡信息
            for (int i = 1; i <= PayDayCount; i++)//遍历页数
            {
                ISheet sheet = workbook.GetSheet(i.ToString());

                int beginRow = 4;
                int beginCol = 0;
                int row = beginRow;
                int col = beginCol;

                foreach (PayInfo payInfo in listPayInfo)
                {
                    sheet.GetRow(row).GetCell(col).SetCellValue(payInfo.Name);
                    sheet.GetRow(row).GetCell(col + 1).SetCellValue(payInfo.BillDay);
                    sheet.GetRow(row).GetCell(col + 2).SetCellValue(payInfo.PayDay);
                    sheet.GetRow(row).GetCell(col + 3).SetCellValue(payInfo.PayLimit);
                    row++;
                }

            }

            //保存商户
            {
                int row = 1;
                int col = 0;
                ISheet sheet = workbook.GetSheet("库");
                foreach (CompanyInfo companyInfo in listCompanyInfo)
                {
                    sheet.GetRow(row).GetCell(col).SetCellValue(companyInfo.LiveliHood);
                    sheet.GetRow(row).GetCell(col + 1).SetCellValue(companyInfo.Normal);
                    sheet.GetRow(row).GetCell(col + 2).SetCellValue(companyInfo.HightConsumption);
                    row++;
                }

            }
            using (FileStream fs = File.OpenWrite(fullName))
            {
                workbook.Write(fs);
            }
            return true;

        }
    }
}
