using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using WorldEvents.API.Models;

namespace WorldEvents.API.Data
{
    public partial class WorldEventsContext : DbContext
    {
        public WorldEventsContext()
        {
        }

        public WorldEventsContext(DbContextOptions<WorldEventsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblCategory> TblCategory { get; set; }
        public virtual DbSet<TblContinent> TblContinent { get; set; }
        public virtual DbSet<TblCountry> TblCountry { get; set; }
        public virtual DbSet<TblEvent> TblEvent { get; set; }
        public virtual DbSet<VwEvents> VwEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblCategory>(entity =>
            {
                entity.HasKey(e => e.CategoryId)
                    .HasName("PK__tblCateg__19093A2B7E64B663");

                entity.ToTable("tblCategory");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CategoryName).HasMaxLength(255);
            });

            modelBuilder.Entity<TblContinent>(entity =>
            {
                entity.HasKey(e => e.ContinentId)
                    .HasName("PK__tblConti__7E522081338707CE");

                entity.ToTable("tblContinent");

                entity.Property(e => e.ContinentId).HasColumnName("ContinentID");

                entity.Property(e => e.ContinentName).HasMaxLength(255);

                entity.Property(e => e.FilePath)
                    .HasColumnName("filePath")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Summary)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblCountry>(entity =>
            {
                entity.HasKey(e => e.CountryId)
                    .HasName("PK__tblCount__10D160BF0C7134C8");

                entity.ToTable("tblCountry");

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.ContinentId).HasColumnName("ContinentID");

                entity.Property(e => e.CountryName).HasMaxLength(255);

                entity.HasOne(d => d.Continent)
                    .WithMany(p => p.TblCountry)
                    .HasForeignKey(d => d.ContinentId)
                    .HasConstraintName("FK_tblCountry_tblContinent");
            });

            modelBuilder.Entity<TblEvent>(entity =>
            {
                entity.HasKey(e => e.EventId)
                    .HasName("PK__tblEvent__7944C870A78AF5F9");

                entity.ToTable("tblEvent");

                entity.Property(e => e.EventId).HasColumnName("EventID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.EventDate).HasColumnType("date");

                entity.Property(e => e.EventDetails).IsUnicode(false);

                entity.Property(e => e.EventName)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.TblEvent)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_tblEvent_tblCategory1");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.TblEvent)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK_tblEvent_tblCountry");
            });

            modelBuilder.Entity<VwEvents>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwEvents");

                entity.Property(e => e.Category).HasMaxLength(255);

                entity.Property(e => e.Continent).HasMaxLength(255);

                entity.Property(e => e.Country).HasMaxLength(255);

                entity.Property(e => e.EventDate).HasColumnType("date");

                entity.Property(e => e.EventDetails).IsUnicode(false);

                entity.Property(e => e.EventName)
                    .HasMaxLength(8000)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
