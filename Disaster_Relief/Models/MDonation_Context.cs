using Microsoft.EntityFrameworkCore;

namespace Disaster_Relief.Models
{
    public class MDonation_Context :DbContext
    {
        public MDonation_Context(DbContextOptions<MDonation_Context> options)
           : base(options)
        { }

        public DbSet<MoneyDonations> MoneyDonations { get; set; }
        public DbSet<AllocatedMDonations> AllocatedMDonations { get; set; }
        public DbSet<Purchase> Purchase { get; set; }
    }
}
