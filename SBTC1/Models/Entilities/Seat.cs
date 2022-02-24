using Microsoft.AspNetCore.Mvc;

namespace SBTC1.Models.Entilities
{
    public class Seat
    {
        public int SeatId { get; set; }
        public string SeatStructure { get; set; }
        public int AvailableSeats { get; set; }
        public DateTime DateOfJourney { get; set; }

        // BusRouteId as foreign key
        // One to many relationship between BusRoute and Seat
        public int TrainRouteId { get; set; }
        public TrainOperator TrainRoute { get; set; }
    }
}
