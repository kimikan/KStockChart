using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace KChart.Chart
{
    public class StockGraph : LinearGraph
    {
        public StockGraph()
        {
        }
        
        private List<Path> _StockGraphs = new List<Path>();

        /// <summary>
        /// Applies control template.
        /// </summary>
        public override void OnCanvasInit(Canvas canvas)
        {
            Canvas = canvas;
        }

        /// <summary>
        /// Renders graph.
        /// </summary>
        public override void Render(double lowest, double highest)
        {
            if (Items != null)
            {
                int changeCount = Math.Min(Items.Count, _StockGraphs.Count);
                ChangeColumns(changeCount, lowest, highest);
                int diff = Items.Count - _StockGraphs.Count;
                
                if (diff > 0)
                {
                    AddColumns(changeCount, lowest, highest);
                }
                else if (diff < 0)
                {
                    RemoveColumns(changeCount);
                }
            }
        }
       

        private void AddColumns(int changeCount, double lowest, double highest)
        {
            for (int i = changeCount; i < Values.Count; i++)
            {
                var path = new Path();
                path.Stroke = new SolidColorBrush(Colors.Black);
                var geo = new PathGeometry();
                PathFigure column = new PathFigure();
                geo.Figures.Add(column);
                //path.Fill = new SolidColorBrush(Colors.Black);
                for (int si = 0; si < 12; si++)
                {
                    column.Segments.Add(new LineSegment());
                }
                path.Data = geo;
                _StockGraphs.Add(path);

                //foreach (var p in _StockGraphs)
                Canvas.Children.Add(path);
                SetColumnSegments(i, lowest, highest);
            }
        }

        private void RemoveColumns(int changeCount)
        {
            for (int i = _StockGraphs.Count - 1; i >= changeCount; i--)
            {
                Canvas.Children.Remove(_StockGraphs[i]);
                _StockGraphs.RemoveAt(i);
            }
        }

        private void ChangeColumns(int changeCount, double lowest, double highest)
        {
            for (int i = 0; i < changeCount; i++)
            {
                SetColumnSegments(i, lowest, highest);
            }
        }

        public override string OnMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var p = e.GetPosition(Canvas);

            var index = (int)((Canvas.ActualWidth - p.X) / IntervalWidth);

            if(index >= 0 && index < Items.Count)
            {
                var item = Items[index];

                return item.ToString();
            }

            return string.Empty;
        }

        public override void OnItemsChanged()
        {
            if(Items != null)
            {
                Values.Clear();
                foreach(var item in Items)
                {
                    var open = Convert.ToDouble(item.GetType().GetProperty(OpenMemberPath).GetValue(item));
                    var close = Convert.ToDouble(item.GetType().GetProperty(CloseMemberPath).GetValue(item));
                    var high = Convert.ToDouble(item.GetType().GetProperty(HighMemberPath).GetValue(item));
                    var low = Convert.ToDouble(item.GetType().GetProperty(LowMemberPath).GetValue(item));
                    Values.Add( new {
                        Open = (open),
                        Close = (close),
                        High = (high),
                        Low = (low) }
                    );
                }
            }
        }

        private void SetColumnSegments(int index, double lowest, double highest)
        {
            var item = Values[index];
            double width = IntervalWidth - 3;
            double left = Canvas.ActualWidth - (index + 1) * IntervalWidth;
            double right = left + width;
            if (right <= 10)
            {
                //No need to draw.
                _StockGraphs[index].Visibility = Visibility.Hidden;
                return;
            }

            double low = TranslateY(item.Low, lowest, highest, Canvas.ActualHeight);
            double open = TranslateY(item.Open, lowest, highest, Canvas.ActualHeight);
            double close = TranslateY(item.Close, lowest, highest, Canvas.ActualHeight);
            double high = TranslateY(item.High, lowest, highest, Canvas.ActualHeight);
            var up = Math.Min(open, close);
            var down = Math.Max(open, close);
            var lw = width / 100;
            var time = left + width / 2;
            
            _StockGraphs[index].Visibility = Visibility.Visible;

            if (low >= down && high <= up)
            {
                if (open >= close)
                {
                    _StockGraphs[index].Fill = Brush;
                }
                else
                    _StockGraphs[index].Fill = new SolidColorBrush(Colors.LawnGreen);
                
                var geo = _StockGraphs[index].Data;
                var g = geo as PathGeometry;
                if (g != null)
                {
                    g.Figures[0].StartPoint = new Point(left, down);
                    (g.Figures[0].Segments[0] as LineSegment).Point = new Point(left, up);
                    (g.Figures[0].Segments[1] as LineSegment).Point = new Point(time - lw, up);
                    (g.Figures[0].Segments[2] as LineSegment).Point = new Point(time - lw, high);
                    (g.Figures[0].Segments[3] as LineSegment).Point = new Point(time + lw, high);
                    (g.Figures[0].Segments[4] as LineSegment).Point = new Point(time + lw, up);
                    
                    (g.Figures[0].Segments[5] as LineSegment).Point = new Point(right, up);
                    (g.Figures[0].Segments[6] as LineSegment).Point = new Point(right, down);
                    (g.Figures[0].Segments[7] as LineSegment).Point = new Point(time + lw, down);
                    (g.Figures[0].Segments[8] as LineSegment).Point = new Point(time + lw, low);
                    (g.Figures[0].Segments[9] as LineSegment).Point = new Point(time - lw, low);
                    (g.Figures[0].Segments[10] as LineSegment).Point = new Point(time - lw, down);
                    (g.Figures[0].Segments[11] as LineSegment).Point = new Point(left, down);

                }
            }
        }

        public static readonly DependencyProperty HighMemberPathProperty = DependencyProperty.Register(
           "HighMemberPath", typeof(string), typeof(StockGraph), null);
        public string HighMemberPath
        {
            get { return (string)GetValue(HighMemberPathProperty); }
            set { SetValue(HighMemberPathProperty, value); }
        }

        public static readonly DependencyProperty LowMemberPathProperty = DependencyProperty.Register(
           "LowMemberPath", typeof(string), typeof(StockGraph), null);
        public string LowMemberPath
        {
            get { return (string)GetValue(LowMemberPathProperty); }
            set { SetValue(LowMemberPathProperty, value); }
        }

        public static readonly DependencyProperty OpenMemberPathProperty = DependencyProperty.Register(
           "OpenMemberPath", typeof(string), typeof(StockGraph), null);
        public string OpenMemberPath
        {
            get { return (string)GetValue(OpenMemberPathProperty); }
            set { SetValue(OpenMemberPathProperty, value); }
        }

        public static readonly DependencyProperty CloseMemberPathProperty = DependencyProperty.Register(
           "CloseMemberPath", typeof(string), typeof(StockGraph), null);
        public string CloseMemberPath
        {
            get { return (string)GetValue(CloseMemberPathProperty); }
            set { SetValue(CloseMemberPathProperty, value); }
        }

    }
}
