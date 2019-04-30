using Microsoft.EntityFrameworkCore;

namespace MusicThingy.Models
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=appdata.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SourceVideo>()
                .HasKey(x => new { x.SourceId, x.VideoId });
        }

        public DbSet<Source> Sources { get; set; }
        public DbSet<Video> Videos { get; set; }
    }
}
