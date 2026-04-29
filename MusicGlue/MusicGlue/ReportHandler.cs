using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MusicGlue
{
    public class ReportHandler
    {
        //Method simulates sending reports, hence the double name
        public void SaveSendReport(string formattedConsignments, string fileName)
        {
            using (StreamWriter sw = new StreamWriter(fileName)) //open ressource, write report into file and close again.
            {
                sw.Write(formattedConsignments);
            }

        }

        public bool CheckReportHasBeenSent(string fileName)
        {
            return File.Exists(fileName); // check file already exists
        }

    }
}
