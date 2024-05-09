using HangFire.Data.Models;

namespace HangFire.Application.Contracts
{
    public interface IUserService
    {
        Task<User> Add ( User user );

        Task<bool> Delete ( Guid id );

        Task<User> Get ( Guid id );

        Task<int> RemoveInactiveUsers ();
    }
}
