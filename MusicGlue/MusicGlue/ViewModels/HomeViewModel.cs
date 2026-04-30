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

namespace MusicGlue.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private ReportingOrganisationRepository _repOrganisationRepo;
        private ConsignmentRepository _consignmentRepo;
        private Dispatcher _dispatcher;

        private ReportHandler _repHandler;
        private string _fileName;

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
            this._dispatcher = dispatcher;
            _repOrganisationRepo = new ReportingOrganisationRepository();
            _consignmentRepo = new ConsignmentRepository();
            _repHandler = new ReportHandler();
            TodaysDate = DateTime.Now.ToString("yyyy-MM-dd");
            _fileName = "MusicGlue_new_platform" + DateTime.Now.ToString("yyMMdd") + ".txt";

            CheckScriptRunStatus();
        }


        public void StartScript()
        {
            if (!_repHandler.CheckReportHasBeenSent(_fileName))
            {
                List<ReportingOrganisation> repOrgs = _repOrganisationRepo.GetAll();
                List<Consignment> allFormattedConsignments = new List<Consignment>();

                string formattedConsignments = "";
                repOrgs.ForEach(repOrg =>
                {
                    List<Consignment> consignments = _consignmentRepo.GetByCustomerCountry(repOrg.Country);

                    if (repOrg.Formatter != null)
                    {
                        formattedConsignments = repOrg.Formatter.Format(consignments);
                        allFormattedConsignments.AddRange(consignments);
                    }
                });

                _repHandler.SaveSendReport(formattedConsignments, _fileName);
                CheckScriptRunStatus();

                allFormattedConsignments.ForEach(consignment =>
                {
                    consignment.ReportingStatus = true;
                    _consignmentRepo.Update(consignment);
                });
            }
        }

        public void ResetReportingStatusAndDeleteFile() // this method is for testing only
        {
            _consignmentRepo.ResetReportingStatus();
            if (File.Exists(_fileName))
            {
                File.Delete(_fileName);
            }
            CheckScriptRunStatus();
        }
        

        public bool CheckScriptRunStatus()
        {
            string message = "";
            bool result = false;
            if (_repHandler.CheckReportHasBeenSent(_fileName))
            {
                message = "Today's report has been sent";
                result = true;
            }
            else
            {
                message = "Today's report has not been sent";
                result = false;
            }

            _dispatcher.Invoke(new Action(() =>
            {
                ScriptRunStatus = message;
            }));

            return result;
        }

        public ICommand StartScriptCommand { get; } = new StartScriptCommand();
        public ICommand ResetCommand { get; } = new ResetCommand();
    }
}
