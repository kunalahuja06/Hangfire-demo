using HangFire.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HangFire.Data.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext () { }

        public UserDbContext ( DbContextOptions<UserDbContext> options ) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
    }
}
