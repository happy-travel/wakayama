using HappyTravel.Wakayama.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HappyTravel.Wakayama.Data;

public class WakayamaContext : DbContext
{
    public WakayamaContext(DbContextOptions<WakayamaContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("postgis")
            .HasPostgresExtension("uuid-ossp");
        

        modelBuilder.Entity<GlobalRegion>(b =>
        {
            b.HasKey(r => r.Id);
            b.Property(r => r.Name)
                .HasColumnType("jsonb")
                .IsRequired();
            b.Property(r => r.Created)
                .IsRequired()
                .HasDefaultValueSql("now() at time zone 'utc'");
            b.Property(r => r.Modified)
                .IsRequired()
                .HasDefaultValueSql("now() at time zone 'utc'");
            b.ToTable("GlobalRegions");
        });
        
        
        modelBuilder.Entity<Country>(b =>
        {
            b.HasKey(c => c.Id);
            b.Property(c => c.Name)
                .HasColumnType("jsonb")
                .IsRequired();
            b.Property(c => c.CountryCode)
                .IsRequired();
            b.Property(c => c.Created)
                .IsRequired()
                .HasDefaultValueSql("now() at time zone 'utc'");
            b.Property(c => c.Modified)
                .IsRequired()
                .HasDefaultValueSql("now() at time zone 'utc'");
            b.HasIndex(c => c.CountryCode);
            b.HasOne(c => c.GlobalRegion)
                .WithMany(r => r.Countries)
                .HasForeignKey(c => c.GlobalRegionId)
                .OnDelete(DeleteBehavior.Restrict);
            b.ToTable("Countries");
        });
        
        modelBuilder.Entity<Province>(b =>
        {
            b.HasKey(p => p.Id);
            b.Property(p => p.Name)
                .HasColumnType("jsonb")
                .IsRequired();
            b.Property(p => p.Created)
                .IsRequired()
                .HasDefaultValueSql("now() at time zone 'utc'");
            b.Property(p => p.Modified)
                .IsRequired()
                .HasDefaultValueSql("now() at time zone 'utc'");
            b.ToTable("Provinces");
        });
        
        modelBuilder.Entity<Locality>(b =>
        {
            b.HasKey(l => l.Id);
            b.Property(l => l.Name)
                .HasColumnType("jsonb")
                .IsRequired();
            b.Property(l => l.Created)
                .IsRequired()
                .HasDefaultValueSql("now() at time zone 'utc'");
            b.Property(l => l.Modified)
                .IsRequired()
                .HasDefaultValueSql("now() at time zone 'utc'");
            b.ToTable("Localities");
        });
        
        modelBuilder.Entity<LocalityZone>(b =>
        {
            b.HasKey(z => z.Id);
            b.Property(z => z.Name)
                .HasColumnType("jsonb")
                .IsRequired();
            b.Property(z => z.Created)
                .IsRequired()
                .HasDefaultValueSql("now() at time zone 'utc'");
            b.Property(z => z.Modified)
                .IsRequired()
                .HasDefaultValueSql("now() at time zone 'utc'");
            b.HasOne(z => z.Locality)
                .WithMany(l => l.LocalityZones)
                .HasForeignKey(l => l.LocalityId)
                .OnDelete(DeleteBehavior.Restrict);
            b.ToTable("LocalityZones");
        });
        
        modelBuilder.Entity<ProvinceCountryRelation>(b =>
        {
            b.HasKey(p => p.Id);
            b.Property(p => p.RelatedLanguageCodes);
            b.Property(p => p.Created)
                .IsRequired()
                .HasDefaultValueSql("now() at time zone 'utc'");
            b.Property(p => p.Modified)
                .IsRequired()
                .HasDefaultValueSql("now() at time zone 'utc'");
            b.HasOne(p => p.Province)
                .WithMany(p => p.ProvinceAndCountryRelations)
                .HasForeignKey(p => p.ProvinceId);
            
            b.HasOne(p => p.Country)
                .WithMany(c => c.ProvinceAndCountryRelations)
                .HasForeignKey(p => p.CountryId)
                .OnDelete(DeleteBehavior.Restrict);
            b.ToTable("ProvinceCountryRelations");
        });
        
        modelBuilder.Entity<LocalityCountryRelation>(b =>
        {
            b.HasKey(p => p.Id);
            b.Property(p => p.RelatedLanguageCodes);
            b.Property(p => p.Created)
                .IsRequired()
                .HasDefaultValueSql("now() at time zone 'utc'");
            b.Property(p => p.Modified)
                .IsRequired()
                .HasDefaultValueSql("now() at time zone 'utc'");
            b.HasOne(l => l.Locality)
                .WithMany(l => l.LocalityAndCountryRelations)
                .HasForeignKey(l => l.LocalityId)
                .OnDelete(DeleteBehavior.Restrict);;
            b.HasOne(p => p.Country)
                .WithMany(c => c.LocalityAndCountryRelations)
                .HasForeignKey(p => p.CountryId)
                .OnDelete(DeleteBehavior.Restrict);
            b.ToTable("LocalityCountryRelations");
        });
        
        modelBuilder.Entity<LocalityProvinceRelation>(b =>
        {
            b.HasKey(r => r.Id);
            b.Property(r => r.RelatedLanguageCodes);
            b.Property(r => r.Created)
                .IsRequired()
                .HasDefaultValueSql("now() at time zone 'utc'");
            b.Property(r => r.Modified)
                .IsRequired()
                .HasDefaultValueSql("now() at time zone 'utc'");
            b.HasOne(r => r.Locality)
                .WithMany(l => l.LocalityAndProvinceRelations)
                .HasForeignKey(r => r.LocalityId)
                .OnDelete(DeleteBehavior.Restrict);;
            b.HasOne(r => r.Province)
                .WithMany(p => p.LocalityAndProvinceRelations)
                .HasForeignKey(r => r.ProvinceId)
                .OnDelete(DeleteBehavior.Restrict);
            b.ToTable("LocalityProvinceRelation");
        });

        modelBuilder.Entity<Synonym>(b =>
        {
            b.HasKey(s => s.Id);
            b.Property(s => s.Name)
                .HasColumnType("jsonb")
                .IsRequired();
            b.Property(s => s.SynonymType)
                .IsRequired();
            b.Property(s => s.SourceId)
                .IsRequired();
            b.Property(s => s.Created)
                .IsRequired()
                .HasDefaultValueSql("now() at time zone 'utc'");
            b.Property(s => s.Modified)
                .IsRequired()
                .HasDefaultValueSql("now() at time zone 'utc'");
            b.HasIndex(s => new {s.SynonymType, s.SourceId});
        });
    }
   
    
    public DbSet<GlobalRegion> Regions => Set<GlobalRegion>(); 
    public DbSet<Country> Countries => Set<Country>();
    public DbSet<Province> Provinces => Set<Province>();
    public DbSet<Locality> Localities => Set<Locality>();
    public DbSet<Synonym> Synonyms => Set<Synonym>();
    public DbSet<ProvinceCountryRelation> ProvinceAndCountryRelations => Set<ProvinceCountryRelation>();
    public DbSet<LocalityCountryRelation> LocalityAndCountryRelations => Set<LocalityCountryRelation>();
    public DbSet<LocalityProvinceRelation> LocalityProvinceRelations => Set<LocalityProvinceRelation>();
}