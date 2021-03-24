namespace FlightRadarCore.Model
{
    public class TimeZone
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public long? Offset { get; set; }
        public string OffsetHours { get; set; }
        public string Abbr { get; set; }
        public string AbbrName { get; set; }
        public bool? IsDst { get; set; }
    }






}
