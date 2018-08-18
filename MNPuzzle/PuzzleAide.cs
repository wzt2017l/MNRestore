using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
        LateralEntityOblique
    }
    /// <summary>
    /// 拼图助手
    /// </summary>
   public class PuzzleAide
   {
        public Queue<Swap> Command { get; set; }//命令队列
        public Puzzle puzzle { get; private set; }
        public long StepNum { get; private set; }

        public PuzzleAide() { }
        public PuzzleAide(Puzzle puzzle)
        {
            this.puzzle = puzzle;
            Command = new Queue<Swap>();
            StepNum = 0;
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
                int[] rans = new int[2];
                for (int m = 0; m < 2; m++)
                {
                    byte[] ranBytes = new byte[4];
                    RNGCryptoServiceProvider rngServiceProvider = new RNGCryptoServiceProvider();
                    rngServiceProvider.GetBytes(ranBytes);
                    rans[m] = Math.Abs(BitConverter.ToInt32(ranBytes, 0));
                }
                k = Math.Abs(rans[0]%(i+1));
                j = Math.Abs(rans[1]%(Total-i))+i;
                if (k != j)
                {
                    puzzle.Swap(k, j);
                }
            });
            puzzle.Swap(Total - 1, r.Next(0, Total - 1));
        }
        public void Disrupt()
        {
            Disrupt(this.puzzle);
        }
        /// <summary>
        /// 打乱拼图，且使拼图可还原
        /// </summary>
        /// <param name="puzzle">被打乱的拼图</param>
        /// <returns>逆序数</returns>
        public long DisruptReducible(Puzzle puzzle)
        {
            Disrupt(puzzle);
            int Total = puzzle.Total;
            puzzle.RetryNiXu();//重算逆序
            puzzle.MnPosition();
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
            return puzzle.NiXu;
        }
        public long DisruptReducible()
        {
           StepNum = 0;
           return DisruptReducible(this.puzzle);
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
        public Position PositionAnalysis(int origin, int target)
        {
            return PositionAnalysis(new PointOffset(origin, target, this.puzzle.LieShu));
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
        /// 移动mn，输出竖向移动命令,生成的命令不加入内置的命令队列
        /// </summary>
        /// <param name="command">被填充的命令队列</param>
        /// <param name="mnPosition">mn当前位置</param>
        /// <param name="offset">偏移量</param>
        /// <param name="direction">方向1或-1</param>
        /// <param name="lieShu">列数</param>
        /// <returns>mn移动后的位置</returns>
        public int EmptyVerticalPlan(Queue<Swap> command ,int mnPosition, int offset, int direction, int lieShu)
        {
            int k = lieShu * direction;
            for (int i = 0; i < offset; i++)
            {
                int j = mnPosition + i * k;//j是当前循环mn的初始位置
                Swap swap = new Swap(j, j + lieShu * direction);
                command.Enqueue(swap);
            }
            return mnPosition + k * offset;
        }
        public int EmptyVerticalPlan(int mnPosition, int offset, int direction ,int lieShu)
        {
            return EmptyVerticalPlan(this.Command, mnPosition, offset, direction, lieShu);
        }
        public int EmptyVerticalPlan(int mnPosition, int offset, int direction)
        {
           return EmptyVerticalPlan(this.Command,mnPosition,offset,direction,puzzle.LieShu);
        }
        public int EmptyVerticalPlan(int offset, int direction)
        {
           return EmptyVerticalPlan(this.Command,puzzle.mnPosition, offset, direction,puzzle.LieShu);
        }
        #endregion
        #region mn横向移动命令
        /// <summary>
        /// 输出横向移动命令,生成的命令不加入内置的命令队列
        /// </summary>
        /// <param name="command">被填充的命令队列</param>
        /// <param name="mnPosition">mn当前位置</param>
        /// <param name="offset">移动格数</param>
        /// <param name="direction">方向-1或1</param>
        /// <returns>mn移动后的位置</returns>
        public int EmptyTransversePlan(Queue<Swap> command,int mnPosition, int offset, int direction)
        {
            for (int i = 0; i < offset; i++)
            {
                int j = mnPosition + i * direction;
                Swap swap = new Swap(j, j + direction);
                command.Enqueue(swap);
            }
            return mnPosition + offset * direction;
        }
        public int EmptyTransversePlan(int mnPosition, int offset, int direction)
        {
            return EmptyTransversePlan(this.Command,mnPosition,offset,direction);
        }
        public int EmptyTransversePlan(int offset, int direction)
        {
           return EmptyTransversePlan(this.Command,puzzle.mnPosition, offset, direction);
        }
        #endregion

        #region 右侧a_j竖向移动命令
        /// <summary>
        /// 输出右侧a_j竖向移动命令,生成的命令不加入内置的命令队列
        /// </summary>
        /// <param name="command">被填充的命令队列</param>
        /// <param name="mnPosition">mn当前位置</param>
        /// <param name="offset">移动格数</param>
        /// <param name="direction">方向1或-1</param>
        /// <param name="lieShu">列数</param>
        /// <returns>mn移动后的位置</returns>
        public int RigthEntityVerticalPlan(Queue<Swap> command,int mnPosition, int offset, int direction, int lieShu)
        {
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
            return mnPosition + offset * k;
        }
        public int RigthEntityVerticalPlan(int mnPosition, int offset, int direction ,int lieShu)
        {
            return RigthEntityVerticalPlan(this.Command,mnPosition,offset,direction,lieShu);
        }
        public int RigthEntityVerticalPlan(int mnPosition, int offset, int direction)
        {
           return RigthEntityVerticalPlan(this.Command,mnPosition,offset,direction,puzzle.LieShu);
        }
        public int RigthEntityVerticalPlan(int offset, int direction)
        {
           return RigthEntityVerticalPlan(this.Command,puzzle.mnPosition, offset, direction,puzzle.LieShu);
        }
        #endregion
        #region 左侧a_j竖向移动命令
        /// <summary>
        ///  输出左侧a_j竖向移动命令,生成的命令不加入内置的命令队列
        /// </summary>
        /// <param name="command">被填充的命令队列</param>
        /// <param name="mnPosition">mn当前位置</param>
        /// <param name="offset">移动格数</param>
        /// <param name="direction">方向1或-1</param>
        /// <param name="lieShu">列数</param>
        /// <returns>mn移动后的位置</returns>
        public int LeftEntityVerticalPlan(Queue<Swap> command,int mnPosition, int offset, int direction, int lieShu)
        {
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
            return mnPosition + offset * k;
        }
        public int LeftEntityVerticalPlan(int mnPosition, int offset, int direction,int lieShu)
        {
            return LeftEntityVerticalPlan(this.Command,mnPosition,offset,direction,lieShu);
        }
        public int LeftEntityVerticalPlan(int mnPosition, int offset, int direction)
        {
           return LeftEntityVerticalPlan(this.Command,mnPosition,offset,direction,puzzle.LieShu);
        }
        public int LeftEntityVerticalPlan(int offset, int direction)
        {
           return LeftEntityVerticalPlan(this.Command,puzzle.mnPosition, offset, direction,puzzle.LieShu);
        }
        #endregion

        #region 下侧a_j横向移动命令
        /// <summary>
        /// 输出下侧a_j横向移动命令,生成的命令不加入内置的命令队列
        /// </summary>
        /// <param name="command">被填充的命令队列</param>
        /// <param name="mnPosition">mn当前位置</param>
        /// <param name="offset">移动格数</param>
        /// <param name="direction">方向1或-1</param>
        /// <param name="lieShu">列数</param>
        /// <returns>mn移动后的位置</returns>
        public int LowerEntityTransversePlan(Queue<Swap> command,int mnPosition, int offset, int direction, int lieShu)
        {
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
            return mnPosition + offset * direction;
        }
        public int LowerEntityTransversePlan(int mnPosition, int offset, int direction ,int lieShu)
        {
            return LowerEntityTransversePlan(this.Command, mnPosition, offset, direction, lieShu);
        }
        public int LowerEntityTransversePlan(int mnPosition, int offset, int direction)
        {
            return LowerEntityTransversePlan(this.Command,mnPosition,offset,direction,puzzle.LieShu);
        }
        public int LowerEntityTransversePlan(int offset, int direction)
        {
            return LowerEntityTransversePlan(this.Command,puzzle.mnPosition, offset, direction,puzzle.LieShu);
        }
        #endregion
        #region 上侧a_j横向移动命令
        /// <summary>
        /// 输出上侧a_j横向移动命令,生成的命令不加入内置的命令队列
        /// </summary>
        /// <param name="command">被填充的命令队列</param>
        /// <param name="mnPosition">mn当前位置</param>
        /// <param name="offset">移动格数</param>
        /// <param name="direction">方向1或-1</param>
        /// <param name="lieShu">列数</param>
        /// <returns>mn移动后的位置</returns>
        public int RiseEntityTransversePlan(Queue<Swap> command,int mnPosition, int offset, int direction, int lieShu)
        {
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
            return mnPosition + offset * direction;
        }
        public int RiseEntityTransversePlan(int mnPosition, int offset, int direction, int lieShu)
        {
            return RiseEntityTransversePlan(this.Command,mnPosition,offset,direction,lieShu);
        }
        public int RiseEntityTransversePlan(int mnPosition, int offset, int direction)
        {
           return RiseEntityTransversePlan(this.Command,mnPosition,offset,direction,puzzle.LieShu);
        }
        public int RiseEntityTransversePlan(int offset, int direction)
        {
           return RiseEntityTransversePlan(this.Command,puzzle.mnPosition, offset, direction,puzzle.LieShu);
        }
        #endregion

        #region 竖侧a_j斜向移动命令
        /// <summary>
        /// 输出竖侧a_j斜向移动命令,生成的命令不加入内置的命令队列
        /// </summary>
        /// <param name="command">被填充的命令队列</param>
        /// <param name="mnPosition">mn当前位置</param>
        /// <param name="offset">移动格数</param>
        /// <param name="directionR">行方向1或-1</param>
        /// <param name="directionC">列方向1或-1</param>
        /// <param name="lieShu">列数</param>
        /// <returns>mn移动后的位置</returns>
        public int RiseEntityObliquePlan(Queue<Swap> command,int mnPosition, int offset, int directionR, int directionC, int lieShu)
        {
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
            return mnPosition + k * offset + offset * directionC;
        }
        public int RiseEntityObliquePlan(int mnPosition, int offset, int directionR, int directionC,int lieShu)
        {
            return RiseEntityObliquePlan(this.Command, mnPosition, offset, directionR, directionC, lieShu);
        }
        public int RiseEntityObliquePlan(int mnPosition, int offset, int directionR, int directionC)
        {
           return RiseEntityObliquePlan(this.Command,mnPosition,offset,directionR,directionC,puzzle.LieShu);
        }
        public int RiseEntityObliquePlan(int offset, int directionR, int directionC)
        {
           return RiseEntityObliquePlan(this.Command,puzzle.mnPosition, offset, directionR, directionC,puzzle.LieShu);
        }
        #endregion
        #region 横侧a_j斜向移动命令
        /// <summary>
        /// 输出横侧a_j斜向移动命令,生成的命令不加入内置的命令队列
        /// </summary>
        /// <param name="command">被填充的命令队列</param>
        /// <param name="mnPosition">mn当前位置</param>
        /// <param name="offset">移动格数</param>
        /// <param name="directionR">行方向1或-1</param>
        /// <param name="directionC">列方向1或-1</param>
        /// <param name="lieShu">列数</param>
        /// <returns>mn移动后的位置</returns>
        public int LateralEntityObliquePlan(Queue<Swap> command,int mnPosition, int offset, int directionR, int directionC, int lieShu)
        {
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
            return mnPosition + offset * k + offset * directionC;
        }
        public int LateralEntityObliquePlan(int mnPosition, int offset, int directionR, int directionC,int lieShu)
        {
            return LateralEntityObliquePlan(this.Command,mnPosition,offset,directionR,directionC,lieShu);
        }
        public int LateralEntityObliquePlan(int mnPosition, int offset, int directionR, int directionC)
        {
           return LateralEntityObliquePlan(this.Command,mnPosition,offset,directionR,directionC,puzzle.LieShu);
        }
        public int LateralEntityObliquePlan(int offset, int directionR, int directionC)
        {
           return LateralEntityObliquePlan(this.Command,puzzle.mnPosition, offset, directionR, directionC,puzzle.LieShu);
        }
        #endregion
        #endregion

        #region 生成复合命令
        #region 生成mn到目标位置的命令,先竖移
        /// <summary>
        /// 移动mn到目标位置,先竖移再横移
        /// </summary>
        /// <param name="command">要填充的命令队列</param>
        /// <param name="mnPosition">mn位置</param>
        /// <param name="target">目标位置</param>
        /// <param name="lieShu">列数</param>
        /// <returns>mn移动后的位置</returns>
        public int EmptyToVt(Queue<Swap> command, int mnPosition, int target, int lieShu)
        {
            PointOffset po = new PointOffset(mnPosition, target, lieShu);
            int mnPos= EmptyVerticalPlan(command,mnPosition, po.Offset.Y, po.Direction.Y,lieShu);
            return EmptyTransversePlan(command,mnPos, po.Offset.X, po.Direction.X);
        }
        public int EmptyToVt(int mnPosition, int target ,int lieShu)
        {
            return EmptyToVt(this.Command,mnPosition,target,lieShu);
        }
        public int EmptyToVt(int mnPosition,int target)
        {
           return EmptyToVt(this.Command,mnPosition,target,puzzle.LieShu);
        }
        public int EmptyToVt(int target)
        {
           return EmptyToVt(this.Command,puzzle.mnPosition,target,puzzle.LieShu);
        }
        #endregion
        #region 生成mn到目标位置的命令,先横移
        /// <summary>
        /// 移动mn到目标位置,先横移再竖移
        /// </summary>
        /// <param name="mnPosition">mn位置</param>
        /// <param name="target">目标位置</param>
        /// <param name="lieShu">列数</param>
        /// <returns>mn移动后的位置</returns>
        public int EmptyToTv(Queue<Swap> command,int mnPosition, int target, int lieShu)
        {
            PointOffset po = new PointOffset(mnPosition, target, lieShu);
            int mnPos= EmptyTransversePlan(command,mnPosition, po.Offset.X, po.Direction.X);
            return EmptyVerticalPlan(command,mnPos, po.Offset.Y, po.Direction.Y, lieShu);
        }
        public int EmptyToTv(int mnPosition, int target ,int lieShu)
        {
            return EmptyToTv(this.Command,mnPosition,target,lieShu);
        }
        public int EmptyToTv(int mnPosition, int target)
        {
           return EmptyToTv(this.Command,mnPosition,target,puzzle.LieShu);
        }
        public int EmptyToTv(int target)
        {
           return EmptyToTv(this.Command,puzzle.mnPosition,target,puzzle.LieShu);
        }
        #endregion

        #region 生成mn到目标位置上方的命令，先竖移
        /// <summary>
        /// 生成mn到目标位置上方的命令，且mn不能经过位置target,先竖移
        /// </summary>
        /// <param name="command">存放命令的队列</param>
        /// <param name="mnPosition">mn当前的位置</param>
        /// <param name="target">目标位置</param>
        /// <param name="lieShu">列数</param>
        /// <param name="rOrL">如果mn在目标正下方，则有两种可选移动方式，默认值为"Right"，如果rOrL！= "Right"则选择另一种方式</param>
        /// <returns>mn移动后的位置</returns>
        public int EmptyToVtUp(Queue<Swap> command, int mnPosition, int target, int lieShu,string rOrL= "Right")
        {
            if (mnPosition>target&&mnPosition%lieShu==target%lieShu)
            {
                int y1 = (mnPosition - mnPosition % lieShu) / lieShu;
                int y2 = (target - target % lieShu) / lieShu;
                int mnPos = EmptyVerticalPlan(command, mnPosition, y1 - y2 - 1, -1, lieShu);
                if (rOrL== "Right")
                {
                    command.Enqueue(new Swap(mnPos, mnPos + 1));
                    command.Enqueue(new Swap(mnPos + 1, mnPos + 1 - lieShu));
                    command.Enqueue(new Swap(mnPos + 1 - lieShu, mnPos + 1 - 2 * lieShu));
                    command.Enqueue(new Swap(mnPos + 1 - 2 * lieShu, mnPos - 2 * lieShu));
                }
                else
                {
                    command.Enqueue(new Swap(mnPos, mnPos - 1));
                    command.Enqueue(new Swap(mnPos - 1, mnPos - 1 - lieShu));
                    command.Enqueue(new Swap(mnPos - 1 - lieShu, mnPos - 1 - 2 * lieShu));
                    command.Enqueue(new Swap(mnPos - 1 - 2 * lieShu, mnPos - 2 * lieShu));
                }
                return target - lieShu;
            }
            else
            {
                return EmptyToVt(command,mnPosition,target-lieShu,lieShu);
            }
        }
        public int EmptyToVtUp( int mnPosition, int target, int lieShu, string rOrL = "Right")
        {
            return EmptyToVtUp(this.Command, mnPosition, target, lieShu,  rOrL);
        }
        public int EmptyToVtUp(int mnPosition, int target, string rOrL = "Right")
        {
            return EmptyToVtUp(this.Command, mnPosition, target, puzzle.LieShu, rOrL);
        }
        public int EmptyToVtUp( int target, string rOrL = "Right")
        {
            return EmptyToVtUp(this.Command, puzzle.mnPosition, target, puzzle.LieShu, rOrL);
        }
        #endregion
        #region 生成mn到目标位置上方的命令，先横移
        /// <summary>
        /// 生成mn到目标位置上方的命令，且mn不能经过位置target,先横移
        /// </summary>
        /// <param name="command">存放命令的队列</param>
        /// <param name="mnPosition">mn当前的位置</param>
        /// <param name="target">目标位置</param>
        /// <param name="lieShu">列数</param>
        /// <param name="rOrL">如果mn在目标正下方，则有两种可选移动方式，默认值为"Right"，如果rOrL！= "Right"则选择另一种方式</param>
        /// <returns>mn移动后的位置</returns>
        public int EmptyToTvUp(Queue<Swap> command, int mnPosition, int target, int lieShu, string rOrL = "Right")
        {
            int x1 = mnPosition % lieShu;
            int y1 = (mnPosition - x1) / lieShu;
            int x2 = target % lieShu;
            int y2 = (target - x2) / lieShu;
            int mnPos = 0;
            if (y1 > y2 && x1 == x2)//正上
            {
                mnPos = EmptyVerticalPlan(command, mnPosition, y1 - y2 - 1, -1, lieShu);
                if (rOrL == "Right")
                {
                    command.Enqueue(new Swap(mnPos, mnPos + 1));
                    command.Enqueue(new Swap(mnPos + 1, mnPos + 1 - lieShu));
                    command.Enqueue(new Swap(mnPos + 1 - lieShu, mnPos + 1 - 2 * lieShu));
                    command.Enqueue(new Swap(mnPos + 1 - 2 * lieShu, mnPos - 2 * lieShu));
                }
                else
                {
                    command.Enqueue(new Swap(mnPos, mnPos - 1));
                    command.Enqueue(new Swap(mnPos - 1, mnPos - 1 - lieShu));
                    command.Enqueue(new Swap(mnPos - 1 - lieShu, mnPos - 1 - 2 * lieShu));
                    command.Enqueue(new Swap(mnPos - 1 - 2 * lieShu, mnPos - 2 * lieShu));
                }
                return target - lieShu;
            }
            else
            if (y1>=y2)//
            {
                if (x1>x2)//偏左上，正左
                {
                    mnPos = EmptyTransversePlan(command, mnPosition,x1-x2-1,-1);
                    mnPos = EmptyVerticalPlan(command,mnPos,y1-y2+1,-1,lieShu);
                    command.Enqueue(new Swap(mnPos,mnPos-1));
                }
                else
                if (x1<x2)//偏右上，正右
                {
                    mnPos = EmptyTransversePlan(command, mnPosition, x2 - x1 - 1, 1);
                    mnPos = EmptyVerticalPlan(command,mnPos,y1-y2+1,-1,lieShu);
                    command.Enqueue(new Swap(mnPos, mnPos +1));
                }
                return target - lieShu;
            }
            else
            {
                return EmptyToTv(command,mnPosition,target-lieShu,lieShu);
            }
        }
        public int EmptyToTvUp(int mnPosition, int target, int lieShu, string rOrL = "Right")
        {
            return EmptyToTvUp(this.Command, mnPosition, target, lieShu, rOrL);
        }
        public int EmptyToTvUp(int mnPosition, int target, string rOrL = "Right")
        {
            return EmptyToTvUp(this.Command, mnPosition, target, puzzle.LieShu, rOrL);
        }
        public int EmptyToTvUp(int target, string rOrL = "Right")
        {
            return EmptyToTvUp(this.Command, puzzle.mnPosition, target, puzzle.LieShu, rOrL);
        }
        #endregion

        #region 生成mn到目标位置左侧的命令，先竖移
        /// <summary>
        /// 生成mn到目标位置左侧的命令，且mn不能经过位置target，先竖移
        /// </summary>
        /// <param name="command">存放命令的队列</param>
        /// <param name="mnPosition">mn当前位置</param>
        /// <param name="target">目标位置</param>
        /// <param name="lieShu">列数</param>
        /// <param name="upOrdown">如果mn在目标正右方，则有两种可选移动方式，默认值为"Down"，如果upOrdown！= "Down"则选择另一种方式</param>
        /// <returns>mn移动后的位置</returns>
        public int EmptyToVtLeft(Queue<Swap> command, int mnPosition, int target, int lieShu, string upOrdown = "Down")
        {
            int x1 = mnPosition % lieShu;
            int y1 = (mnPosition - x1) / lieShu;
            int x2 = target % lieShu;
            int y2 = (target - x2) / lieShu;
            int mnPos = 0;
            if (y1==y2&&x1>x2)
            {
                mnPos = EmptyTransversePlan(command,mnPosition,x1-x2-1,-1);
                if (upOrdown=="Down")
                {
                    command.Enqueue(new Swap(mnPos,mnPos+lieShu));
                    command.Enqueue(new Swap(mnPos+lieShu, mnPos + lieShu-1));
                    command.Enqueue(new Swap(mnPos+lieShu-1, mnPos + lieShu-2));
                    command.Enqueue(new Swap(mnPos+lieShu-2, mnPos-2));
                }
                else
                {
                    command.Enqueue(new Swap(mnPos, mnPos - lieShu));
                    command.Enqueue(new Swap(mnPos - lieShu, mnPos - lieShu - 1));
                    command.Enqueue(new Swap(mnPos - lieShu - 1, mnPos - lieShu - 2));
                    command.Enqueue(new Swap(mnPos - lieShu - 2, mnPos - 2));
                }
                return mnPos - 2;
            }
            else
            if (x1>=x2)
            {
                if (y1>y2)
                {
                    mnPos = EmptyVerticalPlan(command,mnPosition,y1-y2-1,-1,lieShu);
                    mnPos = EmptyTransversePlan(command,mnPos,x1-x2+1,-1);
                    command.Enqueue(new Swap(mnPos,mnPos-lieShu));
                }
                else
                if(y1<y2)
                {
                    mnPos = EmptyVerticalPlan(command, mnPosition, y2 - y1 - 1, 1, lieShu);
                    mnPos = EmptyTransversePlan(command, mnPos, x1 - x2 + 1, -1);
                    command.Enqueue(new Swap(mnPos, mnPos +lieShu));
                }
                return target - 1;
            }
            else
            {
                return EmptyToVt(command,mnPosition,target-1,lieShu);
            }
            
        }
        public int EmptyToVtLeft(int mnPosition, int target, int lieShu, string upOrdown = "Down")
        {
            return EmptyToVtLeft(this.Command,mnPosition,target,lieShu,upOrdown);
        }
        public int EmptyToVtLeft(int mnPosition, int target, string upOrdown = "Down")
        {
            return EmptyToVtLeft(this.Command, mnPosition, target, puzzle.LieShu, upOrdown);
        }
        public int EmptyToVtLeft(int target, string upOrdown = "Down")
        {
            return EmptyToVtLeft(this.Command,puzzle.mnPosition, target, puzzle.LieShu, upOrdown);
        }
        #endregion
        #region 生成mn到目标位置左侧的命令，先横移
        /// <summary>
        /// 生成mn到目标位置左侧的命令，且mn不能经过位置target，先横移
        /// </summary>
        /// <param name="command">存放命令的队列</param>
        /// <param name="mnPosition">mn当前位置</param>
        /// <param name="target">目标位置</param>
        /// <param name="lieShu">列数</param>
        /// <param name="upOrdown">如果mn在目标正右方，则有两种可选移动方式，默认值为"Down"，如果upOrdown！= "Down"则选择另一种方式</param>
        /// <returns>mn移动后的位置</returns>
        public int EmptyToTvLeft(Queue<Swap> command, int mnPosition, int target, int lieShu, string upOrdown = "Down")
        {
            if (target<mnPosition&&(target-target%lieShu)/lieShu==(mnPosition-mnPosition%lieShu)/lieShu)
            {
                int mnPos = EmptyTransversePlan(command, mnPosition, mnPosition%lieShu - target%lieShu - 1, -1);
                if (upOrdown == "Down")
                {
                    command.Enqueue(new Swap(mnPos, mnPos + lieShu));
                    command.Enqueue(new Swap(mnPos + lieShu, mnPos + lieShu - 1));
                    command.Enqueue(new Swap(mnPos + lieShu - 1, mnPos + lieShu - 2));
                    command.Enqueue(new Swap(mnPos + lieShu - 2, mnPos - 2));
                }
                else
                {
                    command.Enqueue(new Swap(mnPos, mnPos - lieShu));
                    command.Enqueue(new Swap(mnPos - lieShu, mnPos - lieShu - 1));
                    command.Enqueue(new Swap(mnPos - lieShu - 1, mnPos - lieShu - 2));
                    command.Enqueue(new Swap(mnPos - lieShu - 2, mnPos - 2));
                }
                return mnPos - 2;
            }
            else
            {
                return EmptyToTv(command,mnPosition,target-1,lieShu);
            }
        }
        public int EmptyToTvLeft(int mnPosition, int target, int lieShu, string upOrdown = "Down")
        {
            return EmptyToTvLeft(this.Command,mnPosition,target,lieShu,upOrdown);
        }
        public int EmptyToTvLeft(int mnPosition, int target, string upOrdown = "Down")
        {
            return EmptyToTvLeft(this.Command, mnPosition, target, puzzle.LieShu, upOrdown);
        }
        public int EmptyToTvLeft(int target, string upOrdown = "Down")
        {
            return EmptyToTvLeft(this.Command, puzzle.mnPosition, target, puzzle.LieShu, upOrdown);
        }
        #endregion

        #region 生成mn到目标位置下侧的命令，先竖移
        /// <summary>
        /// 生成mn到目标位置下侧的命令，且mn不能经过位置target，先竖移
        /// </summary>
        /// <param name="command">存放命令的队列</param>
        /// <param name="mnPosition">mn当前位置</param>
        /// <param name="target">目标位置</param>
        /// <param name="lieShu">列数</param>
        /// <param name="upOrdown">如果mn在目标正上方，则有两种可选移动方式，默认值为"Right"，如果rOrL = "Right"则选择另一种方式</param>
        /// <returns>mn移动后的位置</returns>
        public int EmptyToVtDown(Queue<Swap> command, int mnPosition, int target, int lieShu, string rOrL = "Right")
        {
            if (mnPosition<target&&mnPosition%lieShu==target%lieShu)
            {
                int y1 = (mnPosition - mnPosition % lieShu) / lieShu;
                int y2 = (target - target % lieShu) / lieShu;
                int mnPos = EmptyVerticalPlan(command,mnPosition,y2-y1-1,1,lieShu);
                if (rOrL=="Right")
                {
                    command.Enqueue(new Swap(mnPos,mnPos+1));
                    command.Enqueue(new Swap(mnPos+1,mnPos+1+lieShu));
                    command.Enqueue(new Swap(mnPos+1+lieShu,mnPos+1+2*lieShu));
                    command.Enqueue(new Swap(mnPos + 1 + 2 * lieShu, mnPos + 2 * lieShu));
                }
                else
                {
                    command.Enqueue(new Swap(mnPos, mnPos - 1));
                    command.Enqueue(new Swap(mnPos - 1, mnPos - 1 + lieShu));
                    command.Enqueue(new Swap(mnPos - 1 + lieShu, mnPos - 1 + 2 * lieShu));
                    command.Enqueue(new Swap(mnPos - 1 + 2 * lieShu, mnPos + 2 * lieShu));
                }
                return target + lieShu;
            }
            else
            {
                return EmptyToVt(command,mnPosition,target+lieShu,lieShu);
            }
        }
        public int EmptyToVtDown(int mnPosition, int target, int lieShu, string rOrL = "Right")
        {
            return EmptyToVtDown(this.Command,mnPosition,target,lieShu,rOrL);
        }
        public int EmptyToVtDown(int mnPosition, int target, string rOrL = "Right")
        {
            return EmptyToVtDown(this.Command, mnPosition, target, this.puzzle.LieShu, rOrL);
        }
        public int EmptyToVtDown( int target, string rOrL = "Right")
        {
            return EmptyToVtDown(this.Command, this.puzzle.mnPosition, target, this.puzzle.LieShu, rOrL);
        }
        #endregion
        #region 生成mn到目标位置下侧的命令，先横移
        /// <summary>
        /// 生成mn到目标位置下方的命令，且mn不能经过位置target,先横移
        /// </summary>
        /// <param name="command">存放命令的队列</param>
        /// <param name="mnPosition">mn当前的位置</param>
        /// <param name="target">目标位置</param>
        /// <param name="lieShu">列数</param>
        /// <param name="rOrL">如果mn在目标正上方，则有两种可选移动方式，默认值为"Right"，如果rOrL！= "Right"则选择另一种方式</param>
        /// <returns>mn移动后的位置</returns>
        public int EmptyToTvDown(Queue<Swap> command, int mnPosition, int target, int lieShu, string rOrL = "Right")
        {
            int x1 = mnPosition % lieShu;
            int y1 = (mnPosition - x1) / lieShu;
            int x2 = target % lieShu;
            int y2 = (target - x2) / lieShu;
            int mnPos = 0;
            if (y2 > y1 && x1 == x2)
            {
                mnPos = EmptyVerticalPlan(command, mnPosition, y2-y1 - 1, 1, lieShu);
                if (rOrL == "Right")
                {
                    command.Enqueue(new Swap(mnPos, mnPos + 1));
                    command.Enqueue(new Swap(mnPos + 1, mnPos + 1 + lieShu));
                    command.Enqueue(new Swap(mnPos + 1 + lieShu, mnPos + 1 + 2 * lieShu));
                    command.Enqueue(new Swap(mnPos + 1 + 2 * lieShu, mnPos + 2 * lieShu));
                }
                else
                {
                    command.Enqueue(new Swap(mnPos, mnPos - 1));
                    command.Enqueue(new Swap(mnPos - 1, mnPos - 1 + lieShu));
                    command.Enqueue(new Swap(mnPos - 1 + lieShu, mnPos - 1 + 2 * lieShu));
                    command.Enqueue(new Swap(mnPos - 1 + 2 * lieShu, mnPos + 2 * lieShu));
                }
                return target + lieShu;
            }
            else
            if (y2 >= y1)//
            {
                if (x1 >x2)//偏左下，正左
                {
                    mnPos = EmptyTransversePlan(command, mnPosition, x1-x2 - 1, -1);
                    mnPos = EmptyVerticalPlan(command, mnPos, y2-y1 + 1, 1, lieShu);
                    command.Enqueue(new Swap(mnPos, mnPos - 1));
                }
                else
                if (x1 < x2)//偏右上，正右
                {
                    mnPos = EmptyTransversePlan(command, mnPosition, x2 - x1 - 1, 1);
                    mnPos = EmptyVerticalPlan(command, mnPos, y2-y1 + 1, 1, lieShu);
                    command.Enqueue(new Swap(mnPos, mnPos + 1));
                }
                return target + lieShu;
            }
            else
            {
                return EmptyToTv(command, mnPosition, target + lieShu, lieShu);
            }
        }
        public int EmptyToTvDown( int mnPosition, int target, int lieShu, string rOrL = "Right")
        {
            return EmptyToTvDown(this.Command, mnPosition, target,  lieShu, rOrL);
        }
        public int EmptyToTvDown(int mnPosition, int target, string rOrL = "Right")
        {
            return EmptyToTvDown(this.Command, mnPosition, target, this.puzzle.LieShu, rOrL);
        }
        public int EmptyToTvDown(int target, string rOrL = "Right")
        {
            return EmptyToTvDown(this.Command,this.puzzle.mnPosition, target, this.puzzle.LieShu, rOrL);
        }
        #endregion

        #region 生成mn到目标位置右侧的命令，先竖移
        /// <summary>
        /// 生成mn到目标位置右侧的命令，且mn不能经过位置target，先竖移
        /// </summary>
        /// <param name="command">存放命令的队列</param>
        /// <param name="mnPosition">mn当前位置</param>
        /// <param name="target">目标位置</param>
        /// <param name="lieShu">列数</param>
        /// <param name="upOrdown">如果mn在目标正右方，则有两种可选移动方式，默认值为"Down"，如果upOrdown！= "Down"则选择另一种方式</param>
        /// <returns>mn移动后的位置</returns>
        public int EmptyToVtRight(Queue<Swap> command, int mnPosition, int target, int lieShu, string upOrdown = "Down")
        {
            int x1 = mnPosition % lieShu;
            int y1 = (mnPosition - x1) / lieShu;
            int x2 = target % lieShu;
            int y2 = (target - x2) / lieShu;
            int mnPos = 0;
            if (y1 == y2 && x2 > x1)
            {
                mnPos = EmptyTransversePlan(command, mnPosition, x2-x1-1, 1);
                if (upOrdown == "Down")
                {
                    command.Enqueue(new Swap(mnPos, mnPos + lieShu));
                    command.Enqueue(new Swap(mnPos + lieShu, mnPos + lieShu + 1));
                    command.Enqueue(new Swap(mnPos + lieShu + 1, mnPos + lieShu + 2));
                    command.Enqueue(new Swap(mnPos + lieShu + 2, mnPos + 2));
                }
                else
                {
                    command.Enqueue(new Swap(mnPos, mnPos - lieShu));
                    command.Enqueue(new Swap(mnPos - lieShu, mnPos - lieShu + 1));
                    command.Enqueue(new Swap(mnPos - lieShu + 1, mnPos - lieShu + 2));
                    command.Enqueue(new Swap(mnPos - lieShu + 2, mnPos + 2));
                }
                return mnPos + 2;
            }
            else
            if (x2 >= x1)
            {
                if (y1 > y2)
                {
                    mnPos = EmptyVerticalPlan(command, mnPosition, y1 - y2 - 1, -1, lieShu);
                    mnPos = EmptyTransversePlan(command, mnPos, x2 - x1 + 1, 1);
                    command.Enqueue(new Swap(mnPos, mnPos - lieShu));
                }
                else
                if (y1 < y2)
                {
                    mnPos = EmptyVerticalPlan(command, mnPosition, y2 - y1 - 1, 1, lieShu);
                    mnPos = EmptyTransversePlan(command, mnPos, x2 - x1 + 1, 1);
                    command.Enqueue(new Swap(mnPos, mnPos + lieShu));
                }
                return target + 1;
            }
            else
            {
                return EmptyToVt(command, mnPosition, target + 1, lieShu);
            }

        }
        public int EmptyToVtRight(int mnPosition, int target, int lieShu, string upOrdown = "Down")
        {
            return EmptyToVtRight(this.Command,mnPosition,target,lieShu,upOrdown);
        }
        public int EmptyToVtRight(int mnPosition, int target, string upOrdown = "Down")
        {
            return EmptyToVtRight(this.Command, mnPosition, target, this.puzzle.LieShu, upOrdown);
        }
        public int EmptyToVtRight(int target, string upOrdown = "Down")
        {
            return EmptyToVtRight(this.Command, this.puzzle.mnPosition, target, this.puzzle.LieShu, upOrdown);
        }
        #endregion
        #region 生成mn到目标位置右侧的命令，先横移
        /// <summary>
        /// 生成mn到目标位置右侧的命令，且mn不能经过位置target，先横移
        /// </summary>
        /// <param name="command">存放命令的队列</param>
        /// <param name="mnPosition">mn当前位置</param>
        /// <param name="target">目标位置</param>
        /// <param name="lieShu">列数</param>
        /// <param name="upOrdown">如果mn在目标正右方，则有两种可选移动方式，默认值为"Down"，如果upOrdown！= "Down"则选择另一种方式</param>
        /// <returns>mn移动后的位置</returns>
        public int EmptyToTvRight(Queue<Swap> command, int mnPosition, int target, int lieShu, string upOrdown = "Down")
        {
            if (target > mnPosition && (target - target % lieShu) / lieShu == (mnPosition - mnPosition % lieShu) / lieShu)
            {
                int mnPos = EmptyTransversePlan(command, mnPosition, target % lieShu - mnPosition % lieShu - 1, 1);
                if (upOrdown == "Down")
                {
                    command.Enqueue(new Swap(mnPos, mnPos + lieShu));
                    command.Enqueue(new Swap(mnPos + lieShu, mnPos + lieShu + 1));
                    command.Enqueue(new Swap(mnPos + lieShu + 1, mnPos + lieShu + 2));
                    command.Enqueue(new Swap(mnPos + lieShu + 2, mnPos + 2));
                }
                else
                {
                    command.Enqueue(new Swap(mnPos, mnPos - lieShu));
                    command.Enqueue(new Swap(mnPos - lieShu, mnPos - lieShu + 1));
                    command.Enqueue(new Swap(mnPos - lieShu + 1, mnPos - lieShu + 2));
                    command.Enqueue(new Swap(mnPos - lieShu + 2, mnPos + 2));
                }
                return mnPos + 2;
            }
            else
            {
                return EmptyToTv(command, mnPosition, target + 1, lieShu);
            }
        }
        public int EmptyToTvRight(int mnPosition, int target, int lieShu, string upOrdown = "Down")
        {
            return EmptyToTvRight(this.Command,mnPosition,target,lieShu,upOrdown);
        }
        public int EmptyToTvRight(int mnPosition, int target,string upOrdown = "Down")
        {
            return EmptyToTvRight(this.Command, mnPosition, target, this.puzzle.LieShu, upOrdown);
        }
        public int EmptyToTvRight( int target, string upOrdown = "Down")
        {
            return EmptyToTvRight(this.Command, this.puzzle.mnPosition, target, this.puzzle.LieShu, upOrdown);
        }
        #endregion
        #endregion

        #region 生成高级命令
        #region 将位置在entityPos的图块移动到位置target
        /// <summary>
        /// 将位置在entityPos的图块移动到位置target。
        /// entityPos在边界时，生成的命令会有超出索引的可能，要小心使用。
        /// 当target在边界时无法处理,mn的位置等于entityPos时会出错。
        /// 矛盾的是真实状况下很少复原列数和行数很大的拼图，为了避免设置参数的烦恼 请使用EntityToArgs类
        /// </summary>
        /// <param name="command">存放命令的队列</param>
        /// <param name="mnPosition">mn当前位置</param>
        /// <param name="entityPos">要移动的图块的位置</param>
        /// <param name="target">目标位置</param>
        /// <param name="lieShu">列数</param>
        /// <param name="VorT">将mn移动到entityPos附近的合理位置时，优先移动到其左右还是上下，ture 上下，false 左右</param>
        /// <param name="entityRDorLU">移动entityPos到目标时，优先使用右侧和下侧移动命令，还是使用左侧和上侧移动命令，ture 右侧和下侧，false 左侧和上侧</param>
        /// <param name="mnToVorT">将mn移动到entityPos附近的合理位置前，mn优先横移还是竖移，ture 竖移，false 横移</param>
        /// <param name="mnToDefault">将mn移动到entityPos附近的合理位置时,是否使用默认得移动策略，是ture,否false,默认为true</param>
        /// <returns>mn移动后的位置</returns>
        public int EntityTo(Queue<Swap> command, int mnPosition, int entityPos, int target, int lieShu, bool VorT, bool entityRDorLU, bool mnToVorT, bool mnToDefault = true)
        {
            PointOffset po = new PointOffset(entityPos, target, lieShu);
            Position postion = PositionAnalysis(po);
            int p = 0;//0原点，1,2,3,4上下左右
            switch (postion)
            {
                case Position.Up:
                case Position.UpGtRight:
                case Position.UpGtLeft:
                    p = 1;
                    break;
                case Position.Down:
                case Position.DownGtRight:
                case Position.DownGtLeft:
                    p = 2;
                    break;
                case Position.Left:
                case Position.LeftGtDown:
                case Position.LeftGtUp:
                    p = 3;
                    break;
                case Position.Right:
                case Position.RightGtDown:
                case Position.RightGtUp:
                    p = 4;
                    break;
                case Position.UpEqRight:
                    p = VorT ? 1 : 4;
                    break;
                case Position.RightEqDown:
                    p = VorT ? 2 : 4;
                    break;
                case Position.DownEqLeft:
                    p = VorT ? 2 : 3;
                    break;
                case Position.LeftEqUp:
                    p = VorT ? 1 : 3;
                    break;

            }
            switch (p)
            {
                case 1:
                    mnPosition = mnToVorT ?
                        (mnToDefault ? EmptyToVtUp(command, mnPosition, entityPos, lieShu) : EmptyToVtUp(command, mnPosition, entityPos, lieShu, "Left"))
                        : (mnToDefault ? EmptyToTvUp(command, mnPosition, entityPos, lieShu) : EmptyToTvUp(command, mnPosition, entityPos, lieShu, "Left"));
                    break;
                case 2:
                    mnPosition = mnToVorT ?
                        (mnToDefault ? EmptyToVtDown(command, mnPosition, entityPos, lieShu) : EmptyToVtDown(command, mnPosition, entityPos, lieShu, "Left"))
                        : (mnToDefault ? EmptyToTvDown(command, mnPosition, entityPos, lieShu) : EmptyToTvDown(command, mnPosition, entityPos, lieShu, "Left"));
                    break;
                case 3:
                    mnPosition = mnToVorT ?
                        (mnToDefault ? EmptyToVtLeft(command, mnPosition, entityPos, lieShu) : EmptyToVtLeft(command, mnPosition, entityPos, lieShu, "Up"))
                        : (mnToDefault ? EmptyToTvLeft(command, mnPosition, entityPos, lieShu) : EmptyToTvLeft(command, mnPosition, entityPos, lieShu, "Up"));
                    break;
                case 4:
                    mnPosition = mnToVorT ?
                        (mnToDefault ? EmptyToVtRight(command, mnPosition, entityPos, lieShu) : EmptyToVtRight(command, mnPosition, entityPos, lieShu, "Up"))
                        : (mnToDefault ? EmptyToTvRight(command, mnPosition, entityPos, lieShu) : EmptyToTvRight(command, mnPosition, entityPos, lieShu, "Up"));
                    break;
            }//mn移动到合理位置
            switch (postion)//将entity移动到目标
            {
                case Position.Up:
                case Position.Down:
                    mnPosition = entityRDorLU ? RigthEntityVerticalPlan(command, mnPosition, po.Offset.Y, po.Direction.Y, lieShu) : LeftEntityVerticalPlan(command, mnPosition, po.Offset.Y, po.Direction.Y, lieShu);
                    break;
                case Position.Right:
                case Position.Left:
                    mnPosition = entityRDorLU ? LowerEntityTransversePlan(command, mnPosition, po.Offset.X, po.Direction.X, lieShu) : RiseEntityTransversePlan(command, mnPosition, po.Offset.X, po.Direction.X, lieShu);
                    break;
                case Position.UpEqRight:
                case Position.RightEqDown:
                case Position.DownEqLeft:
                case Position.LeftEqUp:
                    mnPosition = VorT ? RiseEntityObliquePlan(command, mnPosition, po.Offset.X, po.Direction.Y, po.Direction.X, lieShu) : LateralEntityObliquePlan(command, mnPosition, po.Offset.X, po.Direction.Y, po.Direction.X, lieShu);
                    break;
                case Position.UpGtRight:
                case Position.UpGtLeft:
                case Position.DownGtRight:
                case Position.DownGtLeft:
                    mnPosition = entityRDorLU ? RigthEntityVerticalPlan(command, mnPosition, po.OffsetYMinusX, po.Direction.Y, lieShu) : LeftEntityVerticalPlan(command, mnPosition, po.OffsetYMinusX, po.Direction.Y, lieShu);
                    mnPosition = RiseEntityObliquePlan(command, mnPosition, po.Offset.X, po.Direction.Y, po.Direction.X, lieShu);
                    break;
                case Position.RightGtUp:
                case Position.RightGtDown:
                case Position.LeftGtDown:
                case Position.LeftGtUp:
                    mnPosition = entityRDorLU ? LowerEntityTransversePlan(command, mnPosition, -po.OffsetYMinusX, po.Direction.X, lieShu) : RiseEntityTransversePlan(command, mnPosition, -po.OffsetYMinusX, po.Direction.X, lieShu);
                    mnPosition = LateralEntityObliquePlan(command, mnPosition, po.Offset.Y, po.Direction.Y, po.Direction.X, lieShu);
                    break;
            }
            return mnPosition;
        }
        public int EntityTo(int mnPosition, int entityPos, int target, int lieShu, bool VorT, bool entityRDorLU, bool mnToVorT, bool mnToDefault = true)
        {
            return EntityTo(this.Command, mnPosition, entityPos, target, lieShu, VorT, entityRDorLU, mnToVorT, mnToDefault);
        }
        public int EntityTo(int mnPosition, int entityPos, int target, bool VorT, bool entityRDorLU, bool mnToVorT, bool mnToDefault = true)
        {
            return EntityTo(this.Command, mnPosition, entityPos, target, this.puzzle.LieShu, VorT, entityRDorLU, mnToVorT, mnToDefault);
        }
        public int EntityTo(int entityPos, int target, bool VorT, bool entityRDorLU, bool mnToVorT, bool mnToDefault = true)
        {
            return EntityTo(this.Command, this.puzzle.mnPosition, entityPos, target, this.puzzle.LieShu, VorT, entityRDorLU, mnToVorT, mnToDefault);
        }
        /// <summary>
        /// 将位置在entityPos的图块移动到位置target。
        /// 不必担心超出索引的问题
        /// </summary>
        /// <param name="command"></param>
        /// <param name="entityToArgs"></param>
        /// <returns></returns>
        public int EntityTo(Queue<Swap> command, EntityToArgs entityToArgs)
        {
            return EntityTo(command,entityToArgs.mnPosition,entityToArgs.entityPos,entityToArgs.target,entityToArgs.lieShu,entityToArgs.VorT,entityToArgs.entityRDorLU,entityToArgs.mnToVorT);
        }
        public int EntityTo(EntityToArgs entityToArgs)
        {
            return EntityTo(this.Command, entityToArgs.mnPosition, entityToArgs.entityPos, entityToArgs.target, entityToArgs.lieShu, entityToArgs.VorT, entityToArgs.entityRDorLU, entityToArgs.mnToVorT);
        }
        #endregion

        #endregion

       
        #region 复原拼图
        public void Restore(Puzzle puzzle)
        {
          
        }
        #endregion

        #region 执行命令
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="command">命令队列</param>
        public void ExecutePlan(Queue<Swap> command,Action<Swap> action=null)
        {
             int count = command.Count;
             for (int i = 0; i < count; i++)
             {
                Swap swap = command.Dequeue();
                puzzle.SwapAction(swap);
                action?.Invoke(swap);
             }
        }
        public void ExecutePlan(Action<Swap> action=null)
        {
            ExecutePlan(Command,action);
        }
        /// <summary>
        /// 检查执行命令，在交换不足以执行时停止
        /// 必须使用带参构造函数实例化PuzzleAide
        /// </summary>
        /// <param name="command">命令队列</param>
        /// <param name="action">执行命令后要执行的行动</param>
        /// <returns>第一个未被执行的交换，如果都执行了返回null</returns>
        public Swap CheckExecutePlan(Queue<Swap> command,Action<Swap> action=null)
        {
            int count = command.Count;
            for (int i=0;i<count;i++)
            {
                Swap swap = command.Dequeue();
                if (CheckSwap(swap))
                {
                    puzzle.SwapAction(command.Dequeue());
                    action?.Invoke(swap);
                }
                else
                    return swap;
            }
            return null;
        }
        public Swap CheckExecutePlan(Action<Swap> action=null)
        {
            return CheckExecutePlan(this.Command,action);
        }

        /// <summary>
        /// 快速执行命令，复原拼图时不会更新拼图的状态
        /// </summary>
        /// <param name="command">命令队列</param>
        public void ExecutePlanFast(Queue<Swap> command,Action<Swap> action=null)
        {
            int count = command.Count;
            for (int i = 0; i < count; i++)
            {
                Swap swap = command.Dequeue();
                puzzle.Swap(swap);
                action?.Invoke(swap);
            }
        }
        public void ExecutePlanFast(Action<Swap> action=null)
        {
            ExecutePlanFast(Command,action);
        }
        /// <summary>
        /// 检查并快速执行命令，在交换不足以执行时停止
        /// 必须使用带参构造函数实例化PuzzleAide
        /// </summary>
        /// <param name="command">命令队列</param>
        /// <param name="action">执行命令后要执行的行动</param>
        /// <returns>第一个未被执行的交换，如果都执行了返回null</returns>
        public Swap CheckExecutePlanFast(Queue<Swap> command, Action<Swap> action=null)
        {
            int count = command.Count;
            for (int i = 0; i < count; i++)
            {
                Swap swap = command.Dequeue();
                if (CheckSwap(swap))
                {
                    puzzle.Swap(command.Dequeue());
                    action?.Invoke(swap);
                }
                else
                    return swap;
            }
            return null;
        }
        public Swap CheckExecutePlanFast(Action<Swap> action=null)
        {
            return CheckExecutePlanFast(this.Command, action);
        }
        #endregion

        #region 检查交换是否正确
        /// <summary>
        /// 检查交换是否正确
        /// </summary>
        /// <param name="swap">交换</param>
        /// <param name="hanghu">拼图行数</param>
        /// <param name="lieShu">拼图列数</param>
        /// <returns></returns>
        public bool CheckSwap(Swap swap,int hanghu,int lieShu)
        {
            if (swap.Entity<0||swap.Entity>=hanghu*lieShu)
            {
                return false;
            }
            if (Math.Abs(swap.Entity % lieShu- swap.Empty % lieShu) + Math.Abs((swap.Entity-swap.Entity%lieShu)/lieShu-(swap.Empty-swap.Empty%lieShu)/lieShu)!=1)
            {
                return false;
            }
            return true;
        }
        public bool CheckSwap(Swap swap)
        {
            return CheckSwap(swap,puzzle.HangShu,puzzle.LieShu);
        }
        #endregion
        #region 高级命令参数
        /// <summary>
        /// 高级命令参数
        /// 可以自动设置高级命令的合理参数
        /// </summary>
        public sealed class EntityToArgs
        {
            public int mnPosition { get; set; }
            public int entityPos { get; set; }
            public int target { get; set; }
            public int lieShu { get; set; }
            public int hangShu { get; set; }
            public bool VorT { get; set; }
            public bool entityRDorLU { get; set; }
            public bool mnToVorT { get; set; }
            public bool mnToDefault { get; set; }
            /// <summary>
            /// 设置参数值，
            /// 使用第一高级命令时有：entityPos在边界时，生成的命令会有超出索引的可能，要小心使用。
            /// 为避免这个问题请使用构造函数初始化参数
            /// </summary>
            /// <param name="mnPosition"></param>
            /// <param name="entityPos"></param>
            /// <param name="target"></param>
            /// <param name="lieShu"></param>
            /// <param name="hangShu"></param>
            public EntityToArgs(int mnPosition, int entityPos, int target, int lieShu, int hangShu)
            {
                this.mnPosition = mnPosition;
                this.entityPos = entityPos;
                this.target = target;
                this.lieShu = lieShu;
                this.hangShu = hangShu;
                mnToVorT = true;
                mnToDefault = true;
                PointOffset po = new PointOffset(entityPos, target, lieShu);
                if (po.Origin.Y == 0 && target != entityPos && po.OffsetYMinusX < 0)
                {
                    VorT = false;
                    entityRDorLU = true;
                }
                else if (po.Origin.X == lieShu - 1 && target != entityPos && po.OffsetYMinusX > 0)
                {
                    VorT = true;
                    entityRDorLU = false;
                }
                else if (po.Origin.Y == hangShu - 1 && target != entityPos && po.OffsetYMinusX < 0)
                {
                    VorT = false;
                    entityRDorLU = false;
                }
                else
                {
                    VorT = true;
                    entityRDorLU = true;
                }
                if (po.Origin.Y == po.End.Y && (mnPosition - mnPosition % lieShu) / mnPosition > po.Origin.Y && mnPosition % lieShu < po.Origin.X)
                    mnToVorT = false;
            }
        } 
        #endregion
    }
}
