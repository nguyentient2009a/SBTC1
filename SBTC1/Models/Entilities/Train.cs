using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SBTC1.Models.Entilities
{
    public enum TrainType
    {
        Seater = 1, Sleeper = 2, AC = 3
    }

    public class Train
    {
        public int TrainId { get; set; }

        [Required]
        [Display(Name = "Name of Train")]
        public string TrainName { get; set; }

        // Filters
        public TrainType TrainType { get; set; }

        [Display(Name = "Train Time")]
        public string TrainTime { get; set; }
        public string Ratings { get; set; }
        public int TotalRateCounts { get; set; }

        [Required]
        [Display(Name = "Total Seat")]
        public int TotalSeat { get; set; }

        [Required]
        [Display(Name = "Train Vehicle Number")]
        public string TrainVehicleNumber { get; set; }

        [Required]
        [Display(Name = "Route Sequence")]
        public string RouteSequence { get; set; }
        [Display(Name = "Train Operator Name")]
        public string TrainOperatorId { get; set; }
        public TrainOperator TrainOperator { get; set; }
        public ICollection<TrainRoute> TrainRoutes { get; set; }
    }
}


