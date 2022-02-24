using Microsoft.AspNetCore.Mvc;
using SBTC1.Models.Entilities;

namespace SBTC1.Models.IEntityRepositories
{
    public interface IPassengerRepository
    {
        Passenger Add(Passenger passenger);
        Passenger Update(Passenger passengerChanges);
        bool Delete(string passengerUserId);
        Passenger GetPassenger(string passengerUserId);
        IEnumerable<Passenger> GetPassengerList();
    }
}
