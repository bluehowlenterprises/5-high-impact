using Impact.Models;
using Microsoft.EntityFrameworkCore;

namespace Impact.Data
{
    public abstract class IDataContext : DbContext
    {
        public IDataContext()
        {

        }

        public IDataContext(DbContextOptions<IDataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Orders> Orders { get; set; }
    }
}