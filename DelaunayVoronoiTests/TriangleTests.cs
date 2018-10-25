using DelaunayVoronoi;
using NUnit.Framework;

namespace DelaunayVoronoiTests
{
    [TestFixture]
    public class TriangleTests
    {
        private readonly Point point1 = new Point(0d, 0d);
        private readonly Point point2 = new Point(0d, 1d);
        private readonly Point point3 = new Point(1d, 1d);
        private Triangle triangle;

        [SetUp]
        public void SetUp()
        {
            triangle = new Triangle(point1, point2, point3);
        }

        [Test]
        public void TriangleTest()
        {
            Assert.That(triangle, Is.Not.Null);
        }

        [Test]
        public void TriangleTest_Vertices()
        {
            Assert.That(triangle.Vertices, Contains.Item(point1));
            Assert.That(triangle.Vertices, Contains.Item(point2));
            Assert.That(triangle.Vertices, Contains.Item(point3));
        }

        [Test]
        public void TriangleTest_VerticesAdjacentToTriangle()
        {
            foreach (var vertex in triangle.Vertices)
            {
                Assert.That(vertex.AdjacentTriangles, Contains.Item(triangle));
            }
        }

        // TODO: More tests, until here only the constructor was tested and if the vertices are corrrect
        // => correct circumcircle calculated (radius and center)?
        // => shares edges with (positive/negative)
        // => IsPointInsideCircumcircle (positive/negative)
    }
}