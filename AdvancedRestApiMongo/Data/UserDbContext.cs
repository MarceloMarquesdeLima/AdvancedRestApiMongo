using AdvancedRestApiMongo.Models;
using Microsoft.EntityFrameworkCore;

namespace AdvancedRestApiMongo.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext>options): base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
