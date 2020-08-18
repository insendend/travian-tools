using System.Drawing;
using TravianTools.DAL;

namespace TravianTools.Core.Driver
{
    public interface ICountryInformation
    {
        bool TryParseVillage(string hostUrl, Point villagePoint, out ProtectionExpireVillages neighborVillage);

        ITravianDriver Driver { get; }
    }
}