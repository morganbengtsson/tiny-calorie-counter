using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore;
using Npgsql;
using System.Security.Cryptography.X509Certificates;
using Diet.Models;

namespace Diet.Contexts
{
    public class MainContext : IdentityDbContext<ApplicationUser>
    {
        public MainContext(DbContextOptions options)
            : base(options)
        {
        }
        public DbSet<Food> Foods { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<MassUnit> MassUnits { get; set; }
        public DbSet<VolumeUnit> VolumeUnits { get; set; }
        public DbSet<Meassure> Meassures { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<WeightEntry> WeightEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            /*modelBuilder.Entity<Title>()
            .HasIndex(p => p.Name).IsUnique();*/

            modelBuilder.Entity<Food>()
            .HasAlternateKey(p => p.Code);

            modelBuilder.Entity<MassUnit>().HasBaseType<Unit>();
            modelBuilder.Entity<VolumeUnit>().HasBaseType<Unit>();
            modelBuilder.Entity<Meassure>().HasBaseType<Unit>();


            modelBuilder.Entity<Entry>()
                .HasOne(p => p.User)
                .WithMany(b => b.Entries)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WeightEntry>()
                .HasOne(p => p.User)
                .WithMany(b => b.WeightEntries)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}