using System.Collections.Generic;
using System.Threading.Tasks;
using GreenPOS.Common;
using GreenPOS.Models;

namespace GreenPOS.Interfaces
{
    public interface IContactService
    {
        Task<IEnumerable<ContactViewModel>> GetContactsAsync(long loggedinUserId);

        Task<long> SaveContactAsync(ContactViewModel vm, long loggedinUserId);

        Task<IEnumerable<UserViewModel>> GetUsersAsync(long loggedinUserId);

        Task<long> DeleteContactAsync(long Id, long loggedinUserId);

        Task<ContactViewModel> GetContactAsync(long Id, long loggedinUserId);
    }
}
