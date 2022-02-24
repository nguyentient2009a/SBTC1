using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SBTC1.Models.Entilities
{
    public class TrainRoute
    {
        public int TrainRouteId { get; set; }

        [Required]
        [Display(Name = "Route Order")]
        public int RouteOrder { get; set; }

        [Required]
        public string Source { get; set; }

        [Required]
        public string Destination { get; set; }

        [Required]
        [Display(Name = "Departure Time")]
        [DataType(DataType.Time)]
        public DateTime DepartureTime { get; set; }

        [Required]
        [Display(Name = "Arrival Time")]
        [DataType(DataType.Time)]
        public DateTime ArrivalTime { get; set; }

        [NotMapped]
        [DataType(DataType.Time)]
        public DateTime? Duration { get; set; }

        [Display(Name = "Ticket Price")]
        public int TicketPrice { get; set; }

        // BusId as foreign key
        // One to many relationship with Bus class
        [Display(Name = "Bus Id")]
        public int TrainId { get; set; }
        public Train Train { get; set; }

        // One to many relationship between BusRoute and Seat
        public ICollection<Seat> Seat { get; set; }
    }
}
