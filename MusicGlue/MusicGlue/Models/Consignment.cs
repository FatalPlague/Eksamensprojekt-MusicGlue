namespace MusicGlue.Models
{
    public enum ConsignmentStatus
    {
        NotDispatched,
        PreOrdered,
        Dispatched
    }

    public enum ConsignmentReportingStatus
    {
        Reported,
        NotReported
    }
    
    public class Consignment
    {
        public int Id { get; set; }
        public string ZipCode { get; set; }
        public string CustomerCountry { get; set; }
        public ConsignmentStatus ConsignmentStatus { get; set; }
        public ConsignmentReportingStatus ReportingStatus { get; set; }

        public List<MusicProduct> MusicProducts { get; set; }
    }
        
}
