﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Shapes;

namespace Delaunay
{
    public partial class MainWindow : Window
    {
        private DelaunayTriangulator delaunay = new DelaunayTriangulator();

        public MainWindow()
        {
            InitializeComponent();

            var points = delaunay.GeneratePoints(400, 800, 400);
            DrawPoints(points);
            var triangulation = delaunay.BowyerWatson(points);
            DrawTriangulation(triangulation);
        }

        private void DrawPoints(IEnumerable<Point> points)
        {
            foreach (var point in points)
            {
                var myEllipse = new Ellipse();
                myEllipse.Fill = System.Windows.Media.Brushes.Red;
                myEllipse.HorizontalAlignment = HorizontalAlignment.Left;
                myEllipse.VerticalAlignment = VerticalAlignment.Top;
                myEllipse.Width = 10;
                myEllipse.Height = 10;
                var ellipseX = point.X - 0.5 * myEllipse.Height;
                var ellipseY = point.Y - 0.5 * myEllipse.Width;
                myEllipse.Margin = new Thickness(ellipseX, ellipseY, 0, 0);

                Canvas.Children.Add(myEllipse);
            }
        }

        private void DrawTriangulation(HashSet<Triangle> triangulation)
        {
            var edges = triangulation.SelectMany(o => o.Edges).Distinct();

            foreach (var edge in edges)
            {
                var line = new Line();
                line.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
                line.StrokeThickness = 2;

                line.X1 = edge.Point1.X;
                line.X2 = edge.Point2.X;
                line.Y1 = edge.Point1.Y;
                line.Y2 = edge.Point2.Y;

                Canvas.Children.Add(line);
            }
        }
    }
}