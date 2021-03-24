namespace FlightHistoryCore.Model
{
    public class Aircraft
    {
        public int Id { get; set; }
        public string ModelCode { get; set; }
        public string ModelText { get; set; }
        public long? CountryId { get; set; }
        public string Registration { get; set; }
        public string Hex { get; set; }
        public string Age { get; set; }
        public string Msn { get; set; }
    }






}
