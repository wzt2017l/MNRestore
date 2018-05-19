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

        #region 基础命令测试

        //#region 联合测试，这部分必须按顺序运行
        //[TestMethod]
        //public void EmptyTransversePlanTest()
        //{
        //    puzzle.EmptyTransversePlan(999999, 999, -1);
        //    puzzle.ExecutePlan();
        //    Assert.IsTrue(puzzle.Items[999000] == 999999, "mn横向移动命令错误");
        //    puzzle.EmptyTransversePlan(999000, 10, 1);
        //    puzzle.ExecutePlan();
        //    Assert.IsTrue(puzzle.Items[999010] == 999999, "mn横向移动命令错误");
        //}

        //[TestMethod]
        //public void EmptyVerticalPlanTest()
        //{
        //    puzzle.EmptyVerticalPlan(999010, 999, -1);
        //    puzzle.ExecutePlan();
        //    Assert.IsTrue(puzzle.Items[10] == 999999, "mn竖向移动命令错误");
        //    puzzle.EmptyVerticalPlan(10, 10, 1);
        //    puzzle.ExecutePlan();
        //    Assert.IsTrue(puzzle.Items[10010] == 999999, "mn竖向移动命令错误");
        //}

        //[TestMethod]
        //public void LowerEntityTransversePlanTest()
        //{
        //    Assert.IsTrue(puzzle.Items[10009] == 10009, "下侧a_j横向移动命令错误");
        //    puzzle.LowerEntityTransversePlan(10010, 100, 1);
        //    puzzle.ExecutePlan();
        //    Assert.IsTrue(puzzle.Items[10109] == 10009, "下侧a_j横向移动命令错误");
        //    Assert.IsTrue(puzzle.Items[10110] == 999999, "下侧a_j横向移动命令错误");
        //    Assert.IsTrue(puzzle.Items[10111] == 10111, "下侧a_j横向移动命令错误");
        //    puzzle.LowerEntityTransversePlan(10110, 50, -1);
        //    puzzle.ExecutePlan();
        //    Assert.IsTrue(puzzle.Items[10061] == 10111, "下侧a_j横向移动命令错误");
        //    Assert.IsTrue(puzzle.Items[10060] == 999999, "下侧a_j横向移动命令错误");
        //}


        //[TestMethod]
        //public void RigthEntityVerticalPlanTest()
        //{
        //    Assert.IsTrue(puzzle.Items[9060] == 9060, "右侧a_j竖向移动命令错误");
        //    puzzle.RigthEntityVerticalPlan(10060, 100, 1);
        //    puzzle.ExecutePlan();
        //    Assert.IsTrue(puzzle.Items[109060] == 9060, "右侧a_j竖向移动命令错误");
        //    Assert.IsTrue(puzzle.Items[110060] == 999999, "右侧a_j竖向移动命令错误");
        //    Assert.IsTrue(puzzle.Items[111060] == 111060, "右侧a_j竖向移动命令错误");
        //    puzzle.RigthEntityVerticalPlan(110060, 50, -1);
        //    puzzle.ExecutePlan();
        //    Assert.IsTrue(puzzle.Items[61060] == 111060, "右侧a_j竖向移动命令错误");
        //    Assert.IsTrue(puzzle.Items[60060] == 999999, "右侧a_j竖向移动命令错误");
        //}

        //[TestMethod]
        //public void RiseEntityTransversePlanTest()
        //{
        //    Assert.IsTrue(puzzle.Items[60059] == 60059, "上侧a_j横向移动命令错误");
        //    puzzle.RiseEntityTransversePlan(60060, 100, 1);
        //    puzzle.ExecutePlan();
        //    Assert.IsTrue(puzzle.Items[60159] == 60059, "上侧a_j横向移动命令错误");
        //    Assert.IsTrue(puzzle.Items[60160] == 999999, "上侧a_j横向移动命令错误");
        //    Assert.IsTrue(puzzle.Items[60161] == 60161, "上侧a_j横向移动命令错误");
        //    puzzle.RiseEntityTransversePlan(60160, 50, -1);
        //    puzzle.ExecutePlan();
        //    Assert.IsTrue(puzzle.Items[60111] == 60161, "上侧a_j横向移动命令错误");
        //    Assert.IsTrue(puzzle.Items[60110] == 999999, "上侧a_j横向移动命令错误");
        //}

        //[TestMethod]
        //public void LeftEntityVerticalPlanTest()
        //{
        //    Assert.IsTrue(puzzle.Items[61110] == 61110, "左侧a_j竖向移动命令错误");
        //    puzzle.LeftEntityVerticalPlan(60110, 40, -1);
        //    puzzle.ExecutePlan();
        //    Assert.IsTrue(puzzle.Items[21110] == 61110, "左侧a_j竖向移动命令错误");
        //    Assert.IsTrue(puzzle.Items[20110] == 999999, "左侧a_j竖向移动命令错误");
        //    Assert.IsTrue(puzzle.Items[19110] == 19110, "左侧a_j竖向移动命令错误");
        //    puzzle.LeftEntityVerticalPlan(20110, 100, 1);
        //    puzzle.ExecutePlan();
        //    Assert.IsTrue(puzzle.Items[119110] == 19110, "左侧a_j竖向移动命令错误");
        //    Assert.IsTrue(puzzle.Items[120110] == 999999, "左侧a_j竖向移动命令错误");
        //}
        //#endregion

        //[TestMethod]
        //public void RiseEntityObliquePlanTest()
        //{
        //    puzzle = new Puzzle(1000, 1000);
        //    puzzle.EmptyTransversePlan(999999, 999, -1);
        //    puzzle.EmptyVerticalPlan(999000, 998, -1);
        //    puzzle.ExecutePlan();
        //    Assert.IsTrue(puzzle.Items[1000] == 999999, "上侧a_j斜向移动命令错误");
        //    puzzle.RiseEntityObliquePlan(1000, 100, 1, 1);
        //    puzzle.ExecutePlan();
        //    Assert.IsTrue(puzzle.Items[100100] == 0, "上侧a_j斜向移动命令错误");
        //    Assert.IsTrue(puzzle.Items[101100] == 999999, "上侧a_j斜向移动命令错误");

        //    Assert.IsTrue(puzzle.Items[102100] == 102100, "上侧a_j斜向移动命令错误");
        //    puzzle.RiseEntityObliquePlan(101100, 50, -1, 1);
        //    puzzle.ExecutePlan();
        //    Assert.IsTrue(puzzle.Items[52150] == 102100, "上侧a_j斜向移动命令错误");
        //    Assert.IsTrue(puzzle.Items[51150] == 999999, "上侧a_j斜向移动命令错误");

        //    Assert.IsTrue(puzzle.Items[50150] == 50150, "上侧a_j斜向移动命令错误");
        //    puzzle.RiseEntityObliquePlan(51150, 100, 1, -1);
        //    puzzle.ExecutePlan();
        //    Assert.IsTrue(puzzle.Items[150050] == 50150, "上侧a_j斜向移动命令错误");
        //    Assert.IsTrue(puzzle.Items[151050] == 999999, "上侧a_j斜向移动命令错误");

        //    Assert.IsTrue(puzzle.Items[152050] == 152050, "上侧a_j斜向移动命令错误");
        //    puzzle.RiseEntityObliquePlan(151050, 40, -1, -1);
        //    puzzle.ExecutePlan();
        //    Assert.IsTrue(puzzle.Items[112010] == 152050, "上侧a_j斜向移动命令错误");
        //    Assert.IsTrue(puzzle.Items[111010] == 999999, "上侧a_j斜向移动命令错误");
        //}

        //[TestMethod]
        //public void LateralEntityObliquePlanTest()
        //{
        //    puzzle = new Puzzle(1000, 1000);
        //    puzzle.EmptyTransversePlan(999999, 1, -1);
        //    puzzle.ExecutePlan();
        //    Assert.IsTrue(puzzle.Items[999998] == 999999, "横侧a_j斜向移动命令错误");
        //    puzzle.LateralEntityObliquePlan(999998, 200, -1, -1);
        //    puzzle.ExecutePlan();
        //    Assert.IsTrue(puzzle.Items[799799] == 999998, "横侧a_j斜向移动命令错误");
        //    Assert.IsTrue(puzzle.Items[799798] == 999999, "横侧a_j斜向移动命令错误");

        //    Assert.IsTrue(puzzle.Items[799797] == 799797, "横侧a_j斜向移动命令错误");
        //    puzzle.LateralEntityObliquePlan(799798, 100, -1, 1);
        //    puzzle.ExecutePlan();
        //    Assert.IsTrue(puzzle.Items[699897] == 799797, "横侧a_j斜向移动命令错误");
        //    Assert.IsTrue(puzzle.Items[699898] == 999999, "横侧a_j斜向移动命令错误");

        //    Assert.IsTrue(puzzle.Items[699899] == 699899, "横侧a_j斜向移动命令错误");
        //    puzzle.LateralEntityObliquePlan(699898, 150, 1, -1);
        //    puzzle.ExecutePlan();
        //    Assert.IsTrue(puzzle.Items[849749] == 699899, "横侧a_j斜向移动命令错误");
        //    Assert.IsTrue(puzzle.Items[849748] == 999999, "横侧a_j斜向移动命令错误");

        //    Assert.IsTrue(puzzle.Items[849747] == 849747, "横侧a_j斜向移动命令错误");
        //    puzzle.LateralEntityObliquePlan(849748, 100, 1, 1);
        //    puzzle.ExecutePlan();
        //    Assert.IsTrue(puzzle.Items[949847] == 849747, "横侧a_j斜向移动命令错误");
        //    Assert.IsTrue(puzzle.Items[949848] == 999999, "横侧a_j斜向移动命令错误");
        //} 
        #endregion

        //[TestMethod]
        //public void RetryInversionNumberTest()
        //{
        //    Puzzle p = new Puzzle(100,100);
        //    p.Disrupt();
        //    long inv1 = 0;int len = p.Items.Length;
        //    for (int i=1;i<len;i++)
        //    {
        //        for (int j=0;j<i;j++)
        //        {
        //            if (p.Items[i]<p.Items[j])
        //            {
        //                inv1++;
        //            }
        //        }
        //    }
        //   long inv2 = p.RetryNiXu();
        //   Assert.IsTrue(inv1==inv2,"逆序数计算错误");
        //}
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
