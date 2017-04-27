using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace KChart.Chart
{
    /// <summary>
    /// KChart.xaml 的交互逻辑
    /// </summary>
    public partial class KChart : UserControl
    {
        public KChart()
        {
            InitializeComponent();
        }


        #region Properties

        public IList Graphs
        {
            get { return _graphs; }
            set { throw new System.NotSupportedException("Setting Graphs collection is not supported"); }
        }
        private ObservableCollection<LinearGraph> _graphs = new ObservableCollection<LinearGraph>();

        public IList OtherGraphs
        {
            get { return _othergraphs; }
            set { throw new System.NotSupportedException("Setting Graphs collection is not supported"); }
        }
        private ObservableCollection<LinearGraph> _othergraphs = new ObservableCollection<LinearGraph>();

        public static readonly DependencyProperty DataSourceProperty = DependencyProperty.Register(
            "DataSource", typeof(IList), typeof(KChart),
            new PropertyMetadata(null, new PropertyChangedCallback((d, arg) => {
                Trace.TraceInformation("xxxxxx render11");
                var me = d as KChart;
                if (me.DataSource == null)
                    return;

                //Do something
                me.UpdateAmount(me.amountCanvas.RenderSize, false);
                me.UpdateChart(me.chartCanvas.RenderSize, false);
                Trace.TraceInformation("start to render");
            })));
        public IList DataSource
        {
            get { return (IList)GetValue(DataSourceProperty); }
            set { SetValue(DataSourceProperty, value); }
        }


        public static readonly DependencyProperty UpdateByProperty = DependencyProperty.Register(
            "UpdateBy", typeof(object), typeof(KChart),
            new PropertyMetadata(null, new PropertyChangedCallback((d, arg) => {
                var me = d as KChart;
                //Do something
                if (me.DataSource == null)
                    return;
                /*
                foreach (var graph in me.OtherGraphs)
                {
                    (graph as LinearGraph).OnItemsChanged();
                }

                foreach(var graph in me.Graphs)
                {
                    (graph as LinearGraph).OnItemsChanged();
                }*/

                //Do something
                me.UpdateAmount(me.amountCanvas.RenderSize, true);
                me.UpdateChart(me.chartCanvas.RenderSize, true);
                Trace.TraceInformation("xxxxxx render UpdateBy");
            })));
        public object UpdateBy
        {
            get { return GetValue(UpdateByProperty); }
            set { SetValue(UpdateByProperty, value); }
        }


        public int IntervalWidth
        {
            get { return _intervalWidth; }
            set
            {
                if (_intervalWidth != value)
                {
                    _intervalWidth = value;
                }
            }
        }
        private int _intervalWidth = 10;

        #endregion

        public bool Inited
        {
            get;
            set;
        }

        private void InitCanvas()
        {
            foreach (var graph in _graphs)
            {
                if (graph != null && !chartCanvas.Children.Contains(graph))
                {
                    chartCanvas.Children.Add(graph);
                    graph.OnCanvasInit(chartCanvas);
                    graph.IntervalWidth = IntervalWidth;
                }
            }

            foreach (var graph in _othergraphs)
            {
                if (graph != null && !amountCanvas.Children.Contains(graph))
                {
                    amountCanvas.Children.Add(graph);
                    graph.OnCanvasInit(amountCanvas);
                    graph.IntervalWidth = IntervalWidth;
                }
            }
            Inited = true;
        }

        protected void UpdateAmount(Size newSize, bool force)
        {
            if (Inited)
            {
                double highest = double.MinValue;
                double lowest = double.MaxValue;
                int i = 0;
                foreach (var item in DataSource)
                {
                    var high = Convert.ToDouble(item.GetType().GetProperty(HighMemberPath2).GetValue(item));
                    var low = Convert.ToDouble(item.GetType().GetProperty(LowMemberPath2).GetValue(item));

                    if (amountCanvas.ActualWidth < ++i * IntervalWidth)
                        break;

                    if (high > highest)
                        highest = high;

                    if (low < lowest && low >= 0)
                        lowest = low;
                }


                foreach (var graph in _othergraphs)
                {
                    if (graph.Items == DataSource)
                    {
                        if (force)
                        {
                            graph.OnItemsChanged();
                        }
                    }
                    else
                    {
                        graph.Items = DataSource;
                    }
                    graph.Render(lowest, highest);
                }
            }
        }

        protected void UpdateChart(Size newSize, bool force)
        {
            if (Inited)
            {
                double highest = double.MinValue;
                double lowest = double.MaxValue;

                int i = 0;
                foreach (var item in DataSource)
                {
                    var high = Convert.ToDouble(item.GetType().GetProperty(HighMemberPath).GetValue(item));
                    var low = Convert.ToDouble(item.GetType().GetProperty(LowMemberPath).GetValue(item));

                    if ((chartCanvas.ActualWidth * (i + 10) / i) < ++i * IntervalWidth)
                        break;

                    if (high > highest)
                        highest = high;

                    if (low < lowest && low >= 0)
                        lowest = low;
                }

                foreach (var graph in _graphs)
                {
                    if (graph.Items == DataSource)
                    {
                        if (force)
                        {
                            graph.OnItemsChanged();
                        }
                    }
                    else
                    {
                        graph.Items = DataSource;
                    }
                    graph.Render(lowest, highest);
                }
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            InitCanvas();
        }

        public static readonly DependencyProperty HighMemberPathProperty = DependencyProperty.Register(
           "HighMemberPath", typeof(string), typeof(KChart), null);
        public string HighMemberPath
        {
            get { return (string)GetValue(HighMemberPathProperty); }
            set { SetValue(HighMemberPathProperty, value); }
        }

        public static readonly DependencyProperty LowMemberPathProperty = DependencyProperty.Register(
           "LowMemberPath", typeof(string), typeof(KChart), null);
        public string LowMemberPath
        {
            get { return (string)GetValue(LowMemberPathProperty); }
            set { SetValue(LowMemberPathProperty, value); }
        }

        public static readonly DependencyProperty HighMemberPath2Property = DependencyProperty.Register(
           "HighMemberPath2", typeof(string), typeof(KChart), null);
        public string HighMemberPath2
        {
            get { return (string)GetValue(HighMemberPath2Property); }
            set { SetValue(HighMemberPath2Property, value); }
        }

        public static readonly DependencyProperty LowMemberPath2Property = DependencyProperty.Register(
           "LowMemberPath2", typeof(string), typeof(KChart), null);
        public string LowMemberPath2
        {
            get { return (string)GetValue(LowMemberPath2Property); }
            set { SetValue(LowMemberPath2Property, value); }
        }

        public static readonly DependencyProperty Description1Property = DependencyProperty.Register(
           "Description1", typeof(string), typeof(KChart), null);
        public string Description1
        {
            get { return (string)GetValue(Description1Property); }
            set { SetValue(Description1Property, value); }
        }

        public static readonly DependencyProperty Description2Property = DependencyProperty.Register(
           "Description2", typeof(string), typeof(KChart), null);
        public string Description2
        {
            get { return (string)GetValue(Description2Property); }
            set { SetValue(Description2Property, value); }
        }

        private void chartCanvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            foreach (var graph in _graphs)
            {
                if (graph != null)
                {
                    var str = graph.OnMouseMove(sender, e);

                    if (!string.IsNullOrEmpty(str))
                    {
                        description1.Text = str;
                        break;
                    }
                }
            }

            foreach (var graph in _othergraphs)
            {
                if (graph != null)
                {
                    var str = graph.OnMouseMove(sender, e);

                    if (!string.IsNullOrEmpty(str))
                    {
                        description2.Text = str;
                        break;
                    }
                }
            }
        }
    }
}
