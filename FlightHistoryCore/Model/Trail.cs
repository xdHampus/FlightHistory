namespace FlightHistoryCore.Model
{
    public class Trail
    {

        public int Id { get; set; }

        public int FlightId { get; set; }
        public Flight Flight { get; set; }

        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public long? Alt { get; set; }
        public long? Spd { get; set; }
        public long? Ts { get; set; }
        public long? Hd { get; set; }
    }






}
