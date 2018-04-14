using MNPuzzle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNRestore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("命令开始");
            DateTime beforDT = System.DateTime.Now;

            Puzzle puzzle = new Puzzle(1000,1000);
            //puzzle.EmptyTransversePlan(999,-1);//mn横移
            //puzzle.ExecutePlan();//执行命令
            //puzzle.EmptyVerticalPlan(998,-1);//mn竖移
            //puzzle.ExecutePlan();
            //puzzle.RigthEntityVerticalPlan(100,1);//图块0竖移
            //puzzle.ExecutePlan();//执行命令
            //puzzle.SwapAction(101000, 101001);
            //puzzle.SwapAction(101001, 101002);
            //puzzle.LeftEntityVerticalPlan(50, -1);//图块102002竖移
            //puzzle.ExecutePlan();
            //puzzle.EmptyTransversePlan(200, 1);//mn横移
            //puzzle.ExecutePlan();//执行命令
            //puzzle.EmptyVerticalPlan(11, -1);//mn竖移
            //puzzle.ExecutePlan();
            //puzzle.LowerEntityTransversePlan(100,-1);//实体横移
            //puzzle.ExecutePlan();
            //puzzle.EmptyVerticalPlan(20, 1);//mn竖移
            //puzzle.ExecutePlan();
            //puzzle.RiseEntityTransversePlan(10, 1);//实体横移
            //puzzle.ExecutePlan();
            puzzle.SwapAction(999999,998999);
            puzzle.RiseEntityObliquePlan(10,-1,-1);//斜向移动
            puzzle.ExecutePlan();
            puzzle.RiseEntityObliquePlan(1, 1, 1);
            puzzle.ExecutePlan();
            puzzle.RiseEntityObliquePlan(1, 1, -1);
            puzzle.ExecutePlan();
            puzzle.RiseEntityObliquePlan(1, -1, 1);
            puzzle.ExecutePlan();
            DateTime afterDT = System.DateTime.Now;
            TimeSpan ts = afterDT.Subtract(beforDT);
            Console.WriteLine("DateTime总共花费{0}ms.", ts.TotalMilliseconds);
            Console.WriteLine("命令结束");
            for (int i=0;i<=999999;i++)
            { 
                if (puzzle.PuzzleArray[i]==999999)
                {
                    Console.WriteLine("60101:"+puzzle.PuzzleArray[60111]);
                    Console.WriteLine("位置40103:" + puzzle.PuzzleArray[40103]);
                    Console.WriteLine("102002:" + puzzle.PuzzleArray[52002]);
                    Console.WriteLine("0在位置100000：" + puzzle.PuzzleArray[100000]);
                    Console.WriteLine("mn位置：" + i);
                    Console.WriteLine("mn位置："+puzzle.mnPosition);
                    Console.WriteLine("列号："+i%puzzle.LieShu);
                    Console.WriteLine("行号："+(i-i%puzzle.LieShu)/puzzle.LieShu);
                    Console.WriteLine("最后一个是："+puzzle.PuzzleArray[999999]);
                    Console.WriteLine("逆序数:"+puzzle.InversionNumber);
                    break;
                }
            }
            //DateTime afterDT = System.DateTime.Now;
            //TimeSpan ts = afterDT.Subtract(beforDT);
            //Console.WriteLine("DateTime总共花费{0}ms.", ts.TotalMilliseconds);

            Console.Read();
        }
    }
}
