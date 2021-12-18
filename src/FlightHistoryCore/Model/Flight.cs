using System;
using System.Collections.Generic;

namespace FlightHistoryCore.Model
{
    public class Flight
    {

        public int Id { get; set; }

        public bool? ScanCompleted { get; set; } = false;

        public FlightIdentification Identification { get; set; }
        public FlightStatus Status { get; set; }
        public Aircraft Aircraft { get; set; }
        public Airline Airline { get; set; }
        public RouteDestination Airport { get; set; }
        public FlightTime Time { get; set; }
        public List<Trail> Trails { get; set; } = new List<Trail>();



        public long? FirstTimestamp { get; set; }
    }






}
