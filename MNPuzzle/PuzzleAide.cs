using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNPuzzle
{
   public class PuzzleAide
   {
        public Queue<Swap> Command { get; set; }//命令队列
        public Puzzle puzzle { get; private set; }

        public PuzzleAide(Puzzle puzzle)
        {
            this.puzzle = puzzle;
            Command = new Queue<Swap>();
        }

        #region 打乱拼图
        /// <summary>
        /// 打乱拼图
        /// </summary>
        public void Disrupt()
        {
            int Total = puzzle.Total;
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
                    puzzle.Swap(k, j);
                }
            });
            puzzle.Swap(Total - 1, r.Next(0, Total - 1));
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
            int k = puzzle.LieShu * direction;
            for (int i = 0; i < offset; i++)
            {
                int j = mnPosition + i * k;//j是当前循环mn的初始位置
                Swap swap = new Swap(j, j + puzzle.LieShu * direction);
                Command.Enqueue(swap);
            }
        }
        public void EmptyVerticalPlan(int offset, int direction)
        {
            EmptyVerticalPlan(puzzle.mnPosition, offset, direction);
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
            for (int i = 0; i < offset; i++)
            {
                int j = mnPosition + i * direction;
                Swap swap = new Swap(j, j + direction);
                Command.Enqueue(swap);
            }
        }
        public void EmptyTransversePlan(int offset, int direction)
        {
            EmptyTransversePlan(puzzle.mnPosition, offset, direction);
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
            int k = puzzle.LieShu * direction;
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
        }
        public void RigthEntityVerticalPlan(int offset, int direction)
        {
            RigthEntityVerticalPlan(puzzle.mnPosition, offset, direction);
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
            int k = puzzle.LieShu * direction;
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
        }
        public void LeftEntityVerticalPlan(int offset, int direction)
        {
            LeftEntityVerticalPlan(puzzle.mnPosition, offset, direction);
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
            int lieShu = puzzle.LieShu;
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
        }
        public void LowerEntityTransversePlan(int offset, int direction)
        {
            LowerEntityTransversePlan(puzzle.mnPosition, offset, direction);
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
            int lieShu = puzzle.LieShu;
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
        }
        public void RiseEntityTransversePlan(int offset, int direction)
        {
            RiseEntityTransversePlan(puzzle.mnPosition, offset, direction);
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
            int k = puzzle.LieShu * directionR;
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
        }
        public void RiseEntityObliquePlan(int offset, int directionR, int directionC)
        {
            RiseEntityObliquePlan(puzzle.mnPosition, offset, directionR, directionC);
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
            int k = puzzle.LieShu * directionR;
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
        }
        public void LateralEntityObliquePlan(int offset, int directionR, int directionC)
        {
            LateralEntityObliquePlan(puzzle.mnPosition, offset, directionR, directionC);
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
        #endregion

        #region 相对位置分析
        public PointOffset RelativePosition(int origin, int end)
        {
            return new PointOffset(origin, end, puzzle.LieShu);
        }
        #endregion
    }
}
