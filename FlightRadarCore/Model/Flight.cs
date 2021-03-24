using System;
using System.Collections.Generic;

namespace FlightRadarCore.Model
{
    public class Flight
    {

        public int Id { get; set; }

        public bool? ScanCompleted { get; set; } = false;
        //public DateTime? LastScan { get; set; }


        public FlightIdentification Identification { get; set; }
        public FlightStatus Status { get; set; }
        public string Level { get; set; }
        public bool? Promote { get; set; }
        public Aircraft Aircraft { get; set; }
        public Airline Airline { get; set; }
        public string Owner { get; set; }
        public string Airspace { get; set; }
        public RouteDestination Airport { get; set; }
        public List<AircraftElement> FlightHistory { get; set; } = new List<AircraftElement>();
        public string Ems { get; set; }
        public string Availability { get; set; }
        public FlightTime Time { get; set; }
        public List<Trail> Trails { get; set; } = new List<Trail>();



        public long? FirstTimestamp { get; set; }
        public string S { get; set; }
    }






}
