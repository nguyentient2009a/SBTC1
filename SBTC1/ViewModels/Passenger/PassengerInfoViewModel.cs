
using SBTC1.Models.Entilities;
using System.ComponentModel.DataAnnotations;


namespace SBTC1.ViewModels.Passenger
{
    public class PassengerInfoViewModel
    {
        public List<PassengerInfo> PInfo { get; set; }

        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string PEmail { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string PPhoneNumber { get; set; }
    }
}
