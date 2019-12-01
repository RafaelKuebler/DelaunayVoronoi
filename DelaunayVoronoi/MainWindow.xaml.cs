using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Shapes;
using BenchmarkDotNet.Attributes;

namespace DelaunayVoronoi
{
    public partial class MainWindow : Window
    {
        private DelaunayTriangulator delaunay = new DelaunayTriangulator();
        private Voronoi voronoi = new Voronoi();

        public MainWindow()
        {
            InitializeComponent();
            //С оптимизацией

            BuildVoronoiDiagram();
        }

        public void BuildVoronoiDiagram()
        {
            var points = delaunay.GeneratePoints(5000, 800, 400);

            var delaunayTimer = Stopwatch.StartNew();
            var triangulation = delaunay.BowyerWatson(in points);
            delaunayTimer.Stop();

            DrawTriangulation(in triangulation);

            var voronoiTimer = Stopwatch.StartNew();
            var vornoiEdges = voronoi.GenerateEdgesFromDelaunay(in triangulation);
            voronoiTimer.Stop();

            DrawVoronoi(in vornoiEdges);

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

        private void DrawTriangulation(in HashSet<Triangle> triangulation)
        {
            var edges = new List<Edge>(triangulation.Count*3){};
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

        private void DrawVoronoi(in HashSet<Edge> voronoiEdges)
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

        private void Rebuild__Click(object sender, RoutedEventArgs e)
        {
            Canvas.Children.Clear();
            BuildVoronoiDiagram();
        }
    }
}