using System.Collections.Generic;
using System.Threading.Tasks;
using GreenPOS.Common;
using GreenPOS.Models;
using GreenPOS.Entity;

namespace GreenPOS.Interfaces
{
    public interface IRoleService
    {
        Task<long> DeleteRoleByIdAsync(long id, long userId);
        Task<RoleViewModel> GetRoleByIdAsync(long id, long loggedinUserId);
        Task<long> SaveRoleAsync(RoleViewModel vm, long userId);
        Task<IEnumerable<RoleViewModel>> GetRolesAsync(long userId);

        Task<IEnumerable<SelectListItem>> GetRolePermissionsAsync(long userId);

    }
}
