using SBTC1.Models.Entilities;
using System.ComponentModel.DataAnnotations;


namespace SBTC1.ViewModels.Admin
{
    public class TrainRouteListViewModel
    {
        public Train Train { get; set; }

        [Display(Name = "Train Operator Name")]
        public string TrainOperatorName { get; set; }

        public List<TrainRoute> TrainRouteList { get; set; }
    }
}
