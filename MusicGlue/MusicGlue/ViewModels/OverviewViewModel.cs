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
        public ObservableCollection<ReportViewModel> Reports { get; set; }
        public ICommand NavigateToHomeViewCommand { get; }
        public OverviewViewModel(NavigationStore navigationStore, Dispatcher dispatcher, ConsignmentRepository consignmentRepo, ReportingOrganisationRepository repOrganisationRepo)
        {
            reportRepo = new ReportRepository();
            Reports = new ObservableCollection<ReportViewModel>();
            foreach (Report report in reportRepo.GetAll())
            {
                Reports.Add(new ReportViewModel(report));
            }
            consignmentRepo = new ConsignmentRepository();
            repOrganisationRepo = new ReportingOrganisationRepository();

            this.consignmentRepo = consignmentRepo;
            this.repOrganisationRepo = repOrganisationRepo;

            NavigateToHomeViewCommand = new NavigateCommand(new NavigationService(navigationStore, () => new HomeViewModel(navigationStore, dispatcher)));
            this.dispatcher = dispatcher;

        }

        public void Resend()
        {
            List<ReportViewModel> reports = Reports.Where(report => report.Selected).ToList();
            foreach (ReportViewModel reportvm in reports)
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
                    int index = Reports.IndexOf(reportvm);
                    Reports[index] = new ReportViewModel(oldReport);
                }));

                reportRepo.Update(oldReport);
                RemakeReport(newReport);
            }
        }

        public void RemakeReport(Report report)
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
                    Reports.Add(new ReportViewModel(report));
                }));
            }
        }

        public ICommand ResendCommand { get; } = new ResendCommand();
    }
}
