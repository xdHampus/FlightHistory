using Microsoft.EntityFrameworkCore.Migrations;

namespace FlightHistoryCore.Migrations
{
    public partial class NewMigrationFlight34 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Aircraft",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ModelCode = table.Column<string>(type: "TEXT", nullable: true),
                    ModelText = table.Column<string>(type: "TEXT", nullable: true),
                    CountryId = table.Column<long>(type: "INTEGER", nullable: true),
                    Registration = table.Column<string>(type: "TEXT", nullable: true),
                    Hex = table.Column<string>(type: "TEXT", nullable: true),
                    Age = table.Column<string>(type: "TEXT", nullable: true),
                    Msn = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aircraft", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AircraftIdentifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AircraftIdentifier = table.Column<string>(type: "TEXT", nullable: true),
                    PurplerNumber = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AircraftIdentifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Airlines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Short = table.Column<string>(type: "TEXT", nullable: true),
                    DestinationIata = table.Column<string>(type: "TEXT", nullable: true),
                    DestinationIcao = table.Column<string>(type: "TEXT", nullable: true),
                    Url = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airlines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Estimates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Departure = table.Column<long>(type: "INTEGER", nullable: true),
                    Arrival = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estimates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FlightIdentifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FlightIdentifier = table.Column<string>(type: "TEXT", nullable: true),
                    Row = table.Column<long>(type: "INTEGER", nullable: true),
                    NumberDefault = table.Column<string>(type: "TEXT", nullable: true),
                    NumberAlternative = table.Column<string>(type: "TEXT", nullable: true),
                    Callsign = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightIdentifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Latitude = table.Column<double>(type: "REAL", nullable: true),
                    Longitude = table.Column<double>(type: "REAL", nullable: true),
                    Altitude = table.Column<long>(type: "INTEGER", nullable: true),
                    CountryId = table.Column<string>(type: "TEXT", nullable: true),
                    CountryName = table.Column<string>(type: "TEXT", nullable: true),
                    CountryCode = table.Column<string>(type: "TEXT", nullable: true),
                    City = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimeZones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Offset = table.Column<long>(type: "INTEGER", nullable: true),
                    OffsetHours = table.Column<string>(type: "TEXT", nullable: true),
                    Abbr = table.Column<string>(type: "TEXT", nullable: true),
                    AbbrName = table.Column<string>(type: "TEXT", nullable: true),
                    IsDst = table.Column<bool>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeZones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FlightStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Live = table.Column<bool>(type: "INTEGER", nullable: true),
                    Text = table.Column<string>(type: "TEXT", nullable: true),
                    Icon = table.Column<string>(type: "TEXT", nullable: true),
                    EstimateId = table.Column<int>(type: "INTEGER", nullable: true),
                    Ambiguous = table.Column<bool>(type: "INTEGER", nullable: true),
                    GenericStatusText = table.Column<string>(type: "TEXT", nullable: true),
                    GenericStatusColor = table.Column<string>(type: "TEXT", nullable: true),
                    GenericStatusType = table.Column<string>(type: "TEXT", nullable: true),
                    GenericEventcUtc = table.Column<long>(type: "INTEGER", nullable: true),
                    GenericEventLocal = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlightStatuses_Estimates_EstimateId",
                        column: x => x.EstimateId,
                        principalTable: "Estimates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FlightTimes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ScheduledId = table.Column<int>(type: "INTEGER", nullable: true),
                    RealId = table.Column<int>(type: "INTEGER", nullable: true),
                    EstimatedId = table.Column<int>(type: "INTEGER", nullable: true),
                    Eta = table.Column<long>(type: "INTEGER", nullable: true),
                    Updated = table.Column<long>(type: "INTEGER", nullable: true),
                    HistoricalFlighttime = table.Column<long>(type: "INTEGER", nullable: true),
                    HistoricalDelay = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightTimes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlightTimes_Estimates_EstimatedId",
                        column: x => x.EstimatedId,
                        principalTable: "Estimates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FlightTimes_Estimates_RealId",
                        column: x => x.RealId,
                        principalTable: "Estimates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FlightTimes_Estimates_ScheduledId",
                        column: x => x.ScheduledId,
                        principalTable: "Estimates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Destinations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    DestinationIata = table.Column<string>(type: "TEXT", nullable: true),
                    DestinationIcao = table.Column<string>(type: "TEXT", nullable: true),
                    PositionId = table.Column<int>(type: "INTEGER", nullable: true),
                    TimezoneId = table.Column<int>(type: "INTEGER", nullable: true),
                    Visible = table.Column<bool>(type: "INTEGER", nullable: true),
                    Website = table.Column<string>(type: "TEXT", nullable: true),
                    Terminal = table.Column<long>(type: "INTEGER", nullable: true),
                    Baggage = table.Column<string>(type: "TEXT", nullable: true),
                    Gate = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Destinations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Destinations_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Destinations_TimeZones_TimezoneId",
                        column: x => x.TimezoneId,
                        principalTable: "TimeZones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RouteDestinations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OriginId = table.Column<int>(type: "INTEGER", nullable: true),
                    DestinationId = table.Column<int>(type: "INTEGER", nullable: true),
                    RealId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteDestinations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RouteDestinations_Destinations_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "Destinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RouteDestinations_Destinations_OriginId",
                        column: x => x.OriginId,
                        principalTable: "Destinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RouteDestinations_Estimates_RealId",
                        column: x => x.RealId,
                        principalTable: "Estimates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ScanCompleted = table.Column<bool>(type: "INTEGER", nullable: true),
                    IdentificationId = table.Column<int>(type: "INTEGER", nullable: true),
                    StatusId = table.Column<int>(type: "INTEGER", nullable: true),
                    Level = table.Column<string>(type: "TEXT", nullable: true),
                    Promote = table.Column<bool>(type: "INTEGER", nullable: true),
                    AircraftId = table.Column<int>(type: "INTEGER", nullable: true),
                    AirlineId = table.Column<int>(type: "INTEGER", nullable: true),
                    Owner = table.Column<string>(type: "TEXT", nullable: true),
                    Airspace = table.Column<string>(type: "TEXT", nullable: true),
                    AirportId = table.Column<int>(type: "INTEGER", nullable: true),
                    Ems = table.Column<string>(type: "TEXT", nullable: true),
                    Availability = table.Column<string>(type: "TEXT", nullable: true),
                    TimeId = table.Column<int>(type: "INTEGER", nullable: true),
                    FirstTimestamp = table.Column<long>(type: "INTEGER", nullable: true),
                    S = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Flights_Aircraft_AircraftId",
                        column: x => x.AircraftId,
                        principalTable: "Aircraft",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Flights_Airlines_AirlineId",
                        column: x => x.AirlineId,
                        principalTable: "Airlines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Flights_FlightIdentifications_IdentificationId",
                        column: x => x.IdentificationId,
                        principalTable: "FlightIdentifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Flights_FlightStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "FlightStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Flights_FlightTimes_TimeId",
                        column: x => x.TimeId,
                        principalTable: "FlightTimes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Flights_RouteDestinations_AirportId",
                        column: x => x.AirportId,
                        principalTable: "RouteDestinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AircraftElements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdentificationId = table.Column<int>(type: "INTEGER", nullable: true),
                    AirportId = table.Column<int>(type: "INTEGER", nullable: true),
                    Departure = table.Column<long>(type: "INTEGER", nullable: true),
                    FlightId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AircraftElements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AircraftElements_AircraftIdentifications_IdentificationId",
                        column: x => x.IdentificationId,
                        principalTable: "AircraftIdentifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AircraftElements_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AircraftElements_RouteDestinations_AirportId",
                        column: x => x.AirportId,
                        principalTable: "RouteDestinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Trails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FlightId = table.Column<int>(type: "INTEGER", nullable: false),
                    Lat = table.Column<double>(type: "REAL", nullable: true),
                    Lng = table.Column<double>(type: "REAL", nullable: true),
                    Alt = table.Column<long>(type: "INTEGER", nullable: true),
                    Spd = table.Column<long>(type: "INTEGER", nullable: true),
                    Ts = table.Column<long>(type: "INTEGER", nullable: true),
                    Hd = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trails_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AircraftElements_AirportId",
                table: "AircraftElements",
                column: "AirportId");

            migrationBuilder.CreateIndex(
                name: "IX_AircraftElements_FlightId",
                table: "AircraftElements",
                column: "FlightId");

            migrationBuilder.CreateIndex(
                name: "IX_AircraftElements_IdentificationId",
                table: "AircraftElements",
                column: "IdentificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Destinations_PositionId",
                table: "Destinations",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Destinations_TimezoneId",
                table: "Destinations",
                column: "TimezoneId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_AircraftId",
                table: "Flights",
                column: "AircraftId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_AirlineId",
                table: "Flights",
                column: "AirlineId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_AirportId",
                table: "Flights",
                column: "AirportId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_IdentificationId",
                table: "Flights",
                column: "IdentificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_StatusId",
                table: "Flights",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_TimeId",
                table: "Flights",
                column: "TimeId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightStatuses_EstimateId",
                table: "FlightStatuses",
                column: "EstimateId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightTimes_EstimatedId",
                table: "FlightTimes",
                column: "EstimatedId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightTimes_RealId",
                table: "FlightTimes",
                column: "RealId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightTimes_ScheduledId",
                table: "FlightTimes",
                column: "ScheduledId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteDestinations_DestinationId",
                table: "RouteDestinations",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteDestinations_OriginId",
                table: "RouteDestinations",
                column: "OriginId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteDestinations_RealId",
                table: "RouteDestinations",
                column: "RealId");

            migrationBuilder.CreateIndex(
                name: "IX_Trails_FlightId",
                table: "Trails",
                column: "FlightId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AircraftElements");

            migrationBuilder.DropTable(
                name: "Trails");

            migrationBuilder.DropTable(
                name: "AircraftIdentifications");

            migrationBuilder.DropTable(
                name: "Flights");

            migrationBuilder.DropTable(
                name: "Aircraft");

            migrationBuilder.DropTable(
                name: "Airlines");

            migrationBuilder.DropTable(
                name: "FlightIdentifications");

            migrationBuilder.DropTable(
                name: "FlightStatuses");

            migrationBuilder.DropTable(
                name: "FlightTimes");

            migrationBuilder.DropTable(
                name: "RouteDestinations");

            migrationBuilder.DropTable(
                name: "Destinations");

            migrationBuilder.DropTable(
                name: "Estimates");

            migrationBuilder.DropTable(
                name: "Positions");

            migrationBuilder.DropTable(
                name: "TimeZones");
        }
    }
}
