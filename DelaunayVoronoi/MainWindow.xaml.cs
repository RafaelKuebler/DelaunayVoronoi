using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Shapes;

namespace DelaunayVoronoi
{
    public partial class MainWindow : Window
    {
        private DelaunayTriangulator delaunay = new DelaunayTriangulator();
        private Voronoi voronoi = new Voronoi();

        public MainWindow()
        {
            InitializeComponent();

            var points = delaunay.GeneratePoints(5000, 800, 400);

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
                var myEllipse = new Ellipse
                {
                    Fill = System.Windows.Media.Brushes.Red,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Width = 1,
                    Height = 1
                };
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
                var line = new Line
                {
                    Stroke = System.Windows.Media.Brushes.LightSteelBlue,
                    StrokeThickness = 0.5,

                    X1 = edge.Point1.X,
                    X2 = edge.Point2.X,
                    Y1 = edge.Point1.Y,
                    Y2 = edge.Point2.Y
                };

                Canvas.Children.Add(line);
            }
        }

        private void DrawVoronoi(IEnumerable<Edge> voronoiEdges)
        {
            foreach (var edge in voronoiEdges)
            {
                var line = new Line
                {
                    Stroke = System.Windows.Media.Brushes.DarkViolet,
                    StrokeThickness = 1,

                    X1 = edge.Point1.X,
                    X2 = edge.Point2.X,
                    Y1 = edge.Point1.Y,
                    Y2 = edge.Point2.Y
                };

                Canvas.Children.Add(line);
            }
        }
    }
}