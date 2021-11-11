using CourseProject.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace CourseProject.Data
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<MarkedCell> MarkedCells { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<MarkedCell>()
                .Property(e => e.CellOwner)
                .HasConversion<string>();

            modelBuilder
                .Entity<MarkedCell>()
                .Property(e => e.CellType)
                .HasConversion<string>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
