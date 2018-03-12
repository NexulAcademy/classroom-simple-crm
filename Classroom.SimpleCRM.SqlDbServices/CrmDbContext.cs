using Microsoft.EntityFrameworkCore;

namespace Classroom.SimpleCRM.SqlDbServices
{
    public class CrmDbContext : DbContext
    {
        public CrmDbContext(DbContextOptions<CrmDbContext> options) : base(options)
        {
        }
        public DbSet<Customer> Customer { get; set; }
    }
}
