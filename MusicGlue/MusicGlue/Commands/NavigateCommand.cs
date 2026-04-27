using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using MusicGlue.Services;

namespace MusicGlue.Commands
{
    public class NavigateCommand : ICommand
    {
        private readonly NavigationService navigationService;

        public event EventHandler? CanExecuteChanged;

        public NavigateCommand(NavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            navigationService.Navigate();
        }
    }
}
