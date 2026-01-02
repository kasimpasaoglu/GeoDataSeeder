using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GeoDataSeeder.Persistence.Scaffold;

public partial class GeoDbContext : DbContext
{
    public GeoDbContext()
    {
    }

    public GeoDbContext(DbContextOptions<GeoDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<District> Districts { get; set; }

    public virtual DbSet<Neighborhood> Neighborhoods { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=db37013.public.databaseasp.net;Database=db37013;User Id=db37013;Password=7o%HN#6d4j@D;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Turkish_CI_AS");

        modelBuilder.Entity<City>(entity =>
        {
            entity.ToTable("Cities", "geo");

            entity.HasIndex(e => e.CountryId, "IX_Cities_CountryId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code).HasMaxLength(2);

            entity.HasOne(d => d.Country).WithMany(p => p.Cities)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.ToTable("Countries", "geo");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code).HasMaxLength(4);
        });

        modelBuilder.Entity<District>(entity =>
        {
            entity.ToTable("Districts", "geo");

            entity.HasIndex(e => e.CityId, "IX_Districts_CityId");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.City).WithMany(p => p.Districts)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Neighborhood>(entity =>
        {
            entity.ToTable("Neighborhoods", "geo");

            entity.HasIndex(e => e.DistrictId, "IX_Neighborhoods_DistrictId");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.District).WithMany(p => p.Neighborhoods)
                .HasForeignKey(d => d.DistrictId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
