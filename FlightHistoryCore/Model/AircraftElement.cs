namespace FlightHistoryCore.Model
{
    public class AircraftElement
    {
        public int Id { get; set; }

        public AircraftIdentification Identification { get; set; }
        public RouteDestination Airport { get; set; }
        public long? Departure { get; set; }
    }






}
