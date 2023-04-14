using Microsoft.EntityFrameworkCore;
using webapi.StudentFeatures;

namespace webapi.SharedServices.Db
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Student> Students { get; set; } = null!;
    }
}
