namespace FlightHistoryCore.Model
{
    public class FlightStatus
    {
        public int Id { get; set; }

        public bool? Live { get; set; }
        public Estimate Estimate { get; set; }
        public string Text { get; set; }
    }

}
