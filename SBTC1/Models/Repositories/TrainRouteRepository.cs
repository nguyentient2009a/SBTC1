using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SBTC1.Models.Entilities;
using SBTC1.Models.IEntityRepositories;

namespace SBTC1.Models.Repositories
{
    
     public class TrainRouteRepository : ITrainRouteRepository
    {
        private readonly AppDbContext context;
        public TrainRouteRepository(AppDbContext context)
        {
            this.context = context;
        }

        public TrainRoute AddTrainRoute(TrainRoute trainRoute)
        {
            context.TrainRoute.Add(trainRoute);
            context.SaveChanges();
            return trainRoute;
        }

        public bool DeleteTrainRoute(int trainRouteId)
        {
            TrainRoute trainRoute = GetTrainRoute(trainRouteId);
            if (trainRoute != null)
            {
                context.TrainRoute.Remove(trainRoute);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<TrainRoute> GetAllTrainRoutesByTrainId(int trainId)
        {
            return context.TrainRoute.OrderBy(br => br.RouteOrder)
                                .ToList()
                                .FindAll(train => train.TrainId == trainId);
        }

        public TrainRoute GetTrainRoute(int trainRouteId)
        {
            return context.TrainRoute.FirstOrDefault(id => id.TrainId == trainRouteId);
        }

        public TrainRoute GetTrainRouteFromTrainName(string trainName, string source, string destination)
        {
            Train trainObj = context.Train.FirstOrDefault(b => b.TrainName == trainName);
            if (trainObj != null)
            {
                TrainRoute trainRouteObj = SearchTrains(source, destination).Find(b => b.TrainId == trainObj.TrainId);
                return trainRouteObj;
            }
            return null;
        }

        public List<TrainRoute> SearchTrains(string source, string destination)
        {
            return context.TrainRoute.OrderBy(br => br.TicketPrice)
                 .Where(br => br.Source == source && br.Destination == destination)
                 .ToList();
        }

        public TrainRoute UpdateTrainRoute(TrainRoute trainRouteChanges)
        {
            var trainRoute = context.TrainRoute.Attach(trainRouteChanges);
           trainRoute.State = EntityState.Modified;
            context.SaveChanges();
            return trainRouteChanges;
        }
    }
}
    
