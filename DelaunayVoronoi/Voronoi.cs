using System.Collections.Generic;

namespace DelaunayVoronoi
{
    public class Voronoi
    {
        public HashSet<Edge> GenerateEdgesFromDelaunay(in HashSet<Triangle> triangulation)
        {
            var voronoiEdges = new HashSet<Edge>();
            foreach (var triangle in triangulation)
            {
                foreach (var neighbor in triangle.TrianglesWithSharedEdge)
                {
                    var edge = new Edge(triangle.Circumcenter, neighbor.Circumcenter);
                    voronoiEdges.Add(edge);
                }
            }

            return voronoiEdges;
        }
    }
}