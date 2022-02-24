using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SBTC1.Models.Entilities;
using SBTC1.Models.IEntityRepositories;

namespace SBTC1.Models.Repositories
{
    public class SeatRepository : ISeatRepository
    {
        private readonly AppDbContext context;
        public SeatRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Seat Add(Seat seat)
        {
            context.Seat.Add(seat);
            context.SaveChanges();
            return seat;
        }

        public bool Delete(int seatId)
        {
            Seat seat = GetSeatFromSeatId(seatId);
            if (seat != null)
            {
                context.Seat.Remove(seat);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public void DeleteAllSeatsFromBusRouteId(int trainRouteId)
        {
            List<Seat> seats = context.Seat.ToList().FindAll(train => train.TrainRouteId == trainRouteId);
            if (seats != null)
            {
                foreach (Seat seat in seats)
                {
                    Delete(seat.SeatId);
                }
            }
        }

        public IEnumerable<int> GetBookedSeatList(int trainRouteId, DateTime dateOfBooking)
        {
            Seat seat = GetSeatDetails(trainRouteId, dateOfBooking);
            List<int> bookedSeatList = new List<int>();
            if (seat != null)
            {
                if (!string.IsNullOrEmpty(seat.SeatStructure))
                {
                    bookedSeatList = seat.SeatStructure.Split(",").Select(Int32.Parse).ToList();
                }
            }
            return bookedSeatList;
        }

        public Seat GetSeatDetails(int trainRouteId, DateTime dateOfBooking)
        {
            return context.Seat.FirstOrDefault(s => s.TrainRouteId == trainRouteId && s.DateOfJourney == dateOfBooking);
        }

        public Seat GetSeatFromSeatId(int seatId)
        {
            return context.Seat.FirstOrDefault(s => s.SeatId == seatId);
        }

        public Seat Update(Seat seatChanges)
        {
            var seat = context.Seat.Attach(seatChanges);
            seat.State = EntityState.Modified;
            context.SaveChanges();
            return seatChanges;
        }

      
    }
}
