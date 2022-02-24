using Microsoft.AspNetCore.Mvc;
using SBTC1.Models.Entilities;
namespace SBTC1.Models.IEntityRepositories
{
    public interface ITrainRepository
    {
        Train Add(Train train);
        Train Update(Train trainChanges);
        bool Delete(int trainID);
        Train GetTrain(int trainId);
        Train GetTrainFromTrainOperatorId(string trainOperatorId);
        IEnumerable<TrainOperator> GetAvailableTrainOperators();
        IEnumerable<Train> GetAllTrains();
        IEnumerable<Train> FilterTrainByTrainType(List<Train> trains, TrainType trainType);
        IEnumerable<Train> FilterTrainByTrainTime(List<Train> trains, string trainTime);
        IEnumerable<Train> FilterTrainByRatings(List<Train> trains);
    }
}

