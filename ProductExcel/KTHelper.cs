using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductExcel
{
    //康托展开： 求第N个排列
    class KTHelper
    {
        //阶乘
        static int[] fac = { 1, 1, 2, 6, 24, 120, 720, 5040, 40320, 362880, 3628800, 39916800 };

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

        //n<11   s.len<11    k<n!       //求全排列中，第几位的数组是多少
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
       
    }


}
