using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNPuzzle
{
    /// <summary>
    /// 方位，顺时针旋转
    /// </summary>
   public enum Position
    {
        /// <summary>
        /// 原点
        /// </summary>
        Origin=0,
        /// <summary>
        /// 正上方
        /// </summary>
        Up,
        /// <summary>
        /// 右偏上
        /// </summary>
        UpGtRight,
        /// <summary>
        /// 上右
        /// </summary>
        UpEqRight,
        /// <summary>
        /// 上偏右
        /// </summary>
        RightGtUp,
        /// <summary>
        /// 正右
        /// </summary>
        Right,
        /// <summary>
        /// 下偏右
        /// </summary>
        RightGtDown,
        /// <summary>
        /// 右下
        /// </summary>
        RightEqDown,
        /// <summary>
        /// 右偏下
        /// </summary>
        DownGtRight,
        /// <summary>
        /// 正下
        /// </summary>
        Down,
        /// <summary>
        /// 左偏下
        /// </summary>
        DownGtLeft,
        /// <summary>
        /// 下左
        /// </summary>
        DownEqLeft,
        /// <summary>
        /// 下偏左
        /// </summary>
        LeftGtDown,
        /// <summary>
        /// 正左
        /// </summary>
        Left,
        /// <summary>
        /// 上偏左
        /// </summary>
        LeftGtUp,
        /// <summary>
        /// 左上
        /// </summary>
        LeftEqUp,
        /// <summary>
        /// 左偏上
        /// </summary>
        UpGtLeft
    }
    /// <summary>
    /// PuzzleAide类里面的基础命令
    /// </summary>
    public enum Plan
    {
        /// <summary>
        /// mn竖向移动命令
        /// </summary>
        EmptyVertical=1,
        /// <summary>
        /// mn横向移动命令
        /// </summary>
        EmptyTransverse,
        /// <summary>
        /// 右侧a_j竖向移动命令
        /// </summary>
        RigthEntityVertical,
        /// <summary>
        /// 左侧a_j竖向移动命令
        /// </summary>
        LeftEntityVertical,
        /// <summary>
        /// 下侧a_j横向移动命令
        /// </summary>
        LowerEntityTransverse,
        /// <summary>
        /// 上侧a_j横向移动命令
        /// </summary>
        RiseEntityTransverse,
        /// <summary>
        /// 上侧a_j斜向移动命令
        /// </summary>
        RiseEntityOblique,
        /// <summary>
        /// 横侧a_j斜向移动命令
        /// </summary>
        LateralEntityObliquePlan
    }

   public class PuzzleAide
   {
        public Queue<Swap> Command { get; set; }//命令队列
        public Puzzle puzzle { get; private set; }

        public PuzzleAide() { }
        public PuzzleAide(Puzzle puzzle)
        {
            this.puzzle = puzzle;
            Command = new Queue<Swap>();
        }

        #region 打乱拼图
        /// <summary>
        /// 打乱拼图，但不关心是否能够复原
        /// </summary>
        /// <param name="puzzle"></param>
        public void Disrupt(Puzzle puzzle)
        {
            int Total = puzzle.Total;
            int count = (int)(Total * Math.Log(Total, (double)(Total * (Total - 1.0) / 2.0)));
            Random r = new Random();
            Parallel.For(0, count, i =>
            {
                int k, j;
                k = r.Next(0, i + 1);
                j = r.Next(i, Total);
                if (k != j)
                {
                    puzzle.Swap(k, j);
                }
            });
            puzzle.Swap(Total - 1, r.Next(0, Total - 1));
        }
        /// <summary>
        /// 打乱拼图,并使其可以还原，并更新逆序、状态、mn位置
        /// </summary>
        public void Disrupt()
        {
            Disrupt(this.puzzle);
            int Total = puzzle.Total;
            puzzle.RetryNiXu();//重算逆序
            //找到mn的位置
            Parallel.For(0, Total, i => {
                if (puzzle.Items[i] == Total - 1)
                {
                    puzzle.mnPosition = i;
                }
            });
            //矫正拼图
            if ((puzzle.HangShu + puzzle.LieShu) % 2 != (puzzle.mnPosition / puzzle.LieShu + puzzle.mnPosition % puzzle.LieShu + puzzle.NiXu) % 2)
            {
                if (puzzle.mnPosition == 0 || puzzle.mnPosition == 1)
                {
                    puzzle.SwapAction(puzzle.mnPosition, puzzle.LieShu + puzzle.mnPosition);//向下移动一行
                }
                if (puzzle.Items[0] > puzzle.Items[1])
                {
                    puzzle.NiXu--;
                }
                else
                {
                    puzzle.NiXu++;
                }
                int t = puzzle.Items[0];
                puzzle.Items[0] = puzzle.Items[1];
                puzzle.Items[1] = t;
            }
        }
        #endregion

        #region 方位判断
        /// <summary>
        /// 方位判断
        /// </summary>
        /// <param name="origin">起始</param>
        /// <param name="target">目标</param>
        /// <param name="lieShu">列数</param>
        /// <returns>方位枚举</returns>
        public Position PositionAnalysis(int origin, int target, int lieShu)
        {
            return PositionAnalysis(new PointOffset(origin, target, lieShu));
        }
          
        public Position PositionAnalysis(PointOffset po)
        {
            Position position = new Position();
            switch (po.Direction.Y)//行
            {
                case 1:
                    switch (po.Direction.X)//列
                    {
                        case 1:
                            if (po.OffsetYMinusX > 0)
                            {
                                position = Position.DownGtRight;
                            }
                            else
                            if (po.OffsetYMinusX < 0)
                            {
                                position = Position.RightGtDown;
                            }
                            else
                            {
                                position = Position.RightEqDown;
                            }
                            break;
                        case -1:
                            if (po.OffsetYMinusX > 0)
                            {
                                position = Position.DownGtLeft;
                            }
                            else
                             if (po.OffsetYMinusX < 0)
                            {
                                position = Position.LeftGtDown;
                            }
                            else
                            {
                                position = Position.DownEqLeft;
                            }
                            break;
                        case 0:
                            position = Position.Down;
                            break;
                    }
                    break;
                case -1:
                    switch (po.Direction.X)//列
                    {
                        case 1:
                            if (po.OffsetYMinusX > 0)
                            {
                                position = Position.UpGtRight;
                            }
                            else
                            if (po.OffsetYMinusX < 0)
                            {
                                position = Position.RightGtUp;
                            }
                            else
                            {
                                position = Position.UpEqRight;
                            }
                            break;
                        case -1:
                            if (po.OffsetYMinusX > 0)
                            {
                                position = Position.UpGtLeft;
                            }
                            else
                            if (po.OffsetYMinusX < 0)
                            {
                                position = Position.LeftGtUp;
                            }
                            else
                            {
                                position = Position.LeftEqUp;
                            }
                            break;
                        case 0:
                            position = Position.Up;
                            break;
                    }
                    break;
                case 0:
                    switch (po.Direction.X)//列
                    {
                        case 1:
                            position = Position.Right;
                            break;
                        case -1:
                            position = Position.Left;
                            break;
                        case 0:
                            position = Position.Origin;
                            break;
                    }
                    break;
            }
            return position;
        }
        #endregion

        #region 计算出执行一次基础命令后mn的位置
        /// <summary>
        /// 计算出执行一次基础命令后mn的位置，参数参照相应的基础命令，如果基础命令没有与之对应的参数，则使其为0
        /// </summary>
        /// <param name="mnPosition">mn未移动前的位置</param>
        /// <param name="lieShu">列数</param>
        /// <param name="offset">偏移量</param>
        /// <param name="directionR">行方向1/-1/0</param>
        /// <param name="directionC">列方向1/-1/0</param>
        /// <returns></returns>
        public int MnPosition(int mnPosition,int lieShu,int offset, int directionR,int directionC )
        {
            return mnPosition + offset * directionR * lieShu + offset * directionC; ;
        }
        public int MnPosition(int mnPosition, int offset, int directionR, int directionC)
        {
           return MnPosition(mnPosition,puzzle.LieShu,offset,directionR,directionC);
        }
        /// <summary>
        /// 计算出执行一次基础命令后mn的位置，参数参照相应的基础命令，如果基础命令没有与之对应的参数，则使其为0
        /// 这个必须在命令执行前计算
        /// </summary>
        /// <param name="offset">偏移量</param>
        /// <param name="directionR">行方向1/-1/0</param>
        /// <param name="directionC">列方向1/-1/0</param>
        /// <returns></returns>
        public int MnPosition( int offset, int directionR, int directionC)
        {
            return MnPosition(puzzle.mnPosition, puzzle.LieShu, offset, directionR, directionC);
        }
        #endregion

        #region 生成基础命令
        #region mn竖向移动命令
        /// <summary>
        /// 输出竖向移动命令,生成的命令不加入内置的命令队列
        /// </summary>
        /// <param name="mnPosition">mn当前位置</param>
        /// <param name="offset">偏移量</param>
        /// <param name="direction">方向1或-1</param>
        /// <param name="lieShu">列数</param>
        /// <returns>命令队列</returns>
        public Queue<Swap> EmptyVerticalPlanOut(int mnPosition, int offset, int direction, int lieShu)
        {
            Queue<Swap> command = new Queue<Swap>();
            int k = lieShu * direction;
            for (int i = 0; i < offset; i++)
            {
                int j = mnPosition + i * k;//j是当前循环mn的初始位置
                Swap swap = new Swap(j, j + lieShu * direction);
                command.Enqueue(swap);
            }
            return command;
        }
        /// <summary>
        /// 生成竖向移动命令
        /// </summary>
        /// <param name="mnPosition">mn当前位置</param>
        /// <param name="offset">偏移量</param>
        /// <param name="direction">方向1或-1</param>
        /// <param name="lieShu">列数</param>
        /// <returns>mn移动后的位置</returns>
        public int EmptyVerticalPlan(int mnPosition, int offset, int direction ,int lieShu)
        {
            int k = lieShu * direction;
            for (int i = 0; i < offset; i++)
            {
                int j = mnPosition + i * k;//j是当前循环mn的初始位置
                Swap swap = new Swap(j, j + lieShu * direction);
                Command.Enqueue(swap);
            }
            return mnPosition + k * offset;
        }
        /// <summary>
        /// 生成竖向移动命令
        /// </summary>
        /// <param name="mnPosition">mn当前位置</param>
        /// <param name="offset">偏移量</param>
        /// <param name="direction">方向1或-1</param>
        /// <returns>mn移动后的位置</returns>
        public int EmptyVerticalPlan(int mnPosition, int offset, int direction)
        {
           return EmptyVerticalPlan(mnPosition,offset,direction,puzzle.LieShu);
        }
        public int EmptyVerticalPlan(int offset, int direction)
        {
           return EmptyVerticalPlan(puzzle.mnPosition, offset, direction,puzzle.LieShu);
        }
        #endregion
        #region mn横向移动命令
        /// <summary>
        /// 输出横向移动命令,生成的命令不加入内置的命令队列
        /// </summary>
        /// <param name="mnPosition">mn当前位置</param>
        /// <param name="offset">移动格数</param>
        /// <param name="direction">方向-1或1</param>
        /// <returns>命令队列</returns>
        public Queue<Swap> EmptyTransversePlanOut(int mnPosition, int offset, int direction)
        {
            Queue<Swap> command = new Queue<Swap>();
            for (int i = 0; i < offset; i++)
            {
                int j = mnPosition + i * direction;
                Swap swap = new Swap(j, j + direction);
                command.Enqueue(swap);
            }
            return command;
        }
        /// <summary>
        /// 生成横向移动命令
        /// </summary>
        /// <param name="mnPosition">mn的坐标</param>
        /// <param name="offset">移动格数</param>
        /// <param name="direction">方向1或-1</param>
        /// <returns>mn执行完命令所在的位置</returns>
        public int EmptyTransversePlan(int mnPosition, int offset, int direction)
        {
            for (int i = 0; i < offset; i++)
            {
                int j = mnPosition + i * direction;
                Swap swap = new Swap(j, j + direction);
                Command.Enqueue(swap);
            }
            return mnPosition + offset * direction;
        }
        public int EmptyTransversePlan(int offset, int direction)
        {
           return EmptyTransversePlan(puzzle.mnPosition, offset, direction);
        }
        #endregion
        #region 右侧a_j竖向移动命令
        /// <summary>
        /// 输出右侧a_j竖向移动命令,生成的命令不加入内置的命令队列
        /// </summary>
        /// <param name="mnPosition">mn当前位置</param>
        /// <param name="offset">移动格数</param>
        /// <param name="direction">方向1或-1</param>
        /// <param name="lieShu">列数</param>
        /// <returns>命令队列</returns>
        public Queue<Swap> RigthEntityVerticalPlanOut(int mnPosition, int offset, int direction, int lieShu)
        {
            Queue<Swap> command = new Queue<Swap>();
            int k = lieShu * direction;
            for (int i = 0; i < offset; i++)
            {
                int j = mnPosition + i * k;
                Swap swap1 = new Swap(j, j - k);
                command.Enqueue(swap1);
                Swap swap2 = new Swap(j - k, j - k + 1);
                command.Enqueue(swap2);
                Swap swap3 = new Swap(j - k + 1, j + 1);
                command.Enqueue(swap3);
                Swap swap4 = new Swap(j + 1, j + k + 1);
                command.Enqueue(swap4);
                Swap swap5 = new Swap(j + k + 1, j + k);
                command.Enqueue(swap5);
            }
            return command;
        }
        /// <summary>
        /// 生成a_j竖向移动命令,mn右侧一列回到相对位置
        /// </summary>
        /// <param name="mnPosition">mn的坐标</param>
        /// <param name="offset">移动格数</param>
        /// <param name="direction">方向1或-1</param>
        /// <param name="lieShu">列数</param>
        public int RigthEntityVerticalPlan(int mnPosition, int offset, int direction ,int lieShu)
        {
            int k = lieShu * direction;
            for (int i = 0; i < offset; i++)
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
            return mnPosition + offset * k;
        }
        /// <summary>
        /// 生成a_j竖向移动命令,mn右侧一列回到相对位置
        /// </summary>
        /// <param name="mnPosition">mn的坐标</param>
        /// <param name="offset">移动格数</param>
        /// <param name="direction">方向1或-1</param>
        public int RigthEntityVerticalPlan(int mnPosition, int offset, int direction)
        {
           return RigthEntityVerticalPlan(mnPosition,offset,direction,puzzle.LieShu);
        }
        public int RigthEntityVerticalPlan(int offset, int direction)
        {
           return RigthEntityVerticalPlan(puzzle.mnPosition, offset, direction,puzzle.LieShu);
        }
        #endregion
        #region 左侧a_j竖向移动命令
        /// <summary>
        ///  输出左侧a_j竖向移动命令,生成的命令不加入内置的命令队列
        /// </summary>
        /// <param name="mnPosition">mn当前位置</param>
        /// <param name="offset">移动格数</param>
        /// <param name="direction">方向1或-1</param>
        /// <param name="lieShu">列数</param>
        /// <returns>命令队列</returns>
        public Queue<Swap> LeftEntityVerticalPlanOut(int mnPosition, int offset, int direction, int lieShu)
        {
            Queue<Swap> command = new Queue<Swap>();
            int k = lieShu * direction;
            for (int i = 0; i < offset; i++)
            {
                int j = mnPosition + i * k;
                Swap swap1 = new Swap(j, j - k);
                command.Enqueue(swap1);
                Swap swap2 = new Swap(j - k, j - k - 1);
                command.Enqueue(swap2);
                Swap swap3 = new Swap(j - k - 1, j - 1);
                command.Enqueue(swap3);
                Swap swap4 = new Swap(j - 1, j + k - 1);
                command.Enqueue(swap4);
                Swap swap5 = new Swap(j + k - 1, j + k);
                command.Enqueue(swap5);
            }
            return command;
        }
        /// <summary>
        /// 生成a_j竖向移动命令,mn左侧一列回到相对位置
        /// </summary>
        /// <param name="mnPosition">mn的坐标</param>
        /// <param name="offset">移动格数</param>
        /// <param name="direction">方向1或-1</param>
        /// <param name="lieShu">列数</param>
        public int LeftEntityVerticalPlan(int mnPosition, int offset, int direction,int lieShu)
        {
            int k = lieShu * direction;
            for (int i = 0; i < offset; i++)
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
            return mnPosition + offset * k;
        }
        /// <summary>
        /// 生成a_j竖向移动命令,mn左侧一列回到相对位置
        /// </summary>
        /// <param name="mnPosition">mn的坐标</param>
        /// <param name="offset">移动格数</param>
        /// <param name="direction">方向1或-1</param>
        public int LeftEntityVerticalPlan(int mnPosition, int offset, int direction)
        {
           return LeftEntityVerticalPlan(mnPosition,offset,direction,puzzle.LieShu);
        }
        public int LeftEntityVerticalPlan(int offset, int direction)
        {
           return LeftEntityVerticalPlan(puzzle.mnPosition, offset, direction,puzzle.LieShu);
        }
        #endregion
        #region 下侧a_j横向移动命令
        /// <summary>
        /// 输出下侧a_j横向移动命令,生成的命令不加入内置的命令队列
        /// </summary>
        /// <param name="mnPosition">mn当前位置</param>
        /// <param name="offset">移动格数</param>
        /// <param name="direction">方向1或-1</param>
        /// <param name="lieShu">列数</param>
        /// <returns>命令队列</returns>
        public Queue<Swap> LowerEntityTransversePlanOut(int mnPosition, int offset, int direction, int lieShu)
        {
            Queue<Swap> command = new Queue<Swap>();
            for (int i = 0; i < offset; i++)
            {
                int j = mnPosition + i * direction + lieShu;
                Swap swap1 = new Swap(j - lieShu, j - direction - lieShu);
                command.Enqueue(swap1);
                Swap swap2 = new Swap(j - direction - lieShu, j - direction);
                command.Enqueue(swap2);
                Swap swap3 = new Swap(j - direction, j);
                command.Enqueue(swap3);
                Swap swap4 = new Swap(j, j + direction);
                command.Enqueue(swap4);
                Swap swap5 = new Swap(j + direction, j + direction - lieShu);
                command.Enqueue(swap5);
            }
            return command;
        }
        /// <summary>
        /// 生成a_j横向移动命令,mn下侧一行回到相对位置
        /// </summary>
        /// <param name="mnPosition">mn的坐标</param>
        /// <param name="offset">移动格数</param>
        /// <param name="direction">方向1或-1</param>
        /// <param name="lieShu">列数</param>
        public int LowerEntityTransversePlan(int mnPosition, int offset, int direction ,int lieShu)
        {
            for (int i = 0; i < offset; i++)
            {
                int j = mnPosition + i * direction + lieShu;
                Swap swap1 = new Swap(j - lieShu, j - direction - lieShu);
                Command.Enqueue(swap1);
                Swap swap2 = new Swap(j - direction - lieShu, j - direction);
                Command.Enqueue(swap2);
                Swap swap3 = new Swap(j - direction, j);
                Command.Enqueue(swap3);
                Swap swap4 = new Swap(j, j + direction);
                Command.Enqueue(swap4);
                Swap swap5 = new Swap(j + direction, j + direction - lieShu);
                Command.Enqueue(swap5);
            }
            return mnPosition + offset * direction;
        }
        /// <summary>
        /// 生成a_j横向移动命令,mn下侧一行回到相对位置
        /// </summary>
        /// <param name="mnPosition">mn的坐标</param>
        /// <param name="offset">移动格数</param>
        /// <param name="direction">方向1或-1</param>
        public int LowerEntityTransversePlan(int mnPosition, int offset, int direction)
        {
            return LowerEntityTransversePlan(mnPosition,offset,direction,puzzle.LieShu);
        }
        public int LowerEntityTransversePlan(int offset, int direction)
        {
            return LowerEntityTransversePlan(puzzle.mnPosition, offset, direction,puzzle.LieShu);
        }
        #endregion
        #region 上侧a_j横向移动命令
        /// <summary>
        /// 输出上侧a_j横向移动命令,生成的命令不加入内置的命令队列
        /// </summary>
        /// <param name="mnPosition">mn当前位置</param>
        /// <param name="offset">移动格数</param>
        /// <param name="direction">方向1或-1</param>
        /// <param name="lieShu">列数</param>
        /// <returns>命令队列</returns>
        public Queue<Swap> RiseEntityTransversePlanOut(int mnPosition, int offset, int direction, int lieShu)
        {
            Queue<Swap> command = new Queue<Swap>();
            for (int i = 0; i < offset; i++)
            {
                int j = mnPosition + i * direction - lieShu;
                Swap swap1 = new Swap(j + lieShu, j - direction + lieShu);
                command.Enqueue(swap1);
                Swap swap2 = new Swap(j - direction + lieShu, j - direction);
                command.Enqueue(swap2);
                Swap swap3 = new Swap(j - direction, j);
                command.Enqueue(swap3);
                Swap swap4 = new Swap(j, j + direction);
                command.Enqueue(swap4);
                Swap swap5 = new Swap(j + direction, j + direction + lieShu);
                command.Enqueue(swap5);
            }
            return command;
        }
        /// <summary>
        /// 生成a_j横向移动命令,mn上侧一行回到相对位置
        /// </summary>
        /// <param name="mnPosition">mn的坐标</param>
        /// <param name="offset">移动格数</param>
        /// <param name="direction">方向1或-1</param>
        /// <param name="lieShu">列数</param>
        public int RiseEntityTransversePlan(int mnPosition, int offset, int direction, int lieShu)
        {
            for (int i = 0; i < offset; i++)
            {
                int j = mnPosition + i * direction - lieShu;
                Swap swap1 = new Swap(j + lieShu, j - direction + lieShu);
                Command.Enqueue(swap1);
                Swap swap2 = new Swap(j - direction + lieShu, j - direction);
                Command.Enqueue(swap2);
                Swap swap3 = new Swap(j - direction, j);
                Command.Enqueue(swap3);
                Swap swap4 = new Swap(j, j + direction);
                Command.Enqueue(swap4);
                Swap swap5 = new Swap(j + direction, j + direction + lieShu);
                Command.Enqueue(swap5);
            }
            return mnPosition + offset * direction;
        }
        /// <summary>
        /// 生成a_j横向移动命令,mn上侧一行回到相对位置
        /// </summary>
        /// <param name="mnPosition">mn的坐标</param>
        /// <param name="offset">移动格数</param>
        /// <param name="direction">方向1或-1</param>
        public int RiseEntityTransversePlan(int mnPosition, int offset, int direction)
        {
           return RiseEntityTransversePlan(mnPosition,offset,direction,puzzle.LieShu);
        }
        public int RiseEntityTransversePlan(int offset, int direction)
        {
           return RiseEntityTransversePlan(puzzle.mnPosition, offset, direction,puzzle.LieShu);
        }
        #endregion
        #region 竖侧a_j斜向移动命令
        /// <summary>
        /// 输出竖侧a_j斜向移动命令,生成的命令不加入内置的命令队列
        /// </summary>
        /// <param name="mnPosition">mn当前位置</param>
        /// <param name="offset">移动格数</param>
        /// <param name="directionR">行方向1或-1</param>
        /// <param name="directionC">列方向1或-1</param>
        /// <param name="lieShu">列数</param>
        /// <returns>命令队列</returns>
        public Queue<Swap> RiseEntityObliquePlanOut(int mnPosition, int offset, int directionR, int directionC, int lieShu)
        {
            Queue<Swap> command = new Queue<Swap>();
            int k = lieShu * directionR;
            for (int i = 0; i < offset; i++)
            {
                int j = mnPosition + i * k + i * directionC;
                Swap swap1 = new Swap(j, j - k);
                command.Enqueue(swap1);
                Swap swap2 = new Swap(j - k, j - k + directionC);
                command.Enqueue(swap2);
                Swap swap3 = new Swap(j - k + directionC, j + directionC);
                command.Enqueue(swap3);
                Swap swap4 = new Swap(j + directionC, j);
                command.Enqueue(swap4);
                Swap swap5 = new Swap(j, j + k);
                command.Enqueue(swap5);
                Swap swap6 = new Swap(j + k, j + k + directionC);
                command.Enqueue(swap6);
            }
            return command;
        }
        /// <summary>
        /// 生成a_j斜向移动命令,mn竖侧开始移动
        /// </summary>
        /// <param name="mnPosition">mn的坐标</param>
        /// <param name="offset">移动格数</param>
        /// <param name="directionR">行 1或-1</param>
        /// <param name="directionC">列 1或-1</param>
        /// <param name="lieShu">列数</param>
        public int RiseEntityObliquePlan(int mnPosition, int offset, int directionR, int directionC,int lieShu)
        {
            int k = lieShu * directionR;
            for (int i = 0; i < offset; i++)
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
            return mnPosition + k * offset + offset * directionC;
        }
        /// <summary>
        /// 生成a_j斜向移动命令,mn竖侧开始移动
        /// </summary>
        /// <param name="mnPosition">mn的坐标</param>
        /// <param name="offset">移动格数</param>
        /// <param name="directionR">行 1或-1</param>
        /// <param name="directionC">列 1或-1</param>
        public int RiseEntityObliquePlan(int mnPosition, int offset, int directionR, int directionC)
        {
           return RiseEntityObliquePlan(mnPosition,offset,directionR,directionC,puzzle.LieShu);
        }
        public int RiseEntityObliquePlan(int offset, int directionR, int directionC)
        {
           return RiseEntityObliquePlan(puzzle.mnPosition, offset, directionR, directionC,puzzle.LieShu);
        }
        #endregion
        #region 横侧a_j斜向移动命令
        /// <summary>
        /// 输出横侧a_j斜向移动命令,生成的命令不加入内置的命令队列
        /// </summary>
        /// <param name="mnPosition">mn当前位置</param>
        /// <param name="offset">移动格数</param>
        /// <param name="directionR">行方向1或-1</param>
        /// <param name="directionC">列方向1或-1</param>
        /// <param name="lieShu">列数</param>
        /// <returns>命令队列</returns>
        public Queue<Swap> LateralEntityObliquePlanOut(int mnPosition, int offset, int directionR, int directionC, int lieShu)
        {
            Queue<Swap> command = new Queue<Swap>();
            int k = lieShu * directionR;
            for (int i = 0; i < offset; i++)
            {
                int j = mnPosition + i * k + i * directionC;
                Swap swap1 = new Swap(j, j - directionC);
                command.Enqueue(swap1);
                Swap swap2 = new Swap(j - directionC, j + k - directionC);
                command.Enqueue(swap2);
                Swap swap3 = new Swap(j + k - directionC, j + k);
                command.Enqueue(swap3);
                Swap swap4 = new Swap(j + k, j);
                command.Enqueue(swap4);
                Swap swap5 = new Swap(j, j + directionC);
                command.Enqueue(swap5);
                Swap swap6 = new Swap(j + directionC, j + k + directionC);
                command.Enqueue(swap6);
            }
            return command;
        }
        /// <summary>
        /// 生成a_j斜向移动命令,mn横侧开始移动
        /// </summary>
        /// <param name="mnPosition">mn的坐标</param>
        /// <param name="offset">移动格数</param>
        /// <param name="directionR">行 1或-1</param>
        /// <param name="directionC">列 1或-1</param>
        /// <param name="lieShu">列数</param>
        public int LateralEntityObliquePlan(int mnPosition, int offset, int directionR, int directionC,int lieShu)
        {
            int k = lieShu * directionR;
            for (int i = 0; i < offset; i++)
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
            return mnPosition + offset * k + offset * directionC;
        }
        /// <summary>
        /// 生成a_j斜向移动命令,mn横侧开始移动
        /// </summary>
        /// <param name="mnPosition">mn的坐标</param>
        /// <param name="offset">移动格数</param>
        /// <param name="directionR">行 1或-1</param>
        /// <param name="directionC">列 1或-1</param>
        public int LateralEntityObliquePlan(int mnPosition, int offset, int directionR, int directionC)
        {
           return LateralEntityObliquePlan(mnPosition,offset,directionR,directionC,puzzle.LieShu);
        }
        public int LateralEntityObliquePlan(int offset, int directionR, int directionC)
        {
           return LateralEntityObliquePlan(puzzle.mnPosition, offset, directionR, directionC,puzzle.LieShu);
        }
        #endregion
        #endregion

        #region 生成复合命令
        #region 生成mn到目标位置的命令
        /// <summary>
        /// 移动mn到目标位置,先竖移再横移
        /// </summary>
        /// <param name="mnPosition">mn位置</param>
        /// <param name="target">目标位置</param>
        /// <param name="lieShu">列数</param>
        public void EmptyToVt(int mnPosition, int target ,int lieShu)
        {
            PointOffset po = new PointOffset(mnPosition, target, lieShu);
            EmptyVerticalPlan(mnPosition, po.Offset.Y, po.Direction.Y);
            EmptyTransversePlan(mnPosition + lieShu * po.Offset.Y * po.Direction.Y, po.Offset.X, po.Direction.X);
        }
        /// <summary>
        /// 移动mn到目标位置,先竖移再横移
        /// </summary>
        /// <param name="mnPosition">mn位置</param>
        /// <param name="target">目标位置</param>
        public void EmptyToVt(int mnPosition,int target)
        {
            EmptyToVt(mnPosition,target,puzzle.LieShu);
        }
        public void EmptyToVt(int target)
        {
            EmptyToVt(puzzle.mnPosition,target,puzzle.LieShu);
        }
        #endregion
        #region 生成mn到目标位置的命令
        /// <summary>
        /// 移动mn到目标位置,先横移再竖移
        /// </summary>
        /// <param name="mnPosition">mn位置</param>
        /// <param name="target">目标位置</param>
        /// <param name="lieShu">目标位置</param>
        public void EmptyToTv(int mnPosition, int target ,int lieShu)
        {
            PointOffset po = new PointOffset(mnPosition, target, lieShu);
            EmptyTransversePlan(mnPosition, po.Offset.X, po.Direction.X);
            EmptyVerticalPlan(mnPosition + po.Offset.X * po.Direction.X, po.Offset.Y, po.Direction.Y,lieShu);
        }
        /// <summary>
        /// 移动mn到目标位置,先横移再竖移
        /// </summary>
        /// <param name="mnPosition">mn位置</param>
        /// <param name="target">目标位置</param>
        public void EmptyToTv(int mnPosition, int target)
        {
            EmptyToTv(mnPosition,target,puzzle.LieShu);
        }
        public void EmptyToTv(int target)
        {
            EmptyToTv(puzzle.mnPosition,target,puzzle.LieShu);
        }
        #endregion
        #endregion

        #region 执行命令
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="command">命令队列</param>
        public void ExecutePlan(Queue<Swap> command)
        {
            int count = command.Count;
            for (int i = 0; i < count; i++)
            {
                puzzle.SwapAction(command.Dequeue());
            }
        }
        public void ExecutePlan()
        {
            ExecutePlan(Command);
        }

        /// <summary>
        /// 快速执行命令，复原拼图时不会更新拼图的状态
        /// </summary>
        /// <param name="command">命令队列</param>
        public void ExecutePlanFast(Queue<Swap> command)
        {
            int count = command.Count;
            for (int i = 0; i < count; i++)
            {
                puzzle.Swap(command.Dequeue());
            }
        }
        public void ExecutePlanFast()
        {
            ExecutePlanFast(Command);
        }
        #endregion

    }
}
