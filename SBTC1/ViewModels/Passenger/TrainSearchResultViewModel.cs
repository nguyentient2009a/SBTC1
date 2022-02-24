

using SBTC1.Models.Entilities;

namespace SBTC1.ViewModels.Passenger
{
    public class TrainSearchResultViewModel
    {
        public DateTime DateOfJourney { get; set; }        
        public TrainType TrainType { get; set; }
        public List<TrainViewModel> TrainLists { get; set; }
    }
}
