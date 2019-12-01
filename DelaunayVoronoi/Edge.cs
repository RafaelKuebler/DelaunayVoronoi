using System;
using System.Collections.Generic;

namespace DelaunayVoronoi
{
    public readonly struct Edge : IEquatable<Edge>
    {
        public Point Point1 { get; }
        public Point Point2 { get; }

        public Edge(Point point1, Point point2)
        {
            Point1 = point1;
            Point2 = point2;
        }

        public override int GetHashCode()
        {
            int hCode = (int)Point1.X ^ (int)Point1.Y ^ (int)Point2.X ^ (int)Point2.Y;
            return hCode.GetHashCode();
        }

        public bool Equals(Edge other)
        {
            var samePoints         = Point1 == other.Point1 && Point2 == other.Point2;
            var samePointsReversed = Point1 == other.Point2 && Point2 == other.Point1;
            return samePoints || samePointsReversed;
        }
    }
}