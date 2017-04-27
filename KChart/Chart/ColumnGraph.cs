using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace KChart.Chart
{
    public class ColumnGraph : LinearGraph
    {
        public ColumnGraph()
        {
            _columnGraph = new Path();
            _columnGraphGeometry = new PathGeometry();
            _columnGraph.Data = _columnGraphGeometry;

            BindBrush();
        }

        private void BindBrush()
        {
            Binding brushBinding = new Binding("Brush");
            brushBinding.Source = this;
            _columnGraph.SetBinding(Path.FillProperty, brushBinding);
        }
        
        private Path _columnGraph;
        private PathGeometry _columnGraphGeometry;
        
        /// <summary>
        /// Renders graph.
        /// </summary>
        public override void Render(double lowest, double highest)
        {
            if (Items != null)
            {
                int changeCount = Math.Min(Items.Count, _columnGraphGeometry.Figures.Count);
                ChangeColumns(changeCount, lowest, highest);
                int diff = Items.Count - _columnGraphGeometry.Figures.Count;
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
            for (int i = changeCount; i < Items.Count; i++)
            {
                PathFigure column = new PathFigure();
                _columnGraphGeometry.Figures.Add(column);
                for (int si = 0; si < 4; si++)
                {
                    column.Segments.Add(new LineSegment());
                }
                SetColumnSegments(i, lowest, highest);
            }
        }

        private void RemoveColumns(int changeCount)
        {
            for (int i = _columnGraphGeometry.Figures.Count - 1; i >= changeCount; i--)
            {
                _columnGraphGeometry.Figures.RemoveAt(i);
            }
        }

        private void ChangeColumns(int changeCount, double lowest, double highest)
        {
            for (int i = 0; i < changeCount; i++)
            {
                SetColumnSegments(i, lowest, highest);
            }
        }

        private void SetColumnSegments(int index, double lowest, double highest)
        {
            var item = Values[index];
            double width = IntervalWidth - 3;
            double left = Canvas.ActualWidth - (index + 1) * IntervalWidth;
            double right = left + width;
            
            double y1 = Canvas.ActualHeight;
            double y2 = TranslateY(item.Value, lowest, highest, Canvas.ActualHeight);

            if (right < 10)
                y1 = y2;

            _columnGraphGeometry.Figures[index].StartPoint = new Point(left, y1);
            (_columnGraphGeometry.Figures[index].Segments[0] as LineSegment).Point = new Point(right, y1);
            (_columnGraphGeometry.Figures[index].Segments[1] as LineSegment).Point = new Point(right, y2);
            (_columnGraphGeometry.Figures[index].Segments[2] as LineSegment).Point = new Point(left, y2);
            (_columnGraphGeometry.Figures[index].Segments[3] as LineSegment).Point = new Point(left, y1);
        }

        public override void OnCanvasInit(Canvas canvas)
        {
            Canvas = canvas;
            Canvas.Children.Add(_columnGraph);
        }

        public override void OnItemsChanged()
        {
            if (Items != null)
            {
                Values.Clear();
                foreach (var item in Items)
                {
                    var value = Convert.ToDouble(item.GetType().GetProperty(ValueMemberPath).GetValue(item));
                    Values.Add(new
                    {
                        Value = value,
                    });
                }
            }
        }//end func
    }
}