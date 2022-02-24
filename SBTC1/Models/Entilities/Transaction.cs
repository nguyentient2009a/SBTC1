using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SBTC1.Models.Entilities
{
    public enum Banks
    {
        [Display(Name = "Viettin Bank")]
        ViettinBank = 1,
        [Display(Name = "VP Bank")]
        VPBank = 2,
        [Display(Name = "Techcom Bank")]
        TechcomBank = 3,
        [Display(Name = "Vietcom Bank")]
        VietcomBank = 4,
        [Display(Name = "MB Bank")]
        MBBank = 5,
    }

    public class Transaction
    {
        // Composite key - TicketId and PassengerInfoId
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }

        public int PassengerInfoId { get; set; }
        public PassengerInfo PassengerInfo { get; set; }
    }
}
