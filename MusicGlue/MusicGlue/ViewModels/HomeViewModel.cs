using MusicGlue.Commands;
using MusicGlue.Models;
using MusicGlue.Models.Formatters;
using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Text;
using System.Windows.Input;

namespace MusicGlue.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private ReportingOrganisationRepository repOrganisationRepo;
        private ConsignmentRepository consignmentRepo;

        private ReportHandler repHandler;
        private string fileName;

        private string _scriptRunStatus;
        public string ScriptRunStatus
        {
            get { return _scriptRunStatus; }
            set
            {
                _scriptRunStatus = value;
                OnPropertyChanged("ScriptRunStatus");


            }
        }

        public HomeViewModel()
        {
            repOrganisationRepo = new ReportingOrganisationRepository();
            consignmentRepo = new ConsignmentRepository();
            repHandler = new ReportHandler();
            fileName = "MusicGlue_new_platform" + DateTime.Now.ToString("yyMMdd") + ".txt";

            CheckScriptRunStatus();

        }


        public void StartScript()
        {
            List<ReportingOrganisation> repOrgs = repOrganisationRepo.GetAll();

            repOrgs.ForEach(repOrg =>
            {
                List<Consignment> consignments = consignmentRepo.GetByCustomerCountry(repOrg.Country);

                repOrg.Formatter = new OCCFormatter();

                string formattedConsignments = repOrg.Formatter.Format(consignments);

                //ReportHandler repHandler = new ReportHandler();
                if (!repHandler.CheckReportsHasBeenSend(fileName))
                {
                    repHandler.SaveSendReport(formattedConsignments, fileName);
                    CheckScriptRunStatus();
                }
            });
        }

        public void CheckScriptRunStatus()
        {
            if (repHandler.CheckReportsHasBeenSend(fileName))
                ScriptRunStatus = "Todays report has been send";
            else
                ScriptRunStatus = "Todays report has not been send";
        }

        public ICommand StartScriptCommand { get; } = new StartScriptCommand();
    }
}
