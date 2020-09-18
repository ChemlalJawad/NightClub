using Microsoft.EntityFrameworkCore;
using NightClub.Core.Domaines;
using System;
using System.Collections.Generic;
using System.Text;

namespace NightClub.Data.Database
{
    public static class DataLoader
    {
        public static void Seed(this ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<Membre>().HasData(new 
            {
                Id = 1,
                Email = "jawadchemlal@hotmail.com",
                IsBlacklister = false,
                DebutDateBlacklister = new DateTime(),
                FinDateBlacklister = new DateTime()
            });

            modelBuilder.Entity<IDCarte>().HasData(new
            {
                Id = 1,
                Nom = "Chemlal",
                Prenom = "Jawad",
                DateNaissance = new DateTime(1995, 7, 29),
                RegistreNational = "96.07.17-146-23",
                DateValidation = new DateTime(2017,1,1),
                DateExpiration = new DateTime(2030,1,1),
                NumeroCarte = 1000001,
                MembreId = 1
            });

            modelBuilder.Entity<MembreCarte>().HasData(new
            {
                Id = 1,
                Code = "25352525",
                MembreId = 1,
                IsActive = true
            });

        }
    }
}
