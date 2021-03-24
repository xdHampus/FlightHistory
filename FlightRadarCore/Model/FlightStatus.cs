namespace FlightRadarCore.Model
{
    public class FlightStatus
    {
        public int Id { get; set; }

        public bool? Live { get; set; }
        public string Text { get; set; }
        public string Icon { get; set; }
        public Estimate Estimate { get; set; }
        public bool? Ambiguous { get; set; }
        public string GenericStatusText { get; set; }
        public string GenericStatusColor { get; set; }
        public string GenericStatusType { get; set; }
        public long? GenericEventcUtc { get; set; }
        public long? GenericEventLocal { get; set; }

    }






}
