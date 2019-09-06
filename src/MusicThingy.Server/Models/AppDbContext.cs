using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MusicThingy.Server.Models
{
    public class AppDbContext : DbContext
    {
        // private readonly string databasepath;

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            // databasepath = Path.Combine(config.GetSection("config").Get<Configuration>().DataPath, "database.sqlite");
        }

        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     optionsBuilder.UseSqlite($"Data Source={databasepath}",
        //         provideroptions => provideroptions.CommandTimeout(10));
        // }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<YouTubeSourceMedia>();

            modelBuilder.Entity<SourceMedia>()
                .HasKey(x => new { x.SourceId, x.MediaId });
        }

        public DbSet<Source> Sources { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<SyncTarget> SyncTargets { get; set; }
    }

    public class SyncTarget
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
    }
}
