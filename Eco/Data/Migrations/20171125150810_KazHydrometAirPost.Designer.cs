﻿// <auto-generated />
using Eco.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;

namespace Eco.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20171125150810_KazHydrometAirPost")]
    partial class KazHydrometAirPost
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

            modelBuilder.Entity("Eco.Models.AirContaminant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal?>("ApproximateSafeExposureLevel");

                    b.Property<string>("ContaminantCodeERA")
                        .HasMaxLength(4);

                    b.Property<int?>("LimitingIndicatorId");

                    b.Property<decimal?>("MaximumPermissibleConcentrationDailyAverage");

                    b.Property<decimal?>("MaximumPermissibleConcentrationOneTimemaximum");

                    b.Property<string>("Name");

                    b.Property<int?>("Number104");

                    b.Property<int?>("Number168");

                    b.Property<string>("NumberCAS")
                        .HasMaxLength(10);

                    b.Property<bool>("PresenceOfTheMaximumPermissibleConcentration");

                    b.Property<int>("SubstanceHazardClassId");

                    b.Property<string[]>("Synonyms");

                    b.HasKey("Id");

                    b.HasIndex("LimitingIndicatorId");

                    b.HasIndex("SubstanceHazardClassId");

                    b.ToTable("AirContaminant");
                });

            modelBuilder.Entity("Eco.Models.AirPollutionIndicator", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<int>("TypeOfAirPollutionIndicatorId");

                    b.HasKey("Id");

                    b.HasIndex("TypeOfAirPollutionIndicatorId");

                    b.ToTable("AirPollutionIndicator");
                });

            modelBuilder.Entity("Eco.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Eco.Models.CityDistrict", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CATO");

                    b.Property<string>("NameKK");

                    b.Property<string>("NameRU");

                    b.HasKey("Id");

                    b.ToTable("CityDistrict");
                });

            modelBuilder.Entity("Eco.Models.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AbbreviatedName");

                    b.Property<string>("ActualAddressHouse");

                    b.Property<string>("ActualAddressStreet");

                    b.Property<string>("AdditionalInformation");

                    b.Property<string>("BIK");

                    b.Property<int>("CityDistrictId");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("FullName");

                    b.Property<int>("HazardClassId");

                    b.Property<bool>("HierarchicalStructure");

                    b.Property<string>("KindOfActivity");

                    b.Property<string>("LegalAddressHouse");

                    b.Property<string>("LegalAddressStreet");

                    b.HasKey("Id");

                    b.HasIndex("CityDistrictId");

                    b.HasIndex("HazardClassId");

                    b.ToTable("Company");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Company");
                });

            modelBuilder.Entity("Eco.Models.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("NameKK");

                    b.Property<string>("NameRU");

                    b.HasKey("Id");

                    b.ToTable("Event");
                });

            modelBuilder.Entity("Eco.Models.HazardClass", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("NameKK");

                    b.Property<string>("NameRU");

                    b.HasKey("Id");

                    b.ToTable("HazardClass");
                });

            modelBuilder.Entity("Eco.Models.IndustrialSite", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AbbreviatedName");

                    b.Property<int>("CityDistrictId");

                    b.Property<int>("CompanyId");

                    b.Property<decimal>("EastLongitude");

                    b.Property<string>("FullName");

                    b.Property<int>("HazardClassId");

                    b.Property<string>("House");

                    b.Property<decimal>("NorthLatitude");

                    b.Property<string>("Street");

                    b.Property<int?>("SubsidiaryCompanyId");

                    b.HasKey("Id");

                    b.HasIndex("CityDistrictId");

                    b.HasIndex("CompanyId");

                    b.HasIndex("HazardClassId");

                    b.HasIndex("SubsidiaryCompanyId");

                    b.ToTable("IndustrialSite");
                });

            modelBuilder.Entity("Eco.Models.KazHydrometAirPost", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AdditionalInformationKK");

                    b.Property<string>("AdditionalInformationRU");

                    b.Property<decimal>("EastLongitude");

                    b.Property<string>("NameKK");

                    b.Property<string>("NameRU");

                    b.Property<decimal>("NorthLatitude");

                    b.Property<int>("Number");

                    b.HasKey("Id");

                    b.ToTable("KazHydrometAirPost");
                });

            modelBuilder.Entity("Eco.Models.LimitingIndicator", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("NameKK");

                    b.Property<string>("NameRU");

                    b.HasKey("Id");

                    b.ToTable("LimitingIndicator");
                });

            modelBuilder.Entity("Eco.Models.LimitingSoilIndicator", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("NameKK");

                    b.Property<string>("NameRU");

                    b.HasKey("Id");

                    b.ToTable("LimitingSoilIndicator");
                });

            modelBuilder.Entity("Eco.Models.Log", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Class");

                    b.Property<DateTime>("DateTime");

                    b.Property<string>("Email");

                    b.Property<string>("New");

                    b.Property<string>("Old");

                    b.Property<string>("Operation");

                    b.HasKey("Id");

                    b.ToTable("Log");
                });

            modelBuilder.Entity("Eco.Models.MeasurementUnit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("NameKK");

                    b.Property<string>("NameRU");

                    b.HasKey("Id");

                    b.ToTable("MeasurementUnit");
                });

            modelBuilder.Entity("Eco.Models.SoilContaminant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("LimitingSoilIndicatorId");

                    b.Property<decimal?>("MaximumPermissibleConcentrationSoil");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("LimitingSoilIndicatorId");

                    b.ToTable("SoilContaminant");
                });

            modelBuilder.Entity("Eco.Models.SubstanceHazardClass", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("NameKK");

                    b.Property<string>("NameRU");

                    b.HasKey("Id");

                    b.ToTable("SubstanceHazardClass");
                });

            modelBuilder.Entity("Eco.Models.SummationAirContaminantsGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int[]>("AirContaminants");

                    b.Property<decimal>("CoefficientOfPotentiation");

                    b.Property<int>("Number25012012");

                    b.Property<string>("SummationGroupCodeERA")
                        .HasMaxLength(2);

                    b.HasKey("Id");

                    b.ToTable("SummationAirContaminantsGroup");
                });

            modelBuilder.Entity("Eco.Models.Target", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("MeasurementUnitId");

                    b.Property<string>("NameKK");

                    b.Property<string>("NameRU");

                    b.Property<bool>("TypeOfAchievement");

                    b.Property<int>("TypeOfTargetId");

                    b.HasKey("Id");

                    b.HasIndex("MeasurementUnitId");

                    b.HasIndex("TypeOfTargetId");

                    b.ToTable("Target");
                });

            modelBuilder.Entity("Eco.Models.TargetTerritory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AdditionalInformationKK");

                    b.Property<string>("AdditionalInformationRU");

                    b.Property<int?>("CityDistrictId");

                    b.Property<string>("GISConnectionCode");

                    b.Property<string>("TerritoryNameKK");

                    b.Property<string>("TerritoryNameRU");

                    b.Property<int>("TerritoryTypeId");

                    b.HasKey("Id");

                    b.HasIndex("CityDistrictId");

                    b.HasIndex("TerritoryTypeId");

                    b.ToTable("TargetTerritory");
                });

            modelBuilder.Entity("Eco.Models.TerritoryType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AdditionalInformationKK");

                    b.Property<string>("AdditionalInformationRU");

                    b.Property<string>("NameKK");

                    b.Property<string>("NameRU");

                    b.HasKey("Id");

                    b.ToTable("TerritoryType");
                });

            modelBuilder.Entity("Eco.Models.TypeOfAirPollutionIndicator", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("NameKK");

                    b.Property<string>("NameRU");

                    b.HasKey("Id");

                    b.ToTable("TypeOfAirPollutionIndicator");
                });

            modelBuilder.Entity("Eco.Models.TypeOfTarget", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("NameKK");

                    b.Property<string>("NameRU");

                    b.HasKey("Id");

                    b.ToTable("TypeOfTarget");
                });

            modelBuilder.Entity("Eco.Models.WaterContaminant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("LimitingIndicatorId");

                    b.Property<decimal?>("MaximumPermissibleConcentrationWater");

                    b.Property<string>("Name");

                    b.Property<string>("NumberCAS")
                        .HasMaxLength(10);

                    b.Property<int>("SubstanceHazardClassId");

                    b.HasKey("Id");

                    b.HasIndex("LimitingIndicatorId");

                    b.HasIndex("SubstanceHazardClassId");

                    b.ToTable("WaterContaminant");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Eco.Models.SubsidiaryCompany", b =>
                {
                    b.HasBaseType("Eco.Models.Company");

                    b.Property<int>("CompanyId");

                    b.HasIndex("CompanyId");

                    b.ToTable("SubsidiaryCompany");

                    b.HasDiscriminator().HasValue("SubsidiaryCompany");
                });

            modelBuilder.Entity("Eco.Models.AirContaminant", b =>
                {
                    b.HasOne("Eco.Models.LimitingIndicator", "LimitingIndicator")
                        .WithMany()
                        .HasForeignKey("LimitingIndicatorId");

                    b.HasOne("Eco.Models.SubstanceHazardClass", "SubstanceHazardClass")
                        .WithMany()
                        .HasForeignKey("SubstanceHazardClassId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Eco.Models.AirPollutionIndicator", b =>
                {
                    b.HasOne("Eco.Models.TypeOfAirPollutionIndicator", "TypeOfAirPollutionIndicator")
                        .WithMany()
                        .HasForeignKey("TypeOfAirPollutionIndicatorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Eco.Models.Company", b =>
                {
                    b.HasOne("Eco.Models.CityDistrict", "CityDistrict")
                        .WithMany()
                        .HasForeignKey("CityDistrictId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Eco.Models.HazardClass", "HazardClass")
                        .WithMany()
                        .HasForeignKey("HazardClassId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Eco.Models.IndustrialSite", b =>
                {
                    b.HasOne("Eco.Models.CityDistrict", "CityDistrict")
                        .WithMany()
                        .HasForeignKey("CityDistrictId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Eco.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Eco.Models.HazardClass", "HazardClass")
                        .WithMany()
                        .HasForeignKey("HazardClassId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Eco.Models.SubsidiaryCompany", "SubsidiaryCompany")
                        .WithMany()
                        .HasForeignKey("SubsidiaryCompanyId");
                });

            modelBuilder.Entity("Eco.Models.SoilContaminant", b =>
                {
                    b.HasOne("Eco.Models.LimitingSoilIndicator", "LimitingSoilIndicator")
                        .WithMany()
                        .HasForeignKey("LimitingSoilIndicatorId");
                });

            modelBuilder.Entity("Eco.Models.Target", b =>
                {
                    b.HasOne("Eco.Models.MeasurementUnit", "MeasurementUnit")
                        .WithMany()
                        .HasForeignKey("MeasurementUnitId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Eco.Models.TypeOfTarget", "TypeOfTarget")
                        .WithMany()
                        .HasForeignKey("TypeOfTargetId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Eco.Models.TargetTerritory", b =>
                {
                    b.HasOne("Eco.Models.CityDistrict", "CityDistrict")
                        .WithMany()
                        .HasForeignKey("CityDistrictId");

                    b.HasOne("Eco.Models.TerritoryType", "TerritoryType")
                        .WithMany()
                        .HasForeignKey("TerritoryTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Eco.Models.WaterContaminant", b =>
                {
                    b.HasOne("Eco.Models.LimitingIndicator", "LimitingIndicator")
                        .WithMany()
                        .HasForeignKey("LimitingIndicatorId");

                    b.HasOne("Eco.Models.SubstanceHazardClass", "SubstanceHazardClass")
                        .WithMany()
                        .HasForeignKey("SubstanceHazardClassId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Eco.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Eco.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Eco.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Eco.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Eco.Models.SubsidiaryCompany", b =>
                {
                    b.HasOne("Eco.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
