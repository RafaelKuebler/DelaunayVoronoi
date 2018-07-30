using System.Collections.Generic;
using System.Linq;

namespace Delaunay
{
    public class Voronoi
    {
        public IEnumerable<Edge> GenerateEdgesFromDelaunay(IEnumerable<Triangle> triangulation)
        {
            var voronoiEdges = new HashSet<Edge>();
            foreach (var triangle in triangulation)
            {
                if (!triangle.Circumcenter.HasValue)
                {
                    return null;
                }
                var neighbors = NaiveNeigborSearch(triangle, triangulation);
                var neighborCircumcenters = neighbors.Select(o => o.Circumcenter);
                foreach (var circumcenter in neighborCircumcenters)
                {
                    if (!circumcenter.HasValue)
                    {
                        continue;
                    }
                    var edge = new Edge(triangle.Circumcenter.Value, circumcenter.Value);
                    voronoiEdges.Add(edge);
                }
            }

            return voronoiEdges;
        }

        public IEnumerable<Triangle> NaiveNeigborSearch(Triangle triangle, IEnumerable<Triangle> triangulation)
        {
            var neighbors = triangulation.Where(t =>
            {
                var hasCommonEdge = t.Edges.Any(v => triangle.Edges.Contains(v));
                return hasCommonEdge && t != triangle;
            });

            return neighbors;
        }
    }
}