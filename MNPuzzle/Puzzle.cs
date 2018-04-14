using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNPuzzle
{
    public enum PuzzleState { Original = 0, Confusion = 1 }//0复原状态，1混乱状态
    public class Puzzle
    {
        #region 属性
        public int HangShu { get; private set; }//行数
        public int LieShu { get; private set; }//列数
        public int Total { get; private set; }//拼图总块数
        public int mnPosition { get; set; }//mn当前位置
        public int[] PuzzleArray { get; private set; }//拼图数组
        public int InversionNumber { get; set; }//数组逆序数
        public PuzzleState State { get; set; }//状态
        public Stack<Swap> Command { get; set; }//交换命令栈 
        #endregion

        #region 构造函数
        public Puzzle(int hangshu ,int lieshu)
        {
            HangShu = hangshu;
            LieShu = lieshu;
            Total = hangshu * lieshu;
            mnPosition = Total - 1;
            PuzzleArray = new int[Total];
            for (int i = 0; i <Total; i++)
            {
                PuzzleArray[i] = i;
            }
            InversionNumber = 0;
            State = PuzzleState.Original;
            Command = new Stack<Swap>();
        }
        #endregion

        #region 交换位置
        /// <summary>
        /// 交换
        /// </summary>
        /// <param name="empty">空格所在的位置</param>
        /// <param name="entity">要与之交换的拼图块的位置</param>
        public void SwapAction(int empty,int entity)
        {
            InversionNumber = InversionNumber + InversionNumberDifference(entity, empty);
            int t = this.PuzzleArray[empty];
            PuzzleArray[empty] = PuzzleArray[entity];
            PuzzleArray[entity] = t;
            mnPosition = entity;
            switch (InversionNumber)
            {
                case 0: State = PuzzleState.Original; break;
                default: State = PuzzleState.Confusion; break;
            }

        }
        public void SwapAction(Swap swap)
        {
            this.SwapAction(swap.Empty,swap.Entity);
        }
        #endregion
        #region 逆序数差值
        /// <summary>
        /// 交换后逆序数的差值
        /// </summary>
        /// <param name="i">PuzzleArray[i]</param>
        /// <param name="j">PuzzleArray[j]</param>
        /// <returns></returns>
        private int InversionNumberDifference(int i, int j)
        {
            if (i > j)
            {
                int t = i;
                i = j;
                j = t;
            }
            int count = j - i;
            int difference = 0;
            for (int k = 1; k < count; k++)
            {
                if (PuzzleArray[i] < PuzzleArray[i + k])
                {
                    difference++;
                }
                if (PuzzleArray[j] < PuzzleArray[i + k])
                {
                    difference--;
                }
            }
            if (PuzzleArray[j] > PuzzleArray[i])
            {
                difference = 2 * difference + 1;
            }
            else
            {
                difference = 2 * difference - 1;
            }
            return difference;
        }
        #endregion

        #region 基础命令
        #region mn竖向移动命令
        /// <summary>
        /// 生成竖向移动命令
        /// </summary>
        /// <param name="mnPosition">mn的位置</param>
        /// <param name="Offset">移动格数</param>
        /// <param name="direction">方向1或-1</param>
        public void EmptyVerticalPlan( int Offset, int direction)
        {
            for (; Offset > 0; Offset--)
            {
                Swap swap = new Swap(mnPosition + (Offset - 1) * LieShu * direction, mnPosition + Offset * LieShu * direction);
                Command.Push(swap);
            }
        }
        #endregion
        #region mn横向移动命令
        /// <summary>
        /// 生成横向移动命令
        /// </summary>
        /// <param name="mnPosition">mn的坐标</param>
        /// <param name="Offset">移动格数</param>
        /// <param name="direction">方向1或-1</param>
        public void EmptyTransversePlan( int Offset, int direction)
        {
            for (; Offset > 0; Offset--)
            {
                Swap swap = new Swap(mnPosition + (Offset - 1) * direction, mnPosition + Offset  * direction);
                Command.Push(swap);
            }
        }
        #endregion
        #region 右侧a_j竖向移动命令
        /// <summary>
        /// 生成a_j竖向移动命令,mn右侧一列回到相对位置
        /// </summary>
        /// <param name="mnPosition">mn的坐标</param>
        /// <param name="Offset">移动格数</param>
        /// <param name="direction">方向1或-1</param>
        public void RigthEntityVerticalPlan(int Offset, int direction)
        {
            for (; Offset > 0; Offset--)
            {
                Swap swap1 = new Swap(mnPosition + Offset*LieShu * direction+1, mnPosition + Offset *LieShu* direction);
                Command.Push(swap1);
                Swap swap2 = new Swap(mnPosition + (Offset - 1)*LieShu * direction+1, mnPosition + Offset * LieShu * direction + 1);
                Command.Push(swap2);
                Swap swap3 = new Swap(mnPosition + (Offset - 2) *LieShu* direction+1, mnPosition + (Offset - 1) * LieShu * direction + 1);
                Command.Push(swap3);
                Swap swap4 = new Swap(mnPosition + (Offset - 2) *LieShu* direction, mnPosition + (Offset - 2) * LieShu * direction + 1);
                Command.Push(swap4);
                Swap swap5 = new Swap(mnPosition + (Offset - 1)*LieShu * direction, mnPosition + (Offset-2)*LieShu * direction);
                Command.Push(swap5);
            }
        }
        #endregion
        #region 左侧a_j竖向移动命令
        /// <summary>
        /// 生成a_j竖向移动命令,mn左侧一列回到相对位置
        /// </summary>
        /// <param name="mnPosition">mn的坐标</param>
        /// <param name="Offset">移动格数</param>
        /// <param name="direction">方向1或-1</param>
        public void LeftEntityVerticalPlan(int Offset, int direction)
        {
            for (; Offset > 0; Offset--)
            {
                Swap swap1 = new Swap(mnPosition + Offset * LieShu * direction -1, mnPosition + Offset * LieShu * direction);
                Command.Push(swap1);
                Swap swap2 = new Swap(mnPosition + (Offset - 1) * LieShu * direction - 1, mnPosition + Offset * LieShu * direction - 1);
                Command.Push(swap2);
                Swap swap3 = new Swap(mnPosition + (Offset - 2) * LieShu * direction - 1, mnPosition + (Offset - 1) * LieShu * direction - 1);
                Command.Push(swap3);
                Swap swap4 = new Swap(mnPosition + (Offset - 2) * LieShu * direction, mnPosition + (Offset - 2) * LieShu * direction - 1);
                Command.Push(swap4);
                Swap swap5 = new Swap(mnPosition + (Offset - 1) * LieShu * direction, mnPosition + (Offset - 2) * LieShu * direction);
                Command.Push(swap5);
            }
        }
        #endregion
        #region 下侧a_j横向移动命令
        /// <summary>
        /// 生成a_j横向移动命令,mn下侧一行回到相对位置
        /// </summary>
        /// <param name="mnPosition">mn的坐标</param>
        /// <param name="Offset">移动格数</param>
        /// <param name="direction">方向1或-1</param>
        public void LowerEntityTransversePlan(int Offset, int direction)
        {
            for (; Offset > 0; Offset--)
            {
                Swap swap1 = new Swap(mnPosition + Offset * direction +LieShu, mnPosition + Offset * direction);
                Command.Push(swap1);
                Swap swap2 = new Swap(mnPosition + (Offset - 1) * direction +LieShu, mnPosition + Offset * direction +LieShu);
                Command.Push(swap2);
                Swap swap3 = new Swap(mnPosition + (Offset - 2) * direction +LieShu, mnPosition + (Offset - 1) * direction + LieShu);
                Command.Push(swap3);
                Swap swap4 = new Swap(mnPosition + (Offset - 2) * direction, mnPosition + (Offset - 2) * direction +LieShu);
                Command.Push(swap4);
                Swap swap5 = new Swap(mnPosition + (Offset - 1) * direction, mnPosition + (Offset - 2) * direction);
                Command.Push(swap5);
            }
        }
        #endregion
        #region 上侧a_j横向移动命令
        /// <summary>
        /// 生成a_j横向移动命令,mn上侧一行回到相对位置
        /// </summary>
        /// <param name="mnPosition">mn的坐标</param>
        /// <param name="Offset">移动格数</param>
        /// <param name="direction">方向1或-1</param>
        public void RiseEntityTransversePlan(int Offset, int direction)
        {
            for (; Offset > 0; Offset--)
            {
                Swap swap1 = new Swap(mnPosition + Offset * direction - LieShu, mnPosition + Offset * direction);
                Command.Push(swap1);
                Swap swap2 = new Swap(mnPosition + (Offset - 1) * direction - LieShu, mnPosition + Offset * direction - LieShu);
                Command.Push(swap2);
                Swap swap3 = new Swap(mnPosition + (Offset - 2) * direction - LieShu, mnPosition + (Offset - 1) * direction - LieShu);
                Command.Push(swap3);
                Swap swap4 = new Swap(mnPosition + (Offset - 2) * direction, mnPosition + (Offset - 2) * direction - LieShu);
                Command.Push(swap4);
                Swap swap5 = new Swap(mnPosition + (Offset - 1) * direction, mnPosition + (Offset - 2) * direction);
                Command.Push(swap5);
            }
        }
        #endregion
        #region 上侧a_j斜向移动命令
        /// <summary>
        /// 生成a_j斜向移动命令,mn上测开始移动
        /// </summary>
        /// <param name="mnPosition">mn的坐标</param>
        /// <param name="Offset">移动格数</param>
        /// <param name="directionR">行 1或-1</param>
        /// <param name="directionC">列 1或-1</param>
        public void RiseEntityObliquePlan(int Offset, int directionR,int directionC)
        {
            for (; Offset > 0; Offset--)
            {
                Swap swap1 = new Swap(mnPosition + Offset * LieShu * directionR +(Offset-1)*directionC, mnPosition + Offset *LieShu* directionR+Offset*directionC);
                Command.Push(swap1);
                Swap swap2 = new Swap(mnPosition + (Offset - 1) *LieShu* directionR + (Offset-1)*directionC, mnPosition + Offset * LieShu * directionR + (Offset - 1) * directionC);
                Command.Push(swap2);
                Swap swap3 = new Swap(mnPosition + (Offset - 1) *LieShu* directionR + Offset*directionC, mnPosition + (Offset - 1) *LieShu* directionR + (Offset-1)*directionC);
                Command.Push(swap3);
                Swap swap4 = new Swap(mnPosition + (Offset - 2) *LieShu* directionR+Offset*directionC, mnPosition + (Offset - 1) * LieShu * directionR + Offset * directionC);
                Command.Push(swap4);
                Swap swap5 = new Swap(mnPosition + (Offset - 2) *LieShu* directionR+(Offset-1)*directionC, mnPosition + (Offset - 2) * LieShu * directionR + Offset * directionC);
                Command.Push(swap5);
                Swap swap6 = new Swap(mnPosition + (Offset - 1) *LieShu* directionR+(Offset-1)*directionC, mnPosition + (Offset - 2) * LieShu * directionR + (Offset - 1) * directionC);
                Command.Push(swap6);
            }
        }
        #endregion
        #region 横侧a_j斜向移动命令
        /// <summary>
        /// 生成a_j斜向移动命令,mn横测开始移动
        /// </summary>
        /// <param name="mnPosition">mn的坐标</param>
        /// <param name="Offset">移动格数</param>
        /// <param name="directionR">行 1或-1</param>
        /// <param name="directionC">列 1或-1</param>
        public void LateralEntityObliquePlan(int Offset, int directionR, int directionC)
        {
            for (; Offset > 0; Offset--)
            {
                Swap swap1 = new Swap(mnPosition + (Offset-1) * LieShu * directionR + Offset * directionC, mnPosition + Offset * LieShu * directionR + Offset * directionC);
                Command.Push(swap1);
                Swap swap2 = new Swap(mnPosition + (Offset - 1) * LieShu * directionR + (Offset - 1) * directionC, mnPosition + (Offset-1) * LieShu * directionR + Offset * directionC);
                Command.Push(swap2);
                Swap swap3 = new Swap(mnPosition + Offset * LieShu * directionR + (Offset-1) * directionC, mnPosition + (Offset - 1) * LieShu * directionR + (Offset - 1) * directionC);
                Command.Push(swap3);
                Swap swap4 = new Swap(mnPosition + Offset * LieShu * directionR + (Offset-2) * directionC, mnPosition + Offset * LieShu * directionR + (Offset-1) * directionC);
                Command.Push(swap4);
                Swap swap5 = new Swap(mnPosition + (Offset - 1) * LieShu * directionR + (Offset - 2) * directionC, mnPosition + Offset * LieShu * directionR + (Offset-2) * directionC);
                Command.Push(swap5);
                Swap swap6 = new Swap(mnPosition + (Offset - 1) * LieShu * directionR + (Offset - 1) * directionC, mnPosition + (Offset - 2) * LieShu * directionR + (Offset - 2) * directionC);
                Command.Push(swap6);
            }
        }
        #endregion
        #endregion

        #region 执行命令
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="command"></param>
        public void ExecutePlan(Stack<Swap> command)
        {
            int count = command.Count;
            for (int i=0;i<command.Count;i++)
            {
                SwapAction(command.Pop());
            }
        }
        public void ExecutePlan()
        {
            int count = Command.Count;
            for (int i = 0; i < count; i++)
            {
                SwapAction(Command.Pop());
            }
        }
        #endregion
    }
    
}
