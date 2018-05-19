using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNPuzzle
{
    /// <summary>
    /// 拼图接口
    /// 显然拼图不会自己打乱自己，也不会自己复原自己
    /// 它只有自己的固有属性
    /// </summary>
    interface IPuzzle
    {
         int HangShu { get;  }//行数
         int LieShu { get; }//列数
         int Total { get; }//拼图总块数
         int mnPosition { get;  }//mn当前位置
         int[] Items { get; }//拼图数组
         long NiXu { get; set; }//数组逆序数
         PuzzleState State { get; }//状态 

        /// <summary>
        /// 交换位置，不对拼图状态做任何更新
        /// </summary>
        /// <param name="empty">空格所在的位置</param>
        /// <param name="entity">要与之交换的拼图块的位置</param>
        void Swap(int empty, int entity);

        /// <summary>
        /// 交换位置，更新逆序数和拼图的状态
        /// </summary>
        /// <param name="empty">空格所在的位置</param>
        /// <param name="entity">要与之交换的拼图块的位置</param>
        void SwapAction(int empty, int entity);

        /// <summary>
        /// 计算逆序数
        /// </summary>
        /// <param name="array">数组</param>
        /// <returns>逆序数</returns>
        long RetryNiXu(int[] array);

    }
}
