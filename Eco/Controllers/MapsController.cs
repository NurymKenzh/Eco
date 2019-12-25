using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Eco.Data;
using Eco.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace Eco.Controllers
{
    public class MapsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MapsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(1);
        }

        public IActionResult Show()
        {
            string decimaldelimiter = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            List<AirPost> airPosts = _context.AirPost.ToList();
            JObject airPostsObject = JObject.FromObject(new
            {
                type = "FeatureCollection",
                crs = new
                {
                    type = "name",
                    properties = new
                    {
                        name = "urn:ogc:def:crs:EPSG::3857"
                    }
                },
                features = from airPost in airPosts
                    select new
                    {
                        type = "Feature",
                        properties = new
                        {
                            Id = airPost.Id,
                            Name = airPost.Name,
                            Number = airPost.Number
                        },
                        geometry = new
                        {
                            type = "Point",
                            coordinates = new List<decimal>
                            {
                            Convert.ToDecimal(airPost.EastLongitude.ToString().Replace(".", decimaldelimiter)),
                            Convert.ToDecimal(airPost.NorthLatitude.ToString().Replace(".", decimaldelimiter))
                            },
                        }
                    }
            });
            ViewBag.AirPostsLayerJson = airPostsObject.ToString();

            List<KazHydrometAirPost> kazHydrometAirPosts = _context.KazHydrometAirPost.ToList();
            JObject kazHydrometAirPostsObject = JObject.FromObject(new
            {
                type = "FeatureCollection",
                crs = new
                {
                    type = "name",
                    properties = new
                    {
                        name = "urn:ogc:def:crs:EPSG::3857"
                    }
                },
                features = from kazHydrometAirPost in kazHydrometAirPosts
                    select new
                    {
                        type = "Feature",
                        properties = new
                        {
                            Id = kazHydrometAirPost.Id,
                            Name = kazHydrometAirPost.Name,
                            Number = kazHydrometAirPost.Number
                        },
                        geometry = new
                        {
                            type = "Point",
                            coordinates = new List<decimal>
                            {
                            Convert.ToDecimal(kazHydrometAirPost.EastLongitude.ToString().Replace(".", decimaldelimiter)),
                            Convert.ToDecimal(kazHydrometAirPost.NorthLatitude.ToString().Replace(".", decimaldelimiter))
                            },
                        }
                    }
            });
            ViewBag.KazHydrometAirPostsLayerJson = kazHydrometAirPostsObject.ToString();

            List<EmissionSource> emissionSources = _context.EmissionSource
                .Include(e => e.Company)
                .Include(e => e.SubsidiaryCompany)
                .ToList();
            JObject emissionSourcesObject = JObject.FromObject(new
            {
                type = "FeatureCollection",
                crs = new
                {
                    type = "name",
                    properties = new
                    {
                        name = "urn:ogc:def:crs:EPSG::3857"
                    }
                },
                features = from emissionSource in emissionSources
                    select new
                    {
                        type = "Feature",
                        properties = new
                        {
                            Id = emissionSource.Id,
                            CompanyOrSubsidiaryCompanyAbbreviatedName = emissionSource.CompanyOrSubsidiaryCompanyAbbreviatedName,
                            EmissionSourceName = emissionSource.EmissionSourceName
                        },
                        geometry = new
                        {
                            type = "Point",
                            coordinates = new List<decimal>
                            {
                            Convert.ToDecimal(emissionSource.EastLongitude1.ToString().Replace(".", decimaldelimiter)),
                            Convert.ToDecimal(emissionSource.NorthLatitude1.ToString().Replace(".", decimaldelimiter))
                            },
                        }
                    }
            });
            ViewBag.EmissionSourcesLayerJson = emissionSourcesObject.ToString();

            List<TransportPost> transportPosts = _context.TransportPost.ToList();
            JObject transportPostsObject = JObject.FromObject(new
            {
                type = "FeatureCollection",
                crs = new
                {
                    type = "name",
                    properties = new
                    {
                        name = "urn:ogc:def:crs:EPSG::3857"
                    }
                },
                features = from transportPost in transportPosts
                    select new
                    {
                        type = "Feature",
                        properties = new
                        {
                            Id = transportPost.Id,
                            Name = transportPost.Name
                        },
                        geometry = new
                        {
                            type = "Point",
                            coordinates = new List<decimal>
                            {
                            Convert.ToDecimal(transportPost.EastLongitude.ToString().Replace(".", decimaldelimiter)),
                            Convert.ToDecimal(transportPost.NorthLatitude.ToString().Replace(".", decimaldelimiter))
                            },
                        }
                    }
            });
            ViewBag.TransportPostsLayerJson = transportPostsObject.ToString();

            List<WaterSurfacePost> waterSurfacePosts = _context.WaterSurfacePost
                .Include(w => w.WaterObject)
                .ToList();
            JObject waterSurfacePostsObject = JObject.FromObject(new
            {
                type = "FeatureCollection",
                crs = new
                {
                    type = "name",
                    properties = new
                    {
                        name = "urn:ogc:def:crs:EPSG::3857"
                    }
                },
                features = from waterSurfacePost in waterSurfacePosts
                    select new
                    {
                        type = "Feature",
                        properties = new
                        {
                            Id = waterSurfacePost.Id,
                            WaterObjectName = waterSurfacePost.WaterObject.Name,
                            WaterObjectId = waterSurfacePost.WaterObjectId,
                            Number = waterSurfacePost.Number,
                        },
                        geometry = new
                        {
                            type = "Point",
                            coordinates = new List<decimal>
                            {
                            Convert.ToDecimal(waterSurfacePost.EastLongitude.ToString().Replace(".", decimaldelimiter)),
                            Convert.ToDecimal(waterSurfacePost.NorthLatitude.ToString().Replace(".", decimaldelimiter))
                            },
                        }
                    }
            });
            ViewBag.WaterSurfacePostsLayerJson = waterSurfacePostsObject.ToString();

            List<KazHydrometWaterPost> kazHydrometWaterPosts = _context.KazHydrometWaterPost.ToList();
            JObject kazHydrometWaterPostsObject = JObject.FromObject(new
            {
                type = "FeatureCollection",
                crs = new
                {
                    type = "name",
                    properties = new
                    {
                        name = "urn:ogc:def:crs:EPSG::3857"
                    }
                },
                features = from kazHydrometWaterPost in kazHydrometWaterPosts
                           select new
                           {
                               type = "Feature",
                               properties = new
                               {
                                   Id = kazHydrometWaterPost.Id,
                                   Name = kazHydrometWaterPost.Name
                               },
                               geometry = new
                               {
                                   type = "Point",
                                   coordinates = new List<decimal>
                            {
                            Convert.ToDecimal(kazHydrometWaterPost.EastLongitude.ToString().Replace(".", decimaldelimiter)),
                            Convert.ToDecimal(kazHydrometWaterPost.NorthLatitude.ToString().Replace(".", decimaldelimiter))
                            },
                               }
                           }
            });
            ViewBag.KazHydrometWaterPostsLayerJson = kazHydrometWaterPostsObject.ToString();

            List<SoilPost> soilPosts = _context.SoilPost.ToList();
            JObject soilPostsObject = JObject.FromObject(new
            {
                type = "FeatureCollection",
                crs = new
                {
                    type = "name",
                    properties = new
                    {
                        name = "urn:ogc:def:crs:EPSG::3857"
                    }
                },
                features = from soilPost in soilPosts
                           select new
                           {
                               type = "Feature",
                               properties = new
                               {
                                   Id = soilPost.Id,
                                   Name = soilPost.Name
                               },
                               geometry = new
                               {
                                   type = "Point",
                                   coordinates = new List<decimal>
                            {
                            Convert.ToDecimal(soilPost.EastLongitude.ToString().Replace(".", decimaldelimiter)),
                            Convert.ToDecimal(soilPost.NorthLatitude.ToString().Replace(".", decimaldelimiter))
                            },
                               }
                           }
            });
            ViewBag.SoilPostsLayerJson = soilPostsObject.ToString();

            List<KazHydrometSoilPost> kazHydrometSoilPosts = _context.KazHydrometSoilPost.ToList();
            JObject kazHydrometSoilPostsObject = JObject.FromObject(new
            {
                type = "FeatureCollection",
                crs = new
                {
                    type = "name",
                    properties = new
                    {
                        name = "urn:ogc:def:crs:EPSG::3857"
                    }
                },
                features = from kazHydrometSoilPost in kazHydrometSoilPosts
                           select new
                           {
                               type = "Feature",
                               properties = new
                               {
                                   Id = kazHydrometSoilPost.Id,
                                   Name = kazHydrometSoilPost.Name
                               },
                               geometry = new
                               {
                                   type = "Point",
                                   coordinates = new List<decimal>
                            {
                            Convert.ToDecimal(kazHydrometSoilPost.EastLongitude.ToString().Replace(".", decimaldelimiter)),
                            Convert.ToDecimal(kazHydrometSoilPost.NorthLatitude.ToString().Replace(".", decimaldelimiter))
                            },
                               }
                           }
            });
            ViewBag.KazHydrometSoilPostsLayerJson = kazHydrometSoilPostsObject.ToString();

            List<RawMaterialsCompany> rawMaterialsCompanies = _context.RawMaterialsCompany.ToList();
            JObject rawMaterialsCompaniesObject = JObject.FromObject(new
            {
                type = "FeatureCollection",
                crs = new
                {
                    type = "name",
                    properties = new
                    {
                        name = "urn:ogc:def:crs:EPSG::3857"
                    }
                },
                features = from rawMaterialsCompany in rawMaterialsCompanies
                           select new
                           {
                               type = "Feature",
                               properties = new
                               {
                                   Id = rawMaterialsCompany.Id,
                                   Name = rawMaterialsCompany.Name
                               },
                               geometry = new
                               {
                                   type = "Point",
                                   coordinates = new List<decimal>
                            {
                            Convert.ToDecimal(rawMaterialsCompany.EastLongitude.ToString().Replace(".", decimaldelimiter)),
                            Convert.ToDecimal(rawMaterialsCompany.NorthLatitude.ToString().Replace(".", decimaldelimiter))
                            },
                               }
                           }
            });
            ViewBag.RawMaterialsCompaniesLayerJson = rawMaterialsCompaniesObject.ToString();

            List<WasteRecyclingCompany> wasteRecyclingCompanies = _context.WasteRecyclingCompany.ToList();
            JObject wasteRecyclingCompaniesObject = JObject.FromObject(new
            {
                type = "FeatureCollection",
                crs = new
                {
                    type = "name",
                    properties = new
                    {
                        name = "urn:ogc:def:crs:EPSG::3857"
                    }
                },
                features = from wasteRecyclingCompany in wasteRecyclingCompanies
                           select new
                           {
                               type = "Feature",
                               properties = new
                               {
                                   Id = wasteRecyclingCompany.Id,
                                   Name = wasteRecyclingCompany.Name
                               },
                               geometry = new
                               {
                                   type = "Point",
                                   coordinates = new List<decimal>
                            {
                            Convert.ToDecimal(wasteRecyclingCompany.EastLongitude.ToString().Replace(".", decimaldelimiter)),
                            Convert.ToDecimal(wasteRecyclingCompany.NorthLatitude.ToString().Replace(".", decimaldelimiter))
                            },
                               }
                           }
            });
            ViewBag.WasteRecyclingCompaniesLayerJson = wasteRecyclingCompaniesObject.ToString();

            ViewBag.GeoServerPort = Startup.Configuration["GeoServerPort"];

            return View();
        }

        public IActionResult View(string Map)
        {
            ViewBag.Map = Map;

            string decimaldelimiter = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            List<AirPost> airPosts = _context.AirPost.ToList();
            JObject airPostsObject = JObject.FromObject(new
            {
                type = "FeatureCollection",
                crs = new
                {
                    type = "name",
                    properties = new
                    {
                        name = "urn:ogc:def:crs:EPSG::3857"
                    }
                },
                features = from airPost in airPosts
                           select new
                           {
                               type = "Feature",
                               properties = new
                               {
                                   Id = airPost.Id,
                                   Name = airPost.Name,
                                   Number = airPost.Number
                               },
                               geometry = new
                               {
                                   type = "Point",
                                   coordinates = new List<decimal>
                            {
                            Convert.ToDecimal(airPost.EastLongitude.ToString().Replace(".", decimaldelimiter)),
                            Convert.ToDecimal(airPost.NorthLatitude.ToString().Replace(".", decimaldelimiter))
                            },
                               }
                           }
            });
            ViewBag.AirPostsLayerJson = airPostsObject.ToString();

            List<AirPost> ecomons = new List<AirPost>();
            ecomons.Add(new AirPost()
            {
                Id = 0,
                Number = 11,
                NameKK = "11",
                NameRU = "11",
                EastLongitude = 76.89184M,
                NorthLatitude = 43.2507M
            });
            JObject ecomonsObject = JObject.FromObject(new
            {
                type = "FeatureCollection",
                crs = new
                {
                    type = "name",
                    properties = new
                    {
                        name = "urn:ogc:def:crs:EPSG::3857"
                    }
                },
                features = from airPost in ecomons
                           select new
                           {
                               type = "Feature",
                               properties = new
                               {
                                   Id = airPost.Id,
                                   Name = airPost.Name,
                                   Number = airPost.Number
                               },
                               geometry = new
                               {
                                   type = "Point",
                                   coordinates = new List<decimal>
                            {
                            Convert.ToDecimal(airPost.EastLongitude.ToString().Replace(".", decimaldelimiter)),
                            Convert.ToDecimal(airPost.NorthLatitude.ToString().Replace(".", decimaldelimiter))
                            },
                               }
                           }
            });
            ViewBag.EcomonsLayerJson = ecomonsObject.ToString();

            List<KazHydrometAirPost> kazHydrometAirPosts = _context.KazHydrometAirPost.ToList();
            JObject kazHydrometAirPostsObject = JObject.FromObject(new
            {
                type = "FeatureCollection",
                crs = new
                {
                    type = "name",
                    properties = new
                    {
                        name = "urn:ogc:def:crs:EPSG::3857"
                    }
                },
                features = from kazHydrometAirPost in kazHydrometAirPosts
                           select new
                           {
                               type = "Feature",
                               properties = new
                               {
                                   Id = kazHydrometAirPost.Id,
                                   Name = kazHydrometAirPost.Name,
                                   Number = kazHydrometAirPost.Number
                               },
                               geometry = new
                               {
                                   type = "Point",
                                   coordinates = new List<decimal>
                            {
                            Convert.ToDecimal(kazHydrometAirPost.EastLongitude.ToString().Replace(".", decimaldelimiter)),
                            Convert.ToDecimal(kazHydrometAirPost.NorthLatitude.ToString().Replace(".", decimaldelimiter))
                            },
                               }
                           }
            });
            ViewBag.KazHydrometAirPostsLayerJson = kazHydrometAirPostsObject.ToString();

            List<EmissionSource> emissionSources = _context.EmissionSource
                .Include(e => e.Company)
                .Include(e => e.SubsidiaryCompany)
                .ToList();
            JObject emissionSourcesObject = JObject.FromObject(new
            {
                type = "FeatureCollection",
                crs = new
                {
                    type = "name",
                    properties = new
                    {
                        name = "urn:ogc:def:crs:EPSG::3857"
                    }
                },
                features = from emissionSource in emissionSources
                           select new
                           {
                               type = "Feature",
                               properties = new
                               {
                                   Id = emissionSource.Id,
                                   CompanyOrSubsidiaryCompanyAbbreviatedName = emissionSource.CompanyOrSubsidiaryCompanyAbbreviatedName,
                                   EmissionSourceName = emissionSource.EmissionSourceName
                               },
                               geometry = new
                               {
                                   type = "Point",
                                   coordinates = new List<decimal>
                            {
                            Convert.ToDecimal(emissionSource.EastLongitude1.ToString().Replace(".", decimaldelimiter)),
                            Convert.ToDecimal(emissionSource.NorthLatitude1.ToString().Replace(".", decimaldelimiter))
                            },
                               }
                           }
            });
            ViewBag.EmissionSourcesLayerJson = emissionSourcesObject.ToString();

            List<TransportPost> transportPosts = _context.TransportPost.ToList();
            JObject transportPostsObject = JObject.FromObject(new
            {
                type = "FeatureCollection",
                crs = new
                {
                    type = "name",
                    properties = new
                    {
                        name = "urn:ogc:def:crs:EPSG::3857"
                    }
                },
                features = from transportPost in transportPosts
                           select new
                           {
                               type = "Feature",
                               properties = new
                               {
                                   Id = transportPost.Id,
                                   Name = transportPost.Name
                               },
                               geometry = new
                               {
                                   type = "Point",
                                   coordinates = new List<decimal>
                            {
                            Convert.ToDecimal(transportPost.EastLongitude.ToString().Replace(".", decimaldelimiter)),
                            Convert.ToDecimal(transportPost.NorthLatitude.ToString().Replace(".", decimaldelimiter))
                            },
                               }
                           }
            });
            ViewBag.TransportPostsLayerJson = transportPostsObject.ToString();

            List<WaterSurfacePost> waterSurfacePosts = _context.WaterSurfacePost
                .Include(w => w.WaterObject)
                .ToList();
            JObject waterSurfacePostsObject = JObject.FromObject(new
            {
                type = "FeatureCollection",
                crs = new
                {
                    type = "name",
                    properties = new
                    {
                        name = "urn:ogc:def:crs:EPSG::3857"
                    }
                },
                features = from waterSurfacePost in waterSurfacePosts
                           select new
                           {
                               type = "Feature",
                               properties = new
                               {
                                   Id = waterSurfacePost.Id,
                                   WaterObjectName = waterSurfacePost.WaterObject.Name,
                                   WaterObjectId = waterSurfacePost.WaterObjectId,
                                   Number = waterSurfacePost.Number,
                               },
                               geometry = new
                               {
                                   type = "Point",
                                   coordinates = new List<decimal>
                            {
                            Convert.ToDecimal(waterSurfacePost.EastLongitude.ToString().Replace(".", decimaldelimiter)),
                            Convert.ToDecimal(waterSurfacePost.NorthLatitude.ToString().Replace(".", decimaldelimiter))
                            },
                               }
                           }
            });
            ViewBag.WaterSurfacePostsLayerJson = waterSurfacePostsObject.ToString();

            List<KazHydrometWaterPost> kazHydrometWaterPosts = _context.KazHydrometWaterPost.ToList();
            JObject kazHydrometWaterPostsObject = JObject.FromObject(new
            {
                type = "FeatureCollection",
                crs = new
                {
                    type = "name",
                    properties = new
                    {
                        name = "urn:ogc:def:crs:EPSG::3857"
                    }
                },
                features = from kazHydrometWaterPost in kazHydrometWaterPosts
                           select new
                           {
                               type = "Feature",
                               properties = new
                               {
                                   Id = kazHydrometWaterPost.Id,
                                   Name = kazHydrometWaterPost.Name,
                                   Number = kazHydrometWaterPost.Number
                               },
                               geometry = new
                               {
                                   type = "Point",
                                   coordinates = new List<decimal>
                            {
                            Convert.ToDecimal(kazHydrometWaterPost.EastLongitude.ToString().Replace(".", decimaldelimiter)),
                            Convert.ToDecimal(kazHydrometWaterPost.NorthLatitude.ToString().Replace(".", decimaldelimiter))
                            },
                               }
                           }
            });
            ViewBag.KazHydrometWaterPostsLayerJson = kazHydrometWaterPostsObject.ToString();

            List<SoilPost> soilPosts = _context.SoilPost.ToList();
            JObject soilPostsObject = JObject.FromObject(new
            {
                type = "FeatureCollection",
                crs = new
                {
                    type = "name",
                    properties = new
                    {
                        name = "urn:ogc:def:crs:EPSG::3857"
                    }
                },
                features = from soilPost in soilPosts
                           select new
                           {
                               type = "Feature",
                               properties = new
                               {
                                   Id = soilPost.Id,
                                   Name = soilPost.Name
                               },
                               geometry = new
                               {
                                   type = "Point",
                                   coordinates = new List<decimal>
                            {
                            Convert.ToDecimal(soilPost.EastLongitude.ToString().Replace(".", decimaldelimiter)),
                            Convert.ToDecimal(soilPost.NorthLatitude.ToString().Replace(".", decimaldelimiter))
                            },
                               }
                           }
            });
            ViewBag.SoilPostsLayerJson = soilPostsObject.ToString();

            List<KazHydrometSoilPost> kazHydrometSoilPosts = _context.KazHydrometSoilPost.ToList();
            JObject kazHydrometSoilPostsObject = JObject.FromObject(new
            {
                type = "FeatureCollection",
                crs = new
                {
                    type = "name",
                    properties = new
                    {
                        name = "urn:ogc:def:crs:EPSG::3857"
                    }
                },
                features = from kazHydrometSoilPost in kazHydrometSoilPosts
                           select new
                           {
                               type = "Feature",
                               properties = new
                               {
                                   Id = kazHydrometSoilPost.Id,
                                   Name = kazHydrometSoilPost.Name
                               },
                               geometry = new
                               {
                                   type = "Point",
                                   coordinates = new List<decimal>
                            {
                            Convert.ToDecimal(kazHydrometSoilPost.EastLongitude.ToString().Replace(".", decimaldelimiter)),
                            Convert.ToDecimal(kazHydrometSoilPost.NorthLatitude.ToString().Replace(".", decimaldelimiter))
                            },
                               }
                           }
            });
            ViewBag.KazHydrometSoilPostsLayerJson = kazHydrometSoilPostsObject.ToString();

            List<RawMaterialsCompany> rawMaterialsCompanies = _context.RawMaterialsCompany.ToList();
            JObject rawMaterialsCompaniesObject = JObject.FromObject(new
            {
                type = "FeatureCollection",
                crs = new
                {
                    type = "name",
                    properties = new
                    {
                        name = "urn:ogc:def:crs:EPSG::3857"
                    }
                },
                features = from rawMaterialsCompany in rawMaterialsCompanies
                           select new
                           {
                               type = "Feature",
                               properties = new
                               {
                                   Id = rawMaterialsCompany.Id,
                                   Name = rawMaterialsCompany.Name
                               },
                               geometry = new
                               {
                                   type = "Point",
                                   coordinates = new List<decimal>
                            {
                            Convert.ToDecimal(rawMaterialsCompany.EastLongitude.ToString().Replace(".", decimaldelimiter)),
                            Convert.ToDecimal(rawMaterialsCompany.NorthLatitude.ToString().Replace(".", decimaldelimiter))
                            },
                               }
                           }
            });
            ViewBag.RawMaterialsCompaniesLayerJson = rawMaterialsCompaniesObject.ToString();

            List<WasteRecyclingCompany> wasteRecyclingCompanies = _context.WasteRecyclingCompany.ToList();
            JObject wasteRecyclingCompaniesObject = JObject.FromObject(new
            {
                type = "FeatureCollection",
                crs = new
                {
                    type = "name",
                    properties = new
                    {
                        name = "urn:ogc:def:crs:EPSG::3857"
                    }
                },
                features = from wasteRecyclingCompany in wasteRecyclingCompanies
                           select new
                           {
                               type = "Feature",
                               properties = new
                               {
                                   Id = wasteRecyclingCompany.Id,
                                   Name = wasteRecyclingCompany.Name
                               },
                               geometry = new
                               {
                                   type = "Point",
                                   coordinates = new List<decimal>
                            {
                            Convert.ToDecimal(wasteRecyclingCompany.EastLongitude.ToString().Replace(".", decimaldelimiter)),
                            Convert.ToDecimal(wasteRecyclingCompany.NorthLatitude.ToString().Replace(".", decimaldelimiter))
                            },
                               }
                           }
            });
            ViewBag.WasteRecyclingCompaniesLayerJson = wasteRecyclingCompaniesObject.ToString();

            List<Company> companies = _context.Company.ToList();
            JObject CompaniesObject = JObject.FromObject(new
            {
                type = "FeatureCollection",
                crs = new
                {
                    type = "name",
                    properties = new
                    {
                        name = "urn:ogc:def:crs:EPSG::3857"
                    }
                },
                features = from Company in companies
                           select new
                           {
                               type = "Feature",
                               properties = new
                               {
                                   Id = Company.Id,
                                   AbbreviatedName = Company.AbbreviatedName,
                                   Type = Company.GetType().Name
                               },
                               geometry = new
                               {
                                   type = "Point",
                                   coordinates = new List<decimal>
                            {
                            Convert.ToDecimal(Company.EastLongitude.ToString().Replace(".", decimaldelimiter)),
                            Convert.ToDecimal(Company.NorthLatitude.ToString().Replace(".", decimaldelimiter))
                            },
                               }
                           }
            });
            ViewBag.CompaniesLayerJson = CompaniesObject.ToString();

            List<GreemPlantsPassport> greemPlantsPassports = _context.GreemPlantsPassport.ToList();
            JObject GreemPlantsPassportsObject = JObject.FromObject(new
            {
                type = "FeatureCollection",
                crs = new
                {
                    type = "name",
                    properties = new
                    {
                        name = "urn:ogc:def:crs:EPSG::3857"
                    }
                },
                features = from GreemPlantsPassport in greemPlantsPassports
                           select new
                           {
                               type = "Feature",
                               properties = new
                               {
                                   Id = GreemPlantsPassport.Id,
                                   GreenObject = GreemPlantsPassport.GreenObject
                               },
                               geometry = new
                               {
                                   type = "Point",
                                   coordinates = new List<decimal>
                            {
                            Convert.ToDecimal(GreemPlantsPassport.EastLongitude.ToString().Replace(".", decimaldelimiter)),
                            Convert.ToDecimal(GreemPlantsPassport.NorthLatitude.ToString().Replace(".", decimaldelimiter))
                            },
                               }
                           }
            });
            ViewBag.GreemPlantsPassportsLayerJson = GreemPlantsPassportsObject.ToString();

            ViewBag.GeoServerPort = Startup.Configuration["GeoServerPort"];

            ViewData["AbbreviatedName"] = new SelectList(_context.Company.OrderBy(a => a.AbbreviatedName).GroupBy(k => k.AbbreviatedName).Select(g => g.First()), "Id", "AbbreviatedName");

            return View(2);
        }
    }
}