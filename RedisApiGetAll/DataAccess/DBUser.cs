using Microsoft.EntityFrameworkCore;
using RedisApiGetAll.Models;

namespace RedisApiGetAll.DataAccess
{
    public class DBUser:DbContext
    {
        public DBUser(DbContextOptions options):base(options) { }
        
            
        public DbSet<User> Users { get; set; }
    }
}
