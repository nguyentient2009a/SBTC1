using Microsoft.AspNetCore.Mvc;
using SBTC1.Models.Entilities;

namespace SBTC1.Models.IEntityRepositories
{
    public interface IPassengerInfoRepository
    {
        PassengerInfo Add(PassengerInfo passengerInfo);
        PassengerInfo Update(PassengerInfo passengerInfoChanges);
        bool Delete(int passengerInfoId);
        PassengerInfo GetPassengerInfo(int passengerInfoId);
    }
}
