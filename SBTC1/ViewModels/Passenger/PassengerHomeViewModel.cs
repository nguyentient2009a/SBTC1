
using System.ComponentModel.DataAnnotations;



namespace SBTC1.ViewModels.Passenger
{
    public class PassengerHomeViewModel
    {
        [Required]
        public string Source { get; set; }

        [Required]
        public string Destination { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Journey")]
        public DateTime DateOfJourney { get; set; }
    }
}