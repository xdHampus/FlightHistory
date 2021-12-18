namespace FlightHistoryCore.Model
{
    public class Position
    {
        public int Id { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public long? Altitude { get; set; }
        public string CountryId { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string City { get; set; }
    }






}
