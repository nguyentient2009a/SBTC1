using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SBTC1.Models.Entilities;
using SBTC1.Models.IEntityRepositories;

namespace SBTC1.Models.Repositories
{
    public class TrainRepository : ITrainRepository
    {
        private readonly AppDbContext context;
        public TrainRepository(AppDbContext context)
        {
            this.context = context;
        }



        public Train Add(Train train)
        {
            context.Train.Add(train);
            context.SaveChanges();
            return train;
        }

        public IEnumerable<Train> FilterTrainByTrainType(List<Train> trains, TrainType trainType)
        {
            return trains.FindAll(train => train.TrainType == trainType);
        }

        public bool Delete(int trainId)
        {
            Train train = GetTrain(trainId);
            if (train != null)
            {
                context.Train.Remove(train);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public IEnumerable<Train> GetAllTrains()
        {
            return context.Train;
        }

        public IEnumerable<TrainOperator> GetAvailableTrainOperators()
        {
            List<TrainOperator> freeTrainOperators = new List<TrainOperator>();
            List<TrainOperator> allTrainOperators = context.TrainOperator.ToList();
            foreach (TrainOperator trainOperator in allTrainOperators)
            {
                Train train = GetTrainFromTrainOperatorId(trainOperator.Id);
                if (train == null)
                {
                    freeTrainOperators.Add(trainOperator);
                }
            }
            return freeTrainOperators;
        }

        public Train GetTrain(int trainId)
        {
            return context.Train.FirstOrDefault(id => id.TrainId == trainId);
        }

        public Train GetTrainFromTrainOperatorId(string trainOperatorId)
        {
            return context.Train.FirstOrDefault(id => id.TrainOperatorId == trainOperatorId);
        }

        public Train Update(Train trainChanges)
        {
            var bus = context.Train.Attach(trainChanges);
            bus.State = EntityState.Modified;
            context.SaveChanges();
            return trainChanges;
        }

        public IEnumerable<Train> FilterTrainByTrainTime(List<Train> trains, string trainTime)
        {
            return trains.FindAll(train => train.TrainTime == trainTime);
        }

        public IEnumerable<Train> FilterTrainByRatings(List<Train> trains)
        {
            trains.Sort((b1, b2) =>
            {
                double r1 = Convert.ToDouble(b1.Ratings);
                double r2 = Convert.ToDouble(b2.Ratings);
                return r1.CompareTo(r2);
            });
            return trains;
        }

    }
}