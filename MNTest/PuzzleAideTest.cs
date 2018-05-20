using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MNPuzzle;

namespace MNTest
{
    [TestClass]
    public class PuzzleAideTest
    {
        private static Puzzle puzzle;
        private static PuzzleAide puzzleAide;
        [ClassInitialize()]
        public static void PuzzleAideInit(TestContext context)
        {
            puzzle = new Puzzle(1000, 1000);
            puzzleAide = new PuzzleAide(puzzle);
        }

        #region 基础命令测试

        #region 联合测试，这部分必须按顺序运行
        [TestMethod]
        public void EmptyTransversePlanTest()
        {
            puzzleAide.EmptyTransversePlan(999999, 999, -1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[999000] == 999999, "mn横向移动命令错误");
            puzzleAide.EmptyTransversePlan(999000, 10, 1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[999010] == 999999, "mn横向移动命令错误");
        }

        [TestMethod]
        public void EmptyVerticalPlanTest()
        {
            puzzleAide.EmptyVerticalPlan(999010, 999, -1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[10] == 999999, "mn竖向移动命令错误");
            puzzleAide.EmptyVerticalPlan(10, 10, 1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[10010] == 999999, "mn竖向移动命令错误");
        }

        [TestMethod]
        public void LowerEntityTransversePlanTest()
        {
            Assert.IsTrue(puzzle.Items[10009] == 10009, "下侧a_j横向移动命令错误");
            puzzleAide.LowerEntityTransversePlan(10010, 100, 1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[10109] == 10009, "下侧a_j横向移动命令错误");
            Assert.IsTrue(puzzle.Items[10110] == 999999, "下侧a_j横向移动命令错误");
            Assert.IsTrue(puzzle.Items[10111] == 10111, "下侧a_j横向移动命令错误");
            puzzleAide.LowerEntityTransversePlan(10110, 50, -1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[10061] == 10111, "下侧a_j横向移动命令错误");
            Assert.IsTrue(puzzle.Items[10060] == 999999, "下侧a_j横向移动命令错误");
        }


        [TestMethod]
        public void RigthEntityVerticalPlanTest()
        {
            Assert.IsTrue(puzzle.Items[9060] == 9060, "右侧a_j竖向移动命令错误");
            puzzleAide.RigthEntityVerticalPlan(10060, 100, 1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[109060] == 9060, "右侧a_j竖向移动命令错误");
            Assert.IsTrue(puzzle.Items[110060] == 999999, "右侧a_j竖向移动命令错误");
            Assert.IsTrue(puzzle.Items[111060] == 111060, "右侧a_j竖向移动命令错误");
            puzzleAide.RigthEntityVerticalPlan(110060, 50, -1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[61060] == 111060, "右侧a_j竖向移动命令错误");
            Assert.IsTrue(puzzle.Items[60060] == 999999, "右侧a_j竖向移动命令错误");
        }

        [TestMethod]
        public void RiseEntityTransversePlanTest()
        {
            Assert.IsTrue(puzzle.Items[60059] == 60059, "上侧a_j横向移动命令错误");
            puzzleAide.RiseEntityTransversePlan(60060, 100, 1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[60159] == 60059, "上侧a_j横向移动命令错误");
            Assert.IsTrue(puzzle.Items[60160] == 999999, "上侧a_j横向移动命令错误");
            Assert.IsTrue(puzzle.Items[60161] == 60161, "上侧a_j横向移动命令错误");
            puzzleAide.RiseEntityTransversePlan(60160, 50, -1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[60111] == 60161, "上侧a_j横向移动命令错误");
            Assert.IsTrue(puzzle.Items[60110] == 999999, "上侧a_j横向移动命令错误");
        }

        [TestMethod]
        public void LeftEntityVerticalPlanTest()
        {
            Assert.IsTrue(puzzle.Items[61110] == 61110, "左侧a_j竖向移动命令错误");
            puzzleAide.LeftEntityVerticalPlan(60110, 40, -1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[21110] == 61110, "左侧a_j竖向移动命令错误");
            Assert.IsTrue(puzzle.Items[20110] == 999999, "左侧a_j竖向移动命令错误");
            Assert.IsTrue(puzzle.Items[19110] == 19110, "左侧a_j竖向移动命令错误");
            puzzleAide.LeftEntityVerticalPlan(20110, 100, 1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[119110] == 19110, "左侧a_j竖向移动命令错误");
            Assert.IsTrue(puzzle.Items[120110] == 999999, "左侧a_j竖向移动命令错误");
        }
        #endregion

