using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SBTC1.Models.Entilities
{
    public enum Gender
    {
        Male = 0, Female = 1,
    }

    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }

        // Discriminator Field
        public string UserType { get; set; }

        public Gender Gender { get; set; }

        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [NotMapped]
        public int? Age { get; set; }
    }
}
