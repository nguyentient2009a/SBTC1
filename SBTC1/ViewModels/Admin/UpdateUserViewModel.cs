
using SBTC1.Models.Entilities;
using System.ComponentModel.DataAnnotations;


namespace SBTC1.ViewModels.Admin
{
    public class UpdateUserViewModel
    {
        //User Information
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Display(Name = "Type of User")]        
        public string UserType { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        //Bus Operator
        public string Address { get; set; }
        public int Salary { get; set; }

        [Display(Name = "Date of Joining")]
        [DataType(DataType.Date)]
        public DateTime DateOfJoining { get; set; }

        //For saving Query string parameter        
        public string UpdateUserId { get; set; }
    }
}
