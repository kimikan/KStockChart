using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace KChart.Chart
{
    public class LineGraph : LinearGraph
    {
        public LineGraph()
        {
            this.DefaultStyleKey = typeof(LineGraph);
            _lineGraph = new Polyline();

            BindBrush();
            BindStrokeThickness();
        }

        private void BindBrush()
        {
            Binding brushBinding = new Binding("Brush");
            brushBinding.Source = this;
            _lineGraph.SetBinding(Polyline.StrokeProperty, brushBinding);
        }

        private void BindStrokeThickness()
        {
            Binding thicknessBinding = new Binding("StrokeThickness");
            thicknessBinding.Source = this;
            _lineGraph.SetBinding(Polyline.StrokeThicknessProperty, thicknessBinding);
        }
        
        private Polyline _lineGraph;
        
        public override void OnCanvasInit(Canvas canvas)
        {
            Canvas = canvas;
            Canvas.Children.Add(_lineGraph);
        }

        /// <summary>
        /// Renders line graph.
        /// </summary>
        public override void Render(double lowest, double highest)
        {
            var points = new PointCollection();

            for (int i = 0; i < Values.Count; ++i)
            {
                var item = Values[i];
                var x = Canvas.ActualWidth - (i * IntervalWidth)  - (IntervalWidth / 2);
                var y = TranslateY(item.Value, lowest, highest, Canvas.ActualHeight);

                if (x > 10 && y <= Canvas.ActualHeight)
                {
                    points.Add(new Point() { X = x, Y = y });
                }
            }
            _lineGraph.Points = points;
        }

        /// <summary>
        /// Identifies <see cref="StrokeThickness"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(
            "StrokeThickness", typeof(double), typeof(LineGraph),
            new PropertyMetadata(2.0)
            );

        /// <summary>
        /// Gets or sets stroke thickness for a line graph line.
        /// This is a dependency property.
        /// The default is 2.
        /// </summary>
        public double StrokeThickness
        {
            get { return (double)GetValue(LineGraph.StrokeThicknessProperty); }
            set { SetValue(LineGraph.StrokeThicknessProperty, value); }
        }

        public override void OnItemsChanged()
        {
            Values = new List<dynamic>();
            for (int i = 0; i < Items.Count; ++i)
            {
                var item = Items[i];
                
                var value = Convert.ToDouble(item.GetType().GetProperty(ValueMemberPath).GetValue(item));
             
                Values.Add(new { Value = value });
            }
        }
    }
}
