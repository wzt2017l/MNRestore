﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MNPuzzle
{
    public enum PuzzleState
    {
        /// <summary>
        /// 有序
        /// </summary>
        Original = 0,
        /// <summary>
        /// 混乱
        /// </summary>
        Confusion = 1
    }

    public class Puzzle:IPuzzle
    {
        #region 属性
        public int HangShu { get; private set; }//行数
        public int LieShu { get; private set; }//列数
        public int Total { get; private set; }//拼图总块数
        public int mnPosition { get; set; }//mn当前位置
        public int[] Items { get; private set; }//拼图数组
        public long NiXu { get; set; }//数组逆序数
        public PuzzleState State { get; set; }//状态 
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
            NiXu = 0;
            State = PuzzleState.Original;
          
        }
        #endregion

        #region 交换位置
        /// <summary>
        /// 交换位置，不对拼图状态做任何更新
        /// </summary>
        /// <param name="empty">空格所在的位置</param>
        /// <param name="entity">要与之交换的拼图块的位置</param>
        public void Swap(int empty, int entity)
        {
            int t = this.Items[empty];
            Items[empty] = Items[entity];
            Items[entity] = t;
        }
        public void Swap(Swap swap)
        {
            Swap(swap.Empty,swap.Entity);
        }
        /// <summary>
        /// 交换位置，更新逆序数和拼图的状态
        /// </summary>
        /// <param name="empty">空格所在的位置</param>
        /// <param name="entity">要与之交换的拼图块的位置</param>
        public void SwapAction(int empty,int entity)
        {
            NiXu = NiXu + InversionNumberDifference(entity, empty);
            int t = this.Items[empty];
            Items[empty] = Items[entity];
            Items[entity] = t;
            mnPosition = entity;
            switch (NiXu)
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

        #region 重算逆序数
        /// <summary>
        /// 计算逆序数
        /// </summary>
        /// <param name="array">数组</param>
        /// <returns>逆序数</returns>
        public long RetryNiXu(int[] array)
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
        public long RetryNiXu()
        {
            NiXu = RetryNiXu(this.Items);
            return NiXu;
        }
        #endregion

    }
    
}
