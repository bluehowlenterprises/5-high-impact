using Impact.Models;
using Microsoft.EntityFrameworkCore;

namespace Impact.Data
{
    public class DataContext : IDataContext
    {
        public DataContext(DbContextOptions<IDataContext> options)
            : base(options)
        {
        }

        public override DbSet<Orders> Orders { get; set; }
    }
}
