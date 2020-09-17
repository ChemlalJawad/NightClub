using Microsoft.EntityFrameworkCore;
using NightClub.Core.Domaines;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace NightClub.Data.Database
{
    public class NightclubContext : DbContext
    {
        public DbSet<IDCarte> IDCartes { get; set; }
        public DbSet<Membre> Membres { get; set; }
        public DbSet<MembreCarte> MembreCartes { get; set; }
        public NightclubContext(DbContextOptions<NightclubContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MembreCarte>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<MembreCarte>()
               .HasOne(x => x.Membre)
               .WithMany(x => x.MembreCartes);

            modelBuilder.Entity<Membre>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Membre>()
                .HasMany(e => e.MembreCartes)
                .WithOne(e => e.Membre);

            modelBuilder.Entity<Membre>()
                .HasMany(x => x.CarteIdentites)
                .WithOne();

            modelBuilder.Entity<IDCarte>()
                .HasKey(x => x.Id);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.Seed();

        }
    }
}
