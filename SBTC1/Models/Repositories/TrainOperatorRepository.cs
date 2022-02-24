using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SBTC1.Models.Entilities;
using SBTC1.Models.IEntityRepositories;

namespace SBTC1.Models.Repositories
{
    public class TrainOperatorRepository : ITrainOperatorRepository
    {
        private readonly AppDbContext context;
        public TrainOperatorRepository(AppDbContext context)
        {
            this.context = context;
        }

        public TrainOperator Add(TrainOperator trainOperator)
        {
            context.TrainOperator.Add(trainOperator);
            context.SaveChanges();
            return trainOperator;
        }

        public bool Delete(string trainOperatorUserId)
        {
            TrainOperator user = GetTrainOperator(trainOperatorUserId);
            if (user != null)
            {
                context.TrainOperator.Remove(user);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public TrainOperator GetTrainOperator(string trainOperatorUserId)
        {
            return context.TrainOperator.Find(trainOperatorUserId);
        }

        public IEnumerable<TrainOperator> GetBusOperatorList()
        {
            return context.TrainOperator;
        }

  
        public TrainOperator Update(TrainOperator trainOperatorChanges)
        {
            var busOperator = context.TrainOperator.Attach(trainOperatorChanges);
            busOperator.State = EntityState.Modified;
            context.SaveChanges();
            return trainOperatorChanges;
        }

        public IEnumerable<TrainOperator> GetTrainOperatorList()
        {
            throw new NotImplementedException();
        }
    }
}
