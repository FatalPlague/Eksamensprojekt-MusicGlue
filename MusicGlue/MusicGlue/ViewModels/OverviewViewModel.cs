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

        }

        public void Resend()
        {
            List<ReportViewModel> reports = Reports.Where(report => report.Selected).ToList();
            foreach (ReportViewModel reportvm in reports)
            {
                if (File.Exists(reportvm.FileName))
                {
                    File.Move(reportvm.FileName, reportvm.FileName.Replace(".txt", "_failed.txt"));
                }

                Report report = reportvm.GetReport();
                RemakeReport(report);

                reportvm.ReportingStatus = ReportStatus.Failed;
                reportvm.FileName = reportvm.FileName.Replace(".txt", "_failed.txt");

                reportvm.Update(reportRepo);


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

            string formatedConsignments = reportOrganisation.Formatter.Format(consignments);

            ReportHandler.SaveSendReport(formatedConsignments, report.FileName);

        }

        public ICommand ResendCommand { get; } = new ResendCommand();
    }
}