        [TestMethod]
        public void RiseEntityObliquePlanTest()
        {
            puzzle = new Puzzle(1000, 1000);
            puzzleAide = new PuzzleAide(puzzle);
            puzzleAide.EmptyTransversePlan(999999, 999, -1);
            puzzleAide.EmptyVerticalPlan(999000, 998, -1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[1000] == 999999, "上侧a_j斜向移动命令错误");
            puzzleAide.RiseEntityObliquePlan(1000, 100, 1, 1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[100100] == 0, "上侧a_j斜向移动命令错误");
            Assert.IsTrue(puzzle.Items[101100] == 999999, "上侧a_j斜向移动命令错误");

            Assert.IsTrue(puzzle.Items[102100] == 102100, "上侧a_j斜向移动命令错误");
            puzzleAide.RiseEntityObliquePlan(101100, 50, -1, 1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[52150] == 102100, "上侧a_j斜向移动命令错误");
            Assert.IsTrue(puzzle.Items[51150] == 999999, "上侧a_j斜向移动命令错误");

            Assert.IsTrue(puzzle.Items[50150] == 50150, "上侧a_j斜向移动命令错误");
            puzzleAide.RiseEntityObliquePlan(51150, 100, 1, -1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[150050] == 50150, "上侧a_j斜向移动命令错误");
            Assert.IsTrue(puzzle.Items[151050] == 999999, "上侧a_j斜向移动命令错误");

            Assert.IsTrue(puzzle.Items[152050] == 152050, "上侧a_j斜向移动命令错误");
            puzzleAide.RiseEntityObliquePlan(151050, 40, -1, -1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[112010] == 152050, "上侧a_j斜向移动命令错误");
            Assert.IsTrue(puzzle.Items[111010] == 999999, "上侧a_j斜向移动命令错误");
        }

        [TestMethod]
        public void LateralEntityObliquePlanTest()
        {
            puzzle = new Puzzle(1000, 1000);
            puzzleAide = new PuzzleAide(puzzle);
            puzzleAide.EmptyTransversePlan(999999, 1, -1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[999998] == 999999, "横侧a_j斜向移动命令错误");
            puzzleAide.LateralEntityObliquePlan(999998, 200, -1, -1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[799799] == 999998, "横侧a_j斜向移动命令错误");
            Assert.IsTrue(puzzle.Items[799798] == 999999, "横侧a_j斜向移动命令错误");

            Assert.IsTrue(puzzle.Items[799797] == 799797, "横侧a_j斜向移动命令错误");
            puzzleAide.LateralEntityObliquePlan(799798, 100, -1, 1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[699897] == 799797, "横侧a_j斜向移动命令错误");
            Assert.IsTrue(puzzle.Items[699898] == 999999, "横侧a_j斜向移动命令错误");

            Assert.IsTrue(puzzle.Items[699899] == 699899, "横侧a_j斜向移动命令错误");
            puzzleAide.LateralEntityObliquePlan(699898, 150, 1, -1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[849749] == 699899, "横侧a_j斜向移动命令错误");
            Assert.IsTrue(puzzle.Items[849748] == 999999, "横侧a_j斜向移动命令错误");

            Assert.IsTrue(puzzle.Items[849747] == 849747, "横侧a_j斜向移动命令错误");
            puzzleAide.LateralEntityObliquePlan(849748, 100, 1, 1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[949847] == 849747, "横侧a_j斜向移动命令错误");
            Assert.IsTrue(puzzle.Items[949848] == 999999, "横侧a_j斜向移动命令错误");
        }
        #endregion

        #region 复合命令测试,这部分按顺序执行
        [TestMethod]
        public void EmptyToVtTest()
        {
            puzzle = new Puzzle(1000,1000);
            puzzleAide = new PuzzleAide(puzzle);
            puzzleAide.EmptyToVt(999999,123456);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[123456] == 999999, "复合命令，移动mn错误");

