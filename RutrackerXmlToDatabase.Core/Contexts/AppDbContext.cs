using Microsoft.EntityFrameworkCore;
using RutrackerXmlToDatabase.Core.Contexts.Configurations;
using RutrackerXmlToDatabase.Core.Models;

namespace RutrackerXmlToDatabase.Core.Contexts
{
    public sealed class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Forum> Forums { get; set; }
        public DbSet<Torrent> Torrents { get; set; }
        public DbSet<File> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new ForumConfiguration());
            builder.ApplyConfiguration(new TorrentConfiguration());
            builder.ApplyConfiguration(new FileConfiguration());
        }
    }
}