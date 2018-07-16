using System.Collections.Generic;
using System.Linq;

namespace Delaunay
{
    public class Polygon
    {
        public List<Edge> Edges { get; private set; }

        public Polygon(IEnumerable<Edge> edges)
        {
            Edges = edges.ToList();
        }
    }
}