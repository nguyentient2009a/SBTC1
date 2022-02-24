using Microsoft.AspNetCore.Mvc;
using SBTC1.Models.Entilities;

namespace SBTC1.Models.IEntityRepositories
{
    public interface ITrainRouteRepository
    {
        TrainRoute AddTrainRoute(TrainRoute trainRoute);
        TrainRoute UpdateTrainRoute(TrainRoute trainRouteChanges);
        bool DeleteTrainRoute(int trainRouteId);
        TrainRoute GetTrainRoute(int trainRouteId);
        List<TrainRoute> SearchTrains(string source, string destination);
        List<TrainRoute> GetAllTrainRoutesByTrainId(int trainId);

        // For seat cancellation 
        TrainRoute GetTrainRouteFromTrainName(string trainName, string source, string destination);
    }
}
