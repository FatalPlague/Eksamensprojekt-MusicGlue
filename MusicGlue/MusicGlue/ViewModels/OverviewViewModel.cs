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

namespace MusicGlue.ViewModels
{
    public class OverviewViewModel : BaseViewModel
    {
        private ReportRepository reportRepo;
        public ObservableCollection<ReportViewModel> Reports { get; set; }
        public ICommand NavigateToHomeViewCommand { get; }
        public OverviewViewModel(NavigationStore navigationStore, Dispatcher dispatcher)
        {
            reportRepo = new ReportRepository();
            Reports = new ObservableCollection<ReportViewModel>();
            foreach (Report report in reportRepo.GetAll())
            {
                Reports.Add(new ReportViewModel(report));
            }

            NavigateToHomeViewCommand = new NavigateCommand(new NavigationService(navigationStore, () => new HomeViewModel(navigationStore, dispatcher)));

        }


    }
}
