using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TravianTools.DAL
{
    public interface ITravianToolsContext
    {
        DbSet<ProtectionExpireVillages> ProtectionExpireVillages { get; set; }

        DbSet<StateConfig> Configs { get; set; }

        Task SaveChangesAsync();
    }
}