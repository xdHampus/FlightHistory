using FlightHistoryCore.Model;
using Microsoft.EntityFrameworkCore;

namespace FlightHistoryCore
{
    public class FlightDbContext : DbContext
    {

        public DbSet<Flight> Flights { get; set; }
        public DbSet<Aircraft> Aircraft { get; set; }
        public DbSet<Airline> Airlines { get; set; }
        public DbSet<RouteDestination> RouteDestinations { get; set; }
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<FlightIdentification> FlightIdentifications { get; set; }
        public DbSet<FlightStatus> FlightStatuses { get; set; }
        public DbSet<FlightTime> FlightTimes { get; set; }
        public DbSet<Estimate> Estimates { get; set; }
        public DbSet<Trail> Trails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options
            .UseSqlite("Data Source=flightradar.db");




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
