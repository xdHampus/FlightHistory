using System;

namespace FlightRadarCore.Model
{
    public class Destination
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string DestinationIata { get; set; }
        public string DestinationIcao { get; set; }
        public Position Position { get; set; }
        public TimeZone Timezone { get; set; }
        public bool? Visible { get; set; }
        public Uri Website { get; set; }
        public long? Terminal { get; set; }
        public string Baggage { get; set; }
        public string Gate { get; set; }
    }






}
