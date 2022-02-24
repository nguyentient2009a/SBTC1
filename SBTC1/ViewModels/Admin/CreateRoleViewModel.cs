
using System.ComponentModel.DataAnnotations;


namespace SBTC1.ViewModels.Admin
{
    public class CreateRoleViewModel
    {
        [Required]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
    }
}
