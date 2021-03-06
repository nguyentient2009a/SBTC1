using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SBTC1.Models.Entilities
{
    public class PassengerInfo
    {
        public int PassengerInfoId { get; set; }

        [Display(Name = "Seat Number")]
        public int PSeatNo { get; set; }

        [Required]
        [Display(Name = "Passenger Name")]
        public string PName { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public Gender PGender { get; set; }

        [Required]
        [Display(Name = "Age")]
        public int PAge { get; set; }

        // For Many to Many relationship with Ticket class
        // Conventional one to many relationship between Transaction and PassengerInfo
        public ICollection<Transaction> Transactions { get; set; }
    }
}