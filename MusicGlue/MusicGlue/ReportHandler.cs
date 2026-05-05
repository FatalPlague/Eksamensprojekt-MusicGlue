using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MusicGlue
{
    public static class ReportHandler
    {
        //Method simulates sending reports, hence the double name
        public static void SaveSendReport(string formattedConsignments, string fileName)
        {
            using (StreamWriter sw = new StreamWriter(fileName)) //open ressource, write report into file and close again.
            {
                sw.Write(formattedConsignments);
            }

        }

        public static bool CheckReportHasBeenSent(string fileName)
        {
            return File.Exists(fileName); // check file already exists
        }

    }
}
