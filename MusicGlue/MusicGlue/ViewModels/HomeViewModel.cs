using System;
using System.Collections.Generic;
using System.Text;

namespace MusicGlue.ViewModels
{
    public class HomeViewModel
    {
        public string ScriptRunStatus { get; set; }

        public HomeViewModel(string scriptRunStatus)
        {
            ScriptRunStatus = scriptRunStatus;
        }

    }
}
