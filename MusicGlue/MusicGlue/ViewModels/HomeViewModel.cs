using MusicGlue.Commands;
using MusicGlue.Models;
using MusicGlue.Models.Formatters;
using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Text;
using System.Windows.Input;
using System.Threading;
using System.Threading;
using System.Windows.Threading;
using MusicGlue.Stores;
using System.IO;
using MusicGlue.Services;

namespace MusicGlue.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private ReportingOrganisationRepository repOrganisationRepo;
        private ConsignmentRepository consignmentRepo;
        private Dispatcher dispatcher;

        public ICommand NavigateToOverviewCommand { get; }

        private string fileName;

        private string _scriptRunStatus = "";
        public string ScriptRunStatus
        {
            get { return _scriptRunStatus; }
            set
            {
                _scriptRunStatus = value;
                OnPropertyChanged("ScriptRunStatus");
            }
        }
        private string _todaysDate = "";
        public string TodaysDate
        {
            get { return _todaysDate; }
            set
            {
                _todaysDate = value;
                OnPropertyChanged("TodaysDate");
            }
        }

        public HomeViewModel(NavigationStore navigationStore, Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
            repOrganisationRepo = new ReportingOrganisationRepository();
            consignmentRepo = new ConsignmentRepository();
            TodaysDate = DateTime.Now.ToString("yyyy-MM-dd");
            fileName = "MusicGlue_new_platform" + DateTime.Now.ToString("yyMMdd") + ".txt";

            CheckScriptRunStatus();

            NavigateToOverviewCommand = new NavigateCommand(new NavigationService(navigationStore, () => new OverviewViewModel(navigationStore, dispatcher, consignmentRepo, repOrganisationRepo)));

        }


        public void StartScript()
        {
            try
            {
                scriptFailed = false;
                if (!ReportHandler.CheckReportHasBeenSent(fileName))
                {
                    List<ReportingOrganisation> repOrgs = repOrganisationRepo.GetAll();
                    List<Consignment> allFormattedConsignments = new List<Consignment>();

                    string formattedConsignments = "";
                    repOrgs.ForEach(repOrg =>
                    {
                        List<Consignment> consignments = consignmentRepo.GetByCustomerCountry(repOrg.Country);

                        if (repOrg.Formatter != null)
                        {
                            formattedConsignments = repOrg.Formatter.Format(consignments);
                            allFormattedConsignments.AddRange(consignments);
                        }
                    });

                    ReportHandler.SaveSendReport(formattedConsignments, fileName);
                    CheckScriptRunStatus();

                    allFormattedConsignments.ForEach(consignment =>
                    {
                        consignment.ReportingStatus = ConsignmentReportingStatus.Reported;
                        consignmentRepo.Update(consignment);
                    });
                }
            } 
            catch
            {
                scriptFailed = true;
                CheckScriptRunStatus();
            }
            
        }

        public void ResetReportingStatusAndDeleteFile() // this method is for testing only
        {
            scriptFailed = false;
            consignmentRepo.ResetReportingStatus();
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            CheckScriptRunStatus();
        }

        private bool scriptFailed = false;
        public bool CheckScriptRunStatus()
        {
            string message = "";
            bool result = false;

            if (scriptFailed)
            {
                message = "Today's report failed to send";
                result = false;

                dispatcher.Invoke(new Action(() =>
                {
                    ScriptRunStatus = message;
                }));

                return result;
            }

            if (ReportHandler.CheckReportHasBeenSent(fileName))
            {
                message = "Today's report has been sent";
                result = true;
            }
            else
            {
                message = "Today's report has not been sent";
                result = false;
            }

            dispatcher.Invoke(new Action(() =>
            {
                ScriptRunStatus = message;
            }));

            return result;
        }

        public ICommand StartScriptCommand { get; } = new StartScriptCommand();
        public ICommand ResetCommand { get; } = new ResetCommand();
    }
}
