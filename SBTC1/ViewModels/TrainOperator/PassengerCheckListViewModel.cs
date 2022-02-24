

using SBTC1.Models.Entilities;

namespace SBTC1.ViewModels.TrainOperator
{
    public class PassengerCheckListViewModel
    {
        public PassengerInfo Passenger { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public int TicketId { get; set; }
        public bool IsChecked { get; set; }
    }
}
