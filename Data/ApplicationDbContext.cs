using cotuongdemotest1.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace cotuongdemotest1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        public DbSet<GameDb> Games { get; set; }
        public DbSet<UserGameDb> UserGames { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameDb>()
                .Property(g => g.Board)
                .HasColumnType("jsonb");
            modelBuilder.Entity<GameDb>()
                .Property(g => g.MoveHistory)
                .HasColumnType("jsonb");
        }
    }
}
