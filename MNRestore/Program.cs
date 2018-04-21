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
            DateTime afterDT = System.DateTime.Now;
            TimeSpan ts = afterDT.Subtract(beforDT);
            Console.WriteLine("DateTime总共花费{0}ms.", ts.TotalMilliseconds);
            Console.Read();
        }
    }
}
