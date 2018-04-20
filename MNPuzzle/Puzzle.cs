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
        public Queue<Swap> Command1 { get; set; }//命令队列
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
            Command1 = new Queue<Swap>();
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
        /// <param name="offset">移动格数</param>
        /// <param name="direction">方向1或-1</param>
        public void EmptyVerticalPlan( int offset, int direction)
        {
            EmptyVerticalPlan(this.mnPosition,offset,direction);
            //for (; offset > 0; offset--)
            //{
            //    Swap swap = new Swap(mnPosition + (offset - 1) * LieShu * direction, mnPosition + offset * LieShu * direction);
            //    Command.Push(swap);
            //}
        }
        public void EmptyVerticalPlan(int mnPosition, int offset, int direction)
        {
            for (; offset > 0; offset--)
            {
                int j =mnPosition+ offset * LieShu * direction;
                Swap swap = new Swap( j - LieShu * direction, j);
                Command.Push(swap);
            }
        }
        #endregion
        #region mn横向移动命令
        /// <summary>
        /// 生成横向移动命令
        /// </summary>
        /// <param name="mnPosition">mn的坐标</param>
        /// <param name="offset">移动格数</param>
        /// <param name="direction">方向1或-1</param>
        public void EmptyTransversePlan( int offset, int direction)
        {
            EmptyTransversePlan(this.mnPosition,offset,direction);
            //for (; offset > 0; offset--)
            //{
            //    Swap swap = new Swap(mnPosition + (offset - 1) * direction, mnPosition + offset  * direction);
            //    Command.Push(swap);
            //}
        }
        public void EmptyTransversePlan(int mnPosition, int offset, int direction)
        {
            for (; offset > 0; offset--)
            {
                int j =mnPosition+ offset * direction;
                Swap swap = new Swap(j- direction, j);
                Command.Push(swap);
            }
        }
        #endregion
        #region 右侧a_j竖向移动命令
        /// <summary>
        /// 生成a_j竖向移动命令,mn右侧一列回到相对位置
        /// </summary>
        /// <param name="mnPosition">mn的坐标</param>
        /// <param name="offset">移动格数</param>
        /// <param name="direction">方向1或-1</param>
        public void RigthEntityVerticalPlan(int offset, int direction)
        {
            RigthEntityVerticalPlan(this.mnPosition,offset,direction);
            //for (; offset > 0; offset--)
            //{
            //    Swap swap1 = new Swap(mnPosition + offset*LieShu * direction+1, mnPosition + offset *LieShu* direction);
            //    Command.Push(swap1);
            //    Swap swap2 = new Swap(mnPosition + (offset - 1)*LieShu * direction+1, mnPosition + offset * LieShu * direction + 1);
            //    Command.Push(swap2);
            //    Swap swap3 = new Swap(mnPosition + (offset - 2) *LieShu* direction+1, mnPosition + (offset - 1) * LieShu * direction + 1);
            //    Command.Push(swap3);
            //    Swap swap4 = new Swap(mnPosition + (offset - 2) *LieShu* direction, mnPosition + (offset - 2) * LieShu * direction + 1);
            //    Command.Push(swap4);
            //    Swap swap5 = new Swap(mnPosition + (offset - 1)*LieShu * direction, mnPosition + (offset-2)*LieShu * direction);
            //    Command.Push(swap5);
            //}
        }
        public void RigthEntityVerticalPlan(int mnPosition, int offset, int direction)
        {
            int k = LieShu * direction;
            for (; offset > 0; offset--)
            {
                int j = mnPosition+ offset * k-k;
                Swap swap1 = new Swap(j + k+ 1,j + k);
                Command.Push(swap1);
                Swap swap2 = new Swap(j  + 1, j + k+1);
                Command.Push(swap2);
                Swap swap3 = new Swap(j -  k + 1,j + 1);
                Command.Push(swap3);
                Swap swap4 = new Swap(j- k, j - k + 1);
                Command.Push(swap4);
                Swap swap5 = new Swap( j , j -  k);
                Command.Push(swap5);
            }
        }
        #endregion
        #region 左侧a_j竖向移动命令
        /// <summary>
        /// 生成a_j竖向移动命令,mn左侧一列回到相对位置
        /// </summary>
        /// <param name="mnPosition">mn的坐标</param>
        /// <param name="offset">移动格数</param>
        /// <param name="direction">方向1或-1</param>
        public void LeftEntityVerticalPlan(int offset, int direction)
        {
            LeftEntityVerticalPlan(this.mnPosition,offset,direction);
            //for (; offset > 0; offset--)
            //{
            //    Swap swap1 = new Swap(mnPosition + offset * LieShu * direction -1, mnPosition + offset * LieShu * direction);
            //    Command.Push(swap1);
            //    Swap swap2 = new Swap(mnPosition + (offset - 1) * LieShu * direction - 1, mnPosition + offset * LieShu * direction - 1);
            //    Command.Push(swap2);
            //    Swap swap3 = new Swap(mnPosition + (offset - 2) * LieShu * direction - 1, mnPosition + (offset - 1) * LieShu * direction - 1);
            //    Command.Push(swap3);
            //    Swap swap4 = new Swap(mnPosition + (offset - 2) * LieShu * direction, mnPosition + (offset - 2) * LieShu * direction - 1);
            //    Command.Push(swap4);
            //    Swap swap5 = new Swap(mnPosition + (offset - 1) * LieShu * direction, mnPosition + (offset - 2) * LieShu * direction);
            //    Command.Push(swap5);
            //}
        }
        public void LeftEntityVerticalPlan(int mnPosition, int offset, int direction)
        {
            int k = LieShu * direction;
            for (; offset > 0; offset--)
            {
                int j =mnPosition+ offset * k-k;
                Swap swap1 = new Swap( j+k - 1,  j+k);
                Command.Push(swap1);
                Swap swap2 = new Swap(j - 1,  j+k - 1);
                Command.Push(swap2);
                Swap swap3 = new Swap( j- k - 1,  j - 1);
                Command.Push(swap3);
                Swap swap4 = new Swap( j - k, j- k - 1);
                Command.Push(swap4);
                Swap swap5 = new Swap( j ,j - k);
                Command.Push(swap5);
            }
        }
        #endregion
        #region 下侧a_j横向移动命令
        /// <summary>
        /// 生成a_j横向移动命令,mn下侧一行回到相对位置
        /// </summary>
        /// <param name="mnPosition">mn的坐标</param>
        /// <param name="offset">移动格数</param>
        /// <param name="direction">方向1或-1</param>
        public void LowerEntityTransversePlan(int offset, int direction)
        {
            LowerEntityTransversePlan(this.mnPosition,offset,direction);
            //for (; offset > 0; offset--)
            //{
            //    Swap swap1 = new Swap(mnPosition + offset * direction +LieShu, mnPosition + offset * direction);
            //    Command.Push(swap1);
            //    Swap swap2 = new Swap(mnPosition + (offset - 1) * direction +LieShu, mnPosition + offset * direction +LieShu);
            //    Command.Push(swap2);
            //    Swap swap3 = new Swap(mnPosition + (offset - 2) * direction +LieShu, mnPosition + (offset - 1) * direction + LieShu);
            //    Command.Push(swap3);
            //    Swap swap4 = new Swap(mnPosition + (offset - 2) * direction, mnPosition + (offset - 2) * direction +LieShu);
            //    Command.Push(swap4);
            //    Swap swap5 = new Swap(mnPosition + (offset - 1) * direction, mnPosition + (offset - 2) * direction);
            //    Command.Push(swap5);
            //}
        }
        public void LowerEntityTransversePlan(int mnPosition, int offset, int direction)
        {
            for (; offset > 0; offset--)
            {
                int j =mnPosition+ offset * direction-direction+LieShu;
                Swap swap1 = new Swap(j+direction , j+direction-LieShu);
                Command.Push(swap1);
                Swap swap2 = new Swap(j , j+direction );
                Command.Push(swap2);
                Swap swap3 = new Swap( j - direction , j );
                Command.Push(swap3);
                Swap swap4 = new Swap(j - direction-LieShu, j- direction);
                Command.Push(swap4);
                Swap swap5 = new Swap( j-LieShu ,  j -  direction-LieShu);
                Command.Push(swap5);
            }
        }
        #endregion
        #region 上侧a_j横向移动命令
        /// <summary>
        /// 生成a_j横向移动命令,mn上侧一行回到相对位置
        /// </summary>
        /// <param name="mnPosition">mn的坐标</param>
        /// <param name="offset">移动格数</param>
        /// <param name="direction">方向1或-1</param>
        public void RiseEntityTransversePlan(int offset, int direction)
        {
            RiseEntityTransversePlan(this.mnPosition,offset,direction);
            //for (; offset > 0; offset--)
            //{
            //    Swap swap1 = new Swap(mnPosition + offset * direction - LieShu, mnPosition + offset * direction);
            //    Command.Push(swap1);
            //    Swap swap2 = new Swap(mnPosition + (offset - 1) * direction - LieShu, mnPosition + offset * direction - LieShu);
            //    Command.Push(swap2);
            //    Swap swap3 = new Swap(mnPosition + (offset - 2) * direction - LieShu, mnPosition + (offset - 1) * direction - LieShu);
            //    Command.Push(swap3);
            //    Swap swap4 = new Swap(mnPosition + (offset - 2) * direction, mnPosition + (offset - 2) * direction - LieShu);
            //    Command.Push(swap4);
            //    Swap swap5 = new Swap(mnPosition + (offset - 1) * direction, mnPosition + (offset - 2) * direction);
            //    Command.Push(swap5);
            //}
        }
        public void RiseEntityTransversePlan(int mnPosition, int offset, int direction)
        {
            for (; offset > 0; offset--)
            {
                int j = mnPosition+ offset * direction-direction-LieShu;
                Swap swap1 = new Swap(j+direction,  j+direction+LieShu);
                Command.Push(swap1);
                Swap swap2 = new Swap( j , j+direction );
                Command.Push(swap2);
                Swap swap3 = new Swap(j - direction ,  j );
                Command.Push(swap3);
                Swap swap4 = new Swap( j - direction+LieShu,j - direction );
                Command.Push(swap4);
                Swap swap5 = new Swap(j+LieShu , j - direction+LieShu);
                Command.Push(swap5);
            }
        }
        #endregion
        #region 上侧a_j斜向移动命令
        /// <summary>
        /// 生成a_j斜向移动命令,mn上测开始移动
        /// </summary>
        /// <param name="mnPosition">mn的坐标</param>
        /// <param name="offset">移动格数</param>
        /// <param name="directionR">行 1或-1</param>
        /// <param name="directionC">列 1或-1</param>
        public void RiseEntityObliquePlan(int offset, int directionR,int directionC)
        {
            RiseEntityObliquePlan(this.mnPosition,offset,directionR,directionC);
            //for (; offset > 0; offset--)
            //{
            //    Swap swap1 = new Swap(mnPosition + offset * LieShu * directionR +(offset-1)*directionC, mnPosition + offset *LieShu* directionR+offset*directionC);
            //    Command.Push(swap1);
            //    Swap swap2 = new Swap(mnPosition + (offset - 1) *LieShu* directionR + (offset-1)*directionC, mnPosition + offset * LieShu * directionR + (offset - 1) * directionC);
            //    Command.Push(swap2);
            //    Swap swap3 = new Swap(mnPosition + (offset - 1) *LieShu* directionR + offset*directionC, mnPosition + (offset - 1) *LieShu* directionR + (offset-1)*directionC);
            //    Command.Push(swap3);
            //    Swap swap4 = new Swap(mnPosition + (offset - 2) *LieShu* directionR+offset*directionC, mnPosition + (offset - 1) * LieShu * directionR + offset * directionC);
            //    Command.Push(swap4);
            //    Swap swap5 = new Swap(mnPosition + (offset - 2) *LieShu* directionR+(offset-1)*directionC, mnPosition + (offset - 2) * LieShu * directionR + offset * directionC);
            //    Command.Push(swap5);
            //    Swap swap6 = new Swap(mnPosition + (offset - 1) *LieShu* directionR+(offset-1)*directionC, mnPosition + (offset - 2) * LieShu * directionR + (offset - 1) * directionC);
            //    Command.Push(swap6);
            //}
        }
        public void RiseEntityObliquePlan(int mnPosition, int offset, int directionR, int directionC)
        {
            int k = LieShu * directionR;
            for (; offset > 0; offset--)
            {
                int j =mnPosition+offset * k+offset*directionC-k-directionC;
                Swap swap1 = new Swap(j+k , j+k+directionC );
                Command.Push(swap1);
                Swap swap2 = new Swap( j, j +k);
                Command.Push(swap2);
                Swap swap3 = new Swap( j+directionC , j);
                Command.Push(swap3);
                Swap swap4 = new Swap( j -k+directionC , j+directionC );
                Command.Push(swap4);
                Swap swap5 = new Swap( j -  k ,  j- k+directionC );
                Command.Push(swap5);
                Swap swap6 = new Swap( j, j-k);
                Command.Push(swap6);
            }
        }
        #endregion
        #region 横侧a_j斜向移动命令
        /// <summary>
        /// 生成a_j斜向移动命令,mn横测开始移动
        /// </summary>
        /// <param name="mnPosition">mn的坐标</param>
        /// <param name="offset">移动格数</param>
        /// <param name="directionR">行 1或-1</param>
        /// <param name="directionC">列 1或-1</param>
        public void LateralEntityObliquePlan(int offset, int directionR, int directionC)
        {
            LateralEntityObliquePlan(this.mnPosition,offset,directionR,directionC);
            //for (; offset > 0; offset--)
            //{
            //    Swap swap1 = new Swap(mnPosition + (offset-1) * LieShu * directionR + offset * directionC, mnPosition + offset * LieShu * directionR + offset * directionC);
            //    Command.Push(swap1);
            //    Swap swap2 = new Swap(mnPosition + (offset - 1) * LieShu * directionR + (offset - 1) * directionC, mnPosition + (offset-1) * LieShu * directionR + offset * directionC);
            //    Command.Push(swap2);
            //    Swap swap3 = new Swap(mnPosition + offset * LieShu * directionR + (offset-1) * directionC, mnPosition + (offset - 1) * LieShu * directionR + (offset - 1) * directionC);
            //    Command.Push(swap3);
            //    Swap swap4 = new Swap(mnPosition + offset * LieShu * directionR + (offset-2) * directionC, mnPosition + offset * LieShu * directionR + (offset-1) * directionC);
            //    Command.Push(swap4);
            //    Swap swap5 = new Swap(mnPosition + (offset - 1) * LieShu * directionR + (offset - 2) * directionC, mnPosition + offset * LieShu * directionR + (offset-2) * directionC);
            //    Command.Push(swap5);
            //    Swap swap6 = new Swap(mnPosition + (offset - 1) * LieShu * directionR + (offset - 1) * directionC, mnPosition + (offset - 2) * LieShu * directionR + (offset - 2) * directionC);
            //    Command.Push(swap6);
            //}
        }
        public void LateralEntityObliquePlan(int mnPosition,  int offset, int directionR, int directionC)
        {
            int k = LieShu * directionR;
            for (; offset > 0; offset--)
            {
                int j =mnPosition+ offset * k+offset*directionC-k-directionC;
                Swap swap1 = new Swap( j+directionC , j+k+directionC );
                Command.Push(swap1);
                Swap swap2 = new Swap( j,  j+directionC );
                Command.Push(swap2);
                Swap swap3 = new Swap( j+k,  j);
                Command.Push(swap3);
                Swap swap4 = new Swap( j+k - directionC,  j+k);
                Command.Push(swap4);
                Swap swap5 = new Swap(j - directionC,  j+k -directionC);
                Command.Push(swap5);
                Swap swap6 = new Swap(j, j -directionC);
                Command.Push(swap6);
            }
        }
        #endregion
        #endregion

        #region 基础命令队列
        #region mn竖向移动命令
        /// <summary>
        /// 生成竖向移动命令
        /// </summary>
        /// <param name="mnPosition">mn的位置</param>
        /// <param name="offset">移动格数</param>
        /// <param name="direction">方向1或-1</param>
        public void _EmptyVerticalPlan(int offset, int direction)
        {
            _EmptyVerticalPlan(this.mnPosition, offset, direction);
            //for (; offset > 0; offset--)
            //{
            //    Swap swap = new Swap(mnPosition + (offset - 1) * LieShu * direction, mnPosition + offset * LieShu * direction);
            //    Command.Push(swap);
            //}
        }
        public void _EmptyVerticalPlan(int mnPosition, int offset, int direction)
        {
            int k = LieShu * direction;
            for (int i=0;i<offset;i++)
            {
                int j = mnPosition + i * k;//j是当前循环mn的初始位置
                Swap swap = new Swap(j,j+LieShu*direction);
                Command1.Enqueue(swap);
            }
            //for (; offset > 0; offset--)
            //{
            //    int j = mnPosition + offset * LieShu * direction;
            //    Swap swap = new Swap(j - LieShu * direction, j);
            //    Command.Push(swap);
            //}
        }
        #endregion
        #region mn横向移动命令
        /// <summary>
        /// 生成横向移动命令
        /// </summary>
        /// <param name="mnPosition">mn的坐标</param>
        /// <param name="offset">移动格数</param>
        /// <param name="direction">方向1或-1</param>
        public void _EmptyTransversePlan(int offset, int direction)
        {
            _EmptyTransversePlan(this.mnPosition, offset, direction);
            //for (; offset > 0; offset--)
            //{
            //    Swap swap = new Swap(mnPosition + (offset - 1) * direction, mnPosition + offset  * direction);
            //    Command.Push(swap);
            //}
        }
        public void _EmptyTransversePlan(int mnPosition, int offset, int direction)
        {
            for (int i=0;i<offset;i++)
            {
                int j = mnPosition + i * direction;
                Swap swap = new Swap(j, j+direction);
                Command1.Enqueue(swap);
            }
            //for (; offset > 0; offset--)
            //{
            //    int j = mnPosition + offset * direction;
            //    Swap swap = new Swap(j - direction, j);
            //    Command.Push(swap);
            //}
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

        #region 执行命令队列
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="command"></param>
        public void _ExecutePlan(Queue<Swap> command1)
        {
            int count = command1.Count;
            for (int i = 0; i < count; i++)
            {
                SwapAction(command1.Dequeue());
            }
        }
        public void _ExecutePlan()
        {
            int count = Command1.Count;
            for (int i = 0; i < count; i++)
            {
                SwapAction(Command1.Dequeue());
            }
        }
        #endregion

        #region 相对位置分析
        public PointOffset RelativePosition(int origin,int end)
        {
            return new PointOffset(origin,end,this.LieShu);
        }
        #endregion
    }
    
}
