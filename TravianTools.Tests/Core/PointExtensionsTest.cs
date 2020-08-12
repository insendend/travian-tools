using System;
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
        
        [Theory]
        [InlineData(44, 12, 44, 15, 3)]
        [InlineData(44, 12, 43, 13, 1.4)]
        [InlineData(44, 12, 46, 14, 2.8)]
        [InlineData(44, 12, 4, 32, 44.7)]
        [InlineData(44, 12, -56, 15, 100)]
        [InlineData(44, 12, -55, 2, 99.5)]
        [InlineData(44, 12, 71, -89, 104.5)]
        [InlineData(44, 12, -42, -5, 87.7)]
        public void CorrectDistancePythagoras(int fromX, int fromY, int toX, int toY, double expectedDistance)
        {
            var x = Math.Abs(fromX - toX);
            var y = Math.Abs(fromY - toY);

            var actualDistance = Math.Round(Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)), 1);
            
            Assert.Equal(expectedDistance, actualDistance);
        }
    }
}