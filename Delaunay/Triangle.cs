using System.Windows;

namespace Delaunay
{
    public class Triangle
    {
        public Edge[] Edges { get; private set; } = new Edge[3];

        public Point[] Vertices {
            get {
                return new Point[3] { Edges[0].Point1, Edges[1].Point1, Edges[2].Point1 };
            }
        }

        public Triangle(Point point1, Point point2, Point point3)
        {
            if (!IsCounterClockwise(point1, point2, point3))
            {
                Edges[0] = new Edge(point1, point3);
                Edges[1] = new Edge(point3, point2);
                Edges[2] = new Edge(point2, point1);
            }
            else
            {
                Edges[0] = new Edge(point1, point2);
                Edges[1] = new Edge(point2, point3);
                Edges[2] = new Edge(point3, point1);
            }
        }

        public Triangle(Point point, Edge edge) : this(point, edge.Point1, edge.Point2)
        {
        }

        public override string ToString()
        {
            var vertices = Vertices;
            return $"Triangle: ({vertices[0]}, {vertices[1]}, {vertices[2]})";
        }

        private bool IsCounterClockwise(Point point1, Point point2, Point point3)
        {
            var result = (point2.X - point1.X) * (point3.Y - point1.Y) -
                (point3.X - point1.X) * (point2.Y - point1.Y);
            return result > 0;
        }
    }
}