namespace FlightHistoryCore.Model
{
    public class FlightIdentification
    {
        public int Id { get; set; }

        public string FlightIdentifier { get; set; }
        public long? Row { get; set; }
        public string NumberDefault { get; set; }
        public string NumberAlternative { get; set; }
        public string Callsign { get; set; }
    }

}
