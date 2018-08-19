using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNPuzzle
{
    /// <summary>
    /// 3*4结构图
    /// 帮助puzzleAide完成复原拼图时，求解某块最后几步的步骤，若单独使用它复原较小的拼图，请重写SearchPath方法
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
        /// mn位置信息
        /// </summary>
        public PosNo mnPosNo { get; private set; }
        /// <summary>
        /// 当前要移动图块信息
        /// </summary>
        public PosNo EntityPosNo { get; private set; }
        /// <summary>
        /// 目标信息
        /// </summary>
        public PosNo TargetPosNo { get; private set; }
        public int hangShu { get; private set; }
        public int lieShu { get; private set; }

        /// <summary>
        /// 构造函数，初始化结构,以目标为参照点
        /// 专门为从上至下按行复原设计
        /// invalidNo:指出有效编号的起始位置，如果不指定则invalid的值==target
        /// </summary>
        public Structure(int mnPos,int entityPos,int target,int hangShu,int lieShu,int invalidNo=0)
        {
            if (invalidNo == 0) invalidNo = target;
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
                    else if (pos<invalidNo||pos>=hangShu*lieShu|| (pos-pos%lieShu)/lieShu!=(target-target%lieShu)/lieShu+i)
                    {
                        pt = PosNoType.Invalid;
                    }
                    else
                    {
                        pt = PosNoType.Else;
                    }
                    matrix[i, j] = new PosNo(pos,pt,lieShu);
                    switch (pt)
                    {
                        case PosNoType.mnPos:mnPosNo = matrix[i, j]; break;
                        case PosNoType.EntityPos: EntityPosNo= matrix[i, j]; break;
                        case PosNoType.Target: TargetPosNo = matrix[i, j]; break;
                    }
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
            this.hangShu = hangShu;
            this.lieShu = lieShu;
        }
        /// <summary>
        /// 根据现有条件搜索路径
        /// 先求出起始交换
        /// </summary>
        /// <returns>命令队列</returns>
        public Queue<Swap> SearchPath()
        {
            Queue<Swap> comm = new Queue<Swap>();
            IEnumerable<Node> beginNodes = from f in NodeGrid where f.Valid==true&&(f.swap.Empty == mnPosNo.Pos || f.swap.Entity == mnPosNo.Pos )select f;
            Node indexNode = null, beginNode = null ;//当前交换，上一步交换
            PosNo mnIndexNo = mnPosNo, mnNextNo = null;//mn当前位置，mn执行完当前交换后的位置
            PosNo entityIndexNo = EntityPosNo, entityNextNo = EntityPosNo;//entity当前位置，entity执行完当前交换后的位置
            int distance = Math.Abs(entityIndexNo.hangNo-TargetPosNo.hangNo) + Math.Abs(entityIndexNo.lieNo-TargetPosNo.lieNo);//到达目标的距离
            switch (distance)
            {
                case 2:
                    foreach (Node node in beginNodes)
                    {
                        PosNo next = node.PosNos[0].posNoType == PosNoType.mnPos ? node.PosNos[1] : node.PosNos[0];//mn要到达的位置
                        if (next.posNoType != PosNoType.EntityPos && Math.Abs(next.lieNo - EntityPosNo.lieNo) < 2 && Math.Abs(next.hangNo - EntityPosNo.hangNo) < 2)
                        {
                            indexNode = node;
                            comm.Enqueue(node.swap);
                            mnNextNo = node.PosNos[0].Pos == mnIndexNo.Pos ? node.PosNos[1] : node.PosNos[0];
                        }
                    }
                    break;
                case 1:break;
            }
            
            //必须分两个步骤进行：在distance=2时和distance=1时，当distance=2时entityPos=target+lieShu-1,mnPos=entityPos-1
            while (entityNextNo.posNoType!=PosNoType.Target)//假定执行完当前交换后
            {
                mnIndexNo = mnNextNo;
                entityIndexNo = entityNextNo;
                IEnumerable<Node> tempNodes = from f in indexNode.nearNodes where f.swap.Empty == mnIndexNo.Pos || f.swap.Entity == mnIndexNo.Pos select f;//下一步可能的交换
                switch (distance)
                {
                    case 2:
                        foreach (Node node in tempNodes)//可能的下一步
                        {
                            PosNo tempmnNextPos = node.PosNos[0].Pos == mnIndexNo.Pos ? node.PosNos[1] : node.PosNos[1];//假设为node，则执行完node后mn在的位置
                            if (tempmnNextPos.Pos == entityIndexNo.Pos)//如果mn与entity交换，则entity会到达位置mnIndexNo
                            {
                                int tempdis = Math.Abs(mnIndexNo.hangNo - TargetPosNo.hangNo) + Math.Abs(mnIndexNo.lieNo - TargetPosNo.lieNo);//entity是否离目标更近
                                if (tempdis < distance)
                                {
                                    beginNode = indexNode;
                                    indexNode = node;
                                    mnNextNo = tempmnNextPos;
                                    entityNextNo = mnIndexNo;
                                    comm.Enqueue(node.swap);
                                    distance = tempdis;
                                    break;//结束当前循环
                                }
                            }
                            else//mn与不是entity的位置的图块交换？
                            {
                                //if (beginNode == null)
                                //{
                                    if (Math.Abs(tempmnNextPos.hangNo - entityIndexNo.hangNo) + Math.Abs(tempmnNextPos.lieNo - entityIndexNo.lieNo) > 2) continue;
                                    else//下一步距离不大于2则选中
                                    {
                                        beginNode = indexNode;
                                        indexNode = node;
                                        mnNextNo = tempmnNextPos;
                                        entityNextNo = mnIndexNo;
                                        comm.Enqueue(node.swap);
                                        break;//结束当前循环
                                    }
                                //}
                                //else//如果有上一步
                                //{

                                //}
                            }
                        }
                        break;
                    case 1:

                        break;
                }
               
            }
            return comm;
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
            public int Pos { get; private set; }
            /// <summary>
            /// 行编号
            /// </summary>
            public int hangNo { get;private set; }
            /// <summary>
            /// 列编号
            /// </summary>
            public int lieNo { get; private set; }
            /// <summary>
            /// 类型
            /// </summary>
            public PosNoType posNoType { get;private set; }
            public PosNo(int pos, PosNoType posNoType,int lieShu)
            {
                Pos = pos;
                this.posNoType = posNoType;
                lieNo = pos % lieShu;
                hangNo = (pos - lieNo) / lieShu;
            }

        }
        /// <summary>
        /// 位置类型
        /// </summary>
        public enum PosNoType
        {
            /// <summary>
            /// 无效位置
            /// </summary>
            Invalid = 0,
            /// <summary>
            /// 目标
            /// </summary>
            Target,
            /// <summary>
            /// mn所在
            /// </summary>
            mnPos,
            /// <summary>
            /// 当前要移动的图块
            /// </summary>
            EntityPos,
            /// <summary>
            /// 其它有效位置
            /// </summary>
            Else
        } 
        #endregion
    }

}
