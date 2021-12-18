using System;

namespace FlightHistoryCore.Model
{
    public class Destination
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DestinationIata { get; set; }
        public string DestinationIcao { get; set; }
        public Position Position { get; set; }
        public bool? Visible { get; set; }
    }

}
