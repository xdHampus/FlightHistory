using FlightRadarCore.Model;
using Microsoft.EntityFrameworkCore;

namespace FlightRadarCore
{
    public class FlightDbContext : DbContext
    {
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Aircraft> Aircraft { get; set; }
        public DbSet<Airline> Airlines { get; set; }
        public DbSet<RouteDestination> RouteDestinations { get; set; }
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<TimeZone> TimeZones { get; set; }
        public DbSet<AircraftElement> AircraftElements { get; set; }
        public DbSet<AircraftIdentification> AircraftIdentifications { get; set; }
        public DbSet<FlightIdentification> FlightIdentifications { get; set; }
        public DbSet<FlightStatus> FlightStatuses { get; set; }
        public DbSet<FlightTime> FlightTimes { get; set; }
        public DbSet<Estimate> Estimates { get; set; }
        public DbSet<Trail> Trails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=flightradar.db");




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            /*

            #region seed data

            var movies = new Flight[] {
                new Flight { Id = 1, Name = "Avengers: Endgame", WorldwideBoxOfficeGross = 2_797_800_564, DurationInMinutes = 181, Release = new DateTime(2019, 4, 26) },
        new Flight { Id = 2, Name = "The Lion King", WorldwideBoxOfficeGross = 1_654_791_102, DurationInMinutes     = 118, Release = new DateTime(2019, 7, 19) },
        new Flight { Id = 3, Name = "Ip Man 4", WorldwideBoxOfficeGross = 192_617_891, DurationInMinutes = 105, Release = new DateTime(2019, 12, 25) },
        new Flight { Id = 4, Name = "Gemini Man", WorldwideBoxOfficeGross = 166_623_705, DurationInMinutes = 116, Release = new DateTime(2019, 11, 20) },
        new Flight { Id = 5, Name = "Downton Abbey", WorldwideBoxOfficeGross = 194_051_302, DurationInMinutes = 120, Release = new DateTime(2020, 9, 20 )}
    };

            var series = new Trail[] {
        new Series { Id = 6 , Name = "The Fresh Prince of Bel-Air", NumberOfEpisodes = 148, Release = new DateTime(1990, 9, 10) },
        new Series { Id = 7 , Name = "Downton Abbey", NumberOfEpisodes = 52, Release = new DateTime(2010, 09, 26) },
        new Series { Id = 8 , Name = "Stranger Things", NumberOfEpisodes = 34 , Release = new DateTime(2016, 7, 11) },
        new Series { Id = 9 , Name = "Kantaro: The Sweet Tooth Salaryman", NumberOfEpisodes = 12, Release = new DateTime(2017,7, 14) },
        new Series { Id = 10, Name = "The Walking Dead", NumberOfEpisodes = 177 , Release = new DateTime(2010, 10, 31) }
    };

            var productions = movies
                .Cast<Production>()
                .Union(series)
                .ToList();

            modelBuilder.Entity<Movie>().HasData(movies);
            modelBuilder.Entity<Series>().HasData(series);

            // characters
            modelBuilder.Entity<Character>().HasData(new Character[]
            {
        // movies
        new Character { Id = 1, Name = "Tony Stark", ProductionId = 1, ActorId = 1 },
        new Character { Id = 2, Name = "Steve Rogers", ProductionId = 1, ActorId = 2 },
        new Character { Id = 3, Name = "Okoye", ProductionId = 1, ActorId = 3 },
        new Character { Id = 4, Name = "Simba", ProductionId = 2, ActorId = 4 },
        new Character { Id = 5, Name = "Nala", ProductionId = 2, ActorId = 5 },
        new Character { Id = 6, Name = "Ip Man", ProductionId = 3, ActorId = 6 },
        new Character { Id = 7, Name = "Henry Brogan", ProductionId = 4, ActorId = 7 },
        new Character { Id = 8, Name = "Violet Crawley", ProductionId = 5, ActorId = 8 },
        new Character { Id = 9, Name = "Lady Mary Crawley", ProductionId = 5, ActorId = 9 },
        // television
        new Character { Id = 10, Name = "Will Smith", ProductionId = 6, ActorId = 7},
        new Character { Id = 11, Name = "Hilary Banks", ProductionId = 6, ActorId = 10 },
        new Character { Id = 12, Name = "Violet Crawley", ProductionId = 7, ActorId = 8 },
        new Character { Id = 13, Name = "Lady Mary Crawley", ProductionId = 7, ActorId = 9 },
        new Character { Id = 14, Name = "Eleven", ProductionId = 8, ActorId = 11 },
        new Character { Id = 15, Name = "Lucas", ProductionId = 8, ActorId = 12 },
        new Character { Id = 16, Name = "Joyce Byers", ProductionId = 8, ActorId = 13 },
        new Character { Id = 17, Name = "Jim Hopper", ProductionId = 8, ActorId = 14 },
        new Character { Id = 18, Name = "Ametani Kantarou", ProductionId = 9, ActorId = 15},
        new Character { Id = 19, Name = "Sano Erika", ProductionId = 9, ActorId = 16 },
        new Character { Id = 20, Name = "Daryl Dixon", ProductionId = 10, ActorId = 17 },
        new Character { Id = 21, Name = "Michonne", ProductionId = 10, ActorId = 3 },
        new Character { Id = 22, Name = "Carol Peletier", ProductionId = 10, ActorId = 18 }
            });

            // actors
            modelBuilder.Entity<Actor>().HasData(new Actor[]
            {
        new Actor { Id = 1, Name = "Robert Downey Jr." },
        new Actor { Id = 2, Name = "Chris Evans" },
        new Actor { Id = 3, Name = "Danai Guira" },
        new Actor { Id = 4, Name = "Donald Glover" },
        new Actor { Id = 5, Name = "Beyoncé" },
        new Actor { Id = 6, Name = "Donny Yen" },
        new Actor { Id = 7, Name = "Will Smith" },
        new Actor { Id = 8, Name = "Maggie Smith" },
        new Actor { Id = 9, Name = "Michelle Dockery" },
        new Actor { Id = 10, Name = "Karyn Parsons" },
        new Actor { Id = 11, Name = "Millie Bobby Brown" },
        new Actor { Id = 12, Name = "Caleb McLaughlin" },
        new Actor { Id = 13, Name = "Winona Ryder"},
        new Actor { Id = 14, Name = "David Harbour" },
        new Actor { Id = 15, Name = "Matsuya Onoe" },
        new Actor { Id = 16, Name = "Hazuki Shimizu"},
        new Actor { Id = 17, Name = "Norman Reedus" },
        new Actor { Id = 18, Name = "Melissa McBride" }
            });

            // let's generate lots of ratings
            var random = new Random();
            var size = 100;
            var sources = new[] {
        "Internet",
        "Newspaper",
        "Magazine",
        "App"
    };

            var ratings = productions
                .SelectMany((production, index) =>
                {

                    return Enumerable
                        .Range(index * 100 + 1, size - 1)
                        .Select(id => new Rating
                        {
                            Id = id,
                            ProductionId = production.Id,
                            Stars = random.Next(1, 6),
                            Source = sources[random.Next(0, 4)]
                        }).ToList();
                });

            modelBuilder.Entity<Rating>().HasData(ratings);

            #endregion

            */
            base.OnModelCreating(modelBuilder);
        }



    }





}
