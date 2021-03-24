namespace FlightRadarCore.Model
{
    public class RouteDestination
    {
        public int Id { get; set; }

        public Destination Origin { get; set; }
        public Destination Destination { get; set; }
        public Estimate Real { get; set; }
    }






}
