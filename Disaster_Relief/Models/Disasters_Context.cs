using Microsoft.EntityFrameworkCore;

namespace Disaster_Relief.Models
{
    public class Disasters_Context : DbContext
    {
        public Disasters_Context(DbContextOptions<Disasters_Context> options)
        : base(options)
        { }

        public DbSet<Disaster> Disaster { get; set; }
    }
}
