

using SBTC1.Models.Entilities;

namespace SBTC1.ViewModels.Admin
{
    public class UserListViewModel
    {
        public List<ApplicationUser> ApplicationUserList { get; set; }
        public string UserType { get; set; }
    }
}
