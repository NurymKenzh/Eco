using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Eco.Models;

namespace Eco.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            //builder.Entity<Blog>()
            //    .HasIndex(b => b.Url)
            //    .IsUnique(); //не обязательно
        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        public DbSet<Eco.Models.TerritoryType> TerritoryType { get; set; }

        public DbSet<Eco.Models.CityDistrict> CityDistrict { get; set; }

        public DbSet<Eco.Models.TargetTerritory> TargetTerritory { get; set; }

        public DbSet<Eco.Models.TypeOfTarget> TypeOfTarget { get; set; }

        public DbSet<Eco.Models.Target> Target { get; set; }

        public DbSet<Eco.Models.Event> Event { get; set; }

        public DbSet<Eco.Models.HazardClass> HazardClass { get; set; }

        public DbSet<Eco.Models.Company> Company { get; set; }

        public DbSet<Eco.Models.SubsidiaryCompany> SubsidiaryCompany { get; set; }

        public DbSet<Eco.Models.IndustrialSite> IndustrialSite { get; set; }

        public DbSet<Eco.Models.AirContaminant> AirContaminant { get; set; }

        public DbSet<Eco.Models.SummationAirContaminantsGroup> SummationAirContaminantsGroup { get; set; }

        public DbSet<Eco.Models.TypeOfAirPollutionIndicator> TypeOfAirPollutionIndicator { get; set; }

        public DbSet<Eco.Models.AirPollutionIndicator> AirPollutionIndicator { get; set; }

        public DbSet<Eco.Models.SubstanceHazardClass> SubstanceHazardClass { get; set; }

        public DbSet<Eco.Models.LimitingIndicator> LimitingIndicator { get; set; }

        public DbSet<Eco.Models.WaterContaminant> WaterContaminant { get; set; }

        public DbSet<Eco.Models.LimitingSoilIndicator> LimitingSoilIndicator { get; set; }

        public DbSet<Eco.Models.SoilContaminant> SoilContaminant { get; set; }

        public DbSet<Eco.Models.Log> Log { get; set; }

        public DbSet<Eco.Models.MeasurementUnit> MeasurementUnit { get; set; }

        public DbSet<Eco.Models.KazHydrometAirPost> KazHydrometAirPost { get; set; }

        public DbSet<Eco.Models.AirPost> AirPost { get; set; }

        public DbSet<Eco.Models.MobileAirPost> MobileAirPost { get; set; }

        public DbSet<Eco.Models.WaterPost> WaterPost { get; set; }

        public DbSet<Eco.Models.SoilPost> SoilPost { get; set; }

        public DbSet<Eco.Models.TargetValue> TargetValue { get; set; }

        public DbSet<Eco.Models.AActivity> AActivity { get; set; }

        public DbSet<Eco.Models.SamplingTerm> SamplingTerm { get; set; }

        public DbSet<Eco.Models.MovementDirection> MovementDirection { get; set; }

        public DbSet<Eco.Models.TransportPost> TransportPost { get; set; }

        public DbSet<Eco.Models.KazHydrometWaterPost> KazHydrometWaterPost { get; set; }

        public DbSet<Eco.Models.WaterObject> WaterObject { get; set; }

        public DbSet<Eco.Models.WaterSurfacePost> WaterSurfacePost { get; set; }

        public DbSet<Eco.Models.KazHydrometSoilPost> KazHydrometSoilPost { get; set; }

        public DbSet<Eco.Models.KazHydrometAirPostData> KazHydrometAirPostData { get; set; }

        public DbSet<Eco.Models.WindDirection> WindDirection { get; set; }

        public DbSet<Eco.Models.GeneralWeatherCondition> GeneralWeatherCondition { get; set; }

        public DbSet<Eco.Models.AirPostData> AirPostData { get; set; }

        public DbSet<Eco.Models.TransportPostData> TransportPostData { get; set; }

        public DbSet<Eco.Models.WaterSurfacePostData> WaterSurfacePostData { get; set; }

        public DbSet<Eco.Models.NormativeSoilType> NormativeSoilType { get; set; }

        public DbSet<Eco.Models.IssuingPermitsStateAuthority> IssuingPermitsStateAuthority { get; set; }

        public DbSet<Eco.Models.AnnualMaximumPermissibleEmissionsVolume> AnnualMaximumPermissibleEmissionsVolume { get; set; }

        public DbSet<Eco.Models.EmissionSourceType> EmissionSourceType { get; set; }

        public DbSet<Eco.Models.EmissionSource> EmissionSource { get; set; }

        public DbSet<Eco.Models.CompanyEmissionsValue> CompanyEmissionsValue { get; set; }

        public DbSet<Eco.Models.KazHydrometWaterPostData> KazHydrometWaterPostData { get; set; }

        public DbSet<Eco.Models.KazHydrometSoilPostData> KazHydrometSoilPostData { get; set; }

        public DbSet<Eco.Models.SoilPostData> SoilPostData { get; set; }

        public DbSet<Eco.Models.AuthorizedAuthority> AuthorizedAuthority { get; set; }

        public DbSet<Eco.Models.SpeciallyProtectedNaturalTerritory> SpeciallyProtectedNaturalTerritory { get; set; }

        public DbSet<Eco.Models.GreenPlantationsAreaAndSpeciesDiversity> GreenPlantationsAreaAndSpeciesDiversity { get; set; }

        public DbSet<Eco.Models.GreenPlantationsType> GreenPlantationsType { get; set; }

        public DbSet<Eco.Models.GreenPlantationsState> GreenPlantationsState { get; set; }

        public DbSet<Eco.Models.WasteType> WasteType { get; set; }

        public DbSet<Eco.Models.RecyclableWasteType> RecyclableWasteType { get; set; }

        public DbSet<Eco.Models.RawMaterialsCompany> RawMaterialsCompany { get; set; }

        public DbSet<Eco.Models.WasteRecyclingCompany> WasteRecyclingCompany { get; set; }

        public DbSet<Eco.Models.PlantationsType> PlantationsType { get; set; }

        public DbSet<Eco.Models.PlantationsStateType> PlantationsStateType { get; set; }

        public DbSet<Eco.Models.PlantationsState> PlantationsState { get; set; }

        public DbSet<Eco.Models.SpeciesDiversity> SpeciesDiversity { get; set; }

        public DbSet<Eco.Models.GreemPlantsPassport> GreemPlantsPassport { get; set; }

        public DbSet<Eco.Models.TreesByObjectTableOfTheBreedStateList> TreesByObjectTableOfTheBreedStateList { get; set; }

        public DbSet<Eco.Models.TreesByFacilityManagementMeasuresList> TreesByFacilityManagementMeasuresList { get; set; }

        public DbSet<Eco.Models.EcomonData> EcomonData { get; set; }
    }
}
