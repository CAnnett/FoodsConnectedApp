using Microsoft.EntityFrameworkCore;
using FoodsConnectedApp.Models;

namespace FoodsConnectedApp
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options)
          : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
    }
}
