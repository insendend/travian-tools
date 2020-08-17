using System;
using System.Drawing;

namespace TravianTools.DAL
{
    public class NeighborsVillageInfo: BaseEntity
    {
        public int PointX { get; set; }

        public int PointY { get; set; }

        public DateTime? UntilProtectionTime { get; set; }

        public bool IsVillage { get; set; }
    }
}