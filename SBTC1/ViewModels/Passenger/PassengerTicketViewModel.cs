

using SBTC1.Models.Entilities;

namespace SBTC1.ViewModels.Passenger
{
    public class PassengerTicketViewModel
    {
        public Ticket Ticket { get; set; }
        public List<PassengerInfo> Passengers { get; set; }
    }
}
