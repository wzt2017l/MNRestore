using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNPuzzle
{
    /// <summary>
    /// 3*4结构图
    /// </summary>
    public class Structure
    {
        /// <summary>
        /// 大小为3*4局部位置编号
        /// </summary>
        public PosNo[,] matrix = new PosNo[3, 4];
        /// <summary>
        /// 图
        /// </summary>
        public Node[] NodeGrid = new Node[17];
        /// <summary>
        /// 构造函数，初始化结构,以目标为参照点
        /// 专门为从上至下按行复原设计
        /// </summary>
        public Structure(int mnPos,int entityPos,int target,int hangShu,int lieShu)
        {
            for (int i=0;i<3;i++)
            {
                for (int j=0;j<4;j++)
                {
                    int pos = target - 2 + j + i * lieShu;
                    PosNoType pt;
                    if (pos==mnPos)
                    {
                        pt = PosNoType.mnPos;
                    }
                    else if (pos==entityPos)
                    {
                        pt = PosNoType.EntityPos;
                    }
                    else if (pos==target)
                    {
                        pt = PosNoType.Target;
                    }
                    else if (pos<target||pos>=hangShu*lieShu|| (pos-pos%lieShu)/lieShu!=(target-target%lieShu)/lieShu+i)
                    {
                        pt = PosNoType.Invalid;
                    }
                    else
                    {
                        pt = PosNoType.Else;
                    }
                    matrix[i, j] = new PosNo(pos,pt);
                }
            }
            for (int i=0;i<5;i++)
            {
                int count=0;int index = 0;
                switch (i%2)
                {
                    case 0:
                        count = 3;
                        index =i/2*7;
                        break;
                    case 1:
                        count = 4;
                        index = 3 + i / 2 * 7;
                        break;
                }
                for (int j=0;j<count;j++)
                {
                    PosNo[] pns = new PosNo[2];
                    switch (count)
                    {
                        case 3:
                            pns[0] = matrix[i / 2, j];
                            pns[1] = matrix[i / 2, j+1];
                            break;
                        case 4:
                            pns[0] = matrix[i / 2, j];
                            pns[1] = matrix[i / 2 + 1, j];
                            break;
                    }
                    NodeGrid[index + j] = new Node(pns);
                }
            }
            for (int i=0;i<17;i++)
            {
                if (!NodeGrid[i].Valid)continue;
                for (int j=0;j<17;j++)
                {
                    if (i != j && NodeGrid[j].Valid && Swap.IsIntersect(NodeGrid[i].swap, NodeGrid[j].swap))
                    {
                        NodeGrid[i].AddnearNode(NodeGrid[j]);
                    }      
                }
            }
        }
        /// <summary>
        /// 根据现有条件搜索路径
        /// </summary>
        /// <returns>命令队列</returns>
        public Queue<Swap> SearchPath()
        {
            return null;
        }

        #region 辅助实体
        /// <summary>
        /// 交换节点
        /// </summary>
        public class Node
        {
            public Swap swap { get; set; }
            public PosNo[] PosNos;
            public bool Valid { get; set; }
            public List<Node> nearNodes;
            public Node(PosNo[] posNos)
            {
                this.PosNos = posNos;
                swap = new Swap(posNos[0].Pos, posNos[1].Pos);
                if (posNos[0].posNoType == PosNoType.Invalid || posNos[1].posNoType == PosNoType.Invalid)
                    Valid = false;
                else Valid = true;
                nearNodes = new List<Node>();
            }
            public void AddnearNode(Node node)
            {
                nearNodes.Add(node);
            }
        }
        /// <summary>
        /// 位置编号
        /// </summary>
        public class PosNo
        {
            /// <summary>
            /// 位置
            /// </summary>
            public int Pos { get; set; }
            /// <summary>
            /// 类型
            /// </summary>
            public PosNoType posNoType { get; set; }
            public PosNo(int pos, PosNoType posNoType)
            {
                Pos = pos;
                this.posNoType = posNoType;
            }

        }
        /// <summary>
        /// 位置类型
        /// </summary>
        public enum PosNoType
        {
            Invalid = 0,//无效位置
            Target,//目标
            mnPos,//mn所在
            EntityPos,//当前要移动的图块
            Else//其它有效位置
        } 
        #endregion
    }

}
