using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eco.Models
{
    public class Target
    {
        public int Id { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "TypeOfTarget")]
        public TypeOfTarget TypeOfTarget { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "TypeOfTarget")]
        public int TypeOfTargetId { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "NameKK")]
        public string NameKK { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "NameRU")]
        public string NameRU { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Name")]
        public string Name
        {
            get
            {
                string language = new RequestLocalizationOptions().DefaultRequestCulture.Culture.Name,
                    name = NameRU;
                if (language == "kk")
                {
                    name = NameKK;
                }
                if (language == "ru")
                {
                    name = NameRU;
                }
                return name;
            }
        }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "TypeOfAchievement")]
        public bool TypeOfAchievement { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "TypeOfAchievement")]
        public string TypeOfAchievementName
        {
            get
            {
                string language = new RequestLocalizationOptions().DefaultRequestCulture.Culture.Name,
                    TypeOfAchievementName = "";
                if (TypeOfAchievement)
                {
                    TypeOfAchievementName = Resources.Controllers.SharedResources.Direct;
                }
                if (!TypeOfAchievement)
                {
                    TypeOfAchievementName = Resources.Controllers.SharedResources.Reverse;
                }
                return TypeOfAchievementName;
            }
        }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "MeasurementUnit")]
        public MeasurementUnit MeasurementUnit { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "MeasurementUnit")]
        public int MeasurementUnitId { get; set; }
        public override string ToString()
        {
            return $"Id: {Id.ToString()}\r\n" +
                $"TypeOfTargetId: {TypeOfTargetId.ToString()}\r\n" +
                $"NameKK: {NameKK}\r\n" +
                $"NameRU: {NameRU}\r\n" +
                $"TypeOfAchievement: {TypeOfAchievement}\r\n" +
                $"MeasurementUnitId: {MeasurementUnitId.ToString()}";
        }
    }

    public class TargetIndexPageViewModel
    {
        public IEnumerable<Target> Items { get; set; }
        public Pager Pager { get; set; }
    }
}
