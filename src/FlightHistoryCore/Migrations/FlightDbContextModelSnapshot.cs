﻿// <auto-generated />
using System;
using FlightHistoryCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FlightHistoryCore.Migrations
{
    [DbContext(typeof(FlightDbContext))]
    partial class FlightDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.4");

            modelBuilder.Entity("FlightHistoryCore.Model.Aircraft", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long?>("CountryId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ModelCode")
                        .HasColumnType("TEXT");

                    b.Property<string>("ModelText")
                        .HasColumnType("TEXT");

                    b.Property<string>("Registration")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Aircraft");
                });

            modelBuilder.Entity("FlightHistoryCore.Model.Airline", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("DestinationIata")
                        .HasColumnType("TEXT");

                    b.Property<string>("DestinationIcao")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Short")
                        .HasColumnType("TEXT");

                    b.Property<string>("Url")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Airlines");
                });

            modelBuilder.Entity("FlightHistoryCore.Model.Destination", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("DestinationIata")
                        .HasColumnType("TEXT");

                    b.Property<string>("DestinationIcao")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int?>("PositionId")
                        .HasColumnType("INTEGER");

                    b.Property<bool?>("Visible")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PositionId");

                    b.ToTable("Destinations");
                });

            modelBuilder.Entity("FlightHistoryCore.Model.Estimate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long?>("Arrival")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("Departure")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Estimates");
                });

            modelBuilder.Entity("FlightHistoryCore.Model.Flight", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("AircraftId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("AirlineId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("AirportId")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("FirstTimestamp")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdentificationId")
                        .HasColumnType("INTEGER");

                    b.Property<bool?>("ScanCompleted")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("StatusId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("TimeId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("AircraftId");

                    b.HasIndex("AirlineId");

                    b.HasIndex("AirportId");

                    b.HasIndex("IdentificationId");

                    b.HasIndex("StatusId");

                    b.HasIndex("TimeId");

                    b.ToTable("Flights");
                });

            modelBuilder.Entity("FlightHistoryCore.Model.FlightIdentification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Callsign")
                        .HasColumnType("TEXT");

                    b.Property<string>("FlightIdentifier")
                        .HasColumnType("TEXT");

                    b.Property<string>("NumberAlternative")
                        .HasColumnType("TEXT");

                    b.Property<string>("NumberDefault")
                        .HasColumnType("TEXT");

                    b.Property<long?>("Row")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("FlightIdentifications");
                });

            modelBuilder.Entity("FlightHistoryCore.Model.FlightStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("EstimateId")
                        .HasColumnType("INTEGER");

                    b.Property<bool?>("Text")
                        .HasColumnType("TEXT");

                    b.Property<bool?>("Live")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("EstimateId");

                    b.ToTable("FlightStatuses");
                });

            modelBuilder.Entity("FlightHistoryCore.Model.FlightTime", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("EstimatedId")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("Eta")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("RealId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ScheduledId")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("Updated")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("EstimatedId");

                    b.HasIndex("RealId");

                    b.HasIndex("ScheduledId");

                    b.ToTable("FlightTimes");
                });

            modelBuilder.Entity("FlightHistoryCore.Model.Position", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long?>("Altitude")
                        .HasColumnType("INTEGER");

                    b.Property<string>("City")
                        .HasColumnType("TEXT");

                    b.Property<string>("CountryCode")
                        .HasColumnType("TEXT");

                    b.Property<string>("CountryId")
                        .HasColumnType("TEXT");

                    b.Property<string>("CountryName")
                        .HasColumnType("TEXT");

                    b.Property<double?>("Latitude")
                        .HasColumnType("REAL");

                    b.Property<double?>("Longitude")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("Positions");
                });

            modelBuilder.Entity("FlightHistoryCore.Model.RouteDestination", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("DestinationId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("OriginId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("RealId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("DestinationId");

                    b.HasIndex("OriginId");

                    b.HasIndex("RealId");

                    b.ToTable("RouteDestinations");
                });

            modelBuilder.Entity("FlightHistoryCore.Model.Trail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long?>("Alt")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FlightId")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("Hd")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("Lat")
                        .HasColumnType("REAL");

                    b.Property<double?>("Lng")
                        .HasColumnType("REAL");

                    b.Property<long?>("Spd")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("Ts")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("FlightId");

                    b.ToTable("Trails");
                });

            modelBuilder.Entity("FlightHistoryCore.Model.Destination", b =>
                {
                    b.HasOne("FlightHistoryCore.Model.Position", "Position")
                        .WithMany()
                        .HasForeignKey("PositionId");

                    b.Navigation("Position");
                });

            modelBuilder.Entity("FlightHistoryCore.Model.Flight", b =>
                {
                    b.HasOne("FlightHistoryCore.Model.Aircraft", "Aircraft")
                        .WithMany()
                        .HasForeignKey("AircraftId");

                    b.HasOne("FlightHistoryCore.Model.Airline", "Airline")
                        .WithMany()
                        .HasForeignKey("AirlineId");

                    b.HasOne("FlightHistoryCore.Model.RouteDestination", "Airport")
                        .WithMany()
                        .HasForeignKey("AirportId");

                    b.HasOne("FlightHistoryCore.Model.FlightIdentification", "Identification")
                        .WithMany()
                        .HasForeignKey("IdentificationId");

                    b.HasOne("FlightHistoryCore.Model.FlightStatus", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId");

                    b.HasOne("FlightHistoryCore.Model.FlightTime", "Time")
                        .WithMany()
                        .HasForeignKey("TimeId");

                    b.Navigation("Aircraft");

                    b.Navigation("Airline");

                    b.Navigation("Airport");

                    b.Navigation("Identification");

                    b.Navigation("Status");

                    b.Navigation("Time");
                });

            modelBuilder.Entity("FlightHistoryCore.Model.FlightStatus", b =>
                {
                    b.HasOne("FlightHistoryCore.Model.Estimate", "Estimate")
                        .WithMany()
                        .HasForeignKey("EstimateId");

                    b.Navigation("Estimate");
                });

            modelBuilder.Entity("FlightHistoryCore.Model.FlightTime", b =>
                {
                    b.HasOne("FlightHistoryCore.Model.Estimate", "Estimated")
                        .WithMany()
                        .HasForeignKey("EstimatedId");

                    b.HasOne("FlightHistoryCore.Model.Estimate", "Real")
                        .WithMany()
                        .HasForeignKey("RealId");

                    b.HasOne("FlightHistoryCore.Model.Estimate", "Scheduled")
                        .WithMany()
                        .HasForeignKey("ScheduledId");

                    b.Navigation("Estimated");

                    b.Navigation("Real");

                    b.Navigation("Scheduled");
                });

            modelBuilder.Entity("FlightHistoryCore.Model.RouteDestination", b =>
                {
                    b.HasOne("FlightHistoryCore.Model.Destination", "Destination")
                        .WithMany()
                        .HasForeignKey("DestinationId");

                    b.HasOne("FlightHistoryCore.Model.Destination", "Origin")
                        .WithMany()
                        .HasForeignKey("OriginId");

                    b.HasOne("FlightHistoryCore.Model.Estimate", "Real")
                        .WithMany()
                        .HasForeignKey("RealId");

                    b.Navigation("Destination");

                    b.Navigation("Origin");

                    b.Navigation("Real");
                });

            modelBuilder.Entity("FlightHistoryCore.Model.Trail", b =>
                {
                    b.HasOne("FlightHistoryCore.Model.Flight", "Flight")
                        .WithMany("Trails")
                        .HasForeignKey("FlightId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Flight");
                });

            modelBuilder.Entity("FlightHistoryCore.Model.Flight", b =>
                {
                    b.Navigation("Trails");
                });
#pragma warning restore 612, 618
        }
    }
}
