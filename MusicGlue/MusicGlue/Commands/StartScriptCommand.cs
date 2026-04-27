using MusicGlue.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MusicGlue.Commands
{
    public class StartScriptCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if (parameter is HomeViewModel hvm)
            {
                hvm.StartScript();
            }
        }
    }
}
