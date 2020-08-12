using System.Drawing;
using TravianTools.Core.Extensions;
using Xunit;

namespace TravianTools.Tests.Core
{
    public class PointExtensionsTest
    {
        [Theory]
        [InlineData(44, 12, 44, 15, 3)]
        [InlineData(44, 12, 43, 13, 1.4)]
        [InlineData(44, 12, 46, 14, 2.8)]
        [InlineData(44, 12, 4, 32, 44.7)]
        [InlineData(44, 12, -56, 15, 100)]
        [InlineData(44, 12, -55, 2, 99.5)]
        [InlineData(44, 12, 71, -89, 104.5)]
        [InlineData(44, 12, -42, -5, 87.7)]
        public void CorrectDistance(int fromX, int fromY, int toX, int toY, double expectedDistance)
        {
            var pointFrom = new Point(fromX, fromY);
            var pointTo = new Point(toX, toY);

            var actualDistance = pointFrom.GetDistance(pointTo);
            
            Assert.Equal(expectedDistance, actualDistance);
        }
    }
}