using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DelaunayVoronoi
{
    public class Triangle
    {
        public Point[] Vertices { get; }
        public Point[] BorderVertices { get; }
        public Point Circumcenter { get; private set; }
        public double RadiusSquared;

        public IEnumerable<Triangle> TrianglesWithSharedEdge {
            get {
                var neighbors = new HashSet<Triangle>();
                foreach (var vertex in Vertices)
                {
                    var trianglesWithSharedEdge = vertex.AdjacentTriangles.Where(o =>
                    {
                        return o != this && SharesEdgeWith(o);
                    });
                    neighbors.UnionWith(trianglesWithSharedEdge);
                }

                return neighbors;
            }
        }

        public Triangle(Point point1, Point point2, Point point3)
        {
            Vertices = new Point[3] { point1, point2, point3 };
            CorrectVertexOrder();
            StoreTriangleInVertices();
            UpdateCircumcircle();

            BorderVertices = Vertices.Where(v => v.IsBorderPoint).ToArray();
        }

        private void StoreTriangleInVertices()
        {
            Vertices[0].AdjacentTriangles.Add(this);
            Vertices[1].AdjacentTriangles.Add(this);
            Vertices[2].AdjacentTriangles.Add(this);
        }

        private void CorrectVertexOrder()
        {
            if (!AreVerticesCounterClockwise())
            {
                var temp = Vertices[1];
                Vertices[1] = Vertices[2];
                Vertices[2] = temp;
            }
        }

        private bool AreVerticesCounterClockwise()
        {
            var result = (Vertices[1].X - Vertices[0].X) * (Vertices[2].Y - Vertices[0].Y) -
                (Vertices[2].X - Vertices[0].X) * (Vertices[1].Y - Vertices[0].Y);
            return result > 0;
        }

        private void UpdateCircumcircle()
        {
            // https://codefound.wordpress.com/2013/02/21/how-to-compute-a-circumcircle/#more-58
            // https://en.wikipedia.org/wiki/Circumscribed_circle
            var p0 = Vertices[0];
            var p1 = Vertices[1];
            var p2 = Vertices[2];
            var dA = p0.X * p0.X + p0.Y * p0.Y;
            var dB = p1.X * p1.X + p1.Y * p1.Y;
            var dC = p2.X * p2.X + p2.Y * p2.Y;

            var aux1 = dA * (p2.Y - p1.Y) + dB * (p0.Y - p2.Y) + dC * (p1.Y - p0.Y);
            var aux2 = -(dA * (p2.X - p1.X) + dB * (p0.X - p2.X) + dC * (p1.X - p0.X));
            var div = 2 * (p0.X * (p2.Y - p1.Y) + p1.X * (p0.Y - p2.Y) + p2.X * (p1.Y - p0.Y));

            if (div == 0)
            {
                Debug.WriteLine($"({p0.X}, {p0.Y}), ({p1.X}, {p1.Y}), ({p2.X}, {p2.Y})");
                throw new System.Exception();
            }

            var center = new Point(aux1 / div, aux2 / div);
            Circumcenter = center;
            RadiusSquared = (center.X - p0.X) * (center.X - p0.X) + (center.Y - p0.Y) * (center.Y - p0.Y);
        }

        public bool SharesEdgeWith(Triangle triangle)
        {
            var sharedVertices = Vertices.Where(o => triangle.Vertices.Contains(o)).Count();
            return sharedVertices == 2;
        }

        public bool IsPointInsideCircumcircle(Point point)
        {
            if (BorderVertices.Length == 1)
            {
                var otherPoints = Vertices.Where(v => !BorderVertices.Contains(v));
                var p0 = otherPoints.ElementAt(0);
                var p1 = otherPoints.ElementAt(0);

                return IsPointLeftOfLine(p0, p1, point);
            }
            else if (BorderVertices.Length == 2)
            {
                var p0 = BorderVertices[0];
                var p1 = BorderVertices[1];
                var p2 = Vertices.Where(v => !BorderVertices.Contains(v)).First();

                return IsPointLeftOfLine(p2, p1 + (p2 - p0), point);
            }

            var d_squared = (point.X - Circumcenter.X) * (point.X - Circumcenter.X) +
                (point.Y - Circumcenter.Y) * (point.Y - Circumcenter.Y);
            return d_squared < RadiusSquared;
        }

        private bool IsPointLeftOfLine(Point a, Point b, Point point)
        {
            // https://math.stackexchange.com/questions/274712/calculate-on-which-side-of-a-straight-line-is-a-given-point-located
            var d = (point.X - a.X) * (b.Y - a.Y) - (point.Y - a.Y) * (b.X - a.X);
            return d >= 0;
        }
    }
}