using MNPuzzle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Security.Cryptography;

namespace MNRestore
{
    class Program
    {
        static void Main(string[] args)
        {
            int hangshu = 10;
            int lieshu = 3;
            Puzzle puzzle = new Puzzle(hangshu,lieshu);
            PuzzleAide puzzleAide = new PuzzleAide(puzzle);
            Queue<Swap> swaps = new Queue<Swap>();
            int index = 22;
            int mnpos = 24;
            int enpos = 26;
            int target = 23;
            PuzzleAide.EntityToArgs entityToArgs = new PuzzleAide.EntityToArgs(mnpos, enpos, target, lieshu, hangshu);
            puzzleAide.EntityTo(swaps,entityToArgs);
            Swap swap1= puzzleAide.CheckExecutePlanFast(swaps,index, (swap,restoreRunInfo)=> {
                Console.WriteLine($"({swap.Empty},{swap.Entity})");
            });
            Console.WriteLine($"({swap1.Empty},{swap1.Entity})");
           // puzzleAide.CheckSwap(new Swap(37,27),27);
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
