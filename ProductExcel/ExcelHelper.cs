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
using UtilityTool;

namespace ProductExcel
{
    class ExcelHelper
    {
        static string EORLog = @"EORLog.txt";

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
            int PayDaysCount, 
            int PayModeIndex,
            List<PayInfo> listPayInfo, 
            List<CompanyInfo> listCompanyInfo,
            RadomHelper randomHelper,
            ref string  strFailReason)
        {
            int[] rgRadomNumIndex = new int[PayDaysCount];
            RadomHelper.invKT(PayDaysCount, randomHelper.dictRadom[PayDaysCount][PayModeIndex], rgRadomNumIndex);

            string strLog = string.Join(" ", rgRadomNumIndex);
            LogToolsEx.Error2File(EORLog, "OutPutExcel the RadomNumIndex is={0}", strLog);
            
            //因为生成的下标是从1开始，需要转换为从0开始
            for (int i = 0; i < PayDaysCount; i++)
            {
                rgRadomNumIndex[i]-= 1;
            }

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

            AssignInfo assignInfo = new AssignInfo();
            for (int i = 1; i <= PayDaysCount; i++)//遍历页数
            {
                ISheet sheet = workbook.GetSheet(i.ToString());

                int beginRow = 4;
                int beginCol = 0;
                int row = beginRow;
                int col = beginCol;

                int liveliHoodBeginCol = 5;
                int NormalBeginCol = 7;
                int HightConsumptionBeginCol = 9;

                int dayIndex = rgRadomNumIndex[i - 1];
                int payBatchCount = 0;

                int sumOfRowBeginRow = 4;
                int sumOfRowBeginCol = 4;

                int sumOfColBeginRow = 2;
                int sumOfColBeginCol = 5;

                int AllSumRow = 2;
                int AllSumCol = 4;

                Dictionary<int, double> dicSumRow = new Dictionary<int, double>();
                Dictionary<int, double> dicSumCol = new Dictionary<int, double>();

                #region 信用卡信息
                foreach (PayInfo payInfo in listPayInfo)
                {
                    if (dicSumRow.Any(r => r.Key == row) == false)
                    {
                        dicSumRow[row] = 0;
                    }
                   
                    sheet.GetRow(row).GetCell(col).SetCellValue(payInfo.Name);
                    sheet.GetRow(row).GetCell(col + 1).SetCellValue(payInfo.BillDay);
                    sheet.GetRow(row).GetCell(col + 2).SetCellValue(payInfo.PayDay);
                    sheet.GetRow(row).GetCell(col + 3).SetCellValue(payInfo.PayLimit);

                    //各消费等级总金额
                    double LiveliHoodMount = payInfo.PayLimit * assignInfo.dicAmountAssign[payInfo.CostBase][0];
                    double NormalMount = payInfo.PayLimit * assignInfo.dicAmountAssign[payInfo.CostBase][1];
                    double HightConsumptionMount = payInfo.PayLimit * assignInfo.dicAmountAssign[payInfo.CostBase][2];

                    #region 民生消费
                    payBatchCount = assignInfo.dicDaysUnionAssignInfo[PayDaysCount].listLiveliHoodPayInfoUnion[dayIndex].listOneDayPlanPayInfo.Count;
                    for (int j = 0; j < payBatchCount; j++)
                    {
                        double pay = LiveliHoodMount * assignInfo.dicDaysUnionAssignInfo[PayDaysCount].listLiveliHoodPayInfoUnion[dayIndex].listOneDayPlanPayInfo[j] / 100d;
                        sheet.GetRow(row).GetCell(liveliHoodBeginCol + j).SetCellValue(pay);

                        if (dicSumCol.Any(r => r.Key == liveliHoodBeginCol + j) == false)
                        {
                            dicSumCol[liveliHoodBeginCol + j] = 0;
                        }

                        dicSumRow[row] += pay;
                        dicSumCol[liveliHoodBeginCol + j] += pay;
                    }
                    #endregion

                    #region 普通消费
                    payBatchCount = assignInfo.dicDaysUnionAssignInfo[PayDaysCount].listNormalPayInfoUnion[dayIndex].listOneDayPlanPayInfo.Count;
                    for (int j = 0; j < payBatchCount; j++)
                    {
                        double pay = NormalMount * assignInfo.dicDaysUnionAssignInfo[PayDaysCount].listNormalPayInfoUnion[dayIndex].listOneDayPlanPayInfo[j] / 100d;
                        sheet.GetRow(row).GetCell(NormalBeginCol + j).SetCellValue(pay);

                        if (dicSumCol.Any(r => r.Key == NormalBeginCol + j) == false)
                        {
                            dicSumCol[NormalBeginCol + j] = 0;
                        }

                        dicSumRow[row] += pay;
                        dicSumCol[NormalBeginCol + j] += pay;
                    }
                    #endregion

                    #region 高消费
                    payBatchCount = assignInfo.dicDaysUnionAssignInfo[PayDaysCount].listHightConsumptionPayInfoUnion[dayIndex].listOneDayPlanPayInfo.Count;
                    for (int j = 0; j < payBatchCount; j++)
                    {
                        double pay = HightConsumptionMount * assignInfo.dicDaysUnionAssignInfo[PayDaysCount].listHightConsumptionPayInfoUnion[dayIndex].listOneDayPlanPayInfo[j] / 100d;
                        sheet.GetRow(row).GetCell(HightConsumptionBeginCol + j).SetCellValue(pay);

                        if (dicSumCol.Any(r => r.Key == HightConsumptionBeginCol + j) == false)
                        {
                            dicSumCol[HightConsumptionBeginCol + j] = 0;
                        }

                        dicSumRow[row] += pay;
                        dicSumCol[HightConsumptionBeginCol + j] += pay;
                    }
                    #endregion

                    row++;
                }
                #endregion

                #region 计算某行的总和
                foreach (int key in dicSumRow.Keys)
                {
                    double sum = dicSumRow[key];
                    sheet.GetRow(key).GetCell(sumOfRowBeginCol).SetCellValue(sum);
                }
                #endregion

                #region 计算某列的总和
                foreach (int key in dicSumCol.Keys)
                {
                    double sum = dicSumCol[key];
                    sheet.GetRow(sumOfColBeginRow).GetCell(key).SetCellValue(sum);
                }
                #endregion

                #region 计算总和
                {
                    double sum = dicSumRow.Values.Sum() + dicSumCol.Values.Sum() ;
                    sheet.GetRow(AllSumRow).GetCell(AllSumCol).SetCellValue(sum);
                }
                #endregion

                sheet.ForceFormulaRecalculation = true;//重新计算结果
            }

            #region 保存商户
            {
                int row = 1;
                int col = 0;
                ISheet sheet = workbook.GetSheet("库");
                foreach (CompanyInfo companyInfo in listCompanyInfo)
                {
                    sheet.CreateRow(row);
                    sheet.GetRow(row).CreateCell(col).SetCellValue(companyInfo.LiveliHood);
                    sheet.GetRow(row).CreateCell(col + 1).SetCellValue(companyInfo.Normal);
                    sheet.GetRow(row).CreateCell(col + 2).SetCellValue(companyInfo.HightConsumption);
                    row++;
                }

            }
            #endregion
            using (FileStream fs = File.OpenWrite(fullName))
            {
                workbook.Write(fs);
            }
            return true;

        }
    }
}
