using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MusicGlue
{
    public class ReportHandler
    {
        public void SaveSendReport(string report, string fileName)
        {
            using (StreamWriter sw = new StreamWriter(fileName)) //open ressource, write report into file and close again.
            {
                sw.Write(report);
            }

        }

        public bool CheckReportsHasBeenSendToday(string fileName)
        {
            return File.Exists(fileName); // check file already exists
        }

    }
}
