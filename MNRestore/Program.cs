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
            Puzzle puzzle = new Puzzle(1000,1000);
            PuzzleAide puzzleAide = new PuzzleAide(puzzle);
            puzzleAide.Disrupt();
            puzzle.MnPosition();
            for (int i = 0; i < 1000; i++)
            {
                int[] rans = new int[8];
                for (int j=0;j<8;j++)
                {
                    byte[] ranBytes = new byte[4];
                    RNGCryptoServiceProvider rngServiceProvider = new RNGCryptoServiceProvider();
                    rngServiceProvider.GetBytes(ranBytes);
                    rans[j] = Math.Abs(BitConverter.ToInt32(ranBytes, 0));
                }
                int entityPos = (rans[0]%998+1) * 1000 + (rans[1] % 998 + 1);
                int target = (rans[2] % 998 + 1) * 1000 + (rans[3] % 998 + 1);
                bool VorT =rans[4]% 2 == 0 ? true : false;
                bool entityRDorLU = rans[5] % 2 == 0 ? true : false;
                bool mnToVorT = rans[6] % 2 == 0 ? true : false;
                bool mnToDefault = rans[7] % 2 == 0 ? true : false;

                int mnPos = puzzle.mnPosition;
                int entity = puzzle.Items[entityPos];//值
                if (mnPos != entityPos)
                {
                    string error0 = $"前信息：i:{i},entity:{entity},entityPos:{entityPos}，target:{target},mnPos:{mnPos},VorT:{VorT},entityRDorLU:{entityRDorLU},mnToVorT:{mnToVorT},mnToDefault:{mnToDefault}";
                    Console.WriteLine(error0);
                    puzzleAide.EntityTo(entityPos, target, VorT, entityRDorLU, mnToVorT, mnToDefault);
                    puzzleAide.ExecutePlan();
                    int entity1 = puzzle.Items[target];
                    string error = $"信息：i:{i},entity:{entity}==entity1:{entity1},entityPos:{entityPos}，target:{target},mnPos:{mnPos},VorT:{VorT},entityRDorLU:{entityRDorLU},mnToVorT:{mnToVorT},mnToDefault:{mnToDefault}";
                    Console.WriteLine(error);
                    if (entity!=entity1)
                    {
                        Console.WriteLine("发生错误");
                        Console.Read();
                    }
                }
            }
            int mn= puzzle.MnPosition();
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
