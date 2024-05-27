using Data_Integration.Models;
using Microsoft.EntityFrameworkCore;

namespace Data_Integration
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<SubscribeToOffer> SubscribeToOffers { get; set; }
        public DbSet<RewardLoyalty> RewardLoyaltys { get; set; }
    }
}