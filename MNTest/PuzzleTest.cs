using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MNPuzzle;
namespace MNTest
{
    [TestClass]
    public class PuzzleTest
    {
        private static Puzzle puzzle;
    
        [ClassInitialize()]
        public static void PuzzleInit(TestContext context)
        {
            puzzle = new Puzzle(1000,1000);
        }

        [TestMethod]
        public void RetryNiXuTest()
        {
            Puzzle p = new Puzzle(100, 100);
            PuzzleAide pa = new PuzzleAide();
            pa.Disrupt(p);
            long inv1 = 0; int len = p.Items.Length;
            for (int i = 1; i < len; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (p.Items[i] < p.Items[j])
                    {
                        inv1++;
                    }
                }
            }
            long inv2 = p.RetryNiXu();
            Assert.IsTrue(inv1 == inv2, "逆序数计算错误");
        }

        [TestMethod]
        public void MnPositionTest()
        {
            int mnPos = puzzle.MnPosition();
            int mnAss=-2;
            for (int i=0;i<puzzle.Total;i++)
            {
                if (puzzle.Total - 1 == puzzle.Items[i])
                    mnAss = i;
            }
            Assert.IsTrue(mnPos==puzzle.mnPosition,"错误");
            Assert.IsTrue(mnPos == mnAss, "错误");
        }
    }
}
