using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNPuzzle
{
   public class RestoreRunInfo
    {
        public bool BEGIN { get; set; }//命令开始
        public int index { get; set; }//当前要被复原的图块
        public int entityPos { get; set; }//开始时entity的位置
        public int target { get; set; }//目的地位置
        public int beginMnPos { get; set; }//开始时mn的位置
        public PuzzleAide.EntityToArgs entityToArgs { get; set; }//高级命令参数
        public Swap swap { get; set; }//检查执行的返回值
        public bool IsOrigin { get; set; }//是否一开始就在目标位置
        public bool IsCheckOrigin { get; set; }//是否检查执行过后到达目标
        public int checkMnPos { get; set; }//检查执行过后mn的位置
        public int nearBlock { get; set; }//entity的位置
        public bool IsPType { get; set; }//寻找特解的类型
        public int endMnPos { get; set; }//mn最后的位置
        public bool END { get; set; }//命令是否结束
        public string otherMess { get; set; }
    }
}
