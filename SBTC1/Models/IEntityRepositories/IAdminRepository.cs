using Microsoft.AspNetCore.Mvc;
using SBTC1.Models.Entilities;

namespace SBTC1.Models.IEntityRepositories
{
    public interface IAdminRepository
    {
        Admin Add(Admin admin);
        Admin Update(Admin adminChanges);
        bool Delete(string adminUserId);
        Admin GetAdmin(string adminUserId);
        IEnumerable<Admin> GetAdminList();
    }
}
