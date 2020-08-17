using System;

namespace TravianTools.StatCollectWorker
{
    public class StatWorkerSettings
    {
        public int OriginX { get; set; }
        public int OriginY { get; set; }
        public int Radius { get; set; }
        public TimeSpan CheckDelay { get; set; }

        public bool IsEnabled { get; set; }
    }
}