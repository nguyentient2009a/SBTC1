

using SBTC1.Models.Entilities;

namespace SBTC1.ViewModels.Admin
{
    public class UserDetailsViewModel
    {
        public ApplicationUser User { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int Salary { get; set; }
        public string Experience { get; set; }
        public string Age { get; set; }
    }
}
