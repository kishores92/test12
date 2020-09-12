using System.Collections.Generic;
using System.Threading.Tasks;
using GreenPOS.Models;


namespace GreenPOS.Interfaces
{
    public interface IUserService
    {
        UserViewModel Authenticate(LoginViewModel vm);
        Task<long> SaveUserAsync(UserViewModel vm, long loggedinUserId);
        Task<UserViewModel> GetUserAsync(long Id, long loggedinUserId);
        Task<long> DeleteUserAsync(long Id, long loggedinUserId);
        Task<IEnumerable<UserViewModel>> GetUsersAsync(long loggedinUserId);
        
        Task<IEnumerable<SelectListItem>> GetRolesAsync(long userId);

    }
}
