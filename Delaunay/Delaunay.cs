using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Delaunay
{
    public class DelaunayTriangulator
    {
        private double MaxX { get; set; }
        private double MaxY { get; set; }
        private List<Triangle> border;

        public IEnumerable<Point> GeneratePoints(int amount, double maxX, double maxY)
        {
            MaxX = maxX;
            MaxY = maxY;
            // TODO make more beautiful
            var point0 = new Point(0, 0);
            var point1 = new Point(0, MaxY);
            var point2 = new Point(MaxX, MaxY);
            var point3 = new Point(MaxX, 0);
            var points = new List<Point>() { point0, point1, point2, point3 };
            var tri1 = new Triangle(point0, point1, point2);
            var tri2 = new Triangle(point0, point2, point3);
            border = new List<Triangle>() { tri1, tri2 };

            var random = new Random();
            for (int i = 0; i < amount - 4; i++)
            {
                var pointX = random.NextDouble() * MaxX;
                var pointY = random.NextDouble() * MaxY;
                points.Add(new Point(pointX, pointY));
            }

            return points;
        }

        public IEnumerable<Triangle> BowyerWatson(IEnumerable<Point> points)
        {
            // for border: when supratriangle is not big enough, edges of convex hull are sometimes
            // not part of the triangulation, using two triangles enclosing all generated points fixes this
            // (remove all supratriangle references in that case)
            //var tri1 = new Triangle(new Point(0, 0), new Point(0, MaxY), new Point(MaxX, MaxY));
            //var tri2 = new Triangle(new Point(0, 0), new Point(MaxX, MaxY), new Point(MaxX, 0));
            //var triangulation = new HashSet<Triangle>() { tri1, tri2 };
            //var supraTriangle = GenerateSupraTriangle();
            //var triangulation = new HashSet<Triangle>() { supraTriangle };
            var triangulation = new HashSet<Triangle>(border);

            foreach (var point in points)
            {
                var badTriangles = FindBadTriangles(point, triangulation);
                var polygon = FindHoleBoundaries(badTriangles);

                triangulation.RemoveWhere(o => badTriangles.Contains(o));

                foreach (var edge in polygon)
                {
                    var triangle = new Triangle(point, edge);
                    triangulation.Add(triangle);
                }
            }

            //triangulation.RemoveWhere(o => o.Vertices.Any(v => supraTriangle.Vertices.Contains(v)));

            return triangulation;
        }

        private List<Edge> FindHoleBoundaries(ISet<Triangle> badTriangles)
        {
            var edges = badTriangles.SelectMany(o => o.Edges);
            var grouped = edges.GroupBy(o => o);
            var boundaryEdges = edges.GroupBy(o => o).Where(o => o.Count() == 1).Select(o => o.First());
            return boundaryEdges.ToList();
        }

        private Triangle GenerateSupraTriangle()
        {
            //   1  -> maxX
            //  / \
            // 2---3
            // |
            // v maxY
            var margin = 500;
            var point1 = new Point(0.5 * MaxX, -2 * MaxX - margin);
            var point2 = new Point(-2 * MaxY - margin, 2 * MaxY + margin);
            var point3 = new Point(2 * MaxX + MaxY + margin, 2 * MaxY + margin);
            return new Triangle(point1, point2, point3);
        }

        private ISet<Triangle> FindBadTriangles(Point point, HashSet<Triangle> triangles)
        {
            var badTriangles = triangles.Where(o => IsPointInsideCircumcircle(point, o));
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
                (ax_ * ax_ + ay_ * ay_) * (bx_ * cy_ - by_ * cx_) +
                (bx_ * bx_ + by_ * by_) * (ay_ * cx_ - ax_ * cy_) +
                (cx_ * cx_ + cy_ * cy_) * (ax_ * by_ - ay_ * bx_);
            return det > 0;
        }
    }
}