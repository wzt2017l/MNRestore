using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MNPuzzle;
namespace MNTest
{
    [TestClass]
    public class PuzzleTest
    {
        private static Puzzle puzzle;
        //[AssemblyInitialize()]
        //public static void AssemblyInit(TestContext context)
        //{
        //   // 
        //}

        [ClassInitialize()]
        public static void ClassInit(TestContext context)
        {
            puzzle = new Puzzle(1000,1000);
        }

        #region 联合测试，这部分必须按顺序运行
        [TestMethod]
        public void _EmptyTransversePlanTest()
        {
            puzzle._EmptyTransversePlan(999999, 999, -1);
            puzzle._ExecutePlan();
            Assert.IsTrue(puzzle.PuzzleArray[999000] == 999999, "mn横向移动命令错误");
            puzzle._EmptyTransversePlan(999000, 10, 1);
            puzzle._ExecutePlan();
            Assert.IsTrue(puzzle.PuzzleArray[999010] == 999999, "mn横向移动命令错误");
        }

        [TestMethod]
        public void _EmptyVerticalPlanTest()
        {
            puzzle._EmptyVerticalPlan(999010, 999, -1);
            puzzle._ExecutePlan();
            Assert.IsTrue(puzzle.PuzzleArray[10] == 999999, "mn竖向移动命令错误");
            puzzle._EmptyVerticalPlan(10, 10, 1);
            puzzle._ExecutePlan();
            Assert.IsTrue(puzzle.PuzzleArray[10010] == 999999, "mn竖向移动命令错误");
        }

        [TestMethod]
        public void _LowerEntityTransversePlanTest()
        {
            Assert.IsTrue(puzzle.PuzzleArray[10009] == 10009, "下侧a_j横向移动命令错误");
            puzzle._LowerEntityTransversePlan(10010, 100, 1);
            puzzle._ExecutePlan();
            Assert.IsTrue(puzzle.PuzzleArray[10109] == 10009, "下侧a_j横向移动命令错误");
            Assert.IsTrue(puzzle.PuzzleArray[10110] == 999999, "下侧a_j横向移动命令错误");
            Assert.IsTrue(puzzle.PuzzleArray[10111] == 10111, "下侧a_j横向移动命令错误");
            puzzle._LowerEntityTransversePlan(10110, 50, -1);
            puzzle._ExecutePlan();
            Assert.IsTrue(puzzle.PuzzleArray[10061] == 10111, "下侧a_j横向移动命令错误");
            Assert.IsTrue(puzzle.PuzzleArray[10060] == 999999, "下侧a_j横向移动命令错误");
        }


        [TestMethod]
        public void _RigthEntityVerticalPlanTest()
        {
            Assert.IsTrue(puzzle.PuzzleArray[9060] == 9060, "右侧a_j竖向移动命令错误");
            puzzle._RigthEntityVerticalPlan(10060, 100, 1);
            puzzle._ExecutePlan();
            Assert.IsTrue(puzzle.PuzzleArray[109060] == 9060, "右侧a_j竖向移动命令错误");
            Assert.IsTrue(puzzle.PuzzleArray[110060] == 999999, "右侧a_j竖向移动命令错误");
            Assert.IsTrue(puzzle.PuzzleArray[111060] == 111060, "右侧a_j竖向移动命令错误");
            puzzle._RigthEntityVerticalPlan(110060, 50, -1);
            puzzle._ExecutePlan();
            Assert.IsTrue(puzzle.PuzzleArray[61060] == 111060, "右侧a_j竖向移动命令错误");
            Assert.IsTrue(puzzle.PuzzleArray[60060] == 999999, "右侧a_j竖向移动命令错误");
        }

        [TestMethod]
        public void _RiseEntityTransversePlanTest()
        {
            Assert.IsTrue(puzzle.PuzzleArray[60059] == 60059, "上侧a_j横向移动命令错误");
            puzzle._RiseEntityTransversePlan(60060, 100, 1);
            puzzle._ExecutePlan();
            Assert.IsTrue(puzzle.PuzzleArray[60159] == 60059, "上侧a_j横向移动命令错误");
            Assert.IsTrue(puzzle.PuzzleArray[60160] == 999999, "上侧a_j横向移动命令错误");
            Assert.IsTrue(puzzle.PuzzleArray[60161] == 60161, "上侧a_j横向移动命令错误");
            puzzle._RiseEntityTransversePlan(60160, 50, -1);
            puzzle._ExecutePlan();
            Assert.IsTrue(puzzle.PuzzleArray[60111] == 60161, "上侧a_j横向移动命令错误");
            Assert.IsTrue(puzzle.PuzzleArray[60110] == 999999, "上侧a_j横向移动命令错误");
        }

        [TestMethod]
        public void _LeftEntityVerticalPlanTest()
        {
            Assert.IsTrue(puzzle.PuzzleArray[61110] == 61110, "左侧a_j竖向移动命令错误");
            puzzle._LeftEntityVerticalPlan(60110, 40, -1);
            puzzle._ExecutePlan();
            Assert.IsTrue(puzzle.PuzzleArray[21110] == 61110, "左侧a_j竖向移动命令错误");
            Assert.IsTrue(puzzle.PuzzleArray[20110] == 999999, "左侧a_j竖向移动命令错误");
            Assert.IsTrue(puzzle.PuzzleArray[19110] == 19110, "左侧a_j竖向移动命令错误");
            puzzle._LeftEntityVerticalPlan(20110, 100, 1);
            puzzle._ExecutePlan();
            Assert.IsTrue(puzzle.PuzzleArray[119110] == 19110, "左侧a_j竖向移动命令错误");
            Assert.IsTrue(puzzle.PuzzleArray[120110] == 999999, "左侧a_j竖向移动命令错误");
        } 
        #endregion

        //[TestInitialize()]
        //public void Initialize()
        //{
        //   // 每个测试方法运行前都会运行
        //}

        //[TestCleanup()]
        //public void Cleanup()
        //{
        //   
        //}
        //[ClassCleanup()]
        //public static void ClassCleanup()
        //{
        //  
        //}

        //[AssemblyCleanup()]
        //public static void AssemblyCleanup()
        //{
        //  
        //}



    }
}
