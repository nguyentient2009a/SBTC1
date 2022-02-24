using Microsoft.AspNetCore.Mvc;
using SBTC1.Models.Entilities;

namespace SBTC1.Models.IEntityRepositories
{
    public interface ISeatRepository
    {
        Seat Add(Seat seat);
        Seat Update(Seat seatChanges);
        bool Delete(int seatId);
        Seat GetSeatFromSeatId(int seatId);
        Seat GetSeatDetails(int trainRouteId, DateTime dateOfBooking);
        IEnumerable<int> GetBookedSeatList(int trainRouteId, DateTime dateOfBooking);
        void DeleteAllSeatsFromTrainRouteId(int trainRouteId)
        {
            {
                throw new NotImplementedException();
            }

        }
    }
}