            puzzleAide.EmptyToVt(123456, 654321);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[654321] == 999999, "复合命令，移动mn错误");
        }
        [TestMethod]
        public void EmptyToTvTest()
        {
            puzzleAide.EmptyToVt(654321, 876543);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[876543] == 999999, "复合命令，移动mn错误");

            puzzleAide.EmptyToVt(876543, 1001);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[1001] == 999999, "复合命令，移动mn错误");
        }
        #endregion

        [TestMethod]
        public void MnPositionTest()
        {
            puzzle = new Puzzle(1000, 1000);
            puzzleAide = new PuzzleAide(puzzle);
            int mn = 0;
            puzzleAide.EmptyTransversePlan(999999, 999, -1);
            mn = puzzleAide.MnPosition(999999,999,0,-1);
            puzzleAide.ExecutePlan();
           
            Assert.IsTrue(puzzle.Items[mn] == 999999, "mn横向移动命令错误");
            puzzleAide.EmptyTransversePlan(mn, 10, 1);
            mn = puzzleAide.MnPosition(mn, 10, 0, 1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[mn] == 999999, "mn横向移动命令错误");

            puzzleAide.EmptyVerticalPlan(mn, 999, -1);
            mn = puzzleAide.MnPosition(mn, 999, -1,0);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[mn] == 999999, "mn竖向移动命令错误");
            puzzleAide.EmptyVerticalPlan(mn, 10, 1);
            mn = puzzleAide.MnPosition(mn, 10,1,0);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[mn] == 999999, "mn竖向移动命令错误");

            Assert.IsTrue(puzzle.Items[10009] == 10009, "下侧a_j横向移动命令错误");
            puzzleAide.LowerEntityTransversePlan(mn, 100, 1);
            mn = puzzleAide.MnPosition(mn, 100, 0, 1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[10109] == 10009, "下侧a_j横向移动命令错误");
            Assert.IsTrue(puzzle.Items[mn] == 999999, "下侧a_j横向移动命令错误");
            Assert.IsTrue(puzzle.Items[10111] == 10111, "下侧a_j横向移动命令错误");
            puzzleAide.LowerEntityTransversePlan(10110, 50, -1);
            mn = puzzleAide.MnPosition(mn, 50, 0, -1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[10061] == 10111, "下侧a_j横向移动命令错误");
            Assert.IsTrue(puzzle.Items[mn] == 999999, "下侧a_j横向移动命令错误");



            puzzle = new Puzzle(1000, 1000);
            puzzleAide = new PuzzleAide(puzzle);
            puzzleAide.EmptyTransversePlan(999999, 1, -1);
            puzzleAide.ExecutePlan();
            mn = puzzleAide.MnPosition(999999, 1, 0, -1);
            puzzleAide.LateralEntityObliquePlan(mn, 200, -1, -1);
            mn = puzzleAide.MnPosition(mn, 200, -1, -1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[799799] == 999998, "横侧a_j斜向移动命令错误");
            Assert.IsTrue(puzzle.Items[mn] == 999999, "横侧a_j斜向移动命令错误1");

            Assert.IsTrue(puzzle.Items[799797] == 799797, "横侧a_j斜向移动命令错误");
            puzzleAide.LateralEntityObliquePlan(mn, 100, -1, 1);
            mn = puzzleAide.MnPosition(mn, 100, -1, 1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[699897] == 799797, "横侧a_j斜向移动命令错误");
            Assert.IsTrue(puzzle.Items[mn] == 999999, "横侧a_j斜向移动命令错误2");

            Assert.IsTrue(puzzle.Items[699899] == 699899, "横侧a_j斜向移动命令错误");
            puzzleAide.LateralEntityObliquePlan(mn, 150, 1, -1);
            mn = puzzleAide.MnPosition(mn, 150, 1, -1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[849749] == 699899, "横侧a_j斜向移动命令错误");
            Assert.IsTrue(puzzle.Items[mn] == 999999, "横侧a_j斜向移动命令错误3");

            Assert.IsTrue(puzzle.Items[849747] == 849747, "横侧a_j斜向移动命令错误");
            puzzleAide.LateralEntityObliquePlan(mn, 100, 1, 1);
            mn = puzzleAide.MnPosition(mn, 100, 1, 1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[949847] == 849747, "横侧a_j斜向移动命令错误");
            Assert.IsTrue(puzzle.Items[mn] == 999999, "横侧a_j斜向移动命令错误4");
        }

        [TestMethod]
        public void PositionAnalysisTest()
        {
            Assert.IsTrue(puzzleAide.PositionAnalysis(10, 10, 17) == Position.Origin, "方位判断错误");
            Assert.IsTrue(puzzleAide.PositionAnalysis(17, 0, 17) == Position.Up, "方位判断错误");
            Assert.IsTrue(puzzleAide.PositionAnalysis(170, 1, 17) == Position.UpGtRight, "方位判断错误");
            Assert.IsTrue(puzzleAide.PositionAnalysis(170, 10, 17) == Position.UpEqRight, "方位判断错误");
            Assert.IsTrue(puzzleAide.PositionAnalysis(34, 26, 17) == Position.RightGtUp, "方位判断错误");
            Assert.IsTrue(puzzleAide.PositionAnalysis(18, 26, 17) == Position.Right, "方位判断错误");
            Assert.IsTrue(puzzleAide.PositionAnalysis(0,100,17)==Position.RightGtDown,"方位判断错误");
            Assert.IsTrue(puzzleAide.PositionAnalysis(0, 18, 17) == Position.RightEqDown, "方位判断错误");
            Assert.IsTrue(puzzleAide.PositionAnalysis(0, 172, 17) == Position.DownGtRight, "方位判断错误");
            Assert.IsTrue(puzzleAide.PositionAnalysis(17, 170, 17) == Position.Down, "方位判断错误");
            Assert.IsTrue(puzzleAide.PositionAnalysis(100, 1000, 17) == Position.DownGtLeft, "方位判断错误");
            Assert.IsTrue(puzzleAide.PositionAnalysis(32, 64, 17) == Position.DownEqLeft, "方位判断错误");
            Assert.IsTrue(puzzleAide.PositionAnalysis(32, 60, 17) == Position.LeftGtDown, "方位判断错误");
            Assert.IsTrue(puzzleAide.PositionAnalysis(16, 10, 17) == Position.Left, "方位判断错误");
            Assert.IsTrue(puzzleAide.PositionAnalysis(64, 19, 17) == Position.LeftGtUp, "方位判断错误");
            Assert.IsTrue(puzzleAide.PositionAnalysis(288, 18, 17) == Position.LeftEqUp, "方位判断错误");
            Assert.IsTrue(puzzleAide.PositionAnalysis(288,30,17) == Position.UpGtLeft, "方位判断错误");
        }
    }
}
