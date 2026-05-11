using MusicGlue.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicGlue.ViewModels
{
    public class ReportViewModel
    {
        private Report report;

        public string Organisation { get; }

        public string FileName
        {
            get { return report.FileName; } 
            set { report.FileName = value; }
        }

        public int SalesCount
        {
            get { return report.TotalSales; }
        }

        public ReportStatus ReportingStatus
        {
            get { return report.ReportStatus; }
            set { report.ReportStatus = value; }
        }

        public bool Selected { get; set; }

        public ReportViewModel(Report report, ReportingOrganisationRepository reportingOrganisationRepo)
        {
            this.report = report;
            Organisation = reportingOrganisationRepo.Get(report.ReportingOrganisationId).Name;
        }

        public void Update(ReportRepository reportRepository)
        {
            reportRepository.Update(report);
        }
        public Report GetReport()
        {
            return report;
        }

    }
}
