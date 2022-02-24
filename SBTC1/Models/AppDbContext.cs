using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SBTC1.Models.Entilities;

namespace SBTC1.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Calling super class method
            base.OnModelCreating(modelBuilder);

            // Adding Discriminator in the ApplicationUser table
            // Declaring UserType as Discriminator
            modelBuilder.Entity<ApplicationUser>()
                .HasDiscriminator<string>("UserType")
                .HasValue<Passenger>(AppConstant.PASSENGER)
                .HasValue<TrainOperator>(AppConstant.TRAIN_OPERATOR)
                .HasValue<Admin>(AppConstant.ADMIN);

            // Creating CompositeKey for Transaction
            // TicketId and PassengerInfoId as composite key
            modelBuilder.Entity<Transaction>()
              .HasKey(t => new { t.TicketId, t.PassengerInfoId });
        }

        // Entities
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Passenger> Passenger { get; set; }
        public DbSet<TrainOperator> TrainOperator { get; set; }
        public DbSet<Admin> Admin { get; set; }
        public DbSet<Ticket> Ticket { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<PassengerInfo> PassengerInfo { get; set; }
        public DbSet<Train> Train { get; set; }
        public DbSet<TrainRoute> TrainRoute { get; set; }
        public DbSet<Seat> Seat { get; set; }
    }
}
