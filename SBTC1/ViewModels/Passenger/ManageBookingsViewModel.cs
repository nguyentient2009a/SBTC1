
using System.ComponentModel.DataAnnotations;


namespace SBTC1.ViewModels.Passenger
{
    public class ManageBookingsViewModel
    {
        [Required]
        [Display(Name = "Ticket ID")]
        public int TicketId { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Passenger Email")]
        public string PEmail { get; set; }
    }
}
