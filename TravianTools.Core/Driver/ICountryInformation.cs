using System.Drawing;
using TravianTools.DAL;

namespace TravianTools.Core.Driver
{
    public interface ICountryInformation
    {
        bool TryParseVillage(Point villagePoint, out NeighborsVillageInfo neighborVillage);

        ITravianDriver Driver { get; }
    }
}