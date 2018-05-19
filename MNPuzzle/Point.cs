using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNPuzzle
{
    /// <summary>
    /// 坐标类
    /// </summary>
    public class Point
    {
        /// <summary>
        /// 列坐标
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// 行坐标
        /// </summary>
        public int Y { get; set; }

        public Point(){}
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="num">位置编号</param>
        /// <param name="lieShu">列数</param>
        public Point(int num,int lieShu)
        {
            X = num % lieShu;
            Y = (num - X) / lieShu;
        }
    }

    /// <summary>
    /// 交换类
    /// </summary>
    public class Swap
    {
        /// <summary>
        /// 实体位
        /// </summary>
        public int Entity { get; set; }
        /// <summary>
        /// 空的位
        /// </summary>
        public int Empty { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="empty">空缺位</param>
        /// <param name="entity">实体位</param>
        public Swap(int empty,int entity)
        {
            Empty = empty;
            Entity = entity;
        }
    }

    /// <summary>
    /// 坐标偏移
    /// </summary>
    public class PointOffset
    {
        /// <summary>
        /// 起始坐标
        /// </summary>
        public Point Origin { get; }
        /// <summary>
        /// 目标坐标
        /// </summary>
        public Point End { get; }
        /// <summary>
        /// 坐标差，目标与起始差的绝对值
        /// </summary>
        public Point Offset { get; }
        /// <summary>
        /// 指示方向
        /// </summary>
        public Point Direction { get; }
        /// <summary>
        /// =Offset.Y（行） - Offset.X（列）
        /// </summary>
        public int OffsetYMinusX { get; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="origin">图块所在位置编号</param>
        /// <param name="end">目标位置编号</param>
        /// <param name="lieshu">列数</param>
        public PointOffset(int origin, int end,int lieshu)
        {
            Origin = new Point(origin,lieshu);
            End = new Point(end,lieshu);
            Offset = new Point
            {
                X = Math.Abs(End.X - Origin.X),
                Y = Math.Abs(End.Y - Origin.Y)
            };
            Direction = new Point();
            if (Offset.X == 0)
            {
                Direction.X = 0;
            }
            else
            {
                Direction.X = Offset.X / (End.X - Origin.X);
            }
            if (Offset.Y==0)
            {
                Direction.Y = 0;
            }
            else
            {
                Direction.Y = Offset.Y / (End.Y - Origin.Y);
            }
      
            OffsetYMinusX = Offset.Y - Offset.X;

        }
    }

}
