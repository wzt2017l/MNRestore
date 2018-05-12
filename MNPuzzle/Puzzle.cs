using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        public int[] Items { get; private set; }//拼图数组
        public long InversionNumber { get; set; }//数组逆序数
        public PuzzleState State { get; set; }//状态 
        public Queue<Swap> Command { get; set; }//命令队列
        #endregion

        #region 构造函数
        public Puzzle(int hangshu ,int lieshu)
        {
            HangShu = hangshu;
            LieShu = lieshu;
            Total = hangshu * lieshu;
            mnPosition = Total - 1;
            Items = new int[Total];
            for (int i = 0; i <Total; i++)
            {
                Items[i] = i;
            }
            InversionNumber = 0;
            State = PuzzleState.Original;
            Command = new Queue<Swap>();
        }
        #endregion

        #region 交换位置
        /// <summary>
        /// 交换位置，不对拼图状态做任何更新
        /// </summary>
        /// <param name="empty">空格所在的位置</param>
        /// <param name="entity">要与之交换的拼图块的位置</param>
        private void Swap(int empty, int entity)
        {
            int t = this.Items[empty];
            Items[empty] = Items[entity];
            Items[entity] = t;
        }
        /// <summary>
        /// 交换位置，更新逆序数和拼图的状态
        /// </summary>
        /// <param name="empty">空格所在的位置</param>
        /// <param name="entity">要与之交换的拼图块的位置</param>
        public void SwapAction(int empty,int entity)
        {
            InversionNumber = InversionNumber + InversionNumberDifference(entity, empty);
            int t = this.Items[empty];
            Items[empty] = Items[entity];
            Items[entity] = t;
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
                if (Items[i] < Items[i + k])
                {
                    difference++;
                }
                if (Items[j] < Items[i + k])
                {
                    difference--;
                }
            }
            if (Items[j] > Items[i])
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

        #region 打乱拼图
        /// <summary>
        /// 打乱拼图
        /// </summary>
        public void Disrupt()
        {

            int count = (int)(Total * Math.Log(Total, (double)(Total * (Total - 1.0) / 2.0)));
            int Length = (int)Math.Log10(Total) + 1;
            Random r = new Random();
            Parallel.For(0, count, i =>
            {
                int k, j;
                k = r.Next(0, i + 1);
                j = r.Next(i, Total);
                if (k != j)
                {
                    Swap(k, j);
                }
            });
            Swap(Total - 1, r.Next(0, Total - 1));

            //for (int i = 0; i < count; i++)
            //{
            //    Random r = new Random();
            //    int k = r.Next(0, Total);
            //    int j = r.Next(0, Total);
            //    if (k != j)
            //    {
            //        Swap(k, j);
            //    }
            //}
        }
        #endregion
        #region 重算逆序数
        /// <summary>
        /// 计算逆序数
        /// </summary>
        /// <param name="array">数组</param>
        /// <returns>逆序数</returns>
        public long RetryInversionNumber(int[] array)
        {
            long inversion = 0;
            int len = array.Length;
            long[] list = new long[len];
            Parallel.For(1, len, i =>
            {
                long l = 0;
                int length = len - i;
                int index = array[length];
                //如何才能提前结束循环？
                for (int k = 0; k < length; k++)
                {
                    if (index < array[k])
                    {
                        l++;
                    }
                }
                list[i] = l;
            });
            Parallel.For(0, len, i => {
                lock (list)
                {
                    inversion += list[i];
                }
            });
            return inversion;
        }
        public long RetryInversionNumber()
        {
            return RetryInversionNumber(this.Items);
        }
        #endregion

        #region 生成基础命令
        #region mn竖向移动命令
        /// <summary>
        /// 生成竖向移动命令
        /// </summary>
        /// <param name="mnPosition">mn当前位置</param>
        /// <param name="offset">偏移量</param>
        /// <param name="direction">方向1或-1</param>
        public void EmptyVerticalPlan(int mnPosition, int offset, int direction)
        {
            int k = LieShu * direction;
            for (int i=0;i<offset;i++)
            {
                int j = mnPosition + i * k;//j是当前循环mn的初始位置
                Swap swap = new Swap(j,j+LieShu*direction);
                Command.Enqueue(swap);
            }
        }
        public void EmptyVerticalPlan(int offset, int direction)
        {
            EmptyVerticalPlan(this.mnPosition, offset, direction);
        }
        #endregion
        #region mn横向移动命令
        /// <summary>
        /// 生成横向移动命令
        /// </summary>
        /// <param name="mnPosition">mn的坐标</param>
        /// <param name="offset">移动格数</param>
        /// <param name="direction">方向1或-1</param>
        public void EmptyTransversePlan(int mnPosition, int offset, int direction)
        {
            for (int i=0;i<offset;i++)
            {
                int j = mnPosition + i * direction;
                Swap swap = new Swap(j, j+direction);
                Command.Enqueue(swap);
            }
        }
        public void EmptyTransversePlan(int offset, int direction)
        {
            EmptyTransversePlan(this.mnPosition, offset, direction);
        }
        #endregion
        #region 右侧a_j竖向移动命令
        /// <summary>
        /// 生成a_j竖向移动命令,mn右侧一列回到相对位置
        /// </summary>
        /// <param name="mnPosition">mn的坐标</param>
        /// <param name="offset">移动格数</param>
        /// <param name="direction">方向1或-1</param>
        public void RigthEntityVerticalPlan(int mnPosition, int offset, int direction)
        {
            int k = LieShu * direction;
            for (int i=0;i<offset;i++)
            {
                int j = mnPosition + i * k;
                Swap swap1 = new Swap(j, j - k);
                Command.Enqueue(swap1);
                Swap swap2 = new Swap(j - k, j - k + 1);
                Command.Enqueue(swap2);
                Swap swap3 = new Swap(j - k + 1, j + 1);
                Command.Enqueue(swap3);
                Swap swap4 = new Swap(j + 1, j + k + 1);
                Command.Enqueue(swap4);
                Swap swap5 = new Swap(j + k + 1, j + k);
                Command.Enqueue(swap5);
            }
        }
        public void RigthEntityVerticalPlan(int offset, int direction)
        {
            RigthEntityVerticalPlan(this.mnPosition, offset, direction);
        }
        #endregion
        #region 左侧a_j竖向移动命令
        /// <summary>
        /// 生成a_j竖向移动命令,mn左侧一列回到相对位置
        /// </summary>
        /// <param name="mnPosition">mn的坐标</param>
        /// <param name="offset">移动格数</param>
        /// <param name="direction">方向1或-1</param>
        public void LeftEntityVerticalPlan(int mnPosition, int offset, int direction)
        {
            int k = LieShu * direction;
            for (int i=0;i<offset;i++)
            {
                int j = mnPosition + i * k;
                Swap swap1 = new Swap(j, j - k);
                Command.Enqueue(swap1);
                Swap swap2 = new Swap(j - k, j - k - 1);
                Command.Enqueue(swap2);
                Swap swap3 = new Swap(j - k - 1, j - 1);
                Command.Enqueue(swap3);
                Swap swap4 = new Swap(j - 1, j + k - 1);
                Command.Enqueue(swap4);
                Swap swap5 = new Swap(j + k - 1, j + k);
                Command.Enqueue(swap5);
            }
        }
        public void LeftEntityVerticalPlan(int offset, int direction)
        {
            LeftEntityVerticalPlan(this.mnPosition, offset, direction);
        }
        #endregion
        #region 下侧a_j横向移动命令
        /// <summary>
        /// 生成a_j横向移动命令,mn下侧一行回到相对位置
        /// </summary>
        /// <param name="mnPosition">mn的坐标</param>
        /// <param name="offset">移动格数</param>
        /// <param name="direction">方向1或-1</param>
        public void LowerEntityTransversePlan(int mnPosition, int offset, int direction)
        {
            for (int i=0;i<offset;i++)
            {
                int j = mnPosition + i * direction + LieShu;
                Swap swap1 = new Swap(j - LieShu, j - direction - LieShu);
                Command.Enqueue(swap1);
                Swap swap2 = new Swap(j - direction - LieShu, j - direction);
                Command.Enqueue(swap2);
                Swap swap3 = new Swap(j - direction, j);
                Command.Enqueue(swap3);
                Swap swap4 = new Swap(j, j + direction);
                Command.Enqueue(swap4);
                Swap swap5 = new Swap(j + direction, j + direction - LieShu);
                Command.Enqueue(swap5);
            }
        }
        public void LowerEntityTransversePlan(int offset, int direction)
        {
            LowerEntityTransversePlan(this.mnPosition, offset, direction);
        }
        #endregion
        #region 上侧a_j横向移动命令
        /// <summary>
        /// 生成a_j横向移动命令,mn上侧一行回到相对位置
        /// </summary>
        /// <param name="mnPosition">mn的坐标</param>
        /// <param name="offset">移动格数</param>
        /// <param name="direction">方向1或-1</param>
        public void RiseEntityTransversePlan(int mnPosition, int offset, int direction)
        {
            for (int i=0;i<offset;i++)
            {
                int j = mnPosition + i * direction - LieShu;
                Swap swap1 = new Swap(j + LieShu, j - direction + LieShu);
                Command.Enqueue(swap1);
                Swap swap2 = new Swap(j - direction + LieShu, j - direction);
                Command.Enqueue(swap2);
                Swap swap3 = new Swap(j - direction, j);
                Command.Enqueue(swap3);
                Swap swap4 = new Swap(j, j + direction);
                Command.Enqueue(swap4);
                Swap swap5 = new Swap(j + direction, j + direction + LieShu);
                Command.Enqueue(swap5);
            }
        }
        public void RiseEntityTransversePlan(int offset, int direction)
        {
            RiseEntityTransversePlan(this.mnPosition, offset, direction);
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
        public void RiseEntityObliquePlan(int mnPosition, int offset, int directionR, int directionC)
        {
            int k = LieShu * directionR;
            for (int i=0;i<offset;i++)
            {
                int j = mnPosition + i * k + i * directionC;
                Swap swap1 = new Swap(j, j - k);
                Command.Enqueue(swap1);
                Swap swap2 = new Swap(j - k, j - k + directionC);
                Command.Enqueue(swap2);
                Swap swap3 = new Swap(j - k + directionC, j + directionC);
                Command.Enqueue(swap3);
                Swap swap4 = new Swap(j + directionC, j);
                Command.Enqueue(swap4);
                Swap swap5 = new Swap(j, j + k);
                Command.Enqueue(swap5);
                Swap swap6 = new Swap(j + k, j + k + directionC);
                Command.Enqueue(swap6);
            }
        }
        public void RiseEntityObliquePlan(int offset, int directionR, int directionC)
        {
            RiseEntityObliquePlan(this.mnPosition, offset, directionR, directionC);
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
        public void LateralEntityObliquePlan(int mnPosition, int offset, int directionR, int directionC)
        {
            int k = LieShu * directionR;
            for (int i=0;i<offset;i++)
            {
                int j = mnPosition + i * k + i * directionC;
                Swap swap1 = new Swap(j, j - directionC);
                Command.Enqueue(swap1);
                Swap swap2 = new Swap(j - directionC, j + k - directionC);
                Command.Enqueue(swap2);
                Swap swap3 = new Swap(j + k - directionC, j + k);
                Command.Enqueue(swap3);
                Swap swap4 = new Swap(j + k, j);
                Command.Enqueue(swap4);
                Swap swap5 = new Swap(j, j + directionC);
                Command.Enqueue(swap5);
                Swap swap6 = new Swap(j + directionC, j + k + directionC);
                Command.Enqueue(swap6);
            }
        }
        public void LateralEntityObliquePlan(int offset, int directionR, int directionC)
        {
            LateralEntityObliquePlan(this.mnPosition, offset, directionR, directionC);
        }
        #endregion
        #endregion

        #region 执行命令
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="command"></param>
        public void ExecutePlan(Queue<Swap> command)
        {
            int count = command.Count;
            for (int i = 0; i < count; i++)
            {
                SwapAction(command.Dequeue());
            }
        }
        public void ExecutePlan()
        {
            int count = Command.Count;
            for (int i = 0; i < count; i++)
            {
                SwapAction(Command.Dequeue());
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
