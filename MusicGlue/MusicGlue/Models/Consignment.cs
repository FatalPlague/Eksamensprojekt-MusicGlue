using System;
using System.Collections.Generic;
using System.Text;

namespace MusicGlue.Models
{
    public class Consignment
    {
        public List<MusicProduct> MusicProducts { get; set; }
        public int Id { get; set; }

        public string PostalCode { get; set; }

        public string CustomerCountry { get; set; }

        public ConsignmentStatus ConsignmentStatus { get; set; }

        public bool ReportingStatus { get; set; }
    }

    public enum ConsignmentStatus
    {
        NotDispatched,
        PreOrdered,
        Dispatched
    }
}
