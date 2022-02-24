

using SBTC1.Models.Entilities;

namespace SBTC1.ViewModels.Passenger
{
    public class TrainSeatLayoutViewModel
    {
        public DateTime DateOfJourney { get; set; }
        public Train Train { get; set; }
        public TrainRoute TrainRoute { get; set; }
        public Seat Seat { get; set; }
        public List<int> BookedSeats { get; set; }
    }
}
