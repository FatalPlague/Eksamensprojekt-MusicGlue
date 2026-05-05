using MusicGlue.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Threading;

namespace MusicGlue.Commands
{
    public class StartScriptCommand : ICommand
    {
        private Thread scriptThread;
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if (parameter is HomeViewModel hvm)
            {
                scriptThread = new(hvm.StartScript);
                scriptThread.Start();
            }
        }
    }
}
