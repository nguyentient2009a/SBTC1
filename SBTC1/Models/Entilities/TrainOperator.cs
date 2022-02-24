using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace SBTC1.Models.Entilities
{
    public class TrainOperator : ApplicationUser
    {
        public string Address { get; set; }
        public int Salary { get; set; }
        public DateTime DateOfJoining { get; set; }
        [NotMapped]
        public int? Experience { get; set; }
        public Train Train { get; set; }
    }
}
