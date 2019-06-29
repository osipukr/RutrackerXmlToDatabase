using Microsoft.EntityFrameworkCore;
using RutrackerXmlToDatabase.Core.Contexts.Configurations;
using RutrackerXmlToDatabase.Core.Models;

namespace RutrackerXmlToDatabase.Core.Contexts
{
    public class TorrentDbContext : DbContext
    {
        public TorrentDbContext(DbContextOptions<TorrentDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Forum> Forums { get; set; }
        public virtual DbSet<Torrent> Torrents { get; set; }
        public virtual DbSet<File> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new ForumConfiguration());
            builder.ApplyConfiguration(new TorrentConfiguration());
            builder.ApplyConfiguration(new FileConfiguration());
        }
    }
}