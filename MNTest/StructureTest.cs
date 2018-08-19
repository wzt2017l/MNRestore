using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MNPuzzle;

namespace MNTest
{
    [TestClass]
    public class StructureTest
    {
        [TestMethod]
        public void Structure0Test()
        {
            Structure structure = new Structure(1001,0,1,1000,1000);
            for (int i=0;i<3;i++)
            {
                Assert.IsTrue(structure.matrix[i, 0].posNoType == Structure.PosNoType.Invalid, "构造错误");
            }
            Assert.IsTrue(structure.matrix[0, 2].posNoType == Structure.PosNoType.Target,"构造错误");
            Assert.IsTrue(structure.matrix[0, 1].posNoType == Structure.PosNoType.EntityPos, "构造错误");
            Assert.IsTrue(structure.NodeGrid[10].swap.Equals(new Swap(999,1999)), "构造错误");
            Assert.IsTrue(structure.NodeGrid[8].swap.Equals(new Swap(1000,1001)),"构造错误");
            Assert.IsTrue(structure.NodeGrid[8].nearNodes.Count==5,"构造错误");
            Assert.IsTrue(structure.TargetPosNo.Pos==1,"构造错误");
            Structure structure1 = new Structure(101101,101102,100103,1000,1000,100100);
        }
    }
}
