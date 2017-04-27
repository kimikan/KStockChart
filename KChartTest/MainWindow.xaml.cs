using System.Collections;
using System.Collections.Generic;
using System.Windows;

namespace KChartTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitData();

            DataContext = this;
            
        }

        private void InitData()
        {
            for (int i = 0; i < 15; ++i)
            {
                _klines.Add(new { Name = "Test", Low = 20, Open = 30, Close = 40, High = 50, M2 = 43, Amount=15, M5 = 0 });
                _klines.Add(new { Name = "Test", Low = 33, Open = 39, Close = 46, High = 57, M2 = 32, Amount = 15, M5 = 0 });
                _klines.Add(new { Name = "Test", Low = 18, Open = 46, Close = 27, High = 46, M2 = 25, Amount = 12, M5 = 0 });
                _klines.Add(new { Name = "Test", Low = 14, Open = 27, Close = 23, High = 30, M2 = 21, Amount = 17, M5 = 0 });
                _klines.Add(new { Name = "Test", Low = 12, Open = 23, Close = 19, High = 28, M2 = 16, Amount = 15, M5 = 0 });
                _klines.Add(new { Name = "Test", Low = 10, Open = 19, Close = 13, High = 24, M2 = 14, Amount = 8, M5 = 0 });
                _klines.Add(new { Name = "Test", Low = 7, Open = 13, Close = 15, High = 17, M2 = 17, Amount = 15, M5 = 0 });
                _klines.Add(new { Name = "Test", Low = 15, Open = 15, Close = 19, High = 25, M2 = 22, Amount = 3, M5 = 0 });
                _klines.Add(new { Name = "Test", Low = 17, Open = 19, Close = 24, High = 31, M2 = 21, Amount = 6, M5 = 0 });
                _klines.Add(new { Name = "Test", Low = 13, Open = 24, Close = 18, High = 27, M2 = 18, Amount = 9, M5 = 0 });
                _klines.Add(new { Name = "Test", Low = 18, Open = 18, Close = 19, High = 22, M2 = 24, Amount = 21, M5 = 0 });
                _klines.Add(new { Name = "Test", Low = 11, Open = 19, Close = 30, High = 34, M2 = 32, Amount = 3, M5 = 0 });
                _klines.Add(new { Name = "Test", Low = 19, Open = 30, Close = 35, High = 39, M2 = 30, Amount = 67, M5 = 0 });
                _klines.Add(new { Name = "Test", Low = 20, Open = 35, Close = 26, High = 40, M2 = 28, Amount = 6, M5 = 0 });
                _klines.Add(new { Name = "Test", Low = 15, Open = 26, Close = 21, High = 30, M2 = 25, Amount = 34, M5 = 0 });
                _klines.Add(new { Name = "Test", Low = 11, Open = 21, Close = 30, High = 33, M2 = 35, Amount = 3, M5 = 0 });
            }
        }

        public IList KLines
        {
            get
            {
                return _klines;
            }
        }
        private List<dynamic> _klines = new List<dynamic>();
    }
}
