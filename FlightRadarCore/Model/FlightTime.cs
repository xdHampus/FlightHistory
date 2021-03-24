namespace FlightRadarCore.Model
{
    public class FlightTime
    {
        public int Id { get; set; }

        public Estimate Scheduled { get; set; }
        public Estimate Real { get; set; }
        public Estimate Estimated { get; set; }
        public long? Eta { get; set; }
        public long? Updated { get; set; }
        public long? HistoricalFlighttime { get; set; }
        public long? HistoricalDelay { get; set; }

    }






}
