using System.Collections.Generic;

namespace DelaunayVoronoi
{
    public class Point
    {
        public double X { get; }
        public double Y { get; }
        public HashSet<Triangle> AdjacentTriangles { get; } = new HashSet<Triangle>();

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}