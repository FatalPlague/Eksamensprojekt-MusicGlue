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
                string formattedConsignments = "";
                repOrgs.ForEach(repOrg =>
                {
                    List<Consignment> consignments = _consignmentRepo.GetByCustomerCountry(repOrg.Country);
                    if (repOrg.Formatter != null)
                        formattedConsignments = repOrg.Formatter.Format(consignments);
                });
                _repHandler.SaveSendReport(formattedConsignments, _fileName);

                CheckScriptRunStatus();
            }
        }

        public void CheckScriptRunStatus()
        {
            string message = "";
            if (_repHandler.CheckReportHasBeenSent(_fileName))
            {
                message = "Today's report has been sent";
            }
            else
                message = "Today's report has not been sent";

            _dispatcher.Invoke(new Action(() =>
            {
                ScriptRunStatus = message;
            }));
        }

        public ICommand StartScriptCommand { get; } = new StartScriptCommand();
    }
}
