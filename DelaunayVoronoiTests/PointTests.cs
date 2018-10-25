using DelaunayVoronoi;
using NUnit.Framework;

namespace DelaunayVoronoiTests
{
    [TestFixture]
    public class PointTests
    {
        [Test]
        public void PointTest()
        {
            var point = new Point(1, 2);
            Assert.That(point, Is.Not.Null);
            Assert.That(point.X, Is.EqualTo(1));
            Assert.That(point.Y, Is.EqualTo(2));
        }

        [Test]
        public void PointTest_AdjacentEmpty()
        {
            var point = new Point(1, 2);
            Assert.That(point.AdjacentTriangles, Is.Not.Null);
            Assert.That(point.AdjacentTriangles, Is.Empty);
        }

        [Test]
        public void PointTest_AdjacentAdd()
        {
            var point = new Point(0, 0);
            var point2 = new Point(0, 1);
            var point3 = new Point(1, 1);
            var triangle = new Triangle(point, point2, point3);

            point.AdjacentTriangles.Add(triangle);

            Assert.That(point.AdjacentTriangles, Contains.Item(triangle));
            Assert.That(point.AdjacentTriangles, Has.Count.EqualTo(1));
        }
    }
}