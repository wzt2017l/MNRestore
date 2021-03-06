﻿using System;
using System.Security.Cryptography;
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
            int i1=  puzzleAide.EmptyVerticalPlan(999010, 999, -1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(i1==10,"返回值错误");
            Assert.IsTrue(puzzle.Items[10] == 999999, "mn竖向移动命令错误");
            int i2= puzzleAide.EmptyVerticalPlan(10, 10, 1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(i2==10010,"返回值错误");
            Assert.IsTrue(puzzle.Items[10010] == 999999, "mn竖向移动命令错误");
        }

        [TestMethod]
        public void LowerEntityTransversePlanTest()
        {
            Assert.IsTrue(puzzle.Items[10009] == 10009, "下侧a_j横向移动命令错误");
            int i1= puzzleAide.LowerEntityTransversePlan(10010, 100, 1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(i1 == 10110, "返回值错误");
            Assert.IsTrue(puzzle.Items[10109] == 10009, "下侧a_j横向移动命令错误");
            Assert.IsTrue(puzzle.Items[10110] == 999999, "下侧a_j横向移动命令错误");
            Assert.IsTrue(puzzle.Items[10111] == 10111, "下侧a_j横向移动命令错误");
            int i2=puzzleAide.LowerEntityTransversePlan(10110, 50, -1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(i2 == 10060, "返回值错误");
            Assert.IsTrue(puzzle.Items[10061] == 10111, "下侧a_j横向移动命令错误");
            Assert.IsTrue(puzzle.Items[10060] == 999999, "下侧a_j横向移动命令错误");
        }


        [TestMethod]
        public void RigthEntityVerticalPlanTest()
        {
            Assert.IsTrue(puzzle.Items[9060] == 9060, "右侧a_j竖向移动命令错误");
            int i1=puzzleAide.RigthEntityVerticalPlan(10060, 100, 1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(i1 == 110060, "返回值错误");
            Assert.IsTrue(puzzle.Items[109060] == 9060, "右侧a_j竖向移动命令错误");
            Assert.IsTrue(puzzle.Items[110060] == 999999, "右侧a_j竖向移动命令错误");
            Assert.IsTrue(puzzle.Items[111060] == 111060, "右侧a_j竖向移动命令错误");
            int i2=puzzleAide.RigthEntityVerticalPlan(110060, 50, -1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(i2 == 60060, "返回值错误");
            Assert.IsTrue(puzzle.Items[61060] == 111060, "右侧a_j竖向移动命令错误");
            Assert.IsTrue(puzzle.Items[60060] == 999999, "右侧a_j竖向移动命令错误");
        }

        [TestMethod]
        public void RiseEntityTransversePlanTest()
        {
            Assert.IsTrue(puzzle.Items[60059] == 60059, "上侧a_j横向移动命令错误1");
            int i1= puzzleAide.RiseEntityTransversePlan(60060, 100, 1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(i1 == 60160, "返回值错误2");
            Assert.IsTrue(puzzle.Items[60159] == 60059, "上侧a_j横向移动命令错误3");
            Assert.IsTrue(puzzle.Items[60160] == 999999, "上侧a_j横向移动命令错误4");
            Assert.IsTrue(puzzle.Items[60161] == 60161, "上侧a_j横向移动命令错误5");
            int i2=puzzleAide.RiseEntityTransversePlan(60160, 50, -1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(i2 == 60110, "返回值错误6");
            Assert.IsTrue(puzzle.Items[60111] == 60161, "上侧a_j横向移动命令错误7");
            Assert.IsTrue(puzzle.Items[60110] == 999999, "上侧a_j横向移动命令错误8");
        }

        [TestMethod]
        public void LeftEntityVerticalPlanTest()
        {
            Assert.IsTrue(puzzle.Items[61110] == 61110, "左侧a_j竖向移动命令错误");
            int i1=puzzleAide.LeftEntityVerticalPlan(60110, 40, -1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(i1 == 20110, "返回值错误");
            Assert.IsTrue(puzzle.Items[21110] == 61110, "左侧a_j竖向移动命令错误");
            Assert.IsTrue(puzzle.Items[20110] == 999999, "左侧a_j竖向移动命令错误");
            Assert.IsTrue(puzzle.Items[19110] == 19110, "左侧a_j竖向移动命令错误");
            int i2=puzzleAide.LeftEntityVerticalPlan(20110, 100, 1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(i2 == 120110, "返回值错误");
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
            int i1=puzzleAide.EmptyVerticalPlan(999000, 998, -1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(i1 == 1000, "返回值错误1");
            Assert.IsTrue(puzzle.Items[1000] == 999999, "上侧a_j斜向移动命令错误2");
            int i2= puzzleAide.RiseEntityObliquePlan(1000, 100, 1, 1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(i2 == 101100, "返回值错误3");
            Assert.IsTrue(puzzle.Items[100100] == 0, "上侧a_j斜向移动命令错误4");
            Assert.IsTrue(puzzle.Items[101100] == 999999, "上侧a_j斜向移动命令错误5");

            Assert.IsTrue(puzzle.Items[102100] == 102100, "上侧a_j斜向移动命令错误6");
            int i3=puzzleAide.RiseEntityObliquePlan(101100, 50, -1, 1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(i3 == 51150, "返回值错误7");
            Assert.IsTrue(puzzle.Items[52150] == 102100, "上侧a_j斜向移动命令错误8");
            Assert.IsTrue(puzzle.Items[51150] == 999999, "上侧a_j斜向移动命令错误9");

            Assert.IsTrue(puzzle.Items[50150] == 50150, "上侧a_j斜向移动命令错误10");
            int i4=puzzleAide.RiseEntityObliquePlan(51150, 100, 1, -1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(i4 == 151050, "返回值错误11");
            Assert.IsTrue(puzzle.Items[150050] == 50150, "上侧a_j斜向移动命令错误12");
            Assert.IsTrue(puzzle.Items[151050] == 999999, "上侧a_j斜向移动命令错误13");

            Assert.IsTrue(puzzle.Items[152050] == 152050, "上侧a_j斜向移动命令错误14");
            int i5=puzzleAide.RiseEntityObliquePlan(151050, 40, -1, -1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(i5 == 111010, "返回值错误");
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
            int i1=puzzleAide.LateralEntityObliquePlan(999998, 200, -1, -1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(i1 == 799798, "返回值错误");
            Assert.IsTrue(puzzle.Items[799799] == 999998, "横侧a_j斜向移动命令错误");
            Assert.IsTrue(puzzle.Items[799798] == 999999, "横侧a_j斜向移动命令错误");

            Assert.IsTrue(puzzle.Items[799797] == 799797, "横侧a_j斜向移动命令错误");
            int i2=puzzleAide.LateralEntityObliquePlan(799798, 100, -1, 1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(i2 == 699898, "返回值错误");
            Assert.IsTrue(puzzle.Items[699897] == 799797, "横侧a_j斜向移动命令错误");
            Assert.IsTrue(puzzle.Items[699898] == 999999, "横侧a_j斜向移动命令错误");

            Assert.IsTrue(puzzle.Items[699899] == 699899, "横侧a_j斜向移动命令错误");
            int i3=puzzleAide.LateralEntityObliquePlan(699898, 150, 1, -1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(i3 == 849748, "返回值错误");
            Assert.IsTrue(puzzle.Items[849749] == 699899, "横侧a_j斜向移动命令错误");
            Assert.IsTrue(puzzle.Items[849748] == 999999, "横侧a_j斜向移动命令错误");

            Assert.IsTrue(puzzle.Items[849747] == 849747, "横侧a_j斜向移动命令错误");
            int i4=puzzleAide.LateralEntityObliquePlan(849748, 100, 1, 1);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(i4== 949848, "返回值错误");
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
            int i1=puzzleAide.EmptyToVt(999999,123456);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(i1 == 123456, "复合命令，移动mn错误");
            Assert.IsTrue(puzzle.Items[123456] == 999999, "复合命令，移动mn错误");

            int i2=puzzleAide.EmptyToVt(123456, 654321);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(i2 == 654321, "复合命令，移动mn错误");
            Assert.IsTrue(puzzle.Items[654321] == 999999, "复合命令，移动mn错误");

            int i3 = puzzleAide.EmptyToVt(654321, 888888);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(i3 == 888888, "复合命令，移动mn错误");
            Assert.IsTrue(puzzle.Items[888888] == 999999, "复合命令，移动mn错误");
        }
        [TestMethod]
        public void EmptyToTvTest()
        {
            int i1= puzzleAide.EmptyToVt(888888, 876543);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(i1 == 876543, "复合命令，移动mn错误");
            Assert.IsTrue(puzzle.Items[876543] == 999999, "复合命令，移动mn错误");

            int i2=puzzleAide.EmptyToVt(876543, 1001);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(i2 == 1001, "复合命令，移动mn错误");
            Assert.IsTrue(puzzle.Items[1001] == 999999, "复合命令，移动mn错误");

            int i3 = puzzleAide.EmptyToVt(1001, 666666);
            puzzleAide.ExecutePlan();
            Assert.IsTrue(i3 == 666666, "复合命令，移动mn错误");
            Assert.IsTrue(puzzle.Items[666666] == 999999, "复合命令，移动mn错误");
        }

        // i mn移动后的位置，k 目标位置图块的编号
        [TestMethod]
        public void EmptyToVtUpTest()
        {
            int i0 = puzzleAide.EmptyToVtUp(666666, 667666);
            int k0 = puzzle.Items[667666];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i0] == 999999, "生成mn到目标位置上方的命令,先竖移,错误");
            Assert.IsTrue(i0 == 666666, "生成mn到目标位置上方的命令,先竖移,错误");
            Assert.IsTrue(k0 == puzzle.Items[667666], "生成mn到目标位置上方的命令，先竖移,错误");

            int i1 = puzzleAide.EmptyToVtUp(666666,555666);
            int k1 = puzzle.Items[555666];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i1]==999999, "生成mn到目标位置上方的命令,先竖移,错误");
            Assert.IsTrue(i1==554666, "生成mn到目标位置上方的命令,先竖移,错误");
            Assert.IsTrue(k1==puzzle.Items[555666], "生成mn到目标位置上方的命令，先竖移,错误");

            int i2 = puzzleAide.EmptyToVtUp(554666, 500666,"left");
            int k2 = puzzle.Items[500666];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i2] == 999999, "生成mn到目标位置上方的命令,先竖移,错误");
            Assert.IsTrue(i2 == 499666, "生成mn到目标位置上方的命令,先竖移,错误");
            Assert.IsTrue(k2 == puzzle.Items[500666], "生成mn到目标位置上方的命令，先竖移,错误");

            int i3 = puzzleAide.EmptyToVtUp(499666, 400700);
            int k3 = puzzle.Items[400700];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i3] == 999999, "生成mn到目标位置上方的命令,先竖移,错误");
            Assert.IsTrue(i3 == 399700, "生成mn到目标位置上方的命令,先竖移,错误");
            Assert.IsTrue(k3 == puzzle.Items[400700], "生成mn到目标位置上方的命令，先竖移,错误");

            int i4 = puzzleAide.EmptyToVtUp(399700, 350749);
            int k4 = puzzle.Items[350749];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i4] == 999999, "生成mn到目标位置上方的命令,先竖移,错误");
            Assert.IsTrue(i4 == 349749, "生成mn到目标位置上方的命令,先竖移,错误");
            Assert.IsTrue(k4 == puzzle.Items[350749], "生成mn到目标位置上方的命令，先竖移,错误");

            int i5 = puzzleAide.EmptyToVtUp(349749, 300849);
            int k5 = puzzle.Items[300849];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i5] == 999999, "生成mn到目标位置上方的命令,先竖移,错误");
            Assert.IsTrue(i5 == 299849, "生成mn到目标位置上方的命令,先竖移,错误");
            Assert.IsTrue(k5 == puzzle.Items[300849], "生成mn到目标位置上方的命令，先竖移,错误");

            int i6 = puzzleAide.EmptyToVtUp(299849, 299860);
            int k6 = puzzle.Items[299860];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i6] == 999999, "生成mn到目标位置上方的命令,先竖移,错误");
            Assert.IsTrue(i6 == 298860, "生成mn到目标位置上方的命令,先竖移,错误");
            Assert.IsTrue(k6 == puzzle.Items[299860], "生成mn到目标位置上方的命令，先竖移,错误");

            int i7 = puzzleAide.EmptyToVtUp(298860, 305880);
            int k7 = puzzle.Items[305880];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i7] == 999999, "生成mn到目标位置上方的命令,先竖移,错误");
            Assert.IsTrue(i7 == 304880, "生成mn到目标位置上方的命令,先竖移,错误");
            Assert.IsTrue(k7 == puzzle.Items[305880], "生成mn到目标位置上方的命令，先竖移,错误");

            int i8 = puzzleAide.EmptyToVtUp(304880, 314890);
            int k8 = puzzle.Items[314890];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i8] == 999999, "生成mn到目标位置上方的命令,先竖移,错误");
            Assert.IsTrue(i8 == 313890, "生成mn到目标位置上方的命令,先竖移,错误");
            Assert.IsTrue(k8 == puzzle.Items[314890], "生成mn到目标位置上方的命令，先竖移,错误");

            int i9 = puzzleAide.EmptyToVtUp(313890, 400910);
            int k9 = puzzle.Items[400910];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i9] == 999999, "生成mn到目标位置上方的命令,先竖移,错误");
            Assert.IsTrue(i9 == 399910, "生成mn到目标位置上方的命令,先竖移,错误");
            Assert.IsTrue(k9 == puzzle.Items[400910], "生成mn到目标位置上方的命令，先竖移,错误");

            int i10 = puzzleAide.EmptyToVtUp(399910, 450910);
            int k10 = puzzle.Items[450910];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i10] == 999999, "生成mn到目标位置上方的命令,先竖移,错误");
            Assert.IsTrue(i10 == 449910, "生成mn到目标位置上方的命令,先竖移,错误");
            Assert.IsTrue(k10 == puzzle.Items[450910], "生成mn到目标位置上方的命令，先竖移,错误");

            int i11 = puzzleAide.EmptyToVtUp(449910, 600860);
            int k11 = puzzle.Items[600860];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i11] == 999999, "生成mn到目标位置上方的命令,先竖移,错误");
            Assert.IsTrue(i11 == 599860, "生成mn到目标位置上方的命令,先竖移,错误");
            Assert.IsTrue(k11 == puzzle.Items[600860], "生成mn到目标位置上方的命令，先竖移,错误");

            int i12 = puzzleAide.EmptyToVtUp(599860, 699760);
            int k12 = puzzle.Items[699760];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i12] == 999999, "生成mn到目标位置上方的命令,先竖移,错误");
            Assert.IsTrue(i12 == 698760, "生成mn到目标位置上方的命令,先竖移,错误");
            Assert.IsTrue(k12 == puzzle.Items[699760], "生成mn到目标位置上方的命令，先竖移,错误");

            int i13 = puzzleAide.EmptyToVtUp(698760, 598600);
            int k13 = puzzle.Items[598600];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i13] == 999999, "生成mn到目标位置上方的命令,先竖移,错误");
            Assert.IsTrue(i13 == 597600, "生成mn到目标位置上方的命令,先竖移,错误");
            Assert.IsTrue(k13 == puzzle.Items[598600], "生成mn到目标位置上方的命令，先竖移,错误");

            int i14 = puzzleAide.EmptyToVtUp(597600, 597500);
            int k14 = puzzle.Items[597500];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i14] == 999999, "生成mn到目标位置上方的命令,先竖移,错误");
            Assert.IsTrue(i14 == 596500, "生成mn到目标位置上方的命令,先竖移,错误");
            Assert.IsTrue(k14 == puzzle.Items[597500], "生成mn到目标位置上方的命令，先竖移,错误");

            int i15 = puzzleAide.EmptyToVtUp(596500, 555400);
            int k15 = puzzle.Items[555400];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i15] == 999999, "生成mn到目标位置上方的命令,先竖移,错误");
            Assert.IsTrue(i15 == 554400, "生成mn到目标位置上方的命令,先竖移,错误");
            Assert.IsTrue(k15 == puzzle.Items[555400], "生成mn到目标位置上方的命令，先竖移,错误");

            int i16 = puzzleAide.EmptyToVtUp(554400, 534380);
            int k16 = puzzle.Items[534380];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i16] == 999999, "生成mn到目标位置上方的命令,先竖移,错误");
            Assert.IsTrue(i16 == 533380, "生成mn到目标位置上方的命令,先竖移,错误");
            Assert.IsTrue(k16 == puzzle.Items[534380], "生成mn到目标位置上方的命令，先竖移,错误");

            int i17 = puzzleAide.EmptyToVtUp(533380, 421350);
            int k17 = puzzle.Items[421350];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i17] == 999999, "生成mn到目标位置上方的命令,先竖移,错误");
            Assert.IsTrue(i17 == 420350, "生成mn到目标位置上方的命令,先竖移,错误");
            Assert.IsTrue(k17 == puzzle.Items[421350], "生成mn到目标位置上方的命令，先竖移,错误");

            puzzleAide.EmptyToVt(666666);
            puzzleAide.ExecutePlan();
        }

        [TestMethod]
        public void EmptyToTvUpTest()
        {
            int i0 = puzzleAide.EmptyToTvUp(666666, 667666);
            int k0 = puzzle.Items[667666];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i0] == 999999, "生成mn到目标位置上方的命令,先横移,错误");
            Assert.IsTrue(i0 == 666666, "生成mn到目标位置上方的命令,先横移,错误");
            Assert.IsTrue(k0 == puzzle.Items[667666], "生成mn到目标位置上方的命令，先横移,错误");

            int i1 = puzzleAide.EmptyToTvUp(666666, 555666);
            int k1 = puzzle.Items[555666];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i1] == 999999, "生成mn到目标位置上方的命令,先横移,错误");
            Assert.IsTrue(i1 == 554666, "生成mn到目标位置上方的命令,先横移,错误");
            Assert.IsTrue(k1 == puzzle.Items[555666], "生成mn到目标位置上方的命令，先横移,错误");

            int i2 = puzzleAide.EmptyToTvUp(554666, 500666, "left");
            int k2 = puzzle.Items[500666];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i2] == 999999, "生成mn到目标位置上方的命令,先横移,错误");
            Assert.IsTrue(i2 == 499666, "生成mn到目标位置上方的命令,先横移,错误");
            Assert.IsTrue(k2 == puzzle.Items[500666], "生成mn到目标位置上方的命令，先横移,错误");

            int i3 = puzzleAide.EmptyToTvUp(499666, 400700);
            int k3 = puzzle.Items[400700];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i3] == 999999, "生成mn到目标位置上方的命令,先横移,错误");
            Assert.IsTrue(i3 == 399700, "生成mn到目标位置上方的命令,先横移,错误");
            Assert.IsTrue(k3 == puzzle.Items[400700], "生成mn到目标位置上方的命令，先横移,错误");

            int i4 = puzzleAide.EmptyToTvUp(399700, 350749);
            int k4 = puzzle.Items[350749];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i4] == 999999, "生成mn到目标位置上方的命令,先横移,错误");
            Assert.IsTrue(i4 == 349749, "生成mn到目标位置上方的命令,先横移,错误");
            Assert.IsTrue(k4 == puzzle.Items[350749], "生成mn到目标位置上方的命令，先横移,错误");

            int i5 = puzzleAide.EmptyToTvUp(349749, 300849);
            int k5 = puzzle.Items[300849];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i5] == 999999, "生成mn到目标位置上方的命令,先横移,错误");
            Assert.IsTrue(i5 == 299849, "生成mn到目标位置上方的命令,先横移,错误");
            Assert.IsTrue(k5 == puzzle.Items[300849], "生成mn到目标位置上方的命令，先横移,错误");

            int i6 = puzzleAide.EmptyToTvUp(299849, 299860);
            int k6 = puzzle.Items[299860];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i6] == 999999, "生成mn到目标位置上方的命令,先横移,错误1");
            Assert.IsTrue(i6 == 298860, "生成mn到目标位置上方的命令,先横移,错误2");
            Assert.IsTrue(k6 == puzzle.Items[299860], "生成mn到目标位置上方的命令，先横移,错误3");

            int i7 = puzzleAide.EmptyToTvUp(298860, 305880);
            int k7 = puzzle.Items[305880];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i7] == 999999, "生成mn到目标位置上方的命令,先横移,错误4");
            Assert.IsTrue(i7 == 304880, "生成mn到目标位置上方的命令,先横移,错误5");
            Assert.IsTrue(k7 == puzzle.Items[305880], "生成mn到目标位置上方的命令，先横移,错误6");

            int i8 = puzzleAide.EmptyToTvUp(304880, 314890);
            int k8 = puzzle.Items[314890];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i8] == 999999, "生成mn到目标位置上方的命令,先横移,错误");
            Assert.IsTrue(i8 == 313890, "生成mn到目标位置上方的命令,先横移,错误");
            Assert.IsTrue(k8 == puzzle.Items[314890], "生成mn到目标位置上方的命令，先横移,错误");

            int i9 = puzzleAide.EmptyToTvUp(313890, 400910);
            int k9 = puzzle.Items[400910];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i9] == 999999, "生成mn到目标位置上方的命令,先横移,错误");
            Assert.IsTrue(i9 == 399910, "生成mn到目标位置上方的命令,先横移,错误");
            Assert.IsTrue(k9 == puzzle.Items[400910], "生成mn到目标位置上方的命令，先横移,错误");

            int i10 = puzzleAide.EmptyToTvUp(399910, 450910);
            int k10 = puzzle.Items[450910];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i10] == 999999, "生成mn到目标位置上方的命令,先横移,错误");
            Assert.IsTrue(i10 == 449910, "生成mn到目标位置上方的命令,先横移,错误");
            Assert.IsTrue(k10 == puzzle.Items[450910], "生成mn到目标位置上方的命令，先横移,错误");

            int i11 = puzzleAide.EmptyToTvUp(449910, 600860);
            int k11 = puzzle.Items[600860];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i11] == 999999, "生成mn到目标位置上方的命令,先横移,错误");
            Assert.IsTrue(i11 == 599860, "生成mn到目标位置上方的命令,先横移,错误");
            Assert.IsTrue(k11 == puzzle.Items[600860], "生成mn到目标位置上方的命令，先横移,错误");

            int i12 = puzzleAide.EmptyToTvUp(599860, 699760);
            int k12 = puzzle.Items[699760];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i12] == 999999, "生成mn到目标位置上方的命令,先横移,错误");
            Assert.IsTrue(i12 == 698760, "生成mn到目标位置上方的命令,先横移,错误");
            Assert.IsTrue(k12 == puzzle.Items[699760], "生成mn到目标位置上方的命令，先横移,错误");

            int i13 = puzzleAide.EmptyToTvUp(698760, 598600);
            int k13 = puzzle.Items[598600];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i13] == 999999, "生成mn到目标位置上方的命令,先横移,错误");
            Assert.IsTrue(i13 == 597600, "生成mn到目标位置上方的命令,先横移,错误");
            Assert.IsTrue(k13 == puzzle.Items[598600], "生成mn到目标位置上方的命令，先横移,错误");

            int i14 = puzzleAide.EmptyToTvUp(597600, 597500);
            int k14 = puzzle.Items[597500];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i14] == 999999, "生成mn到目标位置上方的命令,先横移,错误");
            Assert.IsTrue(i14 == 596500, "生成mn到目标位置上方的命令,先横移,错误");
            Assert.IsTrue(k14 == puzzle.Items[597500], "生成mn到目标位置上方的命令，先横移,错误");

            int i15 = puzzleAide.EmptyToTvUp(596500, 555400);
            int k15 = puzzle.Items[555400];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i15] == 999999, "生成mn到目标位置上方的命令,先横移,错误");
            Assert.IsTrue(i15 == 554400, "生成mn到目标位置上方的命令,先横移,错误");
            Assert.IsTrue(k15 == puzzle.Items[555400], "生成mn到目标位置上方的命令，先横移,错误");

            int i16 = puzzleAide.EmptyToTvUp(554400, 534380);
            int k16 = puzzle.Items[534380];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i16] == 999999, "生成mn到目标位置上方的命令,先横移,错误");
            Assert.IsTrue(i16 == 533380, "生成mn到目标位置上方的命令,先横移,错误");
            Assert.IsTrue(k16 == puzzle.Items[534380], "生成mn到目标位置上方的命令，先横移,错误");

            int i17 = puzzleAide.EmptyToTvUp(533380, 421350);
            int k17 = puzzle.Items[421350];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i17] == 999999, "生成mn到目标位置上方的命令,先横移,错误");
            Assert.IsTrue(i17 == 420350, "生成mn到目标位置上方的命令,先横移,错误");
            Assert.IsTrue(k17 == puzzle.Items[421350], "生成mn到目标位置上方的命令，先横移,错误");

            puzzleAide.EmptyToTv(666666);
            puzzleAide.ExecutePlan();
        }

        [TestMethod]
        public void EmptyToVtLeftTest()
        {
            int i0 = puzzleAide.EmptyToVtLeft(666666, 666667);
            int k0 = puzzle.Items[666667];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i0] == 999999, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(i0 == 666666, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(k0 == puzzle.Items[666667], "生成mn到目标位置左方的命令，先横移,错误");

            int i1 = puzzleAide.EmptyToVtLeft(666666, 600666);
            int k1 = puzzle.Items[600666];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i1] == 999999, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(i1 == 600665, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(k1 == puzzle.Items[600666], "生成mn到目标位置左方的命令，先横移,错误");

            int i2 = puzzleAide.EmptyToVtLeft(600665, 500700);
            int k2 = puzzle.Items[500700];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i2] == 999999, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(i2 == 500699, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(k2 == puzzle.Items[500700], "生成mn到目标位置左方的命令，先横移,错误");

            int i3 = puzzleAide.EmptyToVtLeft(500699, 450749);
            int k3 = puzzle.Items[450749];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i3] == 999999, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(i3 == 450748, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(k3 == puzzle.Items[450749], "生成mn到目标位置左方的命令，先横移,错误");

            int i4 = puzzleAide.EmptyToVtLeft(450748, 400850);
            int k4 = puzzle.Items[400850];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i4] == 999999, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(i4 == 400849, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(k4 == puzzle.Items[400850], "生成mn到目标位置左方的命令，先横移,错误");

            int i5 = puzzleAide.EmptyToVtLeft(400849, 400900);
            int k5 = puzzle.Items[400900];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i5] == 999999, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(i5 == 400899, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(k5 == puzzle.Items[400900], "生成mn到目标位置左方的命令，先横移,错误");

            int i6 = puzzleAide.EmptyToVtLeft(400899, 420930);
            int k6 = puzzle.Items[420930];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i6] == 999999, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(i6 == 420929, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(k6 == puzzle.Items[420930], "生成mn到目标位置左方的命令，先横移,错误");

            int i7 = puzzleAide.EmptyToVtLeft(420929, 430939);
            int k7 = puzzle.Items[430939];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i7] == 999999, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(i7 == 430938, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(k7 == puzzle.Items[430939], "生成mn到目标位置左方的命令，先横移,错误");

            int i8 = puzzleAide.EmptyToVtLeft(430938, 500950);
            int k8 = puzzle.Items[500950];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i8] == 999999, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(i8 == 500949, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(k8 == puzzle.Items[500950], "生成mn到目标位置左方的命令，先横移,错误");

            int i9 = puzzleAide.EmptyToVtLeft(500949, 550949);
            int k9 = puzzle.Items[550949];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i9] == 999999, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(i9 == 550948, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(k9 == puzzle.Items[550949], "生成mn到目标位置左方的命令，先横移,错误");

            int i10 = puzzleAide.EmptyToVtLeft(550948, 650900);
            int k10 = puzzle.Items[650900];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i10] == 999999, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(i10 == 650899, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(k10 == puzzle.Items[650900], "生成mn到目标位置左方的命令，先横移,错误");

            int i11 = puzzleAide.EmptyToVtLeft(650899, 700849);
            int k11 = puzzle.Items[700849];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i11] == 999999, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(i11 == 700848, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(k11 == puzzle.Items[700849], "生成mn到目标位置左方的命令，先横移,错误");

            int i12 = puzzleAide.EmptyToVtLeft(700848, 750750);
            int k12 = puzzle.Items[750750];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i12] == 999999, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(i12 == 750749, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(k12 == puzzle.Items[750750], "生成mn到目标位置左方的命令，先横移,错误");

            int i13 = puzzleAide.EmptyToVtLeft(750749, 750700);
            int k13 = puzzle.Items[750700];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i13] == 999999, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(i13 == 750699, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(k13 == puzzle.Items[750700], "生成mn到目标位置左方的命令，先横移,错误");

            int i14 = puzzleAide.EmptyToVtLeft(750699, 750650);
            int k14 = puzzle.Items[750650];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i14] == 999999, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(i14 == 750649, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(k14 == puzzle.Items[750650], "生成mn到目标位置左方的命令，先横移,错误");

            int i15 = puzzleAide.EmptyToVtLeft(750649, 700550);
            int k15 = puzzle.Items[700550];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i15] == 999999, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(i15 == 700549, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(k15 == puzzle.Items[700550], "生成mn到目标位置左方的命令，先横移,错误");

            int i16 = puzzleAide.EmptyToVtLeft(700549, 600449);
            int k16 = puzzle.Items[600449];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i16] == 999999, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(i16 == 600448, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(k16 == puzzle.Items[600449], "生成mn到目标位置左方的命令，先横移,错误");

            int i17 = puzzleAide.EmptyToVtLeft(600448, 500400);
            int k17 = puzzle.Items[500400];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i17] == 999999, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(i17 == 500399, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(k17 == puzzle.Items[500400], "生成mn到目标位置左方的命令，先横移,错误");

            puzzleAide.EmptyToTv(666666);
            puzzleAide.ExecutePlan();

        }

        [TestMethod]
        public void EmptyToTvLeftTest()
        {
            int i0 = puzzleAide.EmptyToVtLeft(666666, 666667);
            int k0 = puzzle.Items[666667];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i0] == 999999, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(i0 == 666666, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(k0 == puzzle.Items[666667], "生成mn到目标位置左方的命令，先横移,错误");

            int i1 = puzzleAide.EmptyToTvLeft(666666, 600666);
            int k1 = puzzle.Items[600666];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i1] == 999999, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(i1 == 600665, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(k1 == puzzle.Items[600666], "生成mn到目标位置左方的命令，先横移,错误");

            int i2 = puzzleAide.EmptyToTvLeft(600665, 500700);
            int k2 = puzzle.Items[500700];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i2] == 999999, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(i2 == 500699, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(k2 == puzzle.Items[500700], "生成mn到目标位置左方的命令，先横移,错误");

            int i3 = puzzleAide.EmptyToTvLeft(500699, 450749);
            int k3 = puzzle.Items[450749];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i3] == 999999, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(i3 == 450748, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(k3 == puzzle.Items[450749], "生成mn到目标位置左方的命令，先横移,错误");

            int i4 = puzzleAide.EmptyToTvLeft(450748, 400850);
            int k4 = puzzle.Items[400850];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i4] == 999999, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(i4 == 400849, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(k4 == puzzle.Items[400850], "生成mn到目标位置左方的命令，先横移,错误");

            int i5 = puzzleAide.EmptyToTvLeft(400849, 400900);
            int k5 = puzzle.Items[400900];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i5] == 999999, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(i5 == 400899, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(k5 == puzzle.Items[400900], "生成mn到目标位置左方的命令，先横移,错误");

            int i6 = puzzleAide.EmptyToTvLeft(400899, 420930);
            int k6 = puzzle.Items[420930];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i6] == 999999, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(i6 == 420929, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(k6 == puzzle.Items[420930], "生成mn到目标位置左方的命令，先横移,错误");

            int i7 = puzzleAide.EmptyToTvLeft(420929, 430939);
            int k7 = puzzle.Items[430939];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i7] == 999999, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(i7 == 430938, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(k7 == puzzle.Items[430939], "生成mn到目标位置左方的命令，先横移,错误");

            int i8 = puzzleAide.EmptyToTvLeft(430938, 500950);
            int k8 = puzzle.Items[500950];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i8] == 999999, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(i8 == 500949, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(k8 == puzzle.Items[500950], "生成mn到目标位置左方的命令，先横移,错误");

            int i9 = puzzleAide.EmptyToTvLeft(500949, 550949);
            int k9 = puzzle.Items[550949];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i9] == 999999, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(i9 == 550948, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(k9 == puzzle.Items[550949], "生成mn到目标位置左方的命令，先横移,错误");

            int i10 = puzzleAide.EmptyToTvLeft(550948, 650900);
            int k10 = puzzle.Items[650900];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i10] == 999999, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(i10 == 650899, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(k10 == puzzle.Items[650900], "生成mn到目标位置左方的命令，先横移,错误");

            int i11 = puzzleAide.EmptyToTvLeft(650899, 700849);
            int k11 = puzzle.Items[700849];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i11] == 999999, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(i11 == 700848, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(k11 == puzzle.Items[700849], "生成mn到目标位置左方的命令，先横移,错误");

            int i12 = puzzleAide.EmptyToTvLeft(700848, 750750);
            int k12 = puzzle.Items[750750];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i12] == 999999, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(i12 == 750749, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(k12 == puzzle.Items[750750], "生成mn到目标位置左方的命令，先横移,错误");

            int i13 = puzzleAide.EmptyToTvLeft(750749, 750700);
            int k13 = puzzle.Items[750700];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i13] == 999999, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(i13 == 750699, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(k13 == puzzle.Items[750700], "生成mn到目标位置左方的命令，先横移,错误");

            int i14 = puzzleAide.EmptyToTvLeft(750699, 750650);
            int k14 = puzzle.Items[750650];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i14] == 999999, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(i14 == 750649, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(k14 == puzzle.Items[750650], "生成mn到目标位置左方的命令，先横移,错误");

            int i15 = puzzleAide.EmptyToTvLeft(750649, 700550);
            int k15 = puzzle.Items[700550];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i15] == 999999, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(i15 == 700549, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(k15 == puzzle.Items[700550], "生成mn到目标位置左方的命令，先横移,错误");

            int i16 = puzzleAide.EmptyToTvLeft(700549, 600449);
            int k16 = puzzle.Items[600449];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i16] == 999999, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(i16 == 600448, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(k16 == puzzle.Items[600449], "生成mn到目标位置左方的命令，先横移,错误");

            int i17 = puzzleAide.EmptyToTvLeft(600448, 500400);
            int k17 = puzzle.Items[500400];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i17] == 999999, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(i17 == 500399, "生成mn到目标位置左方的命令,先横移,错误");
            Assert.IsTrue(k17 == puzzle.Items[500400], "生成mn到目标位置左方的命令，先横移,错误");

            puzzleAide.EmptyToTv(666666);
            puzzleAide.ExecutePlan();

        }

        [TestMethod]
        public void EmptyToVtDownTest()
        {
            int i0 = puzzleAide.EmptyToVtDown(666666, 665666);
            int k0 = puzzle.Items[665666];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i0] == 999999, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(i0 == 666666, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(k0 == puzzle.Items[665666], "生成mn到目标位置下方的命令，先竖移,错误");

            int i1 = puzzleAide.EmptyToVtDown(666666, 600666);
            int k1 = puzzle.Items[600666];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i1] == 999999, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(i1 == 601666, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(k1 == puzzle.Items[600666], "生成mn到目标位置下方的命令，先竖移,错误");

            int i2 = puzzleAide.EmptyToVtDown(601666, 550700);
            int k2 = puzzle.Items[550700];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i2] == 999999, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(i2 == 551700, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(k2 == puzzle.Items[550700], "生成mn到目标位置下方的命令，先竖移,错误");

            int i3 = puzzleAide.EmptyToVtDown(551700, 500751);
            int k3 = puzzle.Items[500751];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i3] == 999999, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(i3 == 501751, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(k3 == puzzle.Items[500751], "生成mn到目标位置下方的命令，先竖移,错误");

            int i4 = puzzleAide.EmptyToVtDown(501751, 480800);
            int k4 = puzzle.Items[480800];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i4] == 999999, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(i4 == 481800, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(k4 == puzzle.Items[480800], "生成mn到目标位置下方的命令，先竖移,错误");

            int i5 = puzzleAide.EmptyToVtDown(481800, 481801);
            int k5 = puzzle.Items[481801];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i5] == 999999, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(i5 == 482801, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(k5 == puzzle.Items[481801], "生成mn到目标位置下方的命令，先竖移,错误");

            int i6 = puzzleAide.EmptyToVtDown(482801, 500850);
            int k6 = puzzle.Items[500850];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i6] == 999999, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(i6 == 501850, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(k6 == puzzle.Items[500850], "生成mn到目标位置下方的命令，先竖移,错误");

            int i7 = puzzleAide.EmptyToVtDown(501850, 551900);
            int k7 = puzzle.Items[551900];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i7] == 999999, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(i7 == 552900, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(k7 == puzzle.Items[551900], "生成mn到目标位置下方的命令，先竖移,错误");

            int i8 = puzzleAide.EmptyToVtDown(552900, 600920);
            int k8 = puzzle.Items[600920];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i8] == 999999, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(i8 == 601920, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(k8 == puzzle.Items[600920], "生成mn到目标位置下方的命令，先竖移,错误");

            int i9 = puzzleAide.EmptyToVtDown(601920, 650920);
            int k9 = puzzle.Items[650920];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i9] == 999999, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(i9 == 651920, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(k9 == puzzle.Items[650920], "生成mn到目标位置下方的命令，先竖移,错误");

            int i10 = puzzleAide.EmptyToVtDown(651920, 700920,"Left");
            int k10 = puzzle.Items[700920];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i10] == 999999, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(i10 == 701920, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(k10 == puzzle.Items[700920], "生成mn到目标位置下方的命令，先竖移,错误");

            int i11 = puzzleAide.EmptyToVtDown(701920, 750900);
            int k11 = puzzle.Items[750900];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i11] == 999999, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(i11 == 751900, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(k11 == puzzle.Items[750900], "生成mn到目标位置下方的命令，先竖移,错误");

            int i12 = puzzleAide.EmptyToVtDown(751900, 800851);
            int k12 = puzzle.Items[800851];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i12] == 999999, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(i12 == 801851, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(k12 == puzzle.Items[800851], "生成mn到目标位置下方的命令，先竖移,错误");

            int i13 = puzzleAide.EmptyToVtDown(801851, 850750);
            int k13 = puzzle.Items[850750];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i13] == 999999, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(i13 == 851750, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(k13 == puzzle.Items[850750], "生成mn到目标位置下方的命令，先竖移,错误");

            int i14 = puzzleAide.EmptyToVtDown(851750, 851700);
            int k14 = puzzle.Items[851700];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i14] == 999999, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(i14 == 852700, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(k14 == puzzle.Items[851700], "生成mn到目标位置下方的命令，先竖移,错误");

            int i15 = puzzleAide.EmptyToVtDown(852700, 820650);
            int k15 = puzzle.Items[820650];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i15] == 999999, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(i15 == 821650, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(k15 == puzzle.Items[820650], "生成mn到目标位置下方的命令，先竖移,错误");

            int i16 = puzzleAide.EmptyToVtDown(821650, 800629);
            int k16 = puzzle.Items[800629];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i16] == 999999, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(i16 == 801629, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(k16 == puzzle.Items[800629], "生成mn到目标位置下方的命令，先竖移,错误");

            int i17 = puzzleAide.EmptyToVtDown(801629, 700600);
            int k17 = puzzle.Items[700600];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i17] == 999999, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(i17 == 701600, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(k17 == puzzle.Items[700600], "生成mn到目标位置下方的命令，先竖移,错误");

            puzzleAide.EmptyToVt(666666);
            puzzleAide.ExecutePlan();
        }

        [TestMethod]
        public void EmptyToTvDownTest()
        {
            int i0 = puzzleAide.EmptyToTvDown(666666, 665666);
            int k0 = puzzle.Items[665666];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i0] == 999999, "生成mn到目标位置下方的命令,先竖移,错误1");
            Assert.IsTrue(i0 == 666666, "生成mn到目标位置下方的命令,先竖移,错误1");
            Assert.IsTrue(k0 == puzzle.Items[665666], "生成mn到目标位置下方的命令，先竖移,错误1");

            int i1 = puzzleAide.EmptyToTvDown(666666, 600666);
            int k1 = puzzle.Items[600666];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i1] == 999999, "生成mn到目标位置下方的命令,先竖移,错误2");
            Assert.IsTrue(i1 == 601666, "生成mn到目标位置下方的命令,先竖移,错误2");
            Assert.IsTrue(k1 == puzzle.Items[600666], "生成mn到目标位置下方的命令，先竖移,错误2");

            int i2 = puzzleAide.EmptyToTvDown(601666, 550700);
            int k2 = puzzle.Items[550700];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i2] == 999999, "生成mn到目标位置下方的命令,先竖移,错误3");
            Assert.IsTrue(i2 == 551700, "生成mn到目标位置下方的命令,先竖移,错误3");
            Assert.IsTrue(k2 == puzzle.Items[550700], "生成mn到目标位置下方的命令，先竖移,错误3");

            int i3 = puzzleAide.EmptyToTvDown(551700, 500751);
            int k3 = puzzle.Items[500751];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i3] == 999999, "生成mn到目标位置下方的命令,先竖移,错误4");
            Assert.IsTrue(i3 == 501751, "生成mn到目标位置下方的命令,先竖移,错误4");
            Assert.IsTrue(k3 == puzzle.Items[500751], "生成mn到目标位置下方的命令，先竖移,错误4");

            int i4 = puzzleAide.EmptyToTvDown(501751, 480800);
            int k4 = puzzle.Items[480800];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i4] == 999999, "生成mn到目标位置下方的命令,先竖移,错误5");
            Assert.IsTrue(i4 == 481800, "生成mn到目标位置下方的命令,先竖移,错误5");
            Assert.IsTrue(k4 == puzzle.Items[480800], "生成mn到目标位置下方的命令，先竖移,错误5");

            int i5 = puzzleAide.EmptyToTvDown(481800, 481801);
            int k5 = puzzle.Items[481801];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i5] == 999999, "生成mn到目标位置下方的命令,先竖移,错误6");
            Assert.IsTrue(i5 == 482801, "生成mn到目标位置下方的命令,先竖移,错误6");
            Assert.IsTrue(k5 == puzzle.Items[481801], "生成mn到目标位置下方的命令，先竖移,错误6");

            int i6 = puzzleAide.EmptyToTvDown(482801, 500850);
            int k6 = puzzle.Items[500850];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i6] == 999999, "生成mn到目标位置下方的命令,先竖移,错误7");
            Assert.IsTrue(i6 == 501850, "生成mn到目标位置下方的命令,先竖移,错误7");
            Assert.IsTrue(k6 == puzzle.Items[500850], "生成mn到目标位置下方的命令，先竖移,错误7");

            int i7 = puzzleAide.EmptyToTvDown(501850, 551900);
            int k7 = puzzle.Items[551900];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i7] == 999999, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(i7 == 552900, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(k7 == puzzle.Items[551900], "生成mn到目标位置下方的命令，先竖移,错误");

            int i8 = puzzleAide.EmptyToTvDown(552900, 600920);
            int k8 = puzzle.Items[600920];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i8] == 999999, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(i8 == 601920, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(k8 == puzzle.Items[600920], "生成mn到目标位置下方的命令，先竖移,错误");

            int i9 = puzzleAide.EmptyToTvDown(601920, 650920);
            int k9 = puzzle.Items[650920];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i9] == 999999, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(i9 == 651920, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(k9 == puzzle.Items[650920], "生成mn到目标位置下方的命令，先竖移,错误");

            int i10 = puzzleAide.EmptyToTvDown(651920, 700920, "Left");
            int k10 = puzzle.Items[700920];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i10] == 999999, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(i10 == 701920, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(k10 == puzzle.Items[700920], "生成mn到目标位置下方的命令，先竖移,错误");

            int i11 = puzzleAide.EmptyToTvDown(701920, 750900);
            int k11 = puzzle.Items[750900];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i11] == 999999, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(i11 == 751900, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(k11 == puzzle.Items[750900], "生成mn到目标位置下方的命令，先竖移,错误");

            int i12 = puzzleAide.EmptyToTvDown(751900, 800851);
            int k12 = puzzle.Items[800851];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i12] == 999999, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(i12 == 801851, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(k12 == puzzle.Items[800851], "生成mn到目标位置下方的命令，先竖移,错误");

            int i13 = puzzleAide.EmptyToTvDown(801851, 850750);
            int k13 = puzzle.Items[850750];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i13] == 999999, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(i13 == 851750, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(k13 == puzzle.Items[850750], "生成mn到目标位置下方的命令，先竖移,错误");

            int i14 = puzzleAide.EmptyToTvDown(851750, 851700);
            int k14 = puzzle.Items[851700];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i14] == 999999, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(i14 == 852700, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(k14 == puzzle.Items[851700], "生成mn到目标位置下方的命令，先竖移,错误");

            int i15 = puzzleAide.EmptyToTvDown(852700, 820650);
            int k15 = puzzle.Items[820650];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i15] == 999999, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(i15 == 821650, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(k15 == puzzle.Items[820650], "生成mn到目标位置下方的命令，先竖移,错误");

            int i16 = puzzleAide.EmptyToTvDown(821650, 800629);
            int k16 = puzzle.Items[800629];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i16] == 999999, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(i16 == 801629, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(k16 == puzzle.Items[800629], "生成mn到目标位置下方的命令，先竖移,错误");

            int i17 = puzzleAide.EmptyToTvDown(801629, 700600);
            int k17 = puzzle.Items[700600];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i17] == 999999, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(i17 == 701600, "生成mn到目标位置下方的命令,先竖移,错误");
            Assert.IsTrue(k17 == puzzle.Items[700600], "生成mn到目标位置下方的命令，先竖移,错误");

            puzzleAide.EmptyToVt(666666);
            puzzleAide.ExecutePlan();
        }

        [TestMethod]
        public void EmptyToVtRightTest()
        {
            int i0 = puzzleAide.EmptyToVtRight(666666, 666665);
            int k0 = puzzle.Items[666665];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i0] == 999999, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(i0 == 666666, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(k0 == puzzle.Items[666665], "生成mn到目标位置右方的命令，先竖移,错误");

            int i1 = puzzleAide.EmptyToVtRight(666666, 600666);
            int k1 = puzzle.Items[600666];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i1] == 999999, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(i1 == 600667, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(k1 == puzzle.Items[600666], "生成mn到目标位置右方的命令，先竖移,错误");

            int i2 = puzzleAide.EmptyToVtRight(600667, 550700);
            int k2 = puzzle.Items[550700];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i2] == 999999, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(i2 == 550701, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(k2 == puzzle.Items[550700], "生成mn到目标位置右方的命令，先竖移,错误");

            int i3 = puzzleAide.EmptyToVtRight(550701, 500751);
            int k3 = puzzle.Items[500751];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i3] == 999999, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(i3 == 500752, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(k3 == puzzle.Items[500751], "生成mn到目标位置右方的命令，先竖移,错误");

            int i4 = puzzleAide.EmptyToVtRight(500752, 480800);
            int k4 = puzzle.Items[480800];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i4] == 999999, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(i4 == 480801, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(k4 == puzzle.Items[480800], "生成mn到目标位置右方的命令，先竖移,错误");

            int i5 = puzzleAide.EmptyToVtRight(480801, 480820);
            int k5 = puzzle.Items[480820];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i5] == 999999, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(i5 == 480821, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(k5 == puzzle.Items[480820], "生成mn到目标位置右方的命令，先竖移,错误");

            int i6 = puzzleAide.EmptyToVtRight(480821, 480840);
            int k6 = puzzle.Items[480840];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i6] == 999999, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(i6 == 480841, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(k6 == puzzle.Items[480840], "生成mn到目标位置右方的命令，先竖移,错误");

            int i7 = puzzleAide.EmptyToVtRight(480841, 500900);
            int k7 = puzzle.Items[500900];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i7] == 999999, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(i7 == 500901, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(k7 == puzzle.Items[500900], "生成mn到目标位置右方的命令，先竖移,错误");

            int i8 = puzzleAide.EmptyToVtRight(500901, 550951);
            int k8 = puzzle.Items[550951];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i8] == 999999, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(i8 == 550952, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(k8 == puzzle.Items[550951], "生成mn到目标位置右方的命令，先竖移,错误");

            int i9 = puzzleAide.EmptyToVtRight(550952, 600960);
            int k9 = puzzle.Items[600960];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i9] == 999999, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(i9 == 600961, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(k9 == puzzle.Items[600960], "生成mn到目标位置右方的命令，先竖移,错误");

            int i10 = puzzleAide.EmptyToVtRight(600961, 650961);
            int k10 = puzzle.Items[650961];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i10] == 999999, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(i10 == 650962, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(k10 == puzzle.Items[650961], "生成mn到目标位置右方的命令，先竖移,错误");

            int i11 = puzzleAide.EmptyToVtRight(650962, 700950);
            int k11 = puzzle.Items[700950];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i11] == 999999, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(i11 == 700951, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(k11 == puzzle.Items[700950], "生成mn到目标位置右方的命令，先竖移,错误");

            int i12 = puzzleAide.EmptyToVtRight(700951, 750901);
            int k12 = puzzle.Items[750901];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i12] == 999999, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(i12 == 750902, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(k12 == puzzle.Items[750901], "生成mn到目标位置右方的命令，先竖移,错误");

            int i13 = puzzleAide.EmptyToVtRight(750902, 800820);
            int k13 = puzzle.Items[800820];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i13] == 999999, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(i13 == 800821, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(k13 == puzzle.Items[800820], "生成mn到目标位置右方的命令，先竖移,错误");

            int i14 = puzzleAide.EmptyToVtRight(800821, 800800);
            int k14 = puzzle.Items[800800];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i14] == 999999, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(i14 == 800801, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(k14 == puzzle.Items[800800], "生成mn到目标位置右方的命令，先竖移,错误");

            int i15 = puzzleAide.EmptyToVtRight(800801, 750700);
            int k15 = puzzle.Items[750700];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i15] == 999999, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(i15 == 750701, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(k15 == puzzle.Items[750700], "生成mn到目标位置右方的命令，先竖移,错误");

            int i16 = puzzleAide.EmptyToVtRight(750701, 700651);
            int k16 = puzzle.Items[700651];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i16] == 999999, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(i16 == 700652, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(k16 == puzzle.Items[700651], "生成mn到目标位置右方的命令，先竖移,错误");

            int i17 = puzzleAide.EmptyToVtRight(700652, 600600);
            int k17 = puzzle.Items[600600];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i17] == 999999, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(i17 == 600601, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(k17 == puzzle.Items[600600], "生成mn到目标位置右方的命令，先竖移,错误");

            puzzleAide.EmptyToVt(666666);
            puzzleAide.ExecutePlan();


        }

        [TestMethod]
        public void EmptyToTvRightTest()
        {
            int i0 = puzzleAide.EmptyToTvRight(666666, 666665);
            int k0 = puzzle.Items[666665];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i0] == 999999, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(i0 == 666666, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(k0 == puzzle.Items[666665], "生成mn到目标位置右方的命令，先竖移,错误");

            int i1 = puzzleAide.EmptyToTvRight(666666, 600666);
            int k1 = puzzle.Items[600666];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i1] == 999999, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(i1 == 600667, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(k1 == puzzle.Items[600666], "生成mn到目标位置右方的命令，先竖移,错误");

            int i2 = puzzleAide.EmptyToTvRight(600667, 550700);
            int k2 = puzzle.Items[550700];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i2] == 999999, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(i2 == 550701, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(k2 == puzzle.Items[550700], "生成mn到目标位置右方的命令，先竖移,错误");

            int i3 = puzzleAide.EmptyToTvRight(550701, 500751);
            int k3 = puzzle.Items[500751];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i3] == 999999, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(i3 == 500752, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(k3 == puzzle.Items[500751], "生成mn到目标位置右方的命令，先竖移,错误");

            int i4 = puzzleAide.EmptyToTvRight(500752, 480800);
            int k4 = puzzle.Items[480800];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i4] == 999999, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(i4 == 480801, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(k4 == puzzle.Items[480800], "生成mn到目标位置右方的命令，先竖移,错误");

            int i5 = puzzleAide.EmptyToTvRight(480801, 480820);
            int k5 = puzzle.Items[480820];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i5] == 999999, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(i5 == 480821, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(k5 == puzzle.Items[480820], "生成mn到目标位置右方的命令，先竖移,错误");

            int i6 = puzzleAide.EmptyToTvRight(480821, 480840);
            int k6 = puzzle.Items[480840];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i6] == 999999, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(i6 == 480841, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(k6 == puzzle.Items[480840], "生成mn到目标位置右方的命令，先竖移,错误");

            int i7 = puzzleAide.EmptyToTvRight(480841, 500900);
            int k7 = puzzle.Items[500900];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i7] == 999999, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(i7 == 500901, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(k7 == puzzle.Items[500900], "生成mn到目标位置右方的命令，先竖移,错误");

            int i8 = puzzleAide.EmptyToTvRight(500901, 550951);
            int k8 = puzzle.Items[550951];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i8] == 999999, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(i8 == 550952, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(k8 == puzzle.Items[550951], "生成mn到目标位置右方的命令，先竖移,错误");

            int i9 = puzzleAide.EmptyToTvRight(550952, 600960);
            int k9 = puzzle.Items[600960];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i9] == 999999, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(i9 == 600961, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(k9 == puzzle.Items[600960], "生成mn到目标位置右方的命令，先竖移,错误");

            int i10 = puzzleAide.EmptyToTvRight(600961, 650961);
            int k10 = puzzle.Items[650961];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i10] == 999999, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(i10 == 650962, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(k10 == puzzle.Items[650961], "生成mn到目标位置右方的命令，先竖移,错误");

            int i11 = puzzleAide.EmptyToTvRight(650962, 700950);
            int k11 = puzzle.Items[700950];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i11] == 999999, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(i11 == 700951, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(k11 == puzzle.Items[700950], "生成mn到目标位置右方的命令，先竖移,错误");

            int i12 = puzzleAide.EmptyToTvRight(700951, 750901);
            int k12 = puzzle.Items[750901];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i12] == 999999, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(i12 == 750902, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(k12 == puzzle.Items[750901], "生成mn到目标位置右方的命令，先竖移,错误");

            int i13 = puzzleAide.EmptyToTvRight(750902, 800820);
            int k13 = puzzle.Items[800820];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i13] == 999999, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(i13 == 800821, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(k13 == puzzle.Items[800820], "生成mn到目标位置右方的命令，先竖移,错误");

            int i14 = puzzleAide.EmptyToTvRight(800821, 800800);
            int k14 = puzzle.Items[800800];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i14] == 999999, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(i14 == 800801, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(k14 == puzzle.Items[800800], "生成mn到目标位置右方的命令，先竖移,错误");

            int i15 = puzzleAide.EmptyToTvRight(800801, 750700);
            int k15 = puzzle.Items[750700];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i15] == 999999, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(i15 == 750701, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(k15 == puzzle.Items[750700], "生成mn到目标位置右方的命令，先竖移,错误");

            int i16 = puzzleAide.EmptyToTvRight(750701, 700651);
            int k16 = puzzle.Items[700651];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i16] == 999999, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(i16 == 700652, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(k16 == puzzle.Items[700651], "生成mn到目标位置右方的命令，先竖移,错误");

            int i17 = puzzleAide.EmptyToTvRight(700652, 600600);
            int k17 = puzzle.Items[600600];
            puzzleAide.ExecutePlan();
            Assert.IsTrue(puzzle.Items[i17] == 999999, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(i17 == 600601, "生成mn到目标位置右方的命令,先竖移,错误");
            Assert.IsTrue(k17 == puzzle.Items[600600], "生成mn到目标位置右方的命令，先竖移,错误");

            puzzleAide.EmptyToVt(666666);
            puzzleAide.ExecutePlan();


        }
        #endregion

        #region 高级命令 
        [TestMethod]
        public void EntityToTest()
        {
            for (int i=0;i<100;i++)
            {

                int[] rans = new int[8];
                for (int j = 0; j < 8; j++)
                {
                    byte[] ranBytes = new byte[4];
                    RNGCryptoServiceProvider rngServiceProvider = new RNGCryptoServiceProvider();
                    rngServiceProvider.GetBytes(ranBytes);
                    rans[j] = Math.Abs(BitConverter.ToInt32(ranBytes, 0));
                }
                int entityPos = (rans[0] % 998 + 1) * 1000 + (rans[1] % 998 + 1);
                int target = (rans[2] % 998 + 1) * 1000 + (rans[3] % 998 + 1);
                bool VorT = rans[4] % 2 == 0 ? true : false;
                bool entityRDorLU = rans[5] % 2 == 0 ? true : false;
                bool mnToVorT = rans[6] % 2 == 0 ? true : false;
                bool mnToDefault = rans[7] % 2 == 0 ? true : false;
                int mnPos = puzzle.mnPosition;
                int entity = puzzle.Items[entityPos];//值
                if (mnPos!=entityPos)
                {
                    puzzleAide.EntityTo(entityPos, target, VorT, entityRDorLU, mnToVorT, mnToDefault);
                    puzzleAide.ExecutePlan();
                    string error = $"(高级命令测试错误，错误信息：i:{i},entity:{entity},entityPos:{entityPos}，target:{target},mnPos:{mnPos},VorT:{VorT},entityRDorLU:{entityRDorLU},mnToVorT:{mnToVorT},mnToDefault:{mnToDefault})";
                    Assert.IsTrue(puzzle.Items[target] == entity, error);
                }
            }
        }
        [TestMethod]
        public void EntityToTest2()
        {
            for (int i = 0; i < 400; i++)
            {
                int[] rans = new int[8];
                for (int j = 0; j < 8; j++)
                {
                    byte[] ranBytes = new byte[4];
                    RNGCryptoServiceProvider rngServiceProvider = new RNGCryptoServiceProvider();
                    rngServiceProvider.GetBytes(ranBytes);
                    rans[j] = Math.Abs(BitConverter.ToInt32(ranBytes, 0));
                }
                int entityPos = 0;
                //int target = 0;
                switch (i / 100)
                {
                    case 0: entityPos = 0 + (rans[1] % 1000); break;
                    case 1: entityPos = 999000 + (rans[1] % 1000); break;
                    case 2: entityPos = (rans[0] % 1000) * 1000 + 0; break;
                    case 3: entityPos = (rans[0] % 1000) * 1000 + 999; break;
                }
                //int entityPos = (rans[0]%999) * 1000 + (rans[1] % 999);
                int target = (rans[2] % 998 + 1) * 1000 + (rans[3] % 998 + 1);
                int mnPos = puzzle.mnPosition;
                int entity = puzzle.Items[entityPos];//值
                PuzzleAide.EntityToArgs entityToArgs = new PuzzleAide.EntityToArgs(mnPos, entityPos, target, puzzle.LieShu, puzzle.HangShu);
                if (mnPos != entityPos)
                {
                    string error0 = $"前信息：i:{i},entity:{entity},pj起始位置entityPos:{entityPos}，目标target:{target},mnPos当前:{mnPos},VorT:{entityToArgs.VorT},entityRDorLU:{entityToArgs.entityRDorLU},mnToVorT:{entityToArgs.mnToVorT},mnToDefault:{entityToArgs.mnToDefault}";
                    puzzleAide.EntityTo(entityToArgs);
                    puzzleAide.ExecutePlan();
                    int entity1 = puzzle.Items[target];
                    string error = $"信息：i:{i},entity:{entity}==目标位置的值entity1:{entity1},pj起始entityPos:{entityPos}，target:{target},mnPos当前:{puzzle.mnPosition},VorT:{entityToArgs.VorT},entityRDorLU:{entityToArgs.entityRDorLU},mnToVorT:{entityToArgs.mnToVorT},mnToDefault:{entityToArgs.mnToDefault}";
                    Assert.IsTrue(puzzle.Items[target] == entity, error);
                    
                }
            }
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

        [TestMethod]
        public void DisruptReducibleTest()
        {
            puzzle = new Puzzle(100,100);
            puzzleAide = new PuzzleAide(puzzle);
            long nixu=puzzleAide.DisruptReducible();
            Assert.IsTrue(nixu==puzzle.RetryNiXu(puzzle.Items),"打乱拼图错误");
            Assert.IsTrue((nixu+(puzzle.mnPosition%100)+(puzzle.mnPosition-puzzle.mnPosition%100)/100)%2==0,"无法复原");
        }


    }
}
