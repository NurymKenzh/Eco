using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eco.Models
{
    public class TransportPostData
    {
        public int Id { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "TransportPost")]
        public TransportPost TransportPost { get; set; }
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "TransportPost")]
        public int TransportPostId { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "DateTime")]
        public DateTime DateTime { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Year")]
        [Range(Constants.YearMin, Constants.YearMax, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int Year { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Month")]
        [Range(1, 12, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int Month { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Day")]
        [Range(1, 31, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int Day { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Hour")]
        [Range(0, 23, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int Hour { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Minute")]
        [Range(0, 59, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int Minute { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "Second")]
        [Range(0, 59, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int Second { get; set; }
        
        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "TheLengthOfTheInhibitorySignalSec")]
        [Range(Constants.TransportPostDataTheLengthOfTheInhibitorySignalSecMin, Constants.TransportPostDataTheLengthOfTheInhibitorySignalSecMax, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int? TheLengthOfTheInhibitorySignalSec { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "TotalNumberOfVehiclesIn20Minutes")]
        [Range(Constants.TransportPostDataTotalNumberOfVehiclesIn20MinutesMin, Constants.TransportPostDataTotalNumberOfVehiclesIn20MinutesMax, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int TotalNumberOfVehiclesIn20Minutes { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "RunningLengthm")]
        [Range(Constants.TransportPostDataRunningLengthmMin, Constants.TransportPostDataRunningLengthmMax, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int? RunningLengthm { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "AverageSpeedkmh")]
        [Range(Constants.TransportPostDataAverageSpeedkmhMin, Constants.TransportPostDataAverageSpeedkmhMax, ErrorMessageResourceType = typeof(Resources.Controllers.SharedResources), ErrorMessageResourceName = "ErrorNumberRangeMustBe")]
        public int? AverageSpeedkmh { get; set; }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "CarsNumber")]
        public int? CarsNumber
        {
            get
            {
                int? CarsNumber = null;
                if(TransportPost==null)
                {
                    return null;
                }
                else
                {
                    if(TransportPost.Type)
                    {
                        CarsNumber = (int?)(0.979 * TotalNumberOfVehiclesIn20Minutes);
                    }
                    else
                    {
                        CarsNumber = (int?)(0.919 * TotalNumberOfVehiclesIn20Minutes);
                    }
                }
                return CarsNumber;
            }
        }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "TrucksNumber")]
        public int? TrucksNumber
        {
            get
            {
                int? CarsNumber = null;
                if (TransportPost == null)
                {
                    return null;
                }
                else
                {
                    if (TransportPost.Type)
                    {
                        CarsNumber = 0;
                    }
                    else
                    {
                        CarsNumber = (int?)(0.063 * TotalNumberOfVehiclesIn20Minutes);
                    }
                }
                return CarsNumber;
            }
        }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "BusesDieselNumber")]
        public int? BusesDieselNumber
        {
            get
            {
                int? CarsNumber = null;
                if (TransportPost == null)
                {
                    return null;
                }
                else
                {
                    if (TransportPost.Type)
                    {
                        CarsNumber = (int?)(0.021 * TotalNumberOfVehiclesIn20Minutes);
                    }
                    else
                    {
                        CarsNumber = (int?)(0.018 * TotalNumberOfVehiclesIn20Minutes);
                    }
                }
                return CarsNumber;
            }
        }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "CarsGasolineNumber")]
        public int? CarsGasolineNumber
        {
            get
            {
                return (int?)(0.9487 * CarsNumber);
            }
        }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "CarsDieselNumber")]
        public int? CarsDieselNumber
        {
            get
            {
                return (int?)(0.0513 * CarsNumber);
            }
        }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "TrucksGasolineNumber")]
        public int? TrucksGasolineNumber
        {
            get
            {
                return (int?)(0.405 * TrucksNumber);
            }
        }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "TrucksDieselNumber")]
        public int? TrucksDieselNumber
        {
            get
            {
                return (int?)(0.575 * TrucksNumber);
            }
        }

        [Display(ResourceType = typeof(Resources.Controllers.SharedResources), Name = "TrucksGasNumber")]
        public int? TrucksGasNumber
        {
            get
            {
                return (int?)(0.02 * TrucksNumber);
            }
        }

        public override string ToString()
        {
            return $"Id: {Id.ToString()}\r\n" +
                $"TransportPostId: {TransportPostId.ToString()}\r\n" +
                $"DateTime: {DateTime.ToString()}\r\n" +
                $"TheLengthOfTheInhibitorySignalSec: {TheLengthOfTheInhibitorySignalSec.ToString()}\r\n" +
                $"TotalNumberOfVehiclesIn20Minutes: {TotalNumberOfVehiclesIn20Minutes.ToString()}\r\n" +
                $"RunningLengthm: {RunningLengthm.ToString()}\r\n" +
                $"AverageSpeedkmh: {AverageSpeedkmh.ToString()}";
        }
    }

    public class TransportPostDataIndexPageViewModel
    {
        public IEnumerable<TransportPostData> Items { get; set; }
        public Pager Pager { get; set; }
    }
}
