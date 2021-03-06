// <auto-generated />
using System;
using System.Collections.Generic;
using HappyTravel.MultiLanguage;
using HappyTravel.Wakayama.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HappyTravel.Wakayama.Data.Migrations
{
    [DbContext(typeof(WakayamaContext))]
    [Migration("20220125145551_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "postgis");
            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "uuid-ossp");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("HappyTravel.Wakayama.Data.Models.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CountryCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<int>("GlobalRegionId")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("Modified")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<MultiLanguage<string>>("Name")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.HasKey("Id");

                    b.HasIndex("CountryCode");

                    b.HasIndex("GlobalRegionId");

                    b.ToTable("Countries", (string)null);
                });

            modelBuilder.Entity("HappyTravel.Wakayama.Data.Models.GlobalRegion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<DateTimeOffset>("Modified")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<MultiLanguage<string>>("Name")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.HasKey("Id");

                    b.ToTable("GlobalRegions", (string)null);
                });

            modelBuilder.Entity("HappyTravel.Wakayama.Data.Models.Locality", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<DateTimeOffset>("Modified")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<MultiLanguage<string>>("Name")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.HasKey("Id");

                    b.ToTable("Localities", (string)null);
                });

            modelBuilder.Entity("HappyTravel.Wakayama.Data.Models.LocalityCountryRelation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CountryId")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<int>("LocalityId")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("Modified")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<List<string>>("RelatedLanguageCodes")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.HasIndex("LocalityId");

                    b.ToTable("LocalityCountryRelations", (string)null);
                });

            modelBuilder.Entity("HappyTravel.Wakayama.Data.Models.LocalityProvinceRelation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<int>("LocalityId")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("Modified")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<int>("ProvinceId")
                        .HasColumnType("integer");

                    b.Property<List<string>>("RelatedLanguageCodes")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.HasKey("Id");

                    b.HasIndex("LocalityId");

                    b.HasIndex("ProvinceId");

                    b.ToTable("LocalityProvinceRelation", (string)null);
                });

            modelBuilder.Entity("HappyTravel.Wakayama.Data.Models.LocalityZone", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<int>("LocalityId")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("Modified")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<MultiLanguage<string>>("Name")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.HasKey("Id");

                    b.HasIndex("LocalityId");

                    b.ToTable("LocalityZones", (string)null);
                });

            modelBuilder.Entity("HappyTravel.Wakayama.Data.Models.Province", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<DateTimeOffset>("Modified")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<MultiLanguage<string>>("Name")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.HasKey("Id");

                    b.ToTable("Provinces", (string)null);
                });

            modelBuilder.Entity("HappyTravel.Wakayama.Data.Models.ProvinceCountryRelation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CountryId")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<DateTimeOffset>("Modified")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<int>("ProvinceId")
                        .HasColumnType("integer");

                    b.Property<List<string>>("RelatedLanguageCodes")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.HasIndex("ProvinceId");

                    b.ToTable("ProvinceCountryRelations", (string)null);
                });

            modelBuilder.Entity("HappyTravel.Wakayama.Data.Models.Synonym", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<DateTimeOffset>("Modified")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<MultiLanguage<string>>("Name")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<int>("SourceId")
                        .HasColumnType("integer");

                    b.Property<int>("SynonymType")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("SynonymType", "SourceId");

                    b.ToTable("Synonyms");
                });

            modelBuilder.Entity("HappyTravel.Wakayama.Data.Models.Country", b =>
                {
                    b.HasOne("HappyTravel.Wakayama.Data.Models.GlobalRegion", "GlobalRegion")
                        .WithMany("Countries")
                        .HasForeignKey("GlobalRegionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("GlobalRegion");
                });

            modelBuilder.Entity("HappyTravel.Wakayama.Data.Models.LocalityCountryRelation", b =>
                {
                    b.HasOne("HappyTravel.Wakayama.Data.Models.Country", "Country")
                        .WithMany("LocalityAndCountryRelations")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("HappyTravel.Wakayama.Data.Models.Locality", "Locality")
                        .WithMany("LocalityAndCountryRelations")
                        .HasForeignKey("LocalityId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Country");

                    b.Navigation("Locality");
                });

            modelBuilder.Entity("HappyTravel.Wakayama.Data.Models.LocalityProvinceRelation", b =>
                {
                    b.HasOne("HappyTravel.Wakayama.Data.Models.Locality", "Locality")
                        .WithMany("LocalityAndProvinceRelations")
                        .HasForeignKey("LocalityId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("HappyTravel.Wakayama.Data.Models.Province", "Province")
                        .WithMany("LocalityAndProvinceRelations")
                        .HasForeignKey("ProvinceId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Locality");

                    b.Navigation("Province");
                });

            modelBuilder.Entity("HappyTravel.Wakayama.Data.Models.LocalityZone", b =>
                {
                    b.HasOne("HappyTravel.Wakayama.Data.Models.Locality", "Locality")
                        .WithMany("LocalityZones")
                        .HasForeignKey("LocalityId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Locality");
                });

            modelBuilder.Entity("HappyTravel.Wakayama.Data.Models.ProvinceCountryRelation", b =>
                {
                    b.HasOne("HappyTravel.Wakayama.Data.Models.Country", "Country")
                        .WithMany("ProvinceAndCountryRelations")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("HappyTravel.Wakayama.Data.Models.Province", "Province")
                        .WithMany("ProvinceAndCountryRelations")
                        .HasForeignKey("ProvinceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");

                    b.Navigation("Province");
                });

            modelBuilder.Entity("HappyTravel.Wakayama.Data.Models.Country", b =>
                {
                    b.Navigation("LocalityAndCountryRelations");

                    b.Navigation("ProvinceAndCountryRelations");
                });

            modelBuilder.Entity("HappyTravel.Wakayama.Data.Models.GlobalRegion", b =>
                {
                    b.Navigation("Countries");
                });

            modelBuilder.Entity("HappyTravel.Wakayama.Data.Models.Locality", b =>
                {
                    b.Navigation("LocalityAndCountryRelations");

                    b.Navigation("LocalityAndProvinceRelations");

                    b.Navigation("LocalityZones");
                });

            modelBuilder.Entity("HappyTravel.Wakayama.Data.Models.Province", b =>
                {
                    b.Navigation("LocalityAndProvinceRelations");

                    b.Navigation("ProvinceAndCountryRelations");
                });
#pragma warning restore 612, 618
        }
    }
}
