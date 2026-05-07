using MusicGlue.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MusicGlue.Commands
{
    public class ResendCommand : ICommand
    {
        private Thread resendThread;
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if (parameter is OverviewViewModel ovm)
            {
                resendThread = new Thread(ovm.Resend);
                resendThread.Start();
            }
        }
    }
}
