using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace Delaunay
{
    public class DelaunayTriangulator
    {
        private double MaxX { get; set; }
        private double MaxY { get; set; }

        public IEnumerable<Point> GeneratePoints(int amount, double maxX, double maxY)
        {
            MaxX = maxX;
            MaxY = maxY;
            var points = new List<Point>();
            var random = new Random();

            // border: gives problems when supra triangle is not big enough
            /*points.Add(new Point(0, 0));
            points.Add(new Point(0, maxY));
            points.Add(new Point(maxX, maxY));
            points.Add(new Point(maxX, 0));*/
            for (int i = 0; i < amount; i++)
            {
                var x = random.NextDouble() * maxX;
                var y = random.NextDouble() * maxY;
                points.Add(new Point(x, y));
            }

            return points;
        }

        public HashSet<Triangle> BowyerWatson(IEnumerable<Point> points)
        {
            // workaround for border problem: use two initial triangles and remove all supratriangle references
            /*var tri1 = new Triangle(new Point(0, 0), new Point(0, MaxY), new Point(MaxX, MaxY));
            var tri2 = new Triangle(new Point(0, 0), new Point(MaxX, MaxY), new Point(MaxX, 0));*/
            var superTriangle = GenerateSuperTriangle();
            var triangulation = new HashSet<Triangle>() { superTriangle };

            foreach (var point in points)
            {
                var badTriangles = FindBadTriangles(point, triangulation);
                var polygon = FindHoleBoundaries(badTriangles); // find boundaries of hole

                triangulation.RemoveWhere(o => badTriangles.Contains(o));

                foreach (var edge in polygon.Edges)
                {
                    var triangle = new Triangle(point, edge);
                    triangulation.Add(triangle);
                }
            }

            triangulation.RemoveWhere(o => o.Vertices.Any(v => superTriangle.Vertices.Contains(v)));

            return triangulation;
        }

        private Polygon FindHoleBoundaries(ISet<Triangle> badTriangles)
        {
            var edges = badTriangles.SelectMany(o => o.Edges);
            var grouped = edges.GroupBy(o => o);
            var boundaryEdges = edges.GroupBy(o => o).Where(o => o.Count() == 1).Select(o => o.First());
            return new Polygon(boundaryEdges);
        }

        private Triangle GenerateSuperTriangle()
        {
            var margin = 500;
            //   1  -> maxX
            //  / \
            // 2---3
            // |
            // v maxY
            var point1 = new Point(0.5 * MaxX, -2 * MaxX - margin);
            var point2 = new Point(-2 * MaxY - margin, 2 * MaxY + margin);
            var point3 = new Point(2 * MaxX + MaxY + margin, 2 * MaxY + margin);
            return new Triangle(point1, point2, point3);
        }

        private ISet<Triangle> FindBadTriangles(Point point, HashSet<Triangle> triangulations)
        {
            var badTriangles = triangulations.Where(o => IsPointInsideCircumcircle(point, o));
            return new HashSet<Triangle>(badTriangles);
        }

        private bool IsPointInsideCircumcircle(Point point, Triangle triangle)
        {
            var ax_ = triangle.Vertices[0].X - point.X;
            var ay_ = triangle.Vertices[0].Y - point.Y;
            var bx_ = triangle.Vertices[1].X - point.X;
            var by_ = triangle.Vertices[1].Y - point.Y;
            var cx_ = triangle.Vertices[2].X - point.X;
            var cy_ = triangle.Vertices[2].Y - point.Y;

            var det =
                (ax_ * ax_ + ay_ * ay_) * (bx_ * cy_ - cx_ * by_) -
                (bx_ * bx_ + by_ * by_) * (ax_ * cy_ - cx_ * ay_) +
                (cx_ * cx_ + cy_ * cy_) * (ax_ * by_ - bx_ * ay_);
            return det > 0;
        }
    }
}