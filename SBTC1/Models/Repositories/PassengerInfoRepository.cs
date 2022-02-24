using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SBTC1.Models.Entilities;
using SBTC1.Models.IEntityRepositories;

namespace SBTC1.Models.Repositories
{
    public class PassengerInfoRepository : IPassengerInfoRepository
    {
        private readonly AppDbContext context;
        public PassengerInfoRepository(AppDbContext context)
        {
            this.context = context;
        }

        public PassengerInfo Add(PassengerInfo passengerInfo)
        {
            context.PassengerInfo.Add(passengerInfo);
            context.SaveChanges();
            return passengerInfo;
        }

        public bool Delete(int passengerInfoId)
        {
            PassengerInfo user = GetPassengerInfo(passengerInfoId);
            if (user != null)
            {
                context.PassengerInfo.Remove(user);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public PassengerInfo GetPassengerInfo(int passengerInfoId)
        {
            return context.PassengerInfo.Find(passengerInfoId);
        }

        public PassengerInfo Update(PassengerInfo passengerInfoChanges)
        {
            var passengerInfo = context.PassengerInfo.Attach(passengerInfoChanges);
            passengerInfo.State = EntityState.Modified;
            context.SaveChanges();
            return passengerInfoChanges;
        }
    }
}
