using dgspuanlarim.Models;
using Microsoft.EntityFrameworkCore;

namespace dgspuanlarim.Data
{
    public class DgsDbContext : DbContext
    {
        public DgsDbContext(DbContextOptions<DgsDbContext> options) : base(options) { }
        public DbSet<DgsPuanlarimModel> DgsPuanlarimContext { get; set; }
    }
}
