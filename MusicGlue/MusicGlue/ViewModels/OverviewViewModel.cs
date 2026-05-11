using MusicGlue.Commands;
using MusicGlue.Models;
using MusicGlue.Stores;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using MusicGlue.Services;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using System.IO;

namespace MusicGlue.ViewModels
{
    public class OverviewViewModel : BaseViewModel
    {
        private ReportRepository reportRepo;
        private ConsignmentRepository consignmentRepo;
        private ReportingOrganisationRepository repOrganisationRepo;
        private Dispatcher dispatcher;
        public ObservableCollection<ReportViewModel> ReportsVM { get; set; }
        public ICommand NavigateToHomeViewCommand { get; }
        public OverviewViewModel(NavigationStore navigationStore, Dispatcher dispatcher, ConsignmentRepository consignmentRepo, ReportingOrganisationRepository repOrganisationRepo)
        {
            reportRepo = new ReportRepository();
            consignmentRepo = new ConsignmentRepository();
            repOrganisationRepo = new ReportingOrganisationRepository();
            this.consignmentRepo = consignmentRepo;
            this.repOrganisationRepo = repOrganisationRepo;

            ReportsVM = new ObservableCollection<ReportViewModel>();
            foreach (Report report in reportRepo.GetAll())
            {
                ReportsVM.Add(new ReportViewModel(report, this.repOrganisationRepo));
            }

            NavigateToHomeViewCommand = new NavigateCommand(new NavigationService(navigationStore, () => new HomeViewModel(navigationStore, dispatcher)));
            this.dispatcher = dispatcher;
        }

        public void Resend()
        {
            List<ReportViewModel> reportsVM = ReportsVM.Where(reportVM => reportVM.Selected).ToList();
            foreach (ReportViewModel reportvm in reportsVM)
            {
                Report oldReport = reportvm.GetReport();
                Report newReport = new Report
                {
                    FileName = oldReport.FileName,
                    ReportingDate = DateTime.Now,
                    ReportStatus = ReportStatus.Resent,
                    TotalSales = oldReport.TotalSales,
                    ConsignmentIds = oldReport.ConsignmentIds,
                    ReportingOrganisationId = oldReport.ReportingOrganisationId
                };

                if (File.Exists(oldReport.FileName))
                {
                    File.Move(oldReport.FileName, oldReport.FileName.Replace(".txt", "_failed.txt"));
                }

                oldReport.ReportStatus = ReportStatus.Failed;
                oldReport.FileName = oldReport.FileName.Replace(".txt", "_failed.txt");

                dispatcher.Invoke(new Action(() =>
                {
                    int index = ReportsVM.IndexOf(reportvm);
                    ReportsVM[index] = new ReportViewModel(oldReport, repOrganisationRepo);
                }));

                reportRepo.Update(oldReport);
                RemakeReport(newReport);
            }
        }

        private void RemakeReport(Report report)
        {
            ReportingOrganisation reportOrganisation = repOrganisationRepo.Get(report.ReportingOrganisationId);
            List<Consignment> consignments = new List<Consignment>();
            report.ConsignmentIds.ForEach(consignmentId =>
            {
                consignments.Add(consignmentRepo.GetConsignmentById(consignmentId));
            });

            if (reportOrganisation.Formatter != null)
            {
                string formatedConsignments = reportOrganisation.Formatter.Format(consignments);
                ReportHandler.SaveSendReport(formatedConsignments, report.FileName);

                reportRepo.Create(report);
                dispatcher.Invoke(new Action(() =>
                {
                    ReportsVM.Add(new ReportViewModel(report, repOrganisationRepo));
                }));
            }
        }

        public ICommand ResendCommand { get; } = new ResendCommand();
    }
}
