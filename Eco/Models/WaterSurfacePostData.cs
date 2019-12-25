using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eco.Models
{
    public class WaterSurfacePostData
    {
        public int Id { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "WaterSurfacePost")]
        public WaterSurfacePost WaterSurfacePost { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "WaterSurfacePost")]
        public int WaterSurfacePostId { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "WaterContaminant")]
        public WaterContaminant WaterContaminant { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "WaterContaminant")]
        public int WaterContaminantId { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "DateOfSampling")]
        [DataType(DataType.Date)]
        public DateTime DateOfSampling { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "YearOfSampling")]
        [Range(Constants.YearMin, Constants.YearMax, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int YearOfSampling { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "MonthOfSampling")]
        [Range(1, 12, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int MonthOfSampling { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "DayOfSampling")]
        [Range(1, 31, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int DayOfSampling { get; set; }
        
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "DateOfAnalysis")]
        [DataType(DataType.Date)]
        public DateTime DateOfAnalysis { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "YearOfAnalysis")]
        [Range(Constants.YearMin, Constants.YearMax, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int YearOfAnalysis { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "MonthOfAnalysis")]
        [Range(1, 12, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int MonthOfAnalysis { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "DayOfAnalysis")]
        [Range(1, 31, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int DayOfAnalysis { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Value")]
        [DisplayFormat(DataFormatString = Constants.WaterSurfacePostDataValueDataFormatString, ApplyFormatInEditMode = true)]
        [Range((double)Constants.WaterSurfacePostDataValueMin, (double)Constants.WaterSurfacePostDataValueMax, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public decimal? Value { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Class")]
        public int? Class
        {
            get
            {
                if (WaterContaminant == null)
                {
                    return null;
                }
                if((WaterContaminant.Class1From!=null)&&(WaterContaminant.Class1To!=null))
                {
                    if ((Value >= WaterContaminant.Class1From) && (Value <= WaterContaminant.Class1To))
                    {
                        return 1;
                    }
                }
                if ((WaterContaminant.Class2From != null) && (WaterContaminant.Class2To != null))
                {
                    if ((Value >= WaterContaminant.Class2From) && (Value <= WaterContaminant.Class2To))
                    {
                        return 2;
                    }
                }
                if ((WaterContaminant.Class3From != null) && (WaterContaminant.Class3To != null))
                {
                    if ((Value >= WaterContaminant.Class3From) && (Value <= WaterContaminant.Class3To))
                    {
                        return 3;
                    }
                }
                if ((WaterContaminant.Class4From != null) && (WaterContaminant.Class4To != null))
                {
                    if ((Value >= WaterContaminant.Class4From) && (Value <= WaterContaminant.Class4To))
                    {
                        return 4;
                    }
                }
                if ((WaterContaminant.Class5From != null) && (WaterContaminant.Class5To != null))
                {
                    if ((Value >= WaterContaminant.Class5From) && (Value <= WaterContaminant.Class5To))
                    {
                        return 5;
                    }
                }
                return null;
            }
        }

        public override string ToString()
        {
            return $"Id: {Id.ToString()}\r\n" +
                $"WaterSurfacePostId: {WaterSurfacePostId.ToString()}\r\n" +
                $"WaterContaminantId: {WaterContaminantId.ToString()}\r\n" +
                $"DateOfSampling: {DateOfSampling.ToShortDateString()}\r\n" +
                $"DateOfAnalysis: {DateOfAnalysis.ToShortDateString()}\r\n" +
                $"Value: {Value.ToString()}";
        }
    }

    public class WaterSurfacePostDataIndexPageViewModel
    {
        public IEnumerable<WaterSurfacePostData> Items { get; set; }
        public Pager Pager { get; set; }
    }
}
