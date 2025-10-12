using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Threading;

namespace HabitTrack
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        private string taskSavePath = System.IO.Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "habittrack_tasks.txt"
        );
        private string daySavePath = System.IO.Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "habittrack_day.txt"
        );

        private int _dayCount = 0;
        private Random _rand = new Random();
        private bool _finalStage = false;

        public MainWindow()
        {
            InitializeComponent();

            // 读取天数
            if (File.Exists(daySavePath))
                int.TryParse(File.ReadAllText(daySavePath), out _dayCount);

            // 加载任务
            LoadTasks();

            // 添加任务按钮
            AddTaskButton.Click += (s, e) =>
            {
                string task = TaskInput.Text.Trim();
                if (!string.IsNullOrEmpty(task))
                {
                    TaskList.Items.Add(task);
                    TaskInput.Clear();
                    SaveTasks();
                }
            };

            // 根据天数显示剧情
            ShowDayContent();
        }

        private void ShowDayContent()
        {
            if (_dayCount == 3)
            {
                TaskList.Items.Add("凌晨 3:33 醒来");
            }
            else if (_dayCount == 5)
            {
                TaskList.Items.Add("盯着屏幕 " + _rand.Next(10, 20) + " 分钟");
            }
            else if (_dayCount == 7)
            {
                TaskList.Items.Add("2019.06.15 — 数学挂科后向她表白，被拒绝");
                TaskList.Items.Add("她说：你还是先把数学学好吧。");
            }
            else if (_dayCount == 9)
            {
                TaskList.Items.Add("2024.11.02 — 半夜看了 4 小时 VTuber 直播");
                TaskList.Items.Add("她们不会嫌你笨，不会嘲笑你单身。");
            }
            else if (_dayCount >= 10)
            {
                EnterFinalStage();
            }
        }

        private void EnterFinalStage()
        {
            _finalStage = true;
            MainGrid.Background = Brushes.Black;
            TitleText.Text = "We only track what you forget.";
            TitleText.Foreground = Brushes.Red;

            // 修改输入框颜色
            TaskInput.Background = Brushes.Black;
            TaskInput.Foreground = Brushes.Red;
            TaskInput.BorderBrush = Brushes.Red;

            TaskInput.Visibility = Visibility.Collapsed;
            AddTaskButton.Visibility = Visibility.Collapsed;

            TaskList.Items.Clear();
            TaskList.Items.Add("失败");
            TaskList.Items.Add("嫉妒");
            TaskList.Items.Add("孤独");
            TaskList.Items.Add(Environment.UserName + "...");
            //TaskList.Items.Add("[Press any key to remember her face]");
        }

        private void ShowMeaningfulEnding()
        {
            MainGrid.Background = Brushes.Black;
            TaskList.Items.Clear();
            TitleText.Foreground = Brushes.White;
            TitleText.Text = "If you forget the pain, you forget yourself.";

            // 3 秒后淡出并关闭
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += (s, e) =>
            {
                timer.Stop();
                Application.Current.Shutdown();
            };
            timer.Start();
        }

        private void SaveTasks()
        {
            var tasks = new List<string>();
            foreach (var item in TaskList.Items)
                tasks.Add(item.ToString());
            File.WriteAllLines(taskSavePath, tasks);
        }

        private void LoadTasks()
        {
            if (File.Exists(taskSavePath))
            {
                var tasks = File.ReadAllLines(taskSavePath);
                foreach (var task in tasks)
                    TaskList.Items.Add(task);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveTasks();
            _dayCount++;
            File.WriteAllText(daySavePath, _dayCount.ToString());
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (_finalStage)
            {
                ShowMeaningfulEnding();
            }
        }
    }

}