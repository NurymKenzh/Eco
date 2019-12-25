using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eco.Models
{
    public class TargetTerritory
    {
        public int Id { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "TerritoryType")]
        public TerritoryType TerritoryType { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "TerritoryType")]
        public int TerritoryTypeId { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "CityDistrict")]
        public CityDistrict CityDistrict { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "CityDistrict")]
        public int? CityDistrictId { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "TerritoryNameKK")]
        public string TerritoryNameKK { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "TerritoryNameRU")]
        public string TerritoryNameRU { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "TerritoryName")]
        public string TerritoryName
        {
            get
            {
                string language = new RequestLocalizationOptions().DefaultRequestCulture.Culture.Name,
                    TerritoryName = TerritoryNameRU;
                if (language == "kk")
                {
                    TerritoryName = TerritoryNameKK;
                }
                if (language == "ru")
                {
                    TerritoryName = TerritoryNameRU;
                }
                return TerritoryName;
            }
        }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "GISConnectionCode")]
        public string GISConnectionCode { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AdditionalInformationKK")]
        public string AdditionalInformationKK { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AdditionalInformationRU")]
        public string AdditionalInformationRU { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AdditionalInformation")]
        public string AdditionalInformation
        {
            get
            {
                string language = new RequestLocalizationOptions().DefaultRequestCulture.Culture.Name,
                    AdditionalInformation = AdditionalInformationRU;
                if (language == "kk")
                {
                    AdditionalInformation = AdditionalInformationKK;
                }
                if (language == "ru")
                {
                    AdditionalInformation = AdditionalInformationRU;
                }
                return AdditionalInformation;
            }
        }
        
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "KazHydrometAirPost")]
        public KazHydrometAirPost KazHydrometAirPost { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "KazHydrometAirPost")]
        public int? KazHydrometAirPostId { get; set; }
        
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AirPost")]
        public AirPost AirPost { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AirPost")]
        public int? AirPostId { get; set; }
        
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "TransportPost")]
        public TransportPost TransportPost { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "TransportPost")]
        public int? TransportPostId { get; set; }
        
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "WaterSurfacePost")]
        public WaterSurfacePost WaterSurfacePost { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "WaterSurfacePost")]
        public int? WaterSurfacePostId { get; set; }
        
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "KazHydrometWaterPost")]
        public KazHydrometWaterPost KazHydrometWaterPost { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "KazHydrometWaterPost")]
        public int? KazHydrometWaterPostId { get; set; }
        
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "KazHydrometSoilPost")]
        public KazHydrometSoilPost KazHydrometSoilPost { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "KazHydrometSoilPost")]
        public int? KazHydrometSoilPostId { get; set; }
        
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "SoilPost")]
        public SoilPost SoilPost { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "SoilPost")]
        public int? SoilPostId { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Post")]
        public string Post
        {
            get
            {
                string Post = "";
                if (KazHydrometAirPost!=null)
                {
                    Post = KazHydrometAirPost.Number.ToString();
                }
                if (AirPost != null)
                {
                    Post = AirPost.Name;
                }
                if (TransportPost != null)
                {
                    Post = TransportPost.Name;
                }
                if (WaterSurfacePost != null)
                {
                    Post = WaterSurfacePost.WaterObjectName;
                }
                if (KazHydrometWaterPost != null)
                {
                    Post = KazHydrometWaterPost.Name;
                }
                if (KazHydrometSoilPost != null)
                {
                    Post = KazHydrometSoilPost.Name;
                }
                if (SoilPost != null)
                {
                    Post = SoilPost.Name;
                }
                return Post;
            }
        }

        public override string ToString()
        {
            return $"Id: {Id.ToString()}\r\n" +
                $"TerritoryTypeId: {TerritoryTypeId.ToString()}\r\n" +
                $"CityDistrictId: {CityDistrictId.ToString()}\r\n" +
                $"TerritoryNameKK: {TerritoryNameKK}\r\n" +
                $"TerritoryNameRU: {TerritoryNameRU}\r\n" +
                $"GISConnectionCode: {GISConnectionCode}\r\n" +
                $"KazHydrometAirPostId: {KazHydrometAirPostId.ToString()}\r\n" +
                $"AirPostId: {AirPostId.ToString()}\r\n" +
                $"TransportPostId: {TransportPostId.ToString()}\r\n" +
                $"WaterSurfacePostId: {WaterSurfacePostId.ToString()}\r\n" +
                $"KazHydrometWaterPostId: {KazHydrometWaterPostId.ToString()}\r\n" +
                $"KazHydrometSoilPostId: {KazHydrometSoilPostId.ToString()}\r\n" +
                $"SoilPostId: {SoilPostId.ToString()}\r\n" +
                $"AdditionalInformationKK: \"{AdditionalInformationKK}\"\r\n" +
                $"AdditionalInformationRU: \"{AdditionalInformationRU}\"";
        }
    }

    public class TargetTerritoryIndexPageViewModel
    {
        public IEnumerable<TargetTerritory> Items { get; set; }
        public Pager Pager { get; set; }
    }
}
