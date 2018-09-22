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
using System.Windows.Threading;

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
        private DispatcherTimer timer =null;
        private DateTime dt0 = new DateTime();
        public MainWindow()
        {
            InitializeComponent();
            yanShi.SelectedIndex = 0;
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(100000);//1ms
            timer.Tick += new EventHandler(AddTime);
           
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
            if(timer.IsEnabled)
               timer.Stop();
            timeLab.Content = "计时：";
            indexLab.Content = "被复原：" ;
            buShuLab.Content = "步数:" ;
            SwapLab.Content = "交换:";
            mnPosLab.Content = "mn初始位置:";
            indexPosLab.Content = "index初始位置:";
            tarLab.Content = "目的地位置：";
            messLab.Content = "其它信息：";
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
                if (timer.IsEnabled)
                    timer.Stop();
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
                try
                {
                    initBut.IsEnabled = false;
                    disBut.IsEnabled = false;
                    resBut.IsEnabled = false;
                    if (!timer.IsEnabled)
                    {
                        timer.Start();
                        dt0 = DateTime.Now;
                    }
                    bool res = puzzleAide.Restore(SwapMess);
                    timer.Stop();
                    if (res)
                    {
                        MessageBox.Show("自动复原成功！", "成功", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    }
                    else
                    {
                        MessageBox.Show("自动复原失败！", "失败", MessageBoxButton.OK, MessageBoxImage.Question);
                    }
                }
                catch (Exception ex)
                {
                    timer.Stop();
                    MessageBox.Show("发生异常！", "异常", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    initBut.IsEnabled = true;
                    disBut.IsEnabled = true;
                    resBut.IsEnabled = true;
                }
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
                    if (!timer.IsEnabled)
                    {
                        timer.Start();
                        dt0 = DateTime.Now;
                    }
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
                   // timeLab.Content = timeStr;
                    buShuLab.Content = "步数:" + puzzleAide.StepNum;
                    SwapLab.Content = "交换:(" + swap.Empty.ToString() + "," + swap.Entity.ToString() + ")";
                }
            }
            if (puzzle.IsRestore())
            {
                timer.Stop();
            }
        }
        
        private void SwapMess(Swap swap,RestoreRunInfo restoreRunInfo)
        {
            indexLab.Content = "被复原：" + restoreRunInfo.index;
            buShuLab.Content = "步数:" + puzzleAide.StepNum;
            SwapLab.Content = "交换:(" + swap.Empty.ToString() + "," + swap.Entity.ToString() + ")";
            mnPosLab.Content = "mn初始位置:" + restoreRunInfo.beginMnPos;
            indexPosLab.Content = "index初始位置:" + restoreRunInfo.entityPos;
            tarLab.Content = "目的地位置：" + restoreRunInfo.target;
            messLab.Content = "其它信息："+restoreRunInfo.otherMess;
            string content =( puButs.Children[swap.Entity] as Button).Content.ToString();//被移动的图块
            Button temp = mnBut;
            mnBut = puButs.Children[swap.Entity] as Button;
            mnBut.Background = Brushes.White;
            mnBut.Content = "mn";
            temp.Background = new SolidColorBrush(Color.FromRgb(84, 255, 159));//54FF9F
            temp.Content = content;
            App.DoEvents();
            int time = Convert.ToInt32(yanShi.SelectionBoxItem);
            if(time>0)
            Thread.Sleep(time);
        }
        private void AddTime(object sender, EventArgs e)
        {
            TimeSpan ts = DateTime.Now - dt0;
            string timeStr = "计时："+(ts.Minutes>9?ts.Minutes.ToString():"0"+ts.Minutes)+ ":" + (ts.Seconds>9?ts.Seconds.ToString():"0"+ts.Seconds)+ ":" + (ts.Milliseconds>99?ts.Milliseconds.ToString():(ts.Milliseconds>9?"0"+ts.Milliseconds:"00"+ts.Milliseconds));
            timeLab.Content = timeStr;
            App.DoEvents();
        }
    }
}
