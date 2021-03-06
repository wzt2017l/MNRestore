﻿using MNPuzzle;
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
            // int hangshu = 10;
            // int lieshu = 3;
            // Puzzle puzzle = new Puzzle(hangshu,lieshu);
            // PuzzleAide puzzleAide = new PuzzleAide(puzzle);
            // Queue<Swap> swaps = new Queue<Swap>();
            // int index = 22;
            // int mnpos = 24;
            // int enpos = 26;
            // int target = 23;
            // PuzzleAide.EntityToArgs entityToArgs = new PuzzleAide.EntityToArgs(mnpos, enpos, target, lieshu, hangshu);
            // puzzleAide.EntityTo(swaps,entityToArgs);
            // Swap swap1= puzzleAide.CheckExecutePlanFast(swaps,index, (swap,restoreRunInfo)=> {
            //     Console.WriteLine($"({swap.Empty},{swap.Entity})");
            // });
            // Console.WriteLine($"({swap1.Empty},{swap1.Entity})");
            //// puzzleAide.CheckSwap(new Swap(37,27),27);
            //Puzzle puzzle = new Puzzle(3,3);
            //PuzzleAide puzzleAide = new PuzzleAide(puzzle);
            //for (int i = 0; i < 100000; i++)
            //{
            //    Puzzle puzzle = new Puzzle(3, 3);
            //    PuzzleAide puzzleAide = new PuzzleAide();
            //    puzzleAide.Disrupt(puzzle);
            //    for (int j=0;j<9;j++)
            //    {
            //        for (int k=j+1;k<9;k++)
            //        {
            //            if (puzzle.Items[k]==puzzle.Items[j])
            //            {
            //                Console.WriteLine("重复");
            //                Console.Read();
            //            }
            //        }
            //    }
            //}
            moni();
            int ok = 0,fail=0, err = 0;
            for (int i=0;i<1000;i++)
            {
                //byte[] ranBytes = new byte[4];
                //RNGCryptoServiceProvider rngServiceProvider = new RNGCryptoServiceProvider();
                //rngServiceProvider.GetBytes(ranBytes);
                //int hang = Math.Abs(BitConverter.ToInt32(ranBytes, 0));
                //byte[] ranBytes1 = new byte[4];
                //RNGCryptoServiceProvider rngServiceProvider1 = new RNGCryptoServiceProvider();
                //rngServiceProvider.GetBytes(ranBytes1);
                //int lie = Math.Abs(BitConverter.ToInt32(ranBytes1, 0));
                //hang = hang % 98 + 2;
                //lie = lie % 98 + 2;
                Puzzle puzzle = new Puzzle(10,10);
                PuzzleAide puzzleAide = new PuzzleAide(puzzle);
                puzzleAide.DisruptReducible();
                try
                {
                    Console.WriteLine("");
                    for (int j = 0; j < puzzle.Total; j++)
                    {
                        Console.Write($"{puzzle.Items[j]},");
                    }
                    Console.WriteLine("");
                    bool b= puzzleAide.Restore(jiance);
                    if (b)
                    {
                        Console.WriteLine("");
                        for (int j = 0; j < puzzle.Total; j++)
                        {
                            Console.Write($"{puzzle.Items[j]},");
                        }
                        Console.WriteLine("");
                        Console.WriteLine("成功！" );
                        ok++;
                    }
                    else
                    {
                        Console.WriteLine("");
                        for (int j = 0; j < puzzle.Total; j++)
                        {
                            Console.Write($"{puzzle.Items[j]},");
                        }
                        Console.WriteLine("");
                        Console.WriteLine("失败！");
                        Console.Read();
                        fail++;
                    }
                }
                catch
               {
                    Console.WriteLine("");
                    for (int j = 0; j < puzzle.Total; j++)
                    {
                        Console.Write($"{puzzle.Items[j]},");
                    }
                    Console.WriteLine("");
                    Console.WriteLine("发生异常！");
                    Console.Read();
                    err++;
                //puzzle = new Puzzle(100, 100);
                //puzzleAide = new PuzzleAide(puzzle);
                }

            }


            Console.WriteLine($"成功：{ok },失败：{fail},出错：{err}");
            Console.Read();
            Console.Read();
        }
        public static void jiance(Swap swap, RestoreRunInfo restoreRunInfo)
        {
            Console.WriteLine();
            Console.Write($"({swap.Empty},{swap.Entity})*");
            if (restoreRunInfo != null)
                Console.Write($"[index:{restoreRunInfo.index},enPos:{restoreRunInfo.entityPos},其它消息：{restoreRunInfo.otherMess}]");
           
        }
        public static void moni()
        {
            Puzzle puzzle = new Puzzle(3,3);
            puzzle.Items = new int[] { 0, 2, 5, 3, 4, 8, 6, 7, 1 };
            PuzzleAide puzzleAide = new PuzzleAide(puzzle);
            try
            {
                Console.WriteLine("");
                for (int j = 0; j < puzzle.Total; j++)
                {
                    Console.Write($"{puzzle.Items[j]},");
                }
                Console.WriteLine("");
                bool b = puzzleAide.Restore();
                if (b)
                {
                    Console.WriteLine("");
                    for (int j = 0; j < puzzle.Total; j++)
                    {
                        Console.Write($"{puzzle.Items[j]},");
                    }
                    Console.WriteLine("");
                    Console.WriteLine("成功！");
                    
                }
                else
                {
                    Console.WriteLine("");
                    for (int j = 0; j < puzzle.Total; j++)
                    {
                        Console.Write($"{puzzle.Items[j]},");
                    }
                    Console.WriteLine("");
                    Console.WriteLine("失败！");
                    Console.Read();
                    
                }
            }
            catch
            {
                Console.WriteLine("");
                for (int j = 0; j < puzzle.Total; j++)
                {
                    Console.Write($"{puzzle.Items[j]},");
                }
                Console.WriteLine("");
                Console.WriteLine("发生异常！");
                Console.Read();
                
                //puzzle = new Puzzle(100, 100);
                //puzzleAide = new PuzzleAide(puzzle);
            }

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
