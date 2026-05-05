using System;
using System.Collections.Generic;
using System.Text;

namespace MusicGlue.Models
{
    public enum ReportStatus
    {
        Sent,
        Failed,
        Resent
    }
        
    public class Report
    {
        public string FileName {  get; set; }
        public DateTime ReportingDate { get; set; }
        public int TotalSales { get; set; }
        public ReportStatus ReportStatus { get; set; }
        public List<int> ConsignmentIds { get; set; } 
    }
}
