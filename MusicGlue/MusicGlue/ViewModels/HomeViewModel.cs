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

namespace MusicGlue.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private ReportingOrganisationRepository repOrganisationRepo;
        private ConsignmentRepository consignmentRepo;
        private Dispatcher dispatcher;

        private ReportHandler repHandler;
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
            repHandler = new ReportHandler();
            TodaysDate = DateTime.Now.ToString("yyyy-MM-dd");
            fileName = "MusicGlue_new_platform" + DateTime.Now.ToString("yyMMdd") + ".txt";

            CheckScriptRunStatus();
        }


        public void StartScript()
        {
            if (!repHandler.CheckReportsHasBeenSend(fileName))
            {
                List<ReportingOrganisation> repOrgs = repOrganisationRepo.GetAll();
                string formattedConsignments = "";
                repOrgs.ForEach(repOrg =>
                {
                    List<Consignment> consignments = consignmentRepo.GetByCustomerCountry(repOrg.Country);
                    if (repOrg.Formatter != null)
                        formattedConsignments = repOrg.Formatter.Format(consignments);
                });
                repHandler.SaveSendReport(formattedConsignments, fileName);

                CheckScriptRunStatus();
            }
        }

        public void CheckScriptRunStatus()
        {
            string message = "";
            if (repHandler.CheckReportsHasBeenSend(fileName))
            {
                message = "Today's report has been sent";
            }
            else
                message = "Today's report has not been sent";

            dispatcher.Invoke(new Action(() =>
            {
                ScriptRunStatus = message;
            }));
        }

        public ICommand StartScriptCommand { get; } = new StartScriptCommand();
    }
}
