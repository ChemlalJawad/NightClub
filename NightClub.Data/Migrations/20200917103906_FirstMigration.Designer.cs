﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NightClub.Data.Database;

namespace NightClub.Data.Migrations
{
    [DbContext(typeof(NightclubContext))]
    [Migration("20200917103906_FirstMigration")]
    partial class FirstMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NightClub.Core.Domaines.IDCarte", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateExpiration")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateNaissance")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateValidation")
                        .HasColumnType("datetime2");

                    b.Property<int?>("MembreId")
                        .HasColumnType("int");

                    b.Property<string>("Nom")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumeroCarte")
                        .HasColumnType("int");

                    b.Property<string>("Prenom")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RegistreNational")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MembreId");

                    b.ToTable("IDCartes");
                });

            modelBuilder.Entity("NightClub.Core.Domaines.Membre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DebutDateBlacklister")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FinDateBlacklister")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsBlacklister")
                        .HasColumnType("bit");

                    b.Property<string>("Telephone")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Membres");
                });

            modelBuilder.Entity("NightClub.Core.Domaines.MembreCarte", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int?>("MembreId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MembreId");

                    b.ToTable("MembreCartes");
                });

            modelBuilder.Entity("NightClub.Core.Domaines.IDCarte", b =>
                {
                    b.HasOne("NightClub.Core.Domaines.Membre", null)
                        .WithMany("CarteIdentites")
                        .HasForeignKey("MembreId");
                });

            modelBuilder.Entity("NightClub.Core.Domaines.MembreCarte", b =>
                {
                    b.HasOne("NightClub.Core.Domaines.Membre", "Membre")
                        .WithMany("MembreCartes")
                        .HasForeignKey("MembreId");
                });
#pragma warning restore 612, 618
        }
    }
}