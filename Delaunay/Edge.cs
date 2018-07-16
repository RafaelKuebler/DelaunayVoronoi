using System.Collections.Generic;
using System.Windows;

namespace Delaunay
{
    public class Edge
    {
        public Point Point1 { get; set; }
        public Point Point2 { get; set; }

        public Edge(Point point1, Point point2)
        {
            Point1 = point1;
            Point2 = point2;
        }

        public override string ToString()
        {
            return $"Edge: ({Point1}, {Point2})";
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != GetType()) return false;
            var edge = obj as Edge;

            var samePoints = Point1 == edge.Point1 && Point2 == edge.Point2;
            var samePointsReversed = Point1 == edge.Point2 && Point2 == edge.Point1;
            return samePoints || samePointsReversed;
        }

        public override int GetHashCode()
        {
            int hCode = (int)Point1.X ^ (int)Point1.Y ^ (int)Point2.X ^ (int)Point2.Y;
            return hCode.GetHashCode();
        }
    }

    /*public class EdgeEqualityComparer : IEqualityComparer<Edge>
    {
        public bool Equals(Edge x, Edge y)
        {
            if (x == null || y == null) return false;

            var samePoints = x.Point1 == y.Point1 && x.Point2 == y.Point2;
            var samePointsReversed = x.Point1 == y.Point2 && x.Point2 == y.Point1;
            return samePoints || samePointsReversed;
        }

        public int GetHashCode(Edge obj)
        {
            int hCode = (int)obj.Point1.X ^ (int)obj.Point1.Y ^ (int)obj.Point2.X ^ (int)obj.Point2.Y;
            return hCode.GetHashCode();
        }
    }*/
}