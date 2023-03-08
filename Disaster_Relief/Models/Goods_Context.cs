using Microsoft.EntityFrameworkCore;
using Disaster_Relief.Models;

namespace Disaster_Relief.Models
{
    public class Goods_Context : DbContext 
    {
        public Goods_Context(DbContextOptions<Goods_Context> options)
           : base(options)
        { }

        public DbSet<GoodsDonations> GoodsDonations { get; set; }
        public DbSet<AllocatedGDonations> AllocatedGDonations { get; set; }
    }
}
