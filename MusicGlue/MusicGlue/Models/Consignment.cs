namespace MusicGlue.Models
{
    public class Consignment
    {
        public int Id { get; set; }
        public string ZipCode { get; set; }
        public string CustomerCountry { get; set; }
        public ConsignmentStatus ConsignmentStatus { get; set; }
        public bool ReportingStatus { get; set; }

        public List<MusicProduct> MusicProducts { get; set; }
    }

    public enum ConsignmentStatus
    {
        NotDispatched,
        PreOrdered,
        Dispatched
    }
}
