using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using BenchmarkDotNet.Attributes;

namespace DelaunayVoronoi
{
    public class DelaunayTriangulator
    {
        private double                MaxX { get; set; }
        private double                MaxY { get; set; }
        private IEnumerable<Triangle> border;

        public List<Point> GeneratePoints(int amount, double maxX, double maxY)
        {
            MaxX = maxX;
            MaxY = maxY;

            // TODO make more beautiful
            var point0 = new Point(0, 0);
            var point1 = new Point(0, MaxY);
            var point2 = new Point(MaxX, MaxY);
            var point3 = new Point(MaxX, 0);
            var points = new List<Point>() {point0, point1, point2, point3};
            var tri1   = new Triangle(point0, point1, point2);
            var tri2   = new Triangle(point0, point2, point3);
            border = new List<Triangle>() {tri1, tri2};

            var random = new Random();
            for (int i = 0; i < amount - 4; i++)
            {
                var pointX = random.NextDouble() * MaxX;
                var pointY = random.NextDouble() * MaxY;
                points.Add(new Point(pointX, pointY));
            }

            return points;
        }

        public HashSet<Triangle> BowyerWatson(in List<Point> points)
        {
            var triangulation = new HashSet<Triangle>(border);

            foreach (var point in points)
            {
                var badTriangles = new HashSet<Triangle>();

                FindBadTriangles(in point, in triangulation, ref badTriangles);

                if (badTriangles.Count > 0)
                {
                    var polygon = FindHoleBoundaries(in badTriangles);
                    foreach (var edge in polygon)
                    {
                        var triangle = new Triangle(point, edge.Point1, edge.Point2);
                        triangulation.Add(triangle);
                    }
                }

                foreach (var triangle in badTriangles)
                {
                    foreach (var vertex in triangle.Vertices)
                    {
                        vertex.AdjacentTriangles.Remove(triangle);
                    }
                }

                triangulation.ExceptWith(badTriangles);
            }

            return triangulation;
        }
        
        private List<Edge> FindHoleBoundaries(in HashSet<Triangle> badTriangles)
        {
            var edges = new List<Edge>(badTriangles.Count*3);

            foreach (var triangle in badTriangles)
            {
                edges.Add(new Edge(triangle.Vertices[0], triangle.Vertices[1]));
                edges.Add(new Edge(triangle.Vertices[1], triangle.Vertices[2]));
                edges.Add(new Edge(triangle.Vertices[2], triangle.Vertices[0]));
            }
            
            var boundaryEdges = edges.GroupBy(o => o).Where(o => o.Count() == 1).Select(o => o.First()).ToList();
            return boundaryEdges;
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

        private void FindBadTriangles(in  Point             point, in HashSet<Triangle> triangles,
                                      ref HashSet<Triangle> badTriangles)
        {
            foreach (var triangle in triangles)
            {
                if (triangle.IsPointInsideCircumcircle(in point))
                    badTriangles.Add(triangle);
            }
        }
    }
}