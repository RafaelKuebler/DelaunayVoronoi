using System;
using System.Collections.Generic;
using System.Linq;
namespace DelaunayVoronoi
{
    public class Triangle
    {
        public Point[] Vertices { get; } = new Point[3];
        public Point Circumcenter { get; private set; }
        private double _radiusSquared;

        public HashSet<Triangle> TrianglesWithSharedEdge {
            get
            {
                var neighbors = new HashSet<Triangle>();
                foreach (var vertex in Vertices)
                {
                    foreach (var triangle in vertex.AdjacentTriangles)
                    {
                        if (triangle != this && this.SharesEdgeWith(triangle))
                        {
                            neighbors.Add(triangle);
                        }
                    }
                    neighbors.UnionWith(neighbors);
                }
                return neighbors;
            }
        }

        public Triangle(in Point point1,in Point point2,in Point point3)
        {
            if (!IsCounterClockwise(point1, point2, point3))
            {
                Vertices[0] = point1;
                Vertices[1] = point3;
                Vertices[2] = point2;
            }
            else
            {
                Vertices[0] = point1;
                Vertices[1] = point2;
                Vertices[2] = point3;
            }

            Vertices[0].AdjacentTriangles.Add(this);
            Vertices[1].AdjacentTriangles.Add(this);
            Vertices[2].AdjacentTriangles.Add(this);
            UpdateCircumcircle();
        }

        private void UpdateCircumcircle()
        {
            var p0 = Vertices[0];
            var p1 = Vertices[1];
            var p2 = Vertices[2];
            var dA = p0.X * p0.X + p0.Y * p0.Y;
            var dB = p1.X * p1.X + p1.Y * p1.Y;
            var dC = p2.X * p2.X + p2.Y * p2.Y;

            var aux1 = (dA * (p2.Y - p1.Y) + dB * (p0.Y - p2.Y) + dC * (p1.Y - p0.Y));
            var aux2 = -(dA * (p2.X - p1.X) + dB * (p0.X - p2.X) + dC * (p1.X - p0.X));
            var div = (2 * (p0.X * (p2.Y - p1.Y) + p1.X * (p0.Y - p2.Y) + p2.X * (p1.Y - p0.Y)));

            if (div == 0)
            {
                throw new System.Exception();
            }

            var center = new Point(aux1 / div, aux2 / div);
            Circumcenter = center;
            _radiusSquared = (center.X - p0.X) * (center.X - p0.X) + (center.Y - p0.Y) * (center.Y - p0.Y);
        }

        private bool IsCounterClockwise(in Point point1, in Point point2, in Point point3)
        {
            var result = (point2.X - point1.X) * (point3.Y - point1.Y) -
                (point3.X - point1.X) * (point2.Y - point1.Y);
            return result > 0;
        }

        public bool SharesEdgeWith(in Triangle triangle)
        {
            int shaderVerticesCount = 0;

            foreach (var vertex in Vertices)
            {
                if (triangle.Vertices.Contains(vertex))
                {
                    shaderVerticesCount++;
                }
            }

            return shaderVerticesCount == 2;
        }

        public bool IsPointInsideCircumcircle(in Point point)
        {
            var d_squared = (point.X - Circumcenter.X) * (point.X - Circumcenter.X) +
                            (point.Y - Circumcenter.Y) * (point.Y - Circumcenter.Y);
            return d_squared < _radiusSquared;
        }
    }
}