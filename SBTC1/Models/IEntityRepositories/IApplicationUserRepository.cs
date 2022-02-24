using Microsoft.AspNetCore.Mvc;
using SBTC1.Models.Entilities;

namespace SBTC1.Models.IEntityRepositories
{
    public interface IApplicationUserRepository
    {
        ApplicationUser Add(ApplicationUser user);
        ApplicationUser Update(ApplicationUser userChanges);
        ApplicationUser GetApplicationUser(string userId);
        ApplicationUser GetApplicationUserFromEmail(string email);
        List<ApplicationUser> GetApplicationUserListOfType(string userType);
        bool Delete(string userId);
    }
}
