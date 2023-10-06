using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
using RandomChoice.Misc;
using Timer = System.Threading.Timer;

namespace RandomChoice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FileOperator _operator;
        Process fileP;

        string resultSelection = "";
        uint selectionIndex = 0;

        Timer randomAnimate;
        short period        = 3;

        public MainWindow()
        {
            _operator = new FileOperator();

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // 顯示初始訊息
            choiceDisplay.Text = "點選左邊按鈕\n解決你的選擇困難";
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            // 使用Process開啟檔案
            try
            {
                fileP = Process.Start("Notepad.exe", _operator.FilePath + _operator.FileName);
                fileP.EnableRaisingEvents = true;
                fileP.Exited += P_Exited;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "出錯拉!!!", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void P_Exited(object? sender, EventArgs e)
        {
            // 重新開啟檔案
            var stream = _operator.OpenFile(_operator.FilePath + _operator.FileName);
            // 讀取檔案內容，寫回至Array
            App.choice = _operator.ReadFile(stream);
            // 關閉檔案
            _operator.CloseFile(stream);

            // 釋放資源
            fileP.Dispose();
        }

        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            // 將按鈕設定為不可按下
            btnRun.Dispatcher.Invoke(() => btnRun.IsEnabled = false);
            // 先選出最終的結果
            resultSelection = App.choice[new Random().Next(App.choice.Length)];
            // 重新設定Timer的中斷值 (單位ms)
            period = 3;
            // 使用Timer中斷函式，做出類似隨機選擇的效果
            randomAnimate = new Timer(randomAnimateCallback, "randomAnimateCallback", period, period);
        }

        private void randomAnimateCallback(object? state)
        {
            // 輪流顯示陣列中每個元素
            choiceDisplay.Dispatcher.Invoke(() => 
            {
                choiceDisplay.Foreground = (Brush)new BrushConverter().ConvertFromString("#B2B2B2");
                choiceDisplay.Text = App.choice[selectionIndex];
            });
            selectionIndex = (selectionIndex++ != App.choice.Length - 1) ? selectionIndex : 0;

            // 判斷Timer是否會在 1s 時中斷
            if (period < 1000)
            {
                // 將Timer的中斷值 * 原住民加成分數 (單位ms)
                period = (short)(period * 1.35);
                randomAnimate.Change(period, period);
            }
            else
            {
                // 顯示隨機挑選後的結果
                choiceDisplay.Dispatcher.Invoke(() =>
                {
                    choiceDisplay.Foreground = (Brush)new BrushConverter().ConvertFromString("#000000");
                    choiceDisplay.Text = resultSelection;
                });

                // 使用Timeout.Infinite關閉Timer計時
                randomAnimate.Change(Timeout.Infinite, Timeout.Infinite);
                // 釋放Timer的資源
                randomAnimate.Dispose();

                // 將按鈕設定可按下
                btnRun.Dispatcher.Invoke(() => btnRun.IsEnabled = true );
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
