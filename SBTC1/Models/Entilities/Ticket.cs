using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SBTC1.Models.Entilities
{
    public class Ticket
    {
        public int TicketId { get; set; }

        // Fare details
        [Display(Name = "Trip Code")]
        public string PNRCode { get; set; }

        [Display(Name = "Ticket Price")]
        public int TicketPrice { get; set; }

        [Display(Name = "Train Name")]
        public string TrainName { get; set; }

        [Display(Name = "Bus Vehicle Number")]
        public string TrainVehicleNumber { get; set; }

        public string Source { get; set; }
        public string Destination { get; set; }

        [Display(Name = "Date of Journey")]
        public DateTime DateOfJourney { get; set; }

        [DataType(DataType.Time)]
        public DateTime DepartureTime { get; set; }

        [DataType(DataType.Time)]
        public DateTime ArrivalTime { get; set; }

        // Passenger User details
        [Display(Name = "Passenger Count")]
        public int PassengerCount { get; set; }

        // Status can be "Booked", "Pending", "Cancelled", "Checked"
        public string TicketStatus { get; set; }

        // Contact details        
        public string PEmail { get; set; }
        public string PPhoneNumber { get; set; }

        // For Mimicing Net Bank Scenario
        [Required]
        [Display(Name = "Select Bank")]
        public Banks PBankName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(6)]
        [Display(Name = "Bank Secret Code (6 digits)")]
        public string PBankSecretCode { get; set; }
        public int PUserRatings { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
    }
}
