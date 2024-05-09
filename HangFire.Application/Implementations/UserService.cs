using HangFire.Application.Contracts;
using HangFire.Data.Data;
using HangFire.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HangFire.Application.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserDbContext _context;

        private readonly IMailService _mailService;

        public UserService ( UserDbContext context , IMailService mailService )
        {
            _context = context;
            _mailService = mailService;
        }
        public async Task<User> Add ( User user )
        {
            user.Id = Guid.NewGuid();
            user.IsActive = true;
            user.IsDeleted = false;
            var res = await _context.User.AddAsync(user);
            var count = await _context.SaveChangesAsync();

            return res.Entity;
        }

        public async Task<bool> Delete ( Guid id )
        {
            User user = await _context.User.FindAsync(id);

            if ( user == null )
            {
                return false;
            }

            user.IsDeleted = true;
            user.IsActive = false;
            var res = await _context.SaveChangesAsync();


            return res > 0;
        }

        public async Task<User> Get ( Guid id )
        {
            return await _context.User.Where(x => x.Id == id && !x.IsDeleted).SingleAsync();
        }

        public async Task<int> RemoveInactiveUsers ()
        {
            var users = await _context.User.Where(x => !x.IsActive).ToListAsync();
            _context.User.RemoveRange(users);
            return await _context.SaveChangesAsync();
        }
    }
}
