using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilityTool;

namespace ProductExcel
{    
    

    class PreViewHelper
    {
        static string EORLog = @"EORLog.txt";

        public static List<PreViewInfo> ProductPreViewList(List<double> listSum, List<double> listCard)
        {
            List<PreViewInfo> listPreView = new List<PreViewInfo>();
            double sumOfCard = listCard.Sum();
            double sum = listSum.Sum();
            

            listPreView.Add(new PreViewInfo("总额度", "实际还款", "", "", ""));
            listPreView.Add(new PreViewInfo(sumOfCard.ToString(), sum.ToString(), "", "", ""));
            listPreView.Add(new PreViewInfo());

            
            listPreView.Add(new PreViewInfo("第一天", "第二天", "第三天", "第四天", "第五天"));
            listPreView.Add(new PreViewInfo(
                0>= listSum.Count()? "0": listSum[0].ToString(), 
                1>= listSum.Count()? "0": listSum[1].ToString(), 
                2>= listSum.Count()? "0": listSum[2].ToString(), 
                3>= listSum.Count()? "0": listSum[3].ToString(), 
                4>= listSum.Count()? "0": listSum[4].ToString()
                ));

            listPreView.Add(new PreViewInfo("第六天", "第七天", "第八天", "第九天", "第十天"));
            listPreView.Add(new PreViewInfo(
                5 >= listSum.Count() ? "0" : listSum[5].ToString(), 
                6 >= listSum.Count() ? "0" : listSum[6].ToString(), 
                7 >= listSum.Count() ? "0" : listSum[7].ToString(), 
                8 >= listSum.Count() ? "0" : listSum[8].ToString(), 
                9 >= listSum.Count() ? "0" : listSum[9].ToString()
                ));
            

            return listPreView;
        }

        public static List<double> GetSumList(
            int PayDaysCount,
            int PayModeIndex,
            List<PayInfo> listPayInfo,
            List<CompanyInfo> listCompanyInfo,
            RadomHelper randomHelper,
            ref string strFailReason
            )
        {
            List<double> listSum = new List<double>();

            int[] rgRadomNumIndex = new int[PayDaysCount];
            RadomHelper.invKT(PayDaysCount, randomHelper.dictRadom[PayDaysCount][PayModeIndex], rgRadomNumIndex);

            string strLog = string.Join(" ", rgRadomNumIndex);
            LogToolsEx.Error2File(EORLog, "OutPutExcel the RadomNumIndex is={0}", strLog);

            //因为生成的下标是从1开始，需要转换为从0开始
            for (int i = 0; i < PayDaysCount; i++)
            {
                rgRadomNumIndex[i] -= 1;
            }

            AssignInfo assignInfo = new AssignInfo();
            for (int i = 1; i <= PayDaysCount; i++)//遍历页数
            {                

                int beginRow = 4;
                int beginCol = 0;
                int row = beginRow;
                int col = beginCol;

                int liveliHoodBeginCol = 5;
                int NormalBeginCol = 7;
                int HightConsumptionBeginCol = 9;

                int dayIndex = rgRadomNumIndex[i - 1];
                int payBatchCount = 0;


                Dictionary<int, double> dicSumRow = new Dictionary<int, double>();
                Dictionary<int, double> dicSumCol = new Dictionary<int, double>();

                #region 信用卡信息
                foreach (PayInfo payInfo in listPayInfo)
                {
                    if (dicSumRow.Any(r => r.Key == row) == false)
                    {
                        dicSumRow[row] = 0;
                    }

                    //各消费等级总金额
                    double LiveliHoodMount = payInfo.PayLimit * assignInfo.dicAmountAssign[payInfo.CostBase][0];
                    double NormalMount = payInfo.PayLimit * assignInfo.dicAmountAssign[payInfo.CostBase][1];
                    double HightConsumptionMount = payInfo.PayLimit * assignInfo.dicAmountAssign[payInfo.CostBase][2];

                    #region 民生消费
                    payBatchCount = assignInfo.dicDaysUnionAssignInfo[PayDaysCount].listLiveliHoodPayInfoUnion[dayIndex].listOneDayPlanPayInfo.Count;
                    for (int j = 0; j < payBatchCount; j++)
                    {
                        double pay = LiveliHoodMount * assignInfo.dicDaysUnionAssignInfo[PayDaysCount].listLiveliHoodPayInfoUnion[dayIndex].listOneDayPlanPayInfo[j] / 100d;
                        
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
                }
                #endregion

                #region 计算某列的总和
                foreach (int key in dicSumCol.Keys)
                {
                    double sum = dicSumCol[key];
                }
                #endregion

                #region 计算总和
                {
                    double sum = dicSumRow.Values.Sum();
                    listSum.Add(sum);
                }
                #endregion                
            }

            return listSum;
        }
    }
}
