﻿using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TravianTools.DAL
{
    public sealed class TravianToolsContext : DbContext, ITravianToolsContext
    {
        public TravianToolsContext(DbContextOptions<TravianToolsContext> options) : base(options)
        {

        }

        public DbSet<NeighborsVillageInfo> NeighborsVillageInfos { get; set; }

        public DbSet<StateConfig> Configs { get; set; }
        
        public async Task SaveChangesAsync()
        {
            await base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
