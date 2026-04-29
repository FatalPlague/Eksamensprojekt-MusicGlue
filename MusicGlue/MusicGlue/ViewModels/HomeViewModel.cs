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
                List<Consignment> consignments = new List<Consignment>();
                string formattedConsignments = "";
                repOrgs.ForEach(repOrg =>
                {
                    consignments = consignmentRepo.GetByCustomerCountry(repOrg.Country);
                    if (repOrg.Formatter != null)
                        formattedConsignments = repOrg.Formatter.Format(consignments);
                });
                repHandler.SaveSendReport(formattedConsignments, fileName);

                CheckScriptRunStatus();

                //if (CheckScriptRunStatus())
                //{
                //    consignments.ForEach(consignment =>
                //    {
                //        consignment.ReportingStatus = true;
                //        consignmentRepo.Update(consignment);
                //    });
                //}
            }
        }

        public bool CheckScriptRunStatus()
        {
            string message = "";
            bool result = false;
            if (repHandler.CheckReportsHasBeenSend(fileName))
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
    }
}
