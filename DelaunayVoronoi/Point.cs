using System.Collections.Generic;

namespace DelaunayVoronoi
{
    public class Point
    {
        public double X { get; }
        public double Y { get; }
        public HashSet<Triangle> AdjacentTriangles { get; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
            AdjacentTriangles = new HashSet<Triangle>();
        }
    }
}