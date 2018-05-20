using MNPuzzle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MNRestore
{
    class Program
    {
        static void Main(string[] args)
        {
            //=========================================================================
            Console.WriteLine("命令开始");
            int l = 1000000;
            int[] array = new int[l];
            int[] array1 = new int[l];
            for (int i = 0; i < l; i++)
            {
                array[i] = l - i - 1;
                array1[i] = l - i;
            }
            //Puzzle p = new Puzzle(1000, 1000);
            //Console.WriteLine("开始计算");
            //DateTime beforDT1 = System.DateTime.Now;
            //long lg = p.RetryNiXu();
            //Console.WriteLine(lg);
            //DateTime afterDT1 = System.DateTime.Now;
            //TimeSpan ts1 = afterDT1.Subtract(beforDT1);
            //Console.WriteLine("DateTime总共花费{0}ms.", ts1.TotalMilliseconds);

            Console.WriteLine("开始计算");
            DateTime beforDT2 = System.DateTime.Now;
            long lg2 = RetryNiXu(array);
            Console.WriteLine(lg2);
            DateTime afterDT2 = System.DateTime.Now;
            TimeSpan ts2 = afterDT2.Subtract(beforDT2);
            Console.WriteLine("DateTime总共花费{0}ms.", ts2.TotalMilliseconds);
            //=========================================================================
            Console.Read();
        }

        public static long RetryNiXu(int[] array)
        {
            long inversion = 0;
            int len = array.Length;
            int k =(int)Math.Pow(3.0, Math.Log10(len)) + 1;//对数组进行分组
            long T = (long)len * (long)(len + 1) /(long) k;
            long[] list = new long[k];
            int[] begin = new int[k];
            int[] end = new int[k];
            Parallel.For(0,k,i=> {
                begin[i] = (int)Math.Sqrt(i * T) + 1;
                end[i] = (int)Math.Sqrt((i + 1) * T) + 1;
                if (i == k - 1)
                {
                    end[i] = len;
                }
            });

            Parallel.For(0,k,i=> {
                long l = 0;
                //int begin = (int)Math.Sqrt(i* T)+1;
                //int end = (int)Math.Sqrt((i+1)*T)+1;
                //if (i==k-1)
                //{
                //    end = len;
                //}
                for (int e=begin[i];e<end[i];e++)
                {
                    int m = array[e];
                    for (int j=0;j<e;j++)
                    {
                        if (m<array[j])
                        {
                            l++;
                        }
                    }
                }
                list[i] = l;
            });
            Parallel.For(0,k,i=>{
                lock (list)
                {
                    inversion += list[i];
                }
            });
            //int len = array.Length;
            //long[] list = new long[len];
            //Parallel.For(1, len, i =>
            //{
            //    long l = 0;
            //    int length = len - i;
            //    int index = array[length];
            //    //如何才能提前结束循环？
            //    for (int k = 0; k < length; k++)
            //    {
            //        if (index < array[k])
            //        {
            //            l++;
            //        }
            //    }
            //    list[i] = l;
            //});
            //Parallel.For(0, len, i => {
            //    lock (list)
            //    {
            //        inversion += list[i];
            //    }
            //});
            return inversion;
        }

    }
  

   
 
}
