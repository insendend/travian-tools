using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TravianTools.Core.Extensions
{
    public static class PointExtensions
    {
        public static IEnumerable<Point> GetNearestPoints(this Point self, int radius)
        {
            if (radius < 0)
            {
                throw new ArgumentException("negative radius value", nameof(radius));
            }
            
            var minX = self.X - radius;
            var maxX = self.X + radius;
            var minY = self.Y - radius;
            var maxY = self.Y + radius;

            for (var x = minX; x <= maxX; x++)
            {
                for (var y = minY; y <= maxY; y++)
                {
                    yield return new Point(x, y);
                }
            }
        }
        
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.Shuffle(new Random());
        }
        
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            
            if (rng is null)
            {
                throw new ArgumentNullException(nameof(rng));
            }
            
            return source.ShuffleIterator(rng);
        }
        
        private static IEnumerable<T> ShuffleIterator<T>(this IEnumerable<T> source, Random rng)
        {
            var buffer = source.ToList();
            for (var i = 0; i < buffer.Count; i++)
            {
                var j = rng.Next(i, buffer.Count);
                yield return buffer[j];
                buffer[j] = buffer[i];
            }
        }
        
        public static double GetDistance(this Point self, Point to)
        {
            var deltaX = Delta(to.X, self.X);
            var deltaY = Delta(to.Y, self.Y);
            var distance = Math.Sqrt(deltaX + deltaY);
            var rDistance = Math.Round(distance, 1);

            return rDistance;
        }
        
        private static double Delta(int c1, int c2, int size = 400)
        {
            return Math.Pow((c1 - c2 + (3 * size + 1)) % (2 * size + 1) - size, 2);
        }
    }
}