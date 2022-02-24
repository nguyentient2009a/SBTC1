using Microsoft.AspNetCore.Mvc;
using SBTC1.Models.Entilities;

namespace SBTC1.Models.IEntityRepositories
{
    public interface ITrainOperatorRepository
    {
        TrainOperator Add(TrainOperator trainOperator);
        TrainOperator Update(TrainOperator trainOperatorChanges);
        bool Delete(string trainOperatorUserId);
        TrainOperator GetTrainOperator(string trainOperatorUserId);
        IEnumerable<TrainOperator> GetTrainOperatorList();
    }
}