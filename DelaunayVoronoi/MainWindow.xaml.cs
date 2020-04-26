using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;
using Delaunay.Annotations;

namespace DelaunayVoronoi
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private DelaunayTriangulator delaunay = new DelaunayTriangulator();
        private Voronoi voronoi = new Voronoi();
        public int PointCount { get; set; } = 2000;
        public double DiagramWidth => (int)Canvas.ActualWidth;
        public double DiagramHeight => (int)Canvas.ActualHeight;

        public ICommand DrawCommand { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;

            DrawCommand = new Command(param => GenerateAndDraw());
            Canvas.SizeChanged += (sender, args) =>
            {
                OnPropertyChanged(nameof(DiagramHeight));
                OnPropertyChanged(nameof(DiagramWidth));
            };

        }

        private void GenerateAndDraw()
        {
            Canvas.Children.Clear();
            Canvas.ClipToBounds = true;
            var points = delaunay.GeneratePoints(PointCount, DiagramWidth, DiagramHeight);

            var delaunayTimer = Stopwatch.StartNew();
            var triangulation = delaunay.BowyerWatson(points);
            delaunayTimer.Stop();
            DrawTriangulation(triangulation);

            var voronoiTimer = Stopwatch.StartNew();
            var vornoiEdges = voronoi.GenerateEdgesFromDelaunay(triangulation);
            voronoiTimer.Stop();
            DrawVoronoi(vornoiEdges);

            DrawPoints(points);
        }

        private void DrawPoints(IEnumerable<Point> points)
        {
            foreach (var point in points)
            {
                var myEllipse = new Ellipse();
                myEllipse.Fill = System.Windows.Media.Brushes.Red;
                myEllipse.HorizontalAlignment = HorizontalAlignment.Left;
                myEllipse.VerticalAlignment = VerticalAlignment.Top;
                myEllipse.Width = 1;
                myEllipse.Height = 1;
                var ellipseX = point.X - 0.5 * myEllipse.Height;
                var ellipseY = point.Y - 0.5 * myEllipse.Width;
                myEllipse.Margin = new Thickness(ellipseX, ellipseY, 0, 0);

                Canvas.Children.Add(myEllipse);
            }
        }

        private void DrawTriangulation(IEnumerable<Triangle> triangulation)
        {
            var edges = new List<Edge>();
            foreach (var triangle in triangulation)
            {
                edges.Add(new Edge(triangle.Vertices[0], triangle.Vertices[1]));
                edges.Add(new Edge(triangle.Vertices[1], triangle.Vertices[2]));
                edges.Add(new Edge(triangle.Vertices[2], triangle.Vertices[0]));
            }

            foreach (var edge in edges)
            {
                var line = new Line();
                line.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
                line.StrokeThickness = 0.5;

                line.X1 = edge.Point1.X;
                line.X2 = edge.Point2.X;
                line.Y1 = edge.Point1.Y;
                line.Y2 = edge.Point2.Y;

                Canvas.Children.Add(line);
            }
        }

        private void DrawVoronoi(IEnumerable<Edge> voronoiEdges)
        {
            foreach (var edge in voronoiEdges)
            {
                var line = new Line();
                line.Stroke = System.Windows.Media.Brushes.DarkViolet;
                line.StrokeThickness = 1;

                line.X1 = edge.Point1.X;
                line.X2 = edge.Point2.X;
                line.Y1 = edge.Point1.Y;
                line.Y2 = edge.Point2.Y;

                Canvas.Children.Add(line);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Command : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public Command(Action<object> execute)
            : this(execute, param => true)
        {
        }

        public Command(Action<object> execute, Func<object, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return (_canExecute == null) || _canExecute(parameter);
        }
    }
}