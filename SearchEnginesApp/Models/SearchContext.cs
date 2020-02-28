using Microsoft.EntityFrameworkCore;

namespace SearchEnginesApp.Models
{
    public class SearchContext : DbContext
    {
        public SearchContext(DbContextOptions<SearchContext> options)
            : base(options)
        {            
            //Database.EnsureCreated();
        }

        public DbSet<FoundItem> FoundItems { get; set; }
        public DbSet<SearchResult> SearchResults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FoundItem>(entity =>
            {
                entity.Property(e => e.Title)
                    .HasMaxLength(1024)
                    .IsRequired();
                entity.Property(e => e.Url)
                    .HasMaxLength(1024)
                    .IsRequired();
                entity.Property(e => e.Snippet)
                    .HasMaxLength(1024)
                    .IsRequired();
            });

            modelBuilder.Entity<SearchResult>(entity =>
            {
                entity.Property(e => e.EngineName)
                    .HasMaxLength(30)
                    .IsRequired();
                entity.Property(e => e.Query)
                    .HasMaxLength(1024)
                    .IsRequired();
            });
        }
    }
}
