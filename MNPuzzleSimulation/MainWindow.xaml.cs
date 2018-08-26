using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using MNPuzzle;

namespace MNPuzzleSimulation
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private Puzzle puzzle=null;
        private PuzzleAide puzzleAide = null;
        private Button mnBut = null;
        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void initBut_Click(object sender, RoutedEventArgs e)
        {
            string hangItem= hangShu.SelectionBoxItem.ToString();
            string lieItem = lieShu.SelectionBoxItem.ToString();
            if (hangItem!=""&& lieItem!="")
            {
                puzzle = new Puzzle(Convert.ToInt32(hangItem),Convert.ToInt32(lieItem));
                InitPuzzle();
                puzzleAide = new PuzzleAide(puzzle);
            }
        }
        private void InitPuzzle()
        {
            puButs.Children.Clear();
            foreach (int i in puzzle.Items)
            {
                Button button = new Button()
                {
                    Content = i,
                    Height = puButs.ActualHeight / puzzle.HangShu,
                    Width = puButs.ActualWidth / puzzle.LieShu,
                    Style = (Style)this.FindResource("puButStyle"),
                    Tag=i
                };
                button.Click += puBut_Click;
                puButs.Children.Add(button);
            }
            mnBut= puButs.Children[puzzle.Total-1] as Button;
            mnBut.Background = Brushes.White;
            mnBut.Content = "mn";
        }
        private void disBut_Click(object sender, RoutedEventArgs e)
        {
            if (puzzle != null)
            {
                puzzleAide.DisruptReducible();
                for (int i=0;i<puzzle.Total;i++)
                {
                    Button but = puButs.Children[i] as Button;
                    but.Content = puzzle.Items[i];
                }
                if(mnBut.Content.ToString()!=puzzle.Total.ToString())
                {
                    mnBut.Background= new SolidColorBrush(Color.FromRgb(84, 255, 159));
                    foreach (Button but in puButs.Children)
                    {
                        if (but.Content.ToString() == (puzzle.Total - 1).ToString())
                        {
                            mnBut = but;
                            mnBut.Content = "mn";
                            mnBut.Background = Brushes.White;
                        }
                    }
                }
            }
        }

        private void resBut_Click(object sender, RoutedEventArgs e)
        {
            if (puzzle!=null)
            {
                puzzleAide.Restore(SwapMess);
            }
        }
        //移动
        public void puBut_Click(object sender, RoutedEventArgs e)
        {
            Button but = sender as Button;
            if (but.Background!=Brushes.White)
            {
               int indexPos = Convert.ToInt32(but.Tag);
               int index=Convert.ToInt32(but.Content);
               if (indexPos+puzzle.LieShu==Convert.ToInt32(mnBut.Tag)|| indexPos - puzzle.LieShu == Convert.ToInt32(mnBut.Tag) || indexPos - 1 == Convert.ToInt32(mnBut.Tag) || indexPos + 1 == Convert.ToInt32(mnBut.Tag))//上下左右
               {
                    puzzleAide.Command.Clear();
                    Swap swap = new Swap(Convert.ToInt32(mnBut.Tag), indexPos);
                    puzzleAide.Command.Enqueue(swap);
                    puzzleAide.ExecutePlan();
                    string content = but.Content.ToString();
                    Button temp = mnBut;
                    mnBut = but;
                    mnBut.Background = Brushes.White;
                    mnBut.Content = "mn";
                    temp.Background=new SolidColorBrush(Color.FromRgb(84,255,159));//54FF9F
                    temp.Content = content;
                    buShuLab.Content = "步数:" + puzzleAide.StepNum;
                    SwapLab.Content = "交换:(" + swap.Empty.ToString() + "," + swap.Entity.ToString() + ")";
                }
            }
        }
        
        private void SwapMess(Swap swap,RestoreRunInfo restoreRunInfo)
        {
            buShuLab.Content = "步数:" + puzzleAide.StepNum;
            SwapLab.Content = "交换:(" + swap.Empty.ToString() + "," + swap.Entity.ToString() + ")";
            string content =( puButs.Children[swap.Entity] as Button).Content.ToString();//被移动的图块
            Button temp = mnBut;
            mnBut = puButs.Children[swap.Entity] as Button;
            mnBut.Background = Brushes.White;
            mnBut.Content = "mn";
            temp.Background = new SolidColorBrush(Color.FromRgb(84, 255, 159));//54FF9F
            temp.Content = content;
            App.DoEvents();
            Thread.Sleep(200);
        }
    }
}
