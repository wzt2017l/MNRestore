﻿using System;
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
        public int X { get; set; }
        public int Y { get; set; }
    }

    /// <summary>
    /// 交换类
    /// </summary>
    public class Swap
    {
        public int Entity { get; set; }//实体
        public int Empty { get; set; }//空的 
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
        public int OffsetRow { get; set; }//行差
        public int OffsetCol { get; set; }//列差
        public int OffsetRoC { get; set; }//行列差
        public int DirectionR { get; set; }//指示方向
        public int DirectionC { get; set; }
        public PointOffset(int origin, int end,int lieshu)
        {
            int endCol = end % lieshu;
            int originCol = origin % lieshu;
            OffsetCol = endCol - originCol;
            OffsetRow = (end - origin -OffsetCol) / lieshu;
            OffsetRoC = Math.Abs(OffsetRow) - Math.Abs(OffsetCol);
            if (OffsetRow == 0)
            {
                DirectionR = 0;
            }
            else
            {
                DirectionR = Math.Abs(OffsetRow) / OffsetRow;
            }
            if (OffsetCol == 0)
            {
                DirectionC = 0;
            }
            else
            {
                DirectionC = Math.Abs(OffsetCol) / OffsetCol;
            }

        }
    }

}
