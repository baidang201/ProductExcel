using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductExcel
{
    //康托展开： 求第N个排列
    class RadomHelper
    {
        public Dictionary<int, List<int>> dictRadom;

        public RadomHelper()
        {
            dictRadom = new Dictionary<int,List<int>>();

            int Beginday = 3;
            int dayLimit = 11;
            for (int i = Beginday; i < dayLimit; i++)
            {
                int count = fac[i] > 10 ? 10 : fac[i];
                List<int> listNum = GetNoRepeaterNumList(0, fac[i], count);
                dictRadom.Add(i, listNum);
            }
        }


        //阶乘
        static public int[] fac = { 1, 1, 2, 6, 24, 120, 720, 5040, 40320, 362880, 3628800, 39916800 };

        //n<11   s.len <11, 求一个排列在全排列中第几位
        static public int kt(int n, int[] s)
        {
            int sum = 0, smallNum;
            for (int i = 0; i < n; i++)
            {
                smallNum = 0; //比当前数小的数
                for (int j = i + 1; j < n; j++)
                    if (s[i] > s[j])
                        smallNum++;
                sum += smallNum * fac[n - i - 1];
            }
            return sum;
        }

        //n<11   s.len<11    k<n!       //求n个数的全排列中，第k位的数组s是多少
        static public void invKT(int n, int k, int[] s)
        {
            int t, j;//需要记录该数是否已在前面出现过
            int visitCount = 2 * n;
            bool[] visit = new bool[visitCount];

            for (int i = 0; i < n; i++)
            {
                t = k / fac[n - i - 1];
                for (j = 1; j <= n; j++)
                {
                    if (!visit[j])
                    {
                        if (t == 0) break;
                        t--;
                    }
                }
                s[i] = j;
                visit[j] = true;
                k %= fac[n - i - 1];
            }
        }

        //注意，生成的随机数，不包含end
        static public List<int> GetNoRepeaterNumList(int begin, int end, int count)
        {
            int NumCount = end - begin;
            List<int> listTemp = new List<int>(NumCount);
            List<int> listNum = new List<int>();

            for (int i = begin; i < end; i++)
            {
                listTemp.Add(i);
            }

            int index = 0;
            int value = 0;
            Random random = new Random();
            for (int i = 0; i < count; i++)
            {
                if (0 == listTemp.Count )
                {
                    return null;
                }
                index = random.Next(0, listTemp.Count);
                value = listTemp[index];
                listNum.Add(value);
                listTemp.RemoveAt(index);
            }

            return listNum;
        }


    }


}
